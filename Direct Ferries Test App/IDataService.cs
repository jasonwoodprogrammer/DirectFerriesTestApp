namespace Direct_Ferries_Test_App
{
    public interface IDataService
    {
        public bool IsValidEmail(string email);

        public Task<bool> IsValidImage(string imageLink);   

    }
}
