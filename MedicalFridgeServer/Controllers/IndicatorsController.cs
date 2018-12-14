using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using MedicalFridgeServer.Models;
using MedicalFridgeServer.Classes;

namespace MedicalFridgeServer.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class IndicatorsController : ApiController
    {
        private MedicalFridgeDBEntities db = new MedicalFridgeDBEntities();

        // GET: api/Indicators
        public IEnumerable<Indicators_i> GetIndicators()
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

            List<Indicators_i> result = new List<Indicators_i>() { };

            foreach (var c in indicators)
                result.Add(new Indicators_i
                {
                    IdIndicators = c.IdIndicators,
                    IdFridge = c.IdFridge,
                    Temperature = c.Temperature,
                    Humidity = c.Humidity,
                    DataTime = c.DataTime.Date
                });

            return result;
        }

        // GET: api/Indicators/{IdFridge}
        public IEnumerable<Indicators_i> GetIndicator(int id)
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

            List<Indicators_i> result = new List<Indicators_i>() { };

            foreach (var c in indicator)
                result.Add(new Indicators_i
                {
                    IdIndicators = c.IdIndicators,
                    IdFridge = c.IdFridge,
                    Temperature = c.Temperature,
                    Humidity = c.Humidity,
                    DataTime = c.DataTime.Date
                });

            return result;
        }

        // GET: api/Indicators/?value={IdFridge}
        public IEnumerable<Indicators_i> GetLastIndicator(int value)
        {
            var indicator = (from Indicator in db.Indicators
                             select new
                             {
                                 Indicator.IdIndicators,
                                 Indicator.IdFridge,
                                 Indicator.Temperature,
                                 Indicator.Humidity,
                                 Indicator.DataTime
                             }).Where(i => (i.IdFridge == value)).ToList();

            List<Indicators_i> result = new List<Indicators_i>() { };

            foreach (var c in indicator)
                result.Add(new Indicators_i
                {
                    IdIndicators = c.IdIndicators,
                    IdFridge = c.IdFridge,
                    Temperature = c.Temperature,
                    Humidity = c.Humidity,
                    DataTime = c.DataTime.Date
                });

            yield return result[result.Count - 1];
        }

        // GET: api/Indicators/{IdFridge}/{Days}
        public IEnumerable<Indicators_i> GetIndicatorOfTime(int value1, int value2)
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

            List<Indicators_i> result = new List<Indicators_i>() { };

            foreach (var c in indicator)
                result.Add(new Indicators_i
                {
                    IdIndicators = c.IdIndicators,
                    IdFridge = c.IdFridge,
                    Temperature = c.Temperature,
                    Humidity = c.Humidity,
                    DataTime = c.DataTime.Date
                });

            return result;
        }

        // POST: api/Indicators
        public bool PostIndicator(Indicator indicator)
        {
            try
            {
                db.Indicators.Add(indicator);
                db.SaveChangesAsync();
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}