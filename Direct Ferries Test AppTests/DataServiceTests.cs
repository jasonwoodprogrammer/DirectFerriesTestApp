using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Direct_Ferries_Test_App.Tests
{
    [TestClass()]
    public class DataServiceTests
    {
        [TestMethod()]
        public void DataService_IsValidEmail_Valid_Test()
        {
            //Arrange
            IDataService dataService = new DataService(new Logger());

            //Act
            var goodEmailResult = dataService.IsValidEmail("test@test.com");

            //Assert
            Assert.IsTrue(goodEmailResult);
        }

        [TestMethod()]
        public void DataService_IsValidEmail_Invalid_Test()
        {
            //Arrange
            IDataService dataService = new DataService(new Logger());

            //Act
            var badEmailResult = dataService.IsValidEmail("testtest.com");

            //Assert
            Assert.IsFalse(badEmailResult);
        }

        [TestMethod()]
        public void DataService_IsValidImage_Valid_Test()
        {
            //Arrange
            IDataService dataService = new DataService(new Logger());

            //Act
            var goodImageResult = dataService.IsValidImage("https://robohash.org/excepturiiuremolestiae.png").Result;

            //Assert
            Assert.IsTrue(goodImageResult);
        }

        [TestMethod()]
        public void DataService_IsValidImage_Invalid_Test()
        {
            //Arrange
            IDataService dataService = new DataService(new Logger());

            //Act
            var badImagResult = dataService.IsValidImage("https://google.com").Result;

            //Assert
            Assert.IsFalse(badImagResult);
        }
    }
}