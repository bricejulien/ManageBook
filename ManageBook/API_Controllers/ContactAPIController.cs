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
using ManageBookModels;

namespace ManageBook.API_Controllers
{
    public class ContactAPIController : ApiController
    {
        //private ManageBookContext db = new ManageBookContext();

        // GET: api/ContactAPI
        public IQueryable<Contact> GetContacts()
        {
            using(var db = new ManageBookContext())
            {
                return db.Contacts;
            }
        }

        // GET: api/ContactAPI/AllContacts
        [HttpGet]
        public IHttpActionResult AllContacts(int? projectid)
        {
            using (var db = new ManageBookContext())
            {
                if (projectid == null)
                {
                    return Ok(db.Contacts.ToList());
                }
                else
                {
                    return Ok(db.Contacts.Where(x => x.ProjectId == projectid).ToList());
                }

            }
        }

        // GET: api/ContactAPI/5
        [ResponseType(typeof(Contact))]
        public IHttpActionResult GetContact(int id)
        {
            using (var db = new ManageBookContext())
            {
                Contact contact = db.Contacts.Find(id);
                if (contact == null)
                {
                    return NotFound();
                }

                return Ok(contact);
            }
        }

        // PUT: api/ContactAPI/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutContact(int id, Contact contact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != contact.Id)
            {
                return BadRequest();
            }

            using (var db = new ManageBookContext())
            {
                db.Entry(contact).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactExists(id))
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

        // POST: api/ContactAPI
        [ResponseType(typeof(Contact))]
        public IHttpActionResult PostContact(Contact contact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (var db = new ManageBookContext())
            {
                db.Contacts.Add(contact);
                db.SaveChanges();
            }

            return CreatedAtRoute("DefaultApi", new { id = contact.Id }, contact);
        }

        // DELETE: api/ContactAPI/5
        [ResponseType(typeof(Contact))]
        public IHttpActionResult DeleteContact(int id)
        {
            using (var db = new ManageBookContext())
            {
                Contact contact = db.Contacts.Find(id);
                if (contact == null)
                {
                    return NotFound();
                }

                db.Contacts.Remove(contact);
                db.SaveChanges();

                return Ok(contact);
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

        private bool ContactExists(int id)
        {
            using(var db = new ManageBookContext())
            {
                return db.Contacts.Count(e => e.Id == id) > 0;
            }
        }
    }
}