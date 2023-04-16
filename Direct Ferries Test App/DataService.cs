using Direct_Ferries_Test_App.Models;
using Newtonsoft.Json;
using System.Globalization;
using System.Net;
using System.Net.Http.Headers;

namespace Direct_Ferries_Test_App
{
    public class DataService : IDataService
    {

        const string GET_URL = "https://dummyjson.com/users";
        const string POST_URL = "https://directferries.requestcatcher.com";

        private readonly ILogger _logger;

        public DataService(ILogger logger)
        {
            _logger = logger;
        }
        
        public async Task ProcessData()
        {

            var userData = await GetData();

            if(userData is null)
            {
                _logger.WriteLine("failed to get any data, process cancelled.");
                return;
            }

            //Get valid users

            var validUsers = await GetValidUsers(userData.users);

            _logger.WriteLine("Creating reports...");

            //Percentage report

            var percentOfValidUsers = GetPercentOfValidUsersReport(userData.users.Count(), validUsers.Count());

            _logger.WriteLine(percentOfValidUsers);

            //Grouped by gender and blood group report

            var groupedUserReport = GetGroupedGenderAndBloodGroupUserReport(validUsers);

            _logger.WriteLine(groupedUserReport);

            _logger.WriteLine("Reports complete.", true);

            //Post report data

            _logger.WriteLine("Posting reports...");

            await PostReportData("percentofvalidusers", percentOfValidUsers);

            await PostReportData("usersgroupedbygenderandblood", groupedUserReport);

            _logger.WriteLine("Posting reports complete.", true);

            _logger.WriteLine("Data processing complete.");

        }


        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            var trimmedEmail = email.Trim();

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }


        public async Task<bool> IsValidImage(string imageLink)
        {
            //image is a url

            if (!Uri.IsWellFormedUriString(imageLink, UriKind.Absolute))
            {
                return false;
            }

            //image is a valid type

            try
            {

                var req = (HttpWebRequest)WebRequest.Create(imageLink);
                req.Method = "HEAD";
                using (var resp = req.GetResponse())
                {
                    return resp.ContentType.ToLower(CultureInfo.InvariantCulture)
                               .StartsWith("image/");
                }
            }
            catch (Exception ex)
            {
                //failed to get image url
                return false;
            }
        }


        private async Task<List<User>> GetValidUsers(List<User> users)
        {
            _logger.WriteLine("Validating Users...");

            //users with valid emails

            _logger.WriteLine("Getting valid users by email...");

            var validUsers = users.Where(x => IsValidEmail(x.email));

            //users with valid images

            _logger.WriteLine("Getting valid users by image...");

            //not sure if this is what you wanted? validating each image individually?
            validUsers = validUsers.ToAsyncEnumerable()
                .WhereAwait(async x => await IsValidImage(x.image))
                .Select(t => t)
                .ToListAsync()
                .Result;

            _logger.WriteLine("User validation complete.", true);

            return validUsers.ToList();
        }


        private async Task<UserData> GetData()
        {
            _logger.WriteLine($"GET data from - {GET_URL}...");

            try
            {

                using (HttpClient client = new HttpClient())
                {
                    string response = await client.GetStringAsync(GET_URL);

                    var userData = JsonConvert.DeserializeObject<UserData>(response);

                    _logger.WriteLine("GET completed successfully", true);

                    return userData;
                }
            }
            catch (Exception ex)
            {
                _logger.WriteLine($"GetData failed with the following error: {ex.ToString()}");
            }

            return null;
        }


        private async Task PostReportData(string endpoint, string postData)
        {
            var postEndpoint = $"{POST_URL}/{endpoint}";

            _logger.WriteLine($"POST data to - {postEndpoint}...");

            try
            {

                using (var httpClient = new HttpClient())
                {
                    using (var request = new HttpRequestMessage(new HttpMethod("POST"), postEndpoint))
                    {
                        request.Content = new StringContent(postData);
                        request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

                        var response = await httpClient.SendAsync(request);

                        _logger.WriteLine($"POST - {postEndpoint} data completed successfully");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.WriteLine($"PostReportData failed with the following error: {ex.ToString()}");
            }
        }


        private string GetPercentOfValidUsersReport(int origionalUserCount, int validUserCount)
        {
            return $"Percentage of valid users: {(validUserCount / origionalUserCount) * 100}%";
        }


        private string GetGroupedGenderAndBloodGroupUserReport(List<User> users)
        {
            var usersByGender = users.GroupBy(x => x.gender).OrderBy(x => x.Key).ToList();

            var sb = new System.Text.StringBuilder();

            foreach (var genderGroup in usersByGender)
            {
                sb.AppendLine($"{genderGroup.Key} users by blood group:");

                var usersByBloodGroup = genderGroup.ToList().GroupBy(x => x.bloodGroup).OrderBy(x => x.Key);

                foreach (var bloodGroup in usersByBloodGroup)
                {
                    sb.AppendLine($"  {bloodGroup.Key}");
                    sb.AppendLine($"    {string.Join(", ", bloodGroup.Select(x => $"{x.firstName} {x.lastName}"))}");
                }

            }

            return sb.ToString();
        }

    }
}
