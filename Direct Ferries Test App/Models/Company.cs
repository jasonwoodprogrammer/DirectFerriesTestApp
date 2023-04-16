using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Direct_Ferries_Test_App.Models
{
    public class Company
    {
        public string department { get; set; }
        public string name { get; set; }
        public string title { get; set; }

        public Address address { get; set; }
    }
}
