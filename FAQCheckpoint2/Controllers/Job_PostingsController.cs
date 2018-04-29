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
    public class Job_PostingsController : Controller
    {
        private aqsmalhotraEntities db = new aqsmalhotraEntities();

        // GET: Job_Postings
        public ActionResult Index()
        {
            try
            {
                var job_Postings = db.Job_Postings.Include(j => j.Department);
                return View(job_Postings.ToList());
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

        // GET: Job_Postings
        [Authorize(Roles = "admin,staff")]
        public ActionResult Admin()
        {
            try
            {
                var job_Postings = db.Job_Postings.Include(j => j.Department);
                return View(job_Postings.ToList());

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

        // GET: Job_Postings/Details/5
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
                Job_Postings job_Postings = db.Job_Postings.Find(id);
                if (job_Postings == null)
                {
                    //return HttpNotFound();
                    RedirectToAction("Admin");
                }
                return View(job_Postings);
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

        // GET: Job_Postings/Create
        [Authorize(Roles = "admin,staff")]
        public ActionResult Create()
        {
            try
            {
                ViewBag.dept = new SelectList(db.Departments, "Id", "Name");
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

        // POST: Job_Postings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,staff")]
        public ActionResult Create([Bind(Include = "job_id,job_title,job_description,job_type,job_openings,job_posted_date,job_closing_date,dept")] Job_Postings job_Postings)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    job_Postings.job_posted_date = DateTime.Today;
                    db.Job_Postings.Add(job_Postings);
                    db.SaveChanges();
                    return RedirectToAction("Admin");
                }

                ViewBag.dept = new SelectList(db.Departments, "Id", "Name", job_Postings.dept);
                return View(job_Postings);
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
                ViewBag.ExceptionMessage = e.Message;
                return View("~/Views/Errors/Details.cshtml");
            }
        }

        // GET: Job_Postings/Edit/5
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
                Job_Postings job_Postings = db.Job_Postings.Find(id);
                if (job_Postings == null)
                {
                    //return HttpNotFound();
                    RedirectToAction("Admin");
                }
                ViewBag.dept = new SelectList(db.Departments, "Id", "Name", job_Postings.dept);
                return View(job_Postings);
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
                ViewBag.ExceptionMessage = e.Message;
                return View("~/Views/Errors/Details.cshtml");
            }
        }

        // POST: Job_Postings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,staff")]
        public ActionResult Edit([Bind(Include = "job_id,job_title,job_description,job_type,job_openings,job_posted_date,job_closing_date,dept")] Job_Postings job_Postings)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(job_Postings).State = EntityState.Modified;

                    db.SaveChanges();
                    return RedirectToAction("Admin");
                }
                ViewBag.dept = new SelectList(db.Departments, "Id", "Name", job_Postings.dept);
                return View(job_Postings);
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
                ViewBag.ExceptionMessage = e.Message;
                return View("~/Views/Errors/Details.cshtml");
            }
        }

        // GET: Job_Postings/Delete/5
        [Authorize(Roles = "admin,staff")]
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    return RedirectToAction("Admin");
                }
                Job_Postings job_Postings = db.Job_Postings.Find(id);
                if (job_Postings == null)
                {
                    //return HttpNotFound();
                    return RedirectToAction("Admin");
                }
                return View(job_Postings);
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
                ViewBag.ExceptionMessage = e.Message;
                return View("~/Views/Errors/Details.cshtml");
            }
        }

        // POST: Job_Postings/Delete/5
        [Authorize(Roles = "admin,staff")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                db.Applications.RemoveRange(db.Applications.Where(m => m.job_posting == id));
                Job_Postings job_Postings = db.Job_Postings.Find(id);
                db.Job_Postings.Remove(job_Postings);
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
                ViewBag.ExceptionMessage = e.Message;
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
