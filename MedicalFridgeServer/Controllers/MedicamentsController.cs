using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using MedicalFridgeServer.Classes;
using MedicalFridgeServer.Models;

namespace MedicalFridgeServer.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MedicamentsController : ApiController
    {
        private MedicalFridgeDBEntities2 db = new MedicalFridgeDBEntities2();

        // GET: api/Medicaments
        [HttpGet]
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
                                   Medicament.MinTemperature,
                                   Medicament.MaxTemperature,
                                   Medicament.Status
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
                    MinTemperature = c.MinTemperature,
                    MaxTemperature = c.MaxTemperature,
                    Status = (bool)(c.Status)
                });

            return result;
        }

        // GET: api/Medicaments/id
        [HttpGet]
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
                                  Medicament.MinTemperature,
                                  Medicament.MaxTemperature,
                                  Medicament.Status
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
                    MinTemperature = c.MinTemperature,
                    MaxTemperature = c.MaxTemperature,
                    Status = c.Status
                });

            return result;
        }

        // GET: api/Medicaments/?value=value
        [HttpGet]
        public IEnumerable<Medicament_m> GetMedicamentById(int value)
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
                                  Medicament.MinTemperature,
                                  Medicament.MaxTemperature,
                                  Medicament.Status
                              }).Where(m => m.IdMedicament == value);

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
                    MinTemperature = c.MinTemperature,
                    MaxTemperature = c.MaxTemperature,
                    Status = c.Status
                });

            return result;
        }

        // GET: api/Medicaments/IdFridge/?value=value
        [HttpGet]
        public IEnumerable<Medicament_m> SearchMedicament(int id, string value)
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
                              Medicament.MinTemperature,
                              Medicament.MaxTemperature,
                              Medicament.Status
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
                    MinTemperature = c.MinTemperature,
                    MaxTemperature = c.MaxTemperature,
                    Status = c.Status
                });

            return result;
        }

        // PUT: api/Medicaments/id
        [HttpPut]
        public bool PutMedicament(int id, Medicaments medicament)
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

        // POST: api/Medicaments
        [HttpPost]
        public bool PostMedicament(Medicaments medicament)
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
        [HttpDelete]
        public bool DeleteMedicament(int id)
        {
            Medicaments medicament = db.Medicaments.Find(id);

            if (medicament == null)
                return false;

            db.Medicaments.Remove(medicament);
            db.SaveChanges();

            return true;
        }
    }
}