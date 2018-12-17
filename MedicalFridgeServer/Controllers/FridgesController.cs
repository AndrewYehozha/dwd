﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using MedicalFridgeServer.Classes;
using MedicalFridgeServer.Models;

namespace MedicalFridgeServer.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class FridgesController : ApiController
    {
        private MedicalFridgeDBEntities db = new MedicalFridgeDBEntities();

        // GET: api/Fridges
        [HttpGet]
        public IEnumerable<Fridge_f> GetFridges()
        {
            var fridge = (from Fridge in db.Fridges
                          select new
                          {
                              Fridge.IdFridge,
                              Fridge.IdUser,
                              Fridge.Indicators
                          });

            List<Fridge_f> result = new List<Fridge_f>() { };

            foreach (var c in fridge)
                result.Add(new Fridge_f
                {
                    IdFridge = c.IdFridge,
                    IdUser = c.IdUser,
                    LastTemperature = c.Indicators.Last().Temperature,
                    LastHumidity = c.Indicators.Last().Humidity
                });

            return result;
        }

        // GET: api/Fridges/id
        [HttpGet]
        public IEnumerable<Fridge_f> GetFridge(int id)
        {
            var fridge = (from Fridge in db.Fridges
                          select new
                          {
                              Fridge.IdFridge,
                              Fridge.IdUser,
                              Fridge.Indicators
                          }).Where(f => f.IdUser == id);

            List<Fridge_f> result = new List<Fridge_f>() { };

            foreach (var c in fridge)
                result.Add(new Fridge_f
                {
                    IdFridge = c.IdFridge,
                    IdUser = c.IdUser,
                    LastTemperature = c.Indicators.Last().Temperature,
                    LastHumidity = c.Indicators.Last().Humidity
                });

            return result;
        }

        // PUT: api/Fridges/id
        [HttpPut]
        public bool PutFridge(int id, Fridge fridge)
        {
            if (id != fridge.IdFridge)
                return false;

            db.Entry(fridge).State = EntityState.Modified;

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

        // POST: api/Fridges
        [HttpPost]
        public bool PostFridge(Fridge fridge)
        {
            try
            {
                fridge.IdFridge = 0;
                db.Fridges.Add(fridge);
                db.SaveChanges();
            }
            catch
            {
                return false;
            }

            return true;
        }

        // DELETE: api/Fridges/id
        [HttpDelete]
        public bool DeleteFridge(int id)
        {
            Fridge fridge = db.Fridges.Find(id);

            if (fridge == null)
                return false;

            db.Fridges.Remove(fridge);
            db.SaveChanges();

            return true;
        }
    }
}