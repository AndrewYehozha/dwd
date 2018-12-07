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
    public class WriteOffsController : ApiController
    {
        private MedicalFridgeDBEntities2 db = new MedicalFridgeDBEntities2();

        // GET: api/WriteOffs
        public IQueryable<WriteOff> GetWriteOffs()
        {
            return db.WriteOffs;
        }

        // GET: api/WriteOffs/5
        [ResponseType(typeof(WriteOff))]
        public async Task<IHttpActionResult> GetWriteOff(int id)
        {
            WriteOff writeOff = await db.WriteOffs.FindAsync(id);
            if (writeOff == null)
            {
                return NotFound();
            }

            return Ok(writeOff);
        }

        // PUT: api/WriteOffs/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutWriteOff(int id, WriteOff writeOff)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != writeOff.IdWriteOff)
            {
                return BadRequest();
            }

            db.Entry(writeOff).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WriteOffExists(id))
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

        // POST: api/WriteOffs
        [ResponseType(typeof(WriteOff))]
        public async Task<IHttpActionResult> PostWriteOff(WriteOff writeOff)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.WriteOffs.Add(writeOff);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = writeOff.IdWriteOff }, writeOff);
        }

        // DELETE: api/WriteOffs/5
        [ResponseType(typeof(WriteOff))]
        public async Task<IHttpActionResult> DeleteWriteOff(int id)
        {
            WriteOff writeOff = await db.WriteOffs.FindAsync(id);
            if (writeOff == null)
            {
                return NotFound();
            }

            db.WriteOffs.Remove(writeOff);
            await db.SaveChangesAsync();

            return Ok(writeOff);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool WriteOffExists(int id)
        {
            return db.WriteOffs.Count(e => e.IdWriteOff == id) > 0;
        }
    }
}