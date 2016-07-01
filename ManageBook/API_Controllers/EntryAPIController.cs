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
    public class EntryAPIController : ApiController
    {
        //private ManageBookContext db = new ManageBookContext();

        // GET: api/EntryAPI
        public IQueryable<Entry> GetEntries()
        {
            using(var db = new ManageBookContext())
            {
                return db.Entries;
            }
        }

        // GET: api/EntryAPI/AllEntries
        [HttpGet]
        public IHttpActionResult AllEntries(int? projectid)
        {
            //using (var db = new ManageBookContext())
            //{
            //    return Ok(db.Entries.ToList());
            //}
            using (var db = new ManageBookContext())
            {
                if (projectid == null)
                {
                    return Ok(db.Entries.ToList());
                }
                else
                {
                    return Ok(db.Entries.Where(x => x.ProjectId == projectid).ToList());
                }

            }
        }

        // GET: api/EntryAPI/5
        [ResponseType(typeof(Entry))]
        public IHttpActionResult GetEntry(int id)
        {
            using(var db = new ManageBookContext())
            {
                Entry entry = db.Entries.Find(id);
                if (entry == null)
                {
                    return NotFound();
                }

                return Ok(entry);
            }
        }

        // PUT: api/EntryAPI/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEntry(int id, Entry entry)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != entry.Id)
            {
                return BadRequest();
            }

            using(var db = new ManageBookContext())
            {
                db.Entry(entry).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EntryExists(id))
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

        // POST: api/EntryAPI
        [ResponseType(typeof(Entry))]
        public IHttpActionResult PostEntry(Entry entry)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using(var db = new ManageBookContext())
            {
                db.Entries.Add(entry);
                db.SaveChanges();
            }

            return CreatedAtRoute("DefaultApi", new { id = entry.Id }, entry);
        }

        // DELETE: api/EntryAPI/5
        [ResponseType(typeof(Entry))]
        public IHttpActionResult DeleteEntry(int id)
        {
            using(var db = new ManageBookContext())
            {
                Entry entry = db.Entries.Find(id);
                if (entry == null)
                {
                    return NotFound();
                }

                db.Entries.Remove(entry);
                db.SaveChanges();

                return Ok(entry);
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

        private bool EntryExists(int id)
        {
            using(var db = new ManageBookContext())
            {
                return db.Entries.Count(e => e.Id == id) > 0;
            }
        }

        [HttpGet]
        public IHttpActionResult Filter(string project, string user, DateTime startDate, DateTime endDate,
            bool priorityA, bool priorityB, bool priorityC, bool priorityD, bool priorityE)
        {
            var filteredEntries = new List<Entry>();
            if ((project == null || project.Equals("undefined")) && (user == null || user.Equals("undefined")))
            {
                using (var db = new ManageBookContext())
                {
                    filteredEntries = db.Entries.ToList();
                }
            }
            else
            {
                if (!project.Equals("undefined") && user.Equals("undefined"))
                {
                    var projectid = Int32.Parse(project);

                    using (var db = new ManageBookContext())
                    {
                        filteredEntries = db.Entries.Where(x => x.ProjectId == projectid).ToList();
                    }
                }
                else if (project.Equals("undefined") && !user.Equals("undefined"))
                {
                    using (var db = new ManageBookContext())
                    {
                        filteredEntries = db.Entries.Where(x => x.UserId == user).ToList();
                    }
                }
                else
                {
                    var projectid = Int32.Parse(project);

                    using (var db = new ManageBookContext())
                    {
                        filteredEntries = db.Entries.Where(x => x.ProjectId == projectid && x.UserId == user).ToList();
                    }
                }
            }

            using (var db = new ManageBookContext())
            {
                //filteredEntries = db.Entries.Where(x => x.Date>=startDate && x.Date<=endDate).ToList();
                filteredEntries = filteredEntries.Where(x => x.Date >= startDate && x.Date <= endDate).ToList();
            }

            if(!priorityA)
            {
                filteredEntries = filteredEntries.Where(x => x.Priority!=0).ToList();
            }
            if (!priorityB)
            {
                filteredEntries = filteredEntries.Where(x => x.Priority != 1).ToList();
            }
            if (!priorityC)
            {
                filteredEntries = filteredEntries.Where(x => x.Priority != 2).ToList();
            }
            if (!priorityD)
            {
                filteredEntries = filteredEntries.Where(x => x.Priority != 3).ToList();
            }
            if (!priorityE)
            {
                filteredEntries = filteredEntries.Where(x => x.Priority != 4).ToList();
            }

            if (filteredEntries.Count() == 0)
            {
                return InternalServerError();
            }
            return Ok(filteredEntries);
        }
    }
}