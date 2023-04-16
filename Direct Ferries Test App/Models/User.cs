using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Direct_Ferries_Test_App.Models
{
    public class User
    {
        public int id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string maidenName { get; set; }
        public int age { get; set; }
        public string gender { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string birthDate { get; set; }
        public string image { get; set; }
        public string bloodGroup { get; set; }
        public int height { get; set; }
        public double weight { get; set; }
        public string eyeColor { get; set; }
        public string domain { get; set; }
        public string ip { get; set; }
        public string macAddress { get; set; }
        public string university { get; set; }
        public string ein { get; set; }
        public string ssn { get; set; }
        public string userAgent { get; set; }

        public Hair hair { get; set; }
        public Address address { get; set; }
        public Bank bank { get; set; }
        public Company company { get; set; }
    }
}
