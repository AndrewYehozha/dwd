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
    public class IndicatorsController : ApiController
    {
        private MedicalFridgeDBEntities2 db = new MedicalFridgeDBEntities2();

        // GET: api/Indicators
        public IQueryable<Indicator> GetIndicators()
        {
            return db.Indicators;
        }

        // GET: api/Indicators/5
        [ResponseType(typeof(Indicator))]
        public async Task<IHttpActionResult> GetIndicator(int id)
        {
            Indicator indicator = await db.Indicators.FindAsync(id);
            if (indicator == null)
            {
                return NotFound();
            }

            return Ok(indicator);
        }

        // PUT: api/Indicators/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutIndicator(int id, Indicator indicator)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != indicator.IdIndicators)
            {
                return BadRequest();
            }

            db.Entry(indicator).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IndicatorExists(id))
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

        // POST: api/Indicators
        [ResponseType(typeof(Indicator))]
        public async Task<IHttpActionResult> PostIndicator(Indicator indicator)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Indicators.Add(indicator);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = indicator.IdIndicators }, indicator);
        }

        // DELETE: api/Indicators/5
        [ResponseType(typeof(Indicator))]
        public async Task<IHttpActionResult> DeleteIndicator(int id)
        {
            Indicator indicator = await db.Indicators.FindAsync(id);
            if (indicator == null)
            {
                return NotFound();
            }

            db.Indicators.Remove(indicator);
            await db.SaveChangesAsync();

            return Ok(indicator);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool IndicatorExists(int id)
        {
            return db.Indicators.Count(e => e.IdIndicators == id) > 0;
        }
    }
}