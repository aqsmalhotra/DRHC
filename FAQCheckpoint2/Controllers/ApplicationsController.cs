using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FAQCheckpoint2.Models;
using System.Net.Mail;

namespace FAQCheckpoint2.Controllers
{
    public class ApplicationsController : Controller
    {
        private aqsmalhotraEntities db = new aqsmalhotraEntities();

        // GET: Applications
        [Authorize(Roles = "admin,staff")]
        public ActionResult Index()
        {
            var applications = db.Applications.Include(a => a.Job_Postings);
            return View(applications.ToList());
        }

        public ActionResult Apps(int posting)
        {
            var apps = db.Applications.Where(m => m.job_posting == posting).ToList();
            return View("~/Views/Application/Index.cshtml", apps);
        }

        // GET: Applications/Details/5
        [Authorize(Roles = "admin,staff")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                RedirectToAction("Index");
            }
            Application application = db.Applications.Find(id);
            if (application == null)
            {
                //return HttpNotFound();
                RedirectToAction("Index");
            }
            return View(application);
        }

        // GET: Applications/Create
        public ActionResult Create(int posting)
        {
            ViewBag.posting = posting;
            ViewBag.job_posting = new SelectList(db.Job_Postings, "job_id", "job_title");
            return View();
        }

        // POST: Applications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "app_id,app_first_name,app_last_name,app_address,app_city,app_province,app_postal_code,app_phone,app_email,app_comments,app_submission_date,job_posting")] Application application)
        {
            if (ModelState.IsValid)
            {
                application.app_submission_date = DateTime.Today;
                db.Applications.Add(application);
                db.SaveChanges();

                var fname = application.app_first_name;
                var lname = application.app_last_name;
                var title = db.Job_Postings.Find(application.job_posting).job_title;

                //Send verification email
                var message = "Dear " + fname + " " + lname + ". ";
                message += "This letter is to inform you that we have received your application. ";
                message += "We appreciate your interest in Dryden Regional Health Center and the position of " + title + " for which you applied. ";
                message += "We will be reviewing your application and if you are selected for an interview, you can expect a phone call from us shortly. ";
                message += "Thank you, again, for your interest. We do appreciate the time that you invested in this application.";

                SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                MailMessage myMail = new MailMessage();
                smtp.EnableSsl = true;
                smtp.Port = 587;
                smtp.Credentials = new NetworkCredential("TestahTest123@gmail.com",
                   "MahPass!");
                smtp.Send("TestahTest123@gmail.com", application.app_email,
                   "DRHC Application", message);

                //return RedirectToAction("Index");
                return RedirectToAction("index", "Job_Postings", new { area = "" });
            }

            ViewBag.job_posting = new SelectList(db.Job_Postings, "job_id", "job_title", application.job_posting);
            return View(application);
        }

        // GET: Applications/Edit/5
        [Authorize(Roles = "admin,staff")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                RedirectToAction("Index");
            }
            Application application = db.Applications.Find(id);
            if (application == null)
            {
                //return HttpNotFound();
                RedirectToAction("Index");
            }
            ViewBag.job_posting = new SelectList(db.Job_Postings, "job_id", "job_title", application.job_posting);
            return View(application);
        }

        // POST: Applications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,staff")]
        public ActionResult Edit([Bind(Include = "app_id,app_first_name,app_last_name,app_address,app_city,app_province,app_postal_code,app_phone,app_email,app_comments,app_submission_date,job_posting")] Application application)
        {
            if (ModelState.IsValid)
            {
                db.Entry(application).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.job_posting = new SelectList(db.Job_Postings, "job_id", "job_title", application.job_posting);
            return View(application);
        }

        // GET: Applications/Delete/5
        [Authorize(Roles = "admin,staff")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                RedirectToAction("Index");
            }
            Application application = db.Applications.Find(id);
            if (application == null)
            {
                //return HttpNotFound();
                RedirectToAction("Index");
            }
            return View(application);
        }

        // POST: Applications/Delete/5
        [Authorize(Roles = "admin,staff")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Application application = db.Applications.Find(id);
            db.Applications.Remove(application);
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
