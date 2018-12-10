using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MedicalFridgeServer.Models;

namespace MedicalFridgeServer.Controllers
{
    public class FridgesController : ApiController
    {
        private MedicalFridgeDBEntities2 db = new MedicalFridgeDBEntities2();

        // GET: api/Fridges
        public HttpResponseMessage GetFridges()
        {
            var fridge = (from Fridge in db.Fridges
                          select new
                          {
                              Fridge.IdFridge,
                              Fridge.IdUser,
                              Fridge.SizeFridge.Size
                          });

            return GetInfo(fridge);
        }

        // GET: api/Fridges/id
        public HttpResponseMessage GetFridge(int id)
        {
            var fridge = (from Fridge in db.Fridges
                          select new
                          {
                              Fridge.IdFridge,
                              Fridge.IdUser,
                              Fridge.SizeFridge.Size
                          }).Where(f => f.IdUser == id);

            return GetInfo(fridge);
        }

        // PUT: api/Fridges/id
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
        public bool DeleteFridge(int id)
        {
            Fridge fridge = db.Fridges.Find(id);

            if (fridge == null)
                return false;

            db.Fridges.Remove(fridge);
            db.SaveChanges();

            return true;
        }

        private HttpResponseMessage GetInfo(IQueryable f)
        {
            try
            {
                if (db.Fridges.Count() != 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, f);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NoContent, "Fridges list is empty");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}