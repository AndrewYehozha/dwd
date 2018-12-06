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
        private MedicalFridgeDBEntities1 db = new MedicalFridgeDBEntities1();

        // GET: api/Medicaments
        public IQueryable<Medicament> GetMedicaments()
        {
            return db.Medicaments;
        }

        // GET: api/Medicaments/5
        [ResponseType(typeof(Medicament))]
        public async Task<IHttpActionResult> GetMedicament(int id)
        {
            Medicament medicament = await db.Medicaments.FindAsync(id);
            if (medicament == null)
            {
                return NotFound();
            }

            return Ok(medicament);
        }

        // PUT: api/Medicaments/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutMedicament(int id, Medicament medicament)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != medicament.IdMedicament)
            {
                return BadRequest();
            }

            db.Entry(medicament).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicamentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Medicaments
        [ResponseType(typeof(Medicament))]
        public async Task<IHttpActionResult> PostMedicament(Medicament medicament)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Medicaments.Add(medicament);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = medicament.IdMedicament }, medicament);
        }

        // DELETE: api/Medicaments/5
        [ResponseType(typeof(Medicament))]
        public async Task<IHttpActionResult> DeleteMedicament(int id)
        {
            Medicament medicament = await db.Medicaments.FindAsync(id);
            if (medicament == null)
            {
                return NotFound();
            }

            db.Medicaments.Remove(medicament);
            await db.SaveChangesAsync();

            return Ok(medicament);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MedicamentExists(int id)
        {
            return db.Medicaments.Count(e => e.IdMedicament == id) > 0;
        }
    }
}