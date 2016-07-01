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
    public class ProjectAPIController : ApiController
    {
        //private ManageBookContext db = new ManageBookContext();

        // GET: api/ProjectAPI
        public IQueryable<Project> GetProjects()
        {
            using(var db = new ManageBookContext())
            {
                return db.Projects;
            }
        }

        // GET: api/ProjectAPI/AllProjects
        [HttpGet]
        public IHttpActionResult AllProjects()
        {
            using (var db = new ManageBookContext())
            {
                return Ok(db.Projects.ToList());
            }
        }
        
        // GET: api/ProjectAPI/5
        [ResponseType(typeof(Project))]
        public IHttpActionResult GetProject(int id)
        {
            using(var db = new ManageBookContext())
            {
                Project project = db.Projects.Find(id);
                if (project == null)
                {
                    return NotFound();
                }

                return Ok(project);
            }
        }

        // PUT: api/ProjectAPI/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProject(int id, Project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != project.Id)
            {
                return BadRequest();
            }

            using(var db = new ManageBookContext())
            {
                db.Entry(project).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(id))
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

        // POST: api/ProjectAPI
        [ResponseType(typeof(Project))]
        public IHttpActionResult PostProject(Project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using(var db = new ManageBookContext())
            {
                db.Projects.Add(project);
                db.SaveChanges();
            }

            return CreatedAtRoute("DefaultApi", new { id = project.Id }, project);
        }

        // DELETE: api/ProjectAPI/5
        [ResponseType(typeof(Project))]
        public IHttpActionResult DeleteProject(int id)
        {
            using(var db = new ManageBookContext())
            {
                Project project = db.Projects.Find(id);
                if (project == null)
                {
                    return NotFound();
                }

                db.Projects.Remove(project);
                db.SaveChanges();

                return Ok(project);
            }
        }
        /*
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }*/

        private bool ProjectExists(int id)
        {
            using(var db = new ManageBookContext())
            {
                return db.Projects.Count(e => e.Id == id) > 0;
            }
        }
    }
}