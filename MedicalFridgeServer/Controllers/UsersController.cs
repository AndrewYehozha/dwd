using System;
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
    public class UsersController : System.Web.Http.ApiController
    {
        private MedicalFridgeDBEntities2 db = new MedicalFridgeDBEntities2();

        // GET: api/Users
        [HttpGet]
        public IEnumerable<User_u> GetUsers()
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

            List<User_u> result = new List<User_u>() { };

            foreach (var c in users)
                result.Add(new User_u
                {
                    IdUser = c.IdUser,
                    Login = c.Login.Trim(),
                    Password = c.Password.Trim(),
                    Role = c.Role.Trim(),
                    NameOrganization = c.NameOrganization.Trim(),
                    Phone = c.Phone.Trim(),
                    Country = c.Country.Trim(),
                    City = c.City.Trim(),
                    Address = c.Address.Trim()
                });

            return result;
        }

        // GET: api/Users/id
        [HttpGet]
        public IEnumerable<User_u> GetUser(int id)
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

            List<User_u> result = new List<User_u>() { };

            foreach (var c in user)
                result.Add(new User_u
                {
                    IdUser = c.IdUser,
                    Login = c.Login.Trim(),
                    Password = c.Password.Trim(),
                    Role = c.Role.Trim(),
                    NameOrganization = c.NameOrganization.Trim(),
                    Phone = c.Phone.Trim(),
                    Country = c.Country.Trim(),
                    City = c.City.Trim(),
                    Address = c.Address.Trim()
                });

            return result;
        }

        // GET: api/Users/value1/value2
        [HttpGet]
        public IEnumerable<User_u> GetUserPass(string value1, string value2)
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

            List<User_u> result = new List<User_u>() { };

            foreach (var c in user)
                result.Add(new User_u
                {
                    IdUser = c.IdUser,
                    Login = c.Login.Trim(),
                    Password = c.Password.Trim(),
                    Role = c.Role.Trim(),
                    NameOrganization = c.NameOrganization.Trim(),
                    Phone = c.Phone.Trim(),
                    Country = c.Country.Trim(),
                    City = c.City.Trim(),
                    Address = c.Address.Trim()
                });

            return result;
        }

        // PUT: api/Users/id
        [HttpPut]
        public bool PutUser(int id, Users user)
        {
            if (id != user.IdUser)
                return false;

            var c = (from User in db.Users
                     select new { User.Login, User.IdUser }
                     ).Where(i => (i.IdUser != id) && (i.Login.Trim() == user.Login.Trim()));

            if (c.Count() == 0)
                db.Entry(user).State = EntityState.Modified;
            else
                return false;

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
        [HttpPost]
        public bool PostUser(Users user)
        {
            var c = (from User in db.Users
                     select new { User.Login }
                     ).Where(i => (i.Login.Trim() == user.Login.Trim()));

            if (c.Count() == 0)
            {
                user.IdUser = 0;
                db.Users.Add(user);
                db.SaveChanges();
            }
            else
                return false;

            return true;
        }

        // DELETE: api/Users/id
        [HttpDelete]
        public bool DeleteUser(int id)
        {
            Users user = db.Users.Find(id);

            if (user == null)
                return false;

            db.Users.Remove(user);
            db.SaveChanges();

            return true;
        }
    }
}