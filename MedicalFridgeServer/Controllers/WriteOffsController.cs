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
using System.Web.Http.Cors;
using System.Web.Http.Description;
using MedicalFridgeServer.Models;

namespace MedicalFridgeServer.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class WriteOffsController : ApiController
    {
        private MedicalFridgeDBEntities db = new MedicalFridgeDBEntities();

        // GET: api/WriteOffs
        public HttpResponseMessage GetWriteOffs()
        {
            var writeOff = (from WriteOff in db.WriteOffs
                            select new
                            {
                                WriteOff.IdMedicament,
                                WriteOff.IdWriteOff,
                                WriteOff.Medicament.Name,
                                WriteOff.Amount,
                                WriteOff.Medicament.DataProduction,
                                WriteOff.DataWriteOff,
                                WriteOff.Price,
                                WriteOff.Medicament.Information
                            });

            return GetInfo(writeOff);
        }

        // GET: api/WriteOffs/id
        public HttpResponseMessage GetWriteOff(int id)
        {
            var writeOff = (from WriteOff in db.WriteOffs
                            select new
                            {
                                WriteOff.IdMedicament,
                                WriteOff.IdWriteOff,
                                WriteOff.Medicament.Fridge.IdFridge,
                                WriteOff.Medicament.Name,
                                WriteOff.Amount,
                                WriteOff.Medicament.DataProduction,
                                WriteOff.DataWriteOff,
                                WriteOff.Price,
                                WriteOff.Medicament.Information
                            }).Where(w => w.IdFridge == id);

            return GetInfo(writeOff);
        }

        // PUT: api/WriteOffs/id
        public bool PutWriteOff(int id, WriteOff writeOff)
        {
            if (id != writeOff.IdWriteOff)
                return false;

            db.Entry(writeOff).State = EntityState.Modified;

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

        // POST: api/WriteOffs
        public bool PostWriteOff(WriteOff writeOff)
        {
            try
            {
                writeOff.IdWriteOff = 0;
                db.WriteOffs.Add(writeOff);
                db.SaveChanges();
            }
            catch
            {
                return false;
            }

            return true;
        }

        // POST: api/WriteOffs/?value={value}
        public HttpResponseMessage SearchWriteOffs(string value)
        {
            string res = value.Trim();

            var result = (from WriteOff in db.WriteOffs
                          where ((WriteOff.Amount.ToString().Trim().Contains(res)) || (WriteOff.Medicament.Name.Contains(res))
                                  || (WriteOff.IdWriteOff.ToString().Trim().Contains(res)) || (WriteOff.Price.ToString().Trim().Contains(res))
                                  || (WriteOff.Medicament.Information.Contains(res)) || (WriteOff.Medicament.DataProduction.ToString().Trim().Contains(res))
                                  || (WriteOff.DataWriteOff.ToString().Trim().Contains(res)))
                          select new
                          {
                              WriteOff.IdMedicament,
                              WriteOff.IdWriteOff,
                              WriteOff.Medicament.Name,
                              WriteOff.Amount,
                              WriteOff.Medicament.DataProduction,
                              WriteOff.DataWriteOff,
                              WriteOff.Price,
                              WriteOff.Medicament.Information
                          });

            return GetInfo(result);
        }

        // DELETE: api/WriteOffs/id
        public bool DeleteWriteOff(int id)
        {
            WriteOff writeOff = db.WriteOffs.Find(id);

            if (writeOff == null)
                return false;

            db.WriteOffs.Remove(writeOff);
            db.SaveChanges();

            return true;
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