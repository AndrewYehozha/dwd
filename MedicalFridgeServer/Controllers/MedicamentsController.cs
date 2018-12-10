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
using System.Web.Http.Description;
using MedicalFridgeServer.Models;

namespace MedicalFridgeServer.Controllers
{
    public class MedicamentsController : ApiController
    {
        private MedicalFridgeDBEntities2 db = new MedicalFridgeDBEntities2();

        // GET: api/Medicaments
        public HttpResponseMessage GetMedicaments()
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

            return GetInfo(medicaments);
        }
        
        // GET: api/Medicaments/id
        public HttpResponseMessage GetMedicament(int id)
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

            return GetInfo(medicament);
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
        public HttpResponseMessage SearchMedicament(string value)
        {
            string res = value.Trim();

            var result = (from Medicament in db.Medicaments
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

            return GetInfo(result);
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
        
        private HttpResponseMessage GetInfo(IQueryable i)
        {
            try
            {
                if (db.Indicators.Count() != 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, i);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NoContent, "Indicators list is empty");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}