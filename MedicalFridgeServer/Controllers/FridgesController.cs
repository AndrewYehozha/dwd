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
    public class FridgesController : ApiController
    {
        private MedicalFridgeDBEntities db = new MedicalFridgeDBEntities();

        // GET: api/Fridges
        public IQueryable<Fridge> GetFridges()
        {
            return db.Fridges;
        }

        // GET: api/Fridges/5
        [ResponseType(typeof(Fridge))]
        public async Task<IHttpActionResult> GetFridge(int id)
        {
            Fridge fridge = await db.Fridges.FindAsync(id);
            if (fridge == null)
            {
                return NotFound();
            }

            return Ok(fridge);
        }

        // PUT: api/Fridges/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutFridge(int id, Fridge fridge)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != fridge.IdFridge)
            {
                return BadRequest();
            }

            db.Entry(fridge).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FridgeExists(id))
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

        // POST: api/Fridges
        [ResponseType(typeof(Fridge))]
        public async Task<IHttpActionResult> PostFridge(Fridge fridge)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Fridges.Add(fridge);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (FridgeExists(fridge.IdFridge))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = fridge.IdFridge }, fridge);
        }

        // DELETE: api/Fridges/5
        [ResponseType(typeof(Fridge))]
        public async Task<IHttpActionResult> DeleteFridge(int id)
        {
            Fridge fridge = await db.Fridges.FindAsync(id);
            if (fridge == null)
            {
                return NotFound();
            }

            db.Fridges.Remove(fridge);
            await db.SaveChangesAsync();

            return Ok(fridge);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FridgeExists(int id)
        {
            return db.Fridges.Count(e => e.IdFridge == id) > 0;
        }
    }
}