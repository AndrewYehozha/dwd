﻿using System;
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
        public IEnumerable<Statistic> GetIndicatorOfTime(int value1, string value2)
        {
            var indicator = (from Indicator in db.Indicators
                             select new
                             {
                                 Indicator.IdIndicators,
                                 Indicator.IdFridge,
                                 Indicator.Temperature,
                                 Indicator.Humidity,
                                 Indicator.DataTime
                             }).Where(i => (i.IdFridge == value1));

            List<Indicators_i> ind = new List<Indicators_i>() { };

            foreach (var c in indicator)
            {
                ind.Add(new Indicators_i
                {
                    IdFridge = c.IdFridge,
                    DataTime = Convert.ToDateTime(c.DataTime),
                    Humidity = c.Humidity,
                    Temperature = c.Temperature
                });
            }

            decimal?[,] result = new decimal?[2,0];

            if (value2 == "Day")
                result = getIndicatorForDay(ind);
            if (value2 == "Week")
                result = getIndicatorForWeekAndMonth(ind, "Week", new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 6));
            if (value2 == "Month")
                result = getIndicatorForWeekAndMonth(ind, "Month", new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1));


            List<Statistic> resList = new List<Statistic>() { };

            decimal[] temp1 = new decimal[result.GetLength(1)];
            decimal[] temp2 = new decimal[result.GetLength(1)];

            for (int i = 0; i < result.GetLength(1); i++)
            {
                temp1[i] = result[0, i].Value;
                temp2[i] = result[1, i].Value;
            }

            resList.Add(new Statistic { temperature = temp1, humidity = temp2 });

            return resList;
        }

        private decimal?[,] getIndicatorForDay(List<Indicators_i> indicator)
        {
            DateTime d = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            
            decimal?[,] result = new decimal?[2, 24];

            decimal? temp = 0, hum = 0;
            int k = 0, j = 0;

            while (d.Hour <= DateTime.Now.AddHours(2).Hour)
            {
                int dsss = DateTime.Now.AddHours(2).Hour;
                int ddsss = d.Hour;

                for (int i = 0; i < indicator.Count; i++)
                {
                    if (indicator[i].DataTime >= d && indicator[i].DataTime <= d.AddHours(1))
                    {
                        temp += indicator[i].Temperature;
                        hum += indicator[i].Humidity;
                        j++;
                    }
                }

                if (j != 0)
                {
                    result[0,k] = temp / j;
                    result[1,k] = hum / j;
                }
                else
                {
                    result[0,k] = 0;
                    result[1,k] = 0;
                }

                d = d.AddHours(1);
                k++; j = 0; temp = 0; hum = 0;
            }

            for (int i = 0; i < result.GetLength(1); i++)
            {
                if (result[0, i] == null)
                    result[0, i] = 0;
                if (result[1, i] == null)
                    result[1, i] = 0;
            }

            return result;
        }

        private decimal?[,] getIndicatorForWeekAndMonth(List<Indicators_i> indicator, string period, DateTime d)
        {
            decimal?[,] result = new decimal?[2, 0];

            if (period == "Week")
                result = new decimal?[2, 7];
            else if (period == "Month")
                result = new decimal?[2, DateTime.Now.Day];

            decimal? temp = 0, hum = 0;
            int k = 0, j = 0;

            while (d.Day <= DateTime.Now.Day)
            {
                for (int i = 0; i < indicator.Count; i++)
                {
                    if (indicator[i].DataTime >= d && indicator[i].DataTime <= d.AddDays(1))
                    {
                        temp += indicator[i].Temperature;
                        hum += indicator[i].Humidity;
                        j++;
                    }
                }

                if (j != 0)
                {
                    result[0,k] = temp / j;
                    result[1,k] = hum / j;
                }
                else
                {
                    result[0,k] = 0;
                    result[1,k] = 0;
                }

                d = d.AddDays(1);
                k++; j = 0; temp = 0; hum = 0;
            }

            for (int i = 0; i < result.GetLength(1); i++)
            {
                if (result[0, i] == null)
                    result[0, i] = 0;
                if (result[1, i] == null)
                    result[1, i] = 0;
            }

            return result;
        }

        // POST: api/Indicators
        [HttpPost]
        public bool PostIndicator(Indicators indicator)
        {
            try
            {
                checkStatusMedicaments();
                indicator.IdIndicators = 0;
                indicator.DataTime = DateTime.Now.AddHours(2).ToString("yyyy.MM.ddTHH:mm:ss");
                db.Indicators.Add(indicator);
                db.SaveChangesAsync();
            }
            catch
            {
                return false;
            }

            return true;
        }

        private void checkStatusMedicaments()
        {
            var fridge = (from Fridge in db.Fridges
                          select new
                          {
                              Fridge.IdFridge,
                              Fridge.IdUser,
                              Fridge.Status,
                              Fridge.Indicators
                          });

            List<Fridge_f> resultFridge = new List<Fridge_f>() { };

            foreach (var c in fridge)
            {
                resultFridge.Add(new Fridge_f
                {
                    IdFridge = c.IdFridge,
                    IdUser = c.IdUser,
                    Status = c.Status
                });
            }

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
                              }).Where(m => (m.Status == true));


            decimal? himid = 0, temperat = 0;
            for (int i = 0; i < resultFridge.Count(); i++)
            {
                var indicator = (from Indicator in db.Indicators
                                 select new
                                 {
                                     Indicator.IdIndicators,
                                     Indicator.IdFridge,
                                     Indicator.Temperature,
                                     Indicator.Humidity,
                                     Indicator.DataTime
                                 });

                int countHum = 0, countTemp = 0;
                foreach (var c in indicator)
                    if (c.IdFridge == resultFridge[i].IdFridge)
                    {
                        if (Convert.ToDateTime(c.DataTime) >= DateTime.Now.AddHours(-2))
                        {
                            himid += c.Humidity;
                            countHum++;
                        }
                        if (Convert.ToDateTime(c.DataTime) >= DateTime.Now.AddHours(-6))
                        {
                            temperat += c.Temperature;
                            countTemp++;
                        }
                    }

                if (countHum != 0 && countTemp != 0)
                {
                    himid /= countHum;
                    temperat /= countTemp;

                    List<Medicaments> resultMed = new List<Medicaments>() { };

                    foreach (var c in medicament)
                        if ((c.IdFridge == resultFridge[i].IdFridge) && ((c.ExpirationDate.Date <= DateTime.Now.AddHours(2)) || ((c.MaxTemperature + 5) <= temperat) || ((c.MinTemperature - 5) >= temperat)))
                        {
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
                                Status = false
                            });
                        }

                    foreach (var medic in resultMed)
                    {
                        db.Entry(medic).State = EntityState.Modified;

                        try
                        {
                            db.SaveChanges();
                        }
                        catch { }
                    }

                    Fridges frid = new Fridges() { IdFridge = resultFridge[i].IdFridge, IdUser = resultFridge[i].IdUser, Status = true };

                    if ((himid >= 85) && (temperat >= 15))
                        frid.Status = false;

                    db.Entry(frid).State = EntityState.Modified;

                    try
                    {
                        db.SaveChanges();
                    }
                    catch { }
                }
            }
        }
    }
}