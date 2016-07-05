using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ManageBook.Models;
using ManageBookModels;

namespace ManageBook.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        //private ManageBookContext db = new ManageBookContext();

        // GET: Project
        public ActionResult Index()
        {
            using (var db = new ApplicationDbContext())
            {
                ViewBag.Users = db.Users.ToList();
            }
            using (var db = new ManageBookContext())
            {
                return View(db.Projects.ToList());
            }
        }

        // GET: Project/Details/5
        public ActionResult Details(int? id)
        {
            using (var db = new ManageBookContext())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Project project = db.Projects.Find(id);
                if (project == null)
                {
                    return HttpNotFound();
                }
                return View(project);
            }

            //using (var db = new ApplicationDbContext())
            //{
            //    ViewBag.Users = db.Users.ToList();
            //}
            //using (var db = new ManageBookContext())
            //{
            //    ViewBag.Projects = db.Projects.ToList();
            //    return View(db.Entries.Where(x => x.ProjectId==id).ToList());
            //}
        }

        // GET: Project/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Project/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Invoice,ExpectedHours,ActualHours,ContactPerson,PhoneNumber,QuickbookId,Retainer,Actions,Contacts,Priority,Rate")] Project project)
        {
            if (ModelState.IsValid)
            {
                using (var db = new ManageBookContext())
                {
                    db.Projects.Add(project);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: Project/Edit/5
        public ActionResult Edit(int? id)
        {
            using (var db = new ManageBookContext())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Project project = db.Projects.Find(id);
                if (project == null)
                {
                    return HttpNotFound();
                }
                return View(project);
            }
        }

        // POST: Project/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Invoice,ExpectedHours,ActualHours,ContactPerson,PhoneNumber,QuickbookId,Retainer,Actions,Contacts,Priority,Rate")] Project project)
        {
            if (ModelState.IsValid)
            {
                using (var db = new ManageBookContext())
                {
                    db.Entry(project).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        // GET: Project/Delete/5
        public ActionResult Delete(int? id)
        {
            using (var db = new ManageBookContext())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Project project = db.Projects.Find(id);
                if (project == null)
                {
                    return HttpNotFound();
                }
                return View(project);
            }
        }

        // POST: Project/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            using (var db = new ManageBookContext())
            {
                Project project = db.Projects.Find(id);
                db.Projects.Remove(project);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Filter(int? id)
        {
            using (var db = new ManageBookContext())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                List<Entry> entries = db.Entries.Where(x => x.ProjectId == id).ToList();
                if (entries == null)
                {
                    return HttpNotFound();
                }
                return View(entries);
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
    }
}
