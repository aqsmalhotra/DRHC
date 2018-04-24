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
    public class Job_PostingsController : Controller
    {
        private aqsmalhotraEntities db = new aqsmalhotraEntities();

        // GET: Job_Postings
        public ActionResult Index()
        {
            var job_Postings = db.Job_Postings.Include(j => j.Department);
            return View(job_Postings.ToList());
        }

        // GET: Job_Postings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job_Postings job_Postings = db.Job_Postings.Find(id);
            if (job_Postings == null)
            {
                return HttpNotFound();
            }
            return View(job_Postings);
        }

        // GET: Job_Postings/Create
        public ActionResult Create()
        {
            ViewBag.dept = new SelectList(db.Departments, "Id", "Name");
            return View();
        }

        // POST: Job_Postings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "job_id,job_title,job_description,job_type,job_openings,job_posted_date,job_closing_date,dept")] Job_Postings job_Postings)
        {
            if (ModelState.IsValid)
            {
                db.Job_Postings.Add(job_Postings);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.dept = new SelectList(db.Departments, "Id", "Name", job_Postings.dept);
            return View(job_Postings);
        }

        // GET: Job_Postings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job_Postings job_Postings = db.Job_Postings.Find(id);
            if (job_Postings == null)
            {
                return HttpNotFound();
            }
            ViewBag.dept = new SelectList(db.Departments, "Id", "Name", job_Postings.dept);
            return View(job_Postings);
        }

        // POST: Job_Postings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "job_id,job_title,job_description,job_type,job_openings,job_posted_date,job_closing_date,dept")] Job_Postings job_Postings)
        {
            if (ModelState.IsValid)
            {
                db.Entry(job_Postings).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.dept = new SelectList(db.Departments, "Id", "Name", job_Postings.dept);
            return View(job_Postings);
        }

        // GET: Job_Postings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job_Postings job_Postings = db.Job_Postings.Find(id);
            if (job_Postings == null)
            {
                return HttpNotFound();
            }
            return View(job_Postings);
        }

        // POST: Job_Postings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Job_Postings job_Postings = db.Job_Postings.Find(id);
            db.Job_Postings.Remove(job_Postings);
            db.SaveChanges();
            return RedirectToAction("Index");
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
