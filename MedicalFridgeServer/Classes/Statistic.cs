using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalFridgeServer.Classes
{
    public class Statistic
    {
        public decimal[] temperature { get; set; }
        public decimal[] humidity { get; set; }
    }
}