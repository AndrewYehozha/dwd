using MedicalFridgeServer.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalFridgeServer.Classes
{
    public class Fridge_f
    {
        public int IdFridge { get; set; }
        public int IdUser { get; set; }
        public Nullable<decimal> LastTemperature { get; set; }
        public Nullable<decimal> LastHumidity { get; set; }
    }
}