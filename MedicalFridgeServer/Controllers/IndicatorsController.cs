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
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace MedicalFridgeServer.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class IndicatorsController : ApiController
    {
        private MedicalFridgeDBEntities2 db = new MedicalFridgeDBEntities2();

        // GET: api/Indicators
        [HttpGet]
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
            {
                result.Add(new Indicators_i
                {
                    IdIndicators = c.IdIndicators,
                    IdFridge = c.IdFridge,
                    Temperature = c.Temperature,
                    Humidity = c.Humidity,
                    DataTime = DateTime.Parse(c.DataTime)
                });
            }
            return result;
        }

        // GET: api/Indicators/{IdFridge}
        [HttpGet]
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
                    DataTime = DateTime.Parse(c.DataTime)
                });

            return result;
        }

        // GET: api/Indicators/?value={IdFridge}
        [HttpGet]
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
                    DataTime = DateTime.Parse(c.DataTime)
                });

            yield return result[result.Count - 1];
        }

        // GET: api/Indicators/{IdFridge}/{Days}
        [HttpGet]
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
                             }).Where(i => (i.IdFridge == value1));

            List<Indicators_i> result = new List<Indicators_i>() { };
            foreach (var c in indicator)
                if (DateTime.Parse(c.DataTime) > d)
                    result.Add(new Indicators_i
                    {
                        IdIndicators = c.IdIndicators,
                        IdFridge = c.IdFridge,
                        Temperature = c.Temperature,
                        Humidity = c.Humidity,
                        DataTime = DateTime.Parse(c.DataTime)
                    });

            return result;
        }

        // POST: api/Indicators
        [HttpPost]
        public bool PostIndicator(Indicators indicator)
        {
            try
            {
                indicator.IdIndicators = 0;
                indicator.DataTime = DateTime.Now.AddHours(2).ToString("yyyy.MM.ddTHH:mm:ss");
                db.Indicators.Add(indicator);
                db.SaveChangesAsync();
                checkStatusMedicaments();
            }
            catch
            {
                return false;
            }

            return true;
        }

        private void checkStatusMedicaments()
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
                                  Medicament.DataAddInFridge,
                                  Medicament.Status
                              }).Where(m => m.Status == true);
            
            List<Medicaments> resultMed = new List<Medicaments>() { };

            foreach (var c in medicament)
                resultMed.Add(new Medicaments
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
                    DataAddInFridge = c.DataAddInFridge,
                    Status = c.Status
                });


            var fridge = (from Fridge in db.Fridges
                          select new
                          {
                              Fridge.IdFridge,
                              Fridge.IdUser,
                              Fridge.Indicators
                          });

            List<Fridge_f> resultFridge = new List<Fridge_f>() { };

            foreach (var c in fridge)
            {
                decimal temperature = 0;
                decimal humidity = 0;

                if (c.Indicators.Count != 0)
                {
                    temperature = c.Indicators.Last().Temperature.Value;
                    humidity = c.Indicators.Last().Humidity.Value;
                }

                resultFridge.Add(new Fridge_f
                {
                    IdFridge = c.IdFridge,
                    IdUser = c.IdUser,
                    LastTemperature = temperature,
                    LastHumidity = humidity
                });
            }





            db.Entry(medicament).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
            }
        }
    }
}