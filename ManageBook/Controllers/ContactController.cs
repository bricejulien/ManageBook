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
    public class ContactController : Controller
    {
        //private ManageBookContext db = new ManageBookContext();

        // GET: Contact
        public ActionResult Index()
        {
            using(var db = new ManageBookContext())
            {
                var contacts = db.Contacts.Include(c => c.Project);
                return View(contacts.ToList());
            }
        }

        // GET: Contact/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using(var db = new ManageBookContext())
            {
                Contact contact = db.Contacts.Find(id);
                if (contact == null)
                {
                    return HttpNotFound();
                }
                return View(contact);
            }
        }

        // GET: Contact/Create
        public ActionResult Create()
        {
            //using(var db = new ManageBookContext())
            //{
            //    ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
            //    return View();
            //}
            return View();
        }

        // POST: Contact/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Note,ProjectId")] Contact contact)
        {
            using(var db = new ManageBookContext())
            {
                if (ModelState.IsValid)
                {
                    db.Contacts.Add(contact);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", contact.ProjectId);
                return View(contact);
            }
        }

        // GET: Contact/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using(var db = new ManageBookContext())
            {
                Contact contact = db.Contacts.Find(id);
                if (contact == null)
                {
                    return HttpNotFound();
                }
                ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", contact.ProjectId);
                return View(contact);
            }
        }

        // POST: Contact/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Note,ProjectId")] Contact contact)
        {
            using(var db = new ManageBookContext())
            {
                if (ModelState.IsValid)
                {
                    db.Entry(contact).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", contact.ProjectId);
                return View(contact);
            }
        }

        // GET: Contact/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using(var db = new ManageBookContext())
            {
                Contact contact = db.Contacts.Find(id);
                if (contact == null)
                {
                    return HttpNotFound();
                }
                return View(contact);
            }
        }

        // POST: Contact/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            using(var db = new ManageBookContext())
            {
                Contact contact = db.Contacts.Find(id);
                db.Contacts.Remove(contact);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
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
