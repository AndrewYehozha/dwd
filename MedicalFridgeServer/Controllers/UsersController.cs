using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using MedicalFridgeServer.Models;

namespace MedicalFridgeServer.Controllers
{
    public class UsersController : System.Web.Http.ApiController
    {
        private MedicalFridgeDBEntities2 db = new MedicalFridgeDBEntities2();

        // GET: api/Users
        public HttpResponseMessage GetUsers()
        {
            var users = (from User in db.Users
                         select new
                         {
                             User.IdUser,
                             User.Login,
                             User.Password,
                             User.Role,
                             User.NameOrganization,
                             User.Phone,
                             User.Country,
                             User.City,
                             User.Address
                         });

            return GetInfo(users);
        }

        // GET: api/Users/id
        public HttpResponseMessage GetUser(int id)
        {
            var user = (from User in db.Users
                        select new
                        {
                            User.IdUser,
                            User.Login,
                            User.Password,
                            User.Role,
                            User.NameOrganization,
                            User.Phone,
                            User.Country,
                            User.City,
                            User.Address
                        }).Where(u => u.IdUser == id);

            return GetInfo(user);
        }

        // GET: api/Users/value1/value2
        public HttpResponseMessage GetUserPass(string value1, string value2)
        {
            var user = (from User in db.Users
                        select new
                        {
                            User.IdUser,
                            User.Login,
                            User.Password,
                            User.Role,
                            User.NameOrganization,
                            User.Phone,
                            User.Country,
                            User.City,
                            User.Address
                        }).Where(i => (i.Login.Trim() == value1.Trim()) && (i.Password.Trim() == value2.Trim()));

            return GetInfo(user);
        }

        // PUT: api/Users/id
        public bool PutUser(int id, User user)
        {
            if (id != user.IdUser)
                return false;

            db.Entry(user).State = EntityState.Modified;

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

        // POST: api/Users
        public bool PostUser(User user)
        {
            var c = (from User in db.Users
                     select new { User.Login }
                     ).Where(i => (i.Login.Trim() == user.Login.Trim()));

            if (c.Count() == 0)
            {
                db.Users.Add(user);
                db.SaveChanges();
            }
            else
                return false;

            return true;
        }

        // DELETE: api/Users/id
        public bool DeleteUser(int id)
        {
            User user = db.Users.Find(id);

            if (user == null)
                return false;

            db.Users.Remove(user);
            db.SaveChanges();

            return true;
        }

        private HttpResponseMessage GetInfo(IQueryable u)
        {
            try
            {
                if (db.Users.Count() != 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, u);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "User list is empty");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}