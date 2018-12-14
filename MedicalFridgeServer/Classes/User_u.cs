using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalFridgeServer.Classes
{
    public class User_u
    {
        public int IdUser { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string NameOrganization { get; set; }
        public string Role { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    }
}