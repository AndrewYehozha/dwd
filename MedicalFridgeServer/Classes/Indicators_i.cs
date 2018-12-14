using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalFridgeServer.Controllers
{
    public class Indicators_i
    {
        public int IdIndicators { get; set; }
        public int IdFridge { get; set; }
        public Nullable<decimal> Temperature { get; set; }
        public Nullable<decimal> Humidity { get; set; }
        public System.DateTime DataTime { get; set; }
    }
}