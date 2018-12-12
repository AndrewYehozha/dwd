using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using MedicalFridgeServer.Models;

namespace MedicalFridgeServer.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class IndicatorsController : ApiController
    {
        private MedicalFridgeDBEntities2 db = new MedicalFridgeDBEntities2();

        // GET: api/Indicators
        public HttpResponseMessage GetIndicators()
        {
            var indicators = (from Indicator in db.Indicators
                              select new
                              {
                                  Indicator.IdIndicators,
                                  Indicator.IdFridge,
                                  Indicator.Temperature,
                                  Indicator.Humidity,
                                  Indicator.DataTime
                              });

            return GetInfo(indicators);
        }

        // GET: api/Indicators/{IdFridge}
        public HttpResponseMessage GetIndicator(int id)
        {
            var indicator = (from Indicator in db.Indicators
                             select new
                             {
                                 Indicator.IdIndicators,
                                 Indicator.IdFridge,
                                 Indicator.Temperature,
                                 Indicator.Humidity,
                                 Indicator.DataTime
                             }).Where(i => i.IdFridge == id);

            return GetInfo(indicator);
        }
        
        // GET: api/Indicators/?value={IdFridge}
        public IEnumerable GetLastIndicator(int value)
        {
            var indicator = (from Indicator in db.Indicators
                             select new
                             {
                                 Indicator.IdIndicators,
                                 Indicator.IdFridge,
                                 Indicator.Temperature,
                                 Indicator.Humidity
                             }).Where(i => (i.IdFridge == value)).ToList();

            if(indicator.Count() != 0)
                yield return indicator[indicator.Count - 1];
        }

        // GET: api/Indicators/{IdFridge}/{Days}
        public HttpResponseMessage GetIndicatorOfTime(int value1, int value2)
        {
            DateTime d = DateTime.Now.AddDays(-value2);

            var indicator = (from Indicator in db.Indicators
                             select new
                             {
                                 Indicator.IdIndicators,
                                 Indicator.IdFridge,
                                 Indicator.Temperature,
                                 Indicator.Humidity,
                                 Indicator.DataTime
                             }).Where(i => (i.IdFridge == value1) && (i.DataTime > d));
            
            return GetInfo(indicator);
        }

        // POST: api/Indicators
        public async void PostIndicator(Indicator indicator)
        {
            db.Indicators.Add(indicator);
            await db.SaveChangesAsync();
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