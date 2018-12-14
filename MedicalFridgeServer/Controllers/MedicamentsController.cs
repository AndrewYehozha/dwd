using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using MedicalFridgeServer.Classes;
using MedicalFridgeServer.Models;

namespace MedicalFridgeServer.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MedicamentsController : ApiController
    {
        private MedicalFridgeDBEntities db = new MedicalFridgeDBEntities();

        // GET: api/Medicaments
        public IEnumerable<Medicament_m> GetMedicaments()
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
                                   Medicament.Information
                               });

            List<Medicament_m> result = new List<Medicament_m>() { };

            foreach (var c in medicaments)
                result.Add(new Medicament_m
                {
                    IdMedicament = c.IdMedicament,
                    IdFridge = c.IdFridge,
                    Name = c.Name.Trim(),
                    Amount = c.Amount,
                    DataProduction = c.DataProduction.Date,
                    ExpirationDate = c.ExpirationDate.Date,
                    Price = c.Price,
                    Information = c.Information.Trim()
                });

            return result;
        }

        // GET: api/Medicaments/id
        public IEnumerable<Medicament_m> GetMedicament(int id)
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
                                  Medicament.Information
                              }).Where(m => m.IdFridge == id);

            List<Medicament_m> result = new List<Medicament_m>() { };

            foreach (var c in medicament)
                result.Add(new Medicament_m
                {
                    IdMedicament = c.IdMedicament,
                    IdFridge = c.IdFridge,
                    Name = c.Name.Trim(),
                    Amount = c.Amount,
                    DataProduction = c.DataProduction.Date,
                    ExpirationDate = c.ExpirationDate.Date,
                    Price = c.Price,
                    Information = c.Information.Trim()
                });

            return result;
        }

        // PUT: api/Medicaments/id
        public bool PutMedicament(int id, Medicament medicament)
        {
            if (id != medicament.IdMedicament)
                return false;

            db.Entry(medicament).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }

            return true;
        }

        // POST: api/Medicaments/?value={value}
        public IEnumerable<Medicament_m> SearchMedicament(string value)
        {
            string res = value.Trim();

            var search = (from Medicament in db.Medicaments
                          where ((Medicament.Amount.ToString().Trim().Contains(res)) || (Medicament.Name.Contains(res))
                                  || (Medicament.IdMedicament.ToString().Trim().Contains(res)) || (Medicament.Price.ToString().Trim().Contains(res))
                                  || (Medicament.Information.Contains(res)) || (Medicament.DataProduction.ToString().Trim().Contains(res))
                                  || (Medicament.ExpirationDate.ToString().Trim().Contains(res)))
                          select new
                          {
                              Medicament.IdMedicament,
                              Medicament.IdFridge,
                              Medicament.Name,
                              Medicament.Amount,
                              Medicament.DataProduction,
                              Medicament.ExpirationDate,
                              Medicament.Price,
                              Medicament.Information
                          });

            List<Medicament_m> result = new List<Medicament_m>() { };

            foreach (var c in search)
                result.Add(new Medicament_m
                {
                    IdMedicament = c.IdMedicament,
                    IdFridge = c.IdFridge,
                    Name = c.Name.Trim(),
                    Amount = c.Amount,
                    DataProduction = c.DataProduction.Date,
                    ExpirationDate = c.ExpirationDate.Date,
                    Price = c.Price,
                    Information = c.Information.Trim()
                });

            return result;
        }

        // POST: api/Medicaments
        public bool PostMedicament(Medicament medicament)
        {
            try
            {
                medicament.IdMedicament = 0;
                db.Medicaments.Add(medicament);
                db.SaveChanges();
            }
            catch
            {
                return false;
            }

            return true;
        }

        // DELETE: api/Medicaments/id
        public bool DeleteMedicament(int id)
        {
            Medicament medicament = db.Medicaments.Find(id);

            if (medicament == null)
                return false;

            db.Medicaments.Remove(medicament);
            db.SaveChanges();

            return true;
        }
    }
}