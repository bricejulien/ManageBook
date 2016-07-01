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
    public class EntryController : Controller
    {
        //private ManageBookContext db = new ManageBookContext();

        // GET: Entry
        public ActionResult Index(int? id)
        {
            using(var db = new ApplicationDbContext())
            {
                ViewBag.Users = db.Users.ToList();
            }
            using(var db = new ManageBookContext())
            {
                ViewBag.Projects = db.Projects.ToList();
                return View(db.Entries.ToList());
            }
        }

        // GET: Entry/Details/5
        public ActionResult Details(int? id)
        {
            using (var db = new ManageBookContext())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Entry entry = db.Entries.Find(id);
                ViewBag.Projects = db.Projects.ToList();
                if (entry == null)
                {
                    return HttpNotFound();
                }
                return View(entry);
            }
        }

        // GET: Entry/Create
        public ActionResult Create()
        {
            using(var db = new ApplicationDbContext())
            {
                ViewBag.Users = db.Users.ToList();
            }
            using (var db = new ManageBookContext())
            {
                ViewBag.Projects = db.Projects.ToList();
            }
            return View();
        }

        // POST: Entry/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Priority,Description,Information,UserId,Date,InvoicableHours,ActualHours,Invoiced,ProjectId")] Entry entry)
        {
            if (ModelState.IsValid)
            {
                using (var db = new ManageBookContext())
                {
                    db.Entries.Add(entry);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            return View(entry);
        }

        // GET: Entry/Edit/5
        public ActionResult Edit(int? id)
        {
            using (var db = new ManageBookContext())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Entry entry = db.Entries.Find(id);
                if (entry == null)
                {
                    return HttpNotFound();
                }
                return View(entry);
            }
        }

        // POST: Entry/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Priority,Description,Information,UserId,Date,InvoicableHours,ActualHours,Invoiced,ProjectId")] Entry entry)
        {
            if (ModelState.IsValid)
            {
                using (var db = new ManageBookContext())
                {
                    db.Entry(entry).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return View(entry);
        }

        // GET: Entry/Delete/5
        public ActionResult Delete(int? id)
        {
            using (var db = new ManageBookContext())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Entry entry = db.Entries.Find(id);
                if (entry == null)
                {
                    return HttpNotFound();
                }
                return View(entry);
            } 
        }

        // POST: Entry/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            using (var db = new ManageBookContext())
            {
                Entry entry = db.Entries.Find(id);
                db.Entries.Remove(entry);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Filter(string project, string user)
        {

            if((project == null || project.Equals("")) && (user == null || user.Equals("")))
            {
                return RedirectToAction("Index");
            }
            //else if(project == null && user != null)
            //{

            //}
            else
            {
                if(!project.Equals("") && user.Equals(""))
                {
                    var projectid = Int32.Parse(project);

                    using (var db = new ApplicationDbContext())
                    {
                        ViewBag.Users = db.Users.ToList();
                    }

                    using (var db = new ManageBookContext())
                    {
                        ViewBag.Projects = db.Projects.ToList();
                        List<Entry> entries = db.Entries.Where(x => x.ProjectId == projectid).ToList();
                        if (entries == null)
                        {
                            return HttpNotFound();
                        }
                        return View("Index", entries);
                    }
                }
                else if (project.Equals("") && !user.Equals(""))
                {
                    using (var db = new ApplicationDbContext())
                    {
                        ViewBag.Users = db.Users.ToList();
                    }

                    using (var db = new ManageBookContext())
                    {
                        ViewBag.Projects = db.Projects.ToList();
                        List<Entry> entries = db.Entries.Where(x => x.UserId == user).ToList();
                        if (entries == null)
                        {
                            return HttpNotFound();
                        }
                        return View("Index", entries);
                    }
                }
                else
                {
                    var projectid = Int32.Parse(project);

                    using (var db = new ApplicationDbContext())
                    {
                        ViewBag.Users = db.Users.ToList();
                    }

                    using (var db = new ManageBookContext())
                    {
                        ViewBag.Projects = db.Projects.ToList();
                        List<Entry> entries = db.Entries.Where(x => x.ProjectId == projectid && x.UserId == user).ToList();
                        if (entries == null)
                        {
                            return HttpNotFound();
                        }
                        return View("Index", entries);
                    }
                }
            }
        }

        //public ActionResult Filter(int? id)
        //{
        //    using (var db = new ManageBookContext())
        //    {
        //        if (id == null)
        //        {
        //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //        }
        //        List<Entry> entries = db.Entries.Where(x => x.ProjectId == id).ToList();
        //        if (entries == null)
        //        {
        //            return HttpNotFound();
        //        }
        //        return View("Index", entries);
        //    }
        //}

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
