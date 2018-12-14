using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalFridgeServer.Classes
{
    public class Medicament_m
    {
        public int IdMedicament { get; set; }
        public int IdFridge { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public System.DateTime DataProduction { get; set; }
        public System.DateTime ExpirationDate { get; set; }
        public decimal Price { get; set; }
        public string Information { get; set; }
    }
}