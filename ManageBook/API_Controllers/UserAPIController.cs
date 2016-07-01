using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ManageBook.Models;

namespace ManageBook.API_Controllers
{
    public class UserAPIController : ApiController
    {
        //private ManageBookContext db = new ManageBookContext();

        // GET: api/UserAPI
        public IQueryable<ApplicationUser> GetApplicationUsers()
        {
            using(var db = new ApplicationDbContext())
            {
                return db.Users;
            }
        }

        [HttpGet]
        public IHttpActionResult AllUsers()
        {
            //using (var db = new ApplicationDbContext())
            //{
            //    return Ok(db.Users.ToList());
            //}

            using (var db = new ApplicationDbContext())
            {
                List<UserModel> listOfUsers = new List<UserModel>();
                
                foreach (var user in db.Users)
                {
                    UserModel userModel = new UserModel();
                    userModel.Id = user.Id;
                    userModel.UserName = user.UserName;
                    listOfUsers.Add(userModel);
                }
                List<UserModel> users = listOfUsers;

                return Ok(users);
            }
        }

        // GET: api/UserAPI/5
        [ResponseType(typeof(ApplicationUser))]
        public IHttpActionResult GetApplicationUser(string id)
        {
            UserModel user = new UserModel();
            using(var db = new ApplicationDbContext())
            {
                user.Id = id;
                user.UserName = db.Users.Find(id).UserName;

                return Ok(user);
            }
        }

        // PUT: api/UserAPI/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutApplicationUser(string id, ApplicationUser applicationUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != applicationUser.Id)
            {
                return BadRequest();
            }

            using (var db = new ApplicationDbContext())
            {
                db.Entry(applicationUser).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationUserExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/UserAPI
        [ResponseType(typeof(ApplicationUser))]
        public IHttpActionResult PostApplicationUser(ApplicationUser applicationUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (var db = new ApplicationDbContext())
            {
                db.Users.Add(applicationUser);

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    if (ApplicationUserExists(applicationUser.Id))
                    {
                        return Conflict();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = applicationUser.Id }, applicationUser);
        }

        // DELETE: api/UserAPI/5
        [ResponseType(typeof(ApplicationUser))]
        public IHttpActionResult DeleteApplicationUser(string id)
        {
            using(var db = new ApplicationDbContext())
            {
                ApplicationUser applicationUser = db.Users.Find(id);
                if (applicationUser == null)
                {
                    return NotFound();
                }

                db.Users.Remove(applicationUser);
                db.SaveChanges();

                return Ok(applicationUser);
            }
        }

        /*protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }*/

        private bool ApplicationUserExists(string id)
        {
            using(var db = new ApplicationDbContext())
            {
                return db.Users.Count(e => e.Id == id) > 0;
            }
        }
    }
}