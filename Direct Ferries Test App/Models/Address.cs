using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Direct_Ferries_Test_App.Models
{
    public class Address
    {
        public string address { get; set; }
        public string city { get; set; }
        public string postalCode { get; set; }
        public string state { get; set; }

        public Coordinates coordinates { get; set; }
    }
}
