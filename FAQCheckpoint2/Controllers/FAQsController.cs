using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FAQCheckpoint2.Models;
using FAQCheckpoint2.LIBs;
using HospitalProject.LIB;

namespace FAQCheckpoint2.Controllers
{
    public class FAQsController : Controller
    {
        private HospitalContext db = new HospitalContext();
        private Categories ca = new Categories();

        [AllowAnonymous]
        public ActionResult Index()
        {
            try
            {
                ViewBag.services = ca.Services();
                return View(db.FAQS.ToList());
            }
            catch (Exception e)
            {

                ViewBag.ExceptionMessage = e.Message;
            }
            return View("~/Views/Errors/Details.cshtml");
        }

        [HttpPost]
        public ActionResult Search_By_Category(string category) //search by category
        {
            try
            {
                List<FAQ> f = new List<FAQ>();
                f = db.FAQS.Where(c => c.Faq_category.ToLower().Contains(category.ToLower())).ToList();
                return PartialView("~/Views/FAQs/_Search_Results.cshtml", f);
            }
            catch (Exception e)
            {

                ViewBag.ExceptionMessage = e.Message;
            }
            return View("~/Views/Errors/Details.cshtml");
        }

        [HttpPost]
        public ActionResult Search_By_Question(FormCollection frm) //search by question
        {
            try
            {
                string search = frm["search"];
                List<FAQ> f = new List<FAQ>();
                f = db.FAQS.Where(q => q.Faq_question.ToLower().Contains(search.ToLower())).ToList();
                return PartialView("~/Views/FAQs/_Search_Results.cshtml", f);
            }
            catch (Exception e)
            {

                ViewBag.ExceptionMessage = e.Message;
            }
            return View("~/Views/Errors/Details.cshtml");
        }

        [HttpPost]
        public ActionResult Search_By_Question_Admin(FormCollection frm) //search by question in Admin Page
        {
            try
            {
                string search = frm["search"];
                List<FAQ> f = new List<FAQ>();
                f = db.FAQS.Where(q => q.Faq_question.ToLower().Contains(search.ToLower())).ToList();
                return PartialView("~/Views/FAQs/_Search_Admin_Results.cshtml", f);
            }
            catch (Exception e)
            {

                ViewBag.ExceptionMessage = e.Message;
            }
            return View("~/Views/Errors/Details.cshtml");
        }

        [Authorize(Roles = "admin,staff")]
        public ActionResult Admin()
        {
            try
            {
                ViewBag.services = ca.Services();
                return View(db.FAQS.ToList());
            }
            catch (Exception e)
            {

                ViewBag.ExceptionMessage = e.Message;
            }
            return View("~/Views/Errors/Details.cshtml");
        }

        [Authorize(Roles = "admin,staff")]
        public ActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                FAQ fAQ = db.FAQS.Find(id);
                if (fAQ == null)
                {
                    return HttpNotFound();
                }
                return View(fAQ);
            }
            catch (Exception e)
            {

                ViewBag.ExceptionMessage = e.Message;
            }
            return View("~/Views/Errors/Details.cshtml");
        }



        [Authorize(Roles = "admin,staff")]
        [HttpGet]
        public ActionResult Create()
        {
            try
            {
                ViewBag.services = ca.Services();
                return View();
            }
            catch (Exception e)
            {

                ViewBag.ExceptionMessage = e.Message;
            }
            return View("~/Views/Errors/Details.cshtml");
        }

        [Authorize(Roles = "admin,staff")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Faq_id,Faq_question,Faq_answer,Faq_last_update,Faq_author,Faq_category")] FAQ fAQ)
        {
            try
            {
                ViewBag.services = ca.Services();
                if (ModelState.IsValid)
                {

                    db.FAQS.Add(fAQ);
                    db.SaveChanges();
                    return RedirectToAction("Admin");
                }

                return View(fAQ);
            }
            catch (Exception e)
            {

                ViewBag.ExceptionMessage = e.Message;
            }
            return View("~/Views/Errors/Details.cshtml");
        }

        [Authorize(Roles = "admin,staff")]
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ViewBag.services = ca.Services();
                FAQ fAQ = db.FAQS.Find(id);
                if (fAQ == null)
                {
                    return HttpNotFound();
                }
                return View(fAQ);
            }
            catch (Exception e)
            {

                ViewBag.ExceptionMessage = e.Message;
            }
            return View("~/Views/Errors/Details.cshtml");
        }

        [Authorize(Roles = "admin,staff")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Faq_id,Faq_question,Faq_answer,Faq_last_update,Faq_author,Faq_category")] FAQ fAQ)
        {
            try
            {
                ViewBag.services = ca.Services();
                if (ModelState.IsValid)
                {
                    db.Entry(fAQ).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Admin");
                }
                return View(fAQ);
            }
            catch (Exception e)
            {

                ViewBag.ExceptionMessage = e.Message;
            }
            return View("~/Views/Errors/Details.cshtml");
        }

        [Authorize(Roles = "admin,staff")]
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                FAQ fAQ = db.FAQS.Find(id);
                if (fAQ == null)
                {
                    return HttpNotFound();
                }
                return View(fAQ);
            }
            catch (Exception e)
            {

                ViewBag.ExceptionMessage = e.Message;
            }
            return View("~/Views/Errors/Details.cshtml");
        }

        [Authorize(Roles = "admin,staff")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                FAQ fAQ = db.FAQS.Find(id);
                db.FAQS.Remove(fAQ);
                db.SaveChanges();
                return RedirectToAction("Admin");
            }
            catch (Exception e)
            {

                ViewBag.ExceptionMessage = e.Message;
            }
            return View("~/Views/Errors/Details.cshtml");
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
