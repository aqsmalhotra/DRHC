using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FAQCheckpoint2.Models;

namespace FAQCheckpoint2.Controllers
{
    public class EventsController : Controller
    {
        private aqsmalhotraEntities db = new aqsmalhotraEntities();

        // GET: Events
        public ActionResult Index()
        {
            return View(db.Events.ToList());
        }

        // GET: Events
        [Authorize(Roles = "admin,staff")]
        public ActionResult Admin()
        {
            return View(db.Events.ToList());
        }

        // GET: Events/Details/5
        [Authorize(Roles = "admin,staff")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                RedirectToAction("Admin");
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                //return HttpNotFound();
                RedirectToAction("Admin");
            }
            return View(@event);
        }

        // GET: Events/Create
        [Authorize(Roles = "admin,staff")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ev_id,ev_title,ev_description,ev_start_date,ev_end_date")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Events.Add(@event);
                db.SaveChanges();
                return RedirectToAction("Admin");
            }

            return View(@event);
        }

        // GET: Events/Edit/5
        [Authorize(Roles = "admin,staff")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                RedirectToAction("Admin");
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                //return HttpNotFound();
                RedirectToAction("Admin");
            }
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,staff")]
        public ActionResult Edit([Bind(Include = "ev_id,ev_title,ev_description,ev_start_date,ev_end_date")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Entry(@event).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Admin");
            }
            return View(@event);
        }

        // GET: Events/Delete/5
        [Authorize(Roles = "admin,staff")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                RedirectToAction("Admin");
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                //return HttpNotFound();
                RedirectToAction("Admin");
            }
            return View(@event);
        }

        // POST: Events/Delete/5
        [Authorize(Roles = "admin,staff")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Event @event = db.Events.Find(id);
            db.Events.Remove(@event);
            db.SaveChanges();
            return RedirectToAction("Admin");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
