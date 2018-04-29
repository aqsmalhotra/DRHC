using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FAQCheckpoint2.Models;
using System.Data.SqlClient;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;

namespace FAQCheckpoint2.Controllers
{
    public class EventsController : Controller
    {
        private aqsmalhotraEntities db = new aqsmalhotraEntities();

        // GET: Events
        public ActionResult Index()
        {
            try
            {
                return View(db.Events.ToList());
            }
            catch (DbUpdateException dbException)
            {
                ViewBag.DbExceptionMessage = dbException.Message;
                return View("~/Views/Errors/Details.cshtml");
            }
            catch (EntityException entityException)
            {
                ViewBag.EntityExceptionMessage = entityException.InnerException.Message;
                return View("~/Views/Errors/Details.cshtml");
            }
            catch (SqlException sqlException)
            {
                ViewBag.SqlExceptionNumber = sqlException.Number;
                ViewBag.SqlExceptionMessage = sqlException.Message;
                return View("~/Views/Errors/Details.cshtml");
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("~/Views/Errors/Details.cshtml");
            }
        }

        // GET: Events
        [Authorize(Roles = "admin,staff")]
        public ActionResult Admin()
        {
            try
            {
                return View(db.Events.ToList());
            }
            catch (DbUpdateException dbException)
            {
                ViewBag.DbExceptionMessage = dbException.Message;
                return View("~/Views/Errors/Details.cshtml");
            }
            catch (EntityException entityException)
            {
                ViewBag.EntityExceptionMessage = entityException.InnerException.Message;
                return View("~/Views/Errors/Details.cshtml");
            }
            catch (SqlException sqlException)
            {
                ViewBag.SqlExceptionNumber = sqlException.Number;
                ViewBag.SqlExceptionMessage = sqlException.Message;
                return View("~/Views/Errors/Details.cshtml");
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("~/Views/Errors/Details.cshtml");
            }
        }

        // GET: Events/Details/5
        [Authorize(Roles = "admin,staff")]
        public ActionResult Details(int? id)
        {
            try
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
            catch (DbUpdateException dbException)
            {
                ViewBag.DbExceptionMessage = dbException.Message;
                return View("~/Views/Errors/Details.cshtml");
            }
            catch (EntityException entityException)
            {
                ViewBag.EntityExceptionMessage = entityException.InnerException.Message;
                return View("~/Views/Errors/Details.cshtml");
            }
            catch (SqlException sqlException)
            {
                ViewBag.SqlExceptionNumber = sqlException.Number;
                ViewBag.SqlExceptionMessage = sqlException.Message;
                return View("~/Views/Errors/Details.cshtml");
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("~/Views/Errors/Details.cshtml");
            }
        }

        // GET: Events/Create
        [Authorize(Roles = "admin,staff")]
        public ActionResult Create()
        {
            try
            {
                return View();
            }
            catch (DbUpdateException dbException)
            {
                ViewBag.DbExceptionMessage = dbException.Message;
                return View("~/Views/Errors/Details.cshtml");
            }
            catch (EntityException entityException)
            {
                ViewBag.EntityExceptionMessage = entityException.InnerException.Message;
                return View("~/Views/Errors/Details.cshtml");
            }
            catch (SqlException sqlException)
            {
                ViewBag.SqlExceptionNumber = sqlException.Number;
                ViewBag.SqlExceptionMessage = sqlException.Message;
                return View("~/Views/Errors/Details.cshtml");
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("~/Views/Errors/Details.cshtml");
            }
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ev_id,ev_title,ev_description,ev_start_date,ev_end_date")] Event @event)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Events.Add(@event);
                    db.SaveChanges();
                    return RedirectToAction("Admin");
                }

                return View(@event);
            }
            catch (DbUpdateException dbException)
            {
                ViewBag.DbExceptionMessage = dbException.Message;
                return View("~/Views/Errors/Details.cshtml");
            }
            catch (EntityException entityException)
            {
                ViewBag.EntityExceptionMessage = entityException.InnerException.Message;
                return View("~/Views/Errors/Details.cshtml");
            }
            catch (SqlException sqlException)
            {
                ViewBag.SqlExceptionNumber = sqlException.Number;
                ViewBag.SqlExceptionMessage = sqlException.Message;
                return View("~/Views/Errors/Details.cshtml");
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("~/Views/Errors/Details.cshtml");
            }
        }

        // GET: Events/Edit/5
        [Authorize(Roles = "admin,staff")]
        public ActionResult Edit(int? id)
        {
            try
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
            catch (DbUpdateException dbException)
            {
                ViewBag.DbExceptionMessage = dbException.Message;
                return View("~/Views/Errors/Details.cshtml");
            }
            catch (EntityException entityException)
            {
                ViewBag.EntityExceptionMessage = entityException.InnerException.Message;
                return View("~/Views/Errors/Details.cshtml");
            }
            catch (SqlException sqlException)
            {
                ViewBag.SqlExceptionNumber = sqlException.Number;
                ViewBag.SqlExceptionMessage = sqlException.Message;
                return View("~/Views/Errors/Details.cshtml");
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("~/Views/Errors/Details.cshtml");
            }
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,staff")]
        public ActionResult Edit([Bind(Include = "ev_id,ev_title,ev_description,ev_start_date,ev_end_date")] Event @event)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(@event).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Admin");
                }
                return View(@event);
            }
            catch (DbUpdateException dbException)
            {
                ViewBag.DbExceptionMessage = dbException.Message;
                return View("~/Views/Errors/Details.cshtml");
            }
            catch (EntityException entityException)
            {
                ViewBag.EntityExceptionMessage = entityException.InnerException.Message;
                return View("~/Views/Errors/Details.cshtml");
            }
            catch (SqlException sqlException)
            {
                ViewBag.SqlExceptionNumber = sqlException.Number;
                ViewBag.SqlExceptionMessage = sqlException.Message;
                return View("~/Views/Errors/Details.cshtml");
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("~/Views/Errors/Details.cshtml");
            }
        }

        // GET: Events/Delete/5
        [Authorize(Roles = "admin,staff")]
        public ActionResult Delete(int? id)
        {
            try
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
            catch (DbUpdateException dbException)
            {
                ViewBag.DbExceptionMessage = dbException.Message;
                return View("~/Views/Errors/Details.cshtml");
            }
            catch (EntityException entityException)
            {
                ViewBag.EntityExceptionMessage = entityException.InnerException.Message;
                return View("~/Views/Errors/Details.cshtml");
            }
            catch (SqlException sqlException)
            {
                ViewBag.SqlExceptionNumber = sqlException.Number;
                ViewBag.SqlExceptionMessage = sqlException.Message;
                return View("~/Views/Errors/Details.cshtml");
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("~/Views/Errors/Details.cshtml");
            }
        }

        // POST: Events/Delete/5
        [Authorize(Roles = "admin,staff")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Event @event = db.Events.Find(id);
                db.Events.Remove(@event);
                db.SaveChanges();
                return RedirectToAction("Admin");
            }
            catch (DbUpdateException dbException)
            {
                ViewBag.DbExceptionMessage = dbException.Message;
                return View("~/Views/Errors/Details.cshtml");
            }
            catch (EntityException entityException)
            {
                ViewBag.EntityExceptionMessage = entityException.InnerException.Message;
                return View("~/Views/Errors/Details.cshtml");
            }
            catch (SqlException sqlException)
            {
                ViewBag.SqlExceptionNumber = sqlException.Number;
                ViewBag.SqlExceptionMessage = sqlException.Message;
                return View("~/Views/Errors/Details.cshtml");
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("~/Views/Errors/Details.cshtml");
            }
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
