using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Http;
using MedicalFridgeServer.Classes;
using MedicalFridgeServer.Models;

namespace MedicalFridgeServer.Controllers
{
    public class WriteOffsController : ApiController
    {
        private MedicalFridgeDBEntities2 db = new MedicalFridgeDBEntities2();

        // GET: api/WriteOffs
        [HttpGet]
        public IEnumerable<WriteOff> GetMedicaments()
        {
            var medicaments = (from Medicament in db.Medicaments
                               select new
                               {
                                   Medicament.IdMedicament,
                                   Medicament.IdFridge,
                                   Medicament.Name,
                                   Medicament.Amount,
                                   Medicament.DataProduction,
                                   Medicament.ExpirationDate,
                                   Medicament.Price,
                                   Medicament.Status
                               });

            List<WriteOff> result = new List<WriteOff>() { };

            foreach (var c in medicaments)
            {
                if (c.ExpirationDate.Date <= DateTime.Now.Date)
                    result.Add(new WriteOff
                    {
                        IdMedicament = c.IdMedicament,
                        IdFridge = c.IdFridge,
                        Name = c.Name.Trim(),
                        Amount = c.Amount,
                        DataProduction = c.DataProduction.Date,
                        ExpirationDate = c.ExpirationDate.Date,
                        Price = c.Price,
                        Status = (bool)(c.Status)
                    });
            }

            return result;
        }

        // GET: api/WriteOffs/5
        [HttpGet]
        public IEnumerable<WriteOff> GetMedicament(int id)
        {
            var medicament = (from Medicament in db.Medicaments
                              select new
                              {
                                  Medicament.IdMedicament,
                                  Medicament.IdFridge,
                                  Medicament.Name,
                                  Medicament.Amount,
                                  Medicament.DataProduction,
                                  Medicament.ExpirationDate,
                                  Medicament.Price,
                                  Medicament.Status
                              }).Where(m => (m.IdFridge == id));

            List<WriteOff> result = new List<WriteOff>() { };

            foreach (var c in medicament)
                if (c.ExpirationDate.Date <= DateTime.Now.Date)
                    result.Add(new WriteOff
                    {
                        IdMedicament = c.IdMedicament,
                        IdFridge = c.IdFridge,
                        Name = c.Name.Trim(),
                        Amount = c.Amount,
                        DataProduction = c.DataProduction.Date,
                        ExpirationDate = c.ExpirationDate.Date,
                        Price = c.Price,
                        Status = (bool)(c.Status)
                    });

            return result;
        }

        // GET: api/WriteOffs/IdFridge/?value=value
        [HttpGet]
        public IEnumerable<WriteOff> SearchWriteOffsMedicament(int id, string value)
        {
            string res = value.Trim();

            var search = (from Medicament in db.Medicaments
                          where ((Medicament.IdFridge == id) && (Medicament.Name.Contains(res)))
                          select new
                          {
                              Medicament.IdMedicament,
                              Medicament.IdFridge,
                              Medicament.Name,
                              Medicament.Amount,
                              Medicament.DataProduction,
                              Medicament.ExpirationDate,
                              Medicament.Price,
                              Medicament.Status
                          });

            List<WriteOff> result = new List<WriteOff>() { };

            foreach (var c in search)
                if (c.ExpirationDate.Date <= DateTime.Now.Date)
                    result.Add(new WriteOff
                    {
                        IdMedicament = c.IdMedicament,
                        IdFridge = c.IdFridge,
                        Name = c.Name.Trim(),
                        Amount = c.Amount,
                        DataProduction = c.DataProduction.Date,
                        ExpirationDate = c.ExpirationDate.Date,
                        Price = c.Price,
                        Status = (bool)(c.Status)
                    });

            return result;
        }
    }
}