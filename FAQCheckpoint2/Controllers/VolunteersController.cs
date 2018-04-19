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
using System.IO;

namespace FAQCheckpoint2.Controllers
{
    public class VolunteersController : Controller
    {
        private HospitalContext db = new HospitalContext();
        private Categories ca = new Categories();

        [Authorize(Roles = "admin,staff")]
        public ActionResult Admin()
        {
            try
            {
                return View(db.Volunteers.ToList());
            }
            catch (Exception e)
            {

                ViewBag.ExceptionMessage = e.Message;
            }
            return View("~/Views/Errors/Details.cshtml");
        }

        [Authorize(Roles = "admin,staff")]
        public ActionResult Search_By_Name(FormCollection frm) //search by name in Admin Page
        {
            try
            {
                string search = frm["search"];
                List<Volunteers> v = new List<Volunteers>();
                v = db.Volunteers.Where(n => n.volunteer_name.ToLower().Contains(search.ToLower())).ToList();
                return PartialView("~/Views/Volunteers/_Search_By_Name.cshtml", v);
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
                Volunteers volunteers = db.Volunteers.Find(id);
                if (volunteers == null)
                {
                    return HttpNotFound();
                }
                return View(volunteers);
            }
            catch (Exception e)
            {

                ViewBag.ExceptionMessage = e.Message;
            }
            return View("~/Views/Errors/Details.cshtml");

        }

        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                ViewBag.Shifts = ca.Shifts();
                ViewBag.Interests = ca.Areas_Of_Interest();
                return View();
            }
            catch (Exception e)
            {

                ViewBag.ExceptionMessage = e.Message;
            }
            return View("~/Views/Errors/Details.cshtml");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(HttpPostedFileBase volunteer_cv, [Bind(Include = "volunteer_id,volunteer_name,volunteer_dob,volunteer_email,volunteer_phone,volunteer_interests,volunteer_shifts,volunteer_message,volunteer_cv")] Volunteers volunteers)
        {
            try
            {
                //load list of check box items
                ViewBag.Shifts = ca.Shifts();
                ViewBag.Interests = ca.Areas_Of_Interest();

                if (volunteer_cv != null)
                {
                    var file_ext = System.IO.Path.GetExtension(volunteer_cv.FileName).Substring(1);
                    if (ca.file_extensions.Contains(file_ext) == true)
                    {
                        var resume = Path.GetFileName(volunteer_cv.FileName);
                        volunteers.volunteer_cv = resume;
                        //move file to server and save
                        var cv_path = Path.Combine(Server.MapPath("~/resumes/"), resume);
                        volunteer_cv.SaveAs(cv_path);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "File upload is not allowed. Please try again!");
                    }
                }


                if (ModelState.IsValid)
                {


                    //save to DB
                    db.Volunteers.Add(volunteers);
                    db.SaveChanges();
                    ViewBag.err_msg = "Thanks for submiting your application. We will contact you soon!";
                    //return RedirectToAction("Index");


                }

                return View(volunteers);
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
                Volunteers volunteers = db.Volunteers.Find(id);
                if (volunteers == null)
                {
                    return HttpNotFound();
                }
                return View(volunteers);
            }
            catch (Exception e)
            {

                ViewBag.ExceptionMessage = e.Message;
            }
            return View("~/Views/Errors/Details.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HttpPostedFileBase volunteer_cv, [Bind(Include = "volunteer_id,volunteer_name,volunteer_dob,volunteer_email,volunteer_phone,volunteer_interests,volunteer_shifts,volunteer_message,volunteer_cv")] Volunteers volunteers)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(volunteers).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Admin");
                }
                return View(volunteers);
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
                Volunteers volunteers = db.Volunteers.Find(id);
                if (volunteers == null)
                {
                    return HttpNotFound();
                }
                return View(volunteers);
            }
            catch (Exception e)
            {

                ViewBag.ExceptionMessage = e.Message;
            }
            return View("~/Views/Errors/Details.cshtml");
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Volunteers volunteers = db.Volunteers.Find(id);
                db.Volunteers.Remove(volunteers);
                if (volunteers.volunteer_cv != null)
                {
                    string resume_path = Request.MapPath("~/resumes/" + volunteers.volunteer_cv);
                    System.IO.File.Delete(resume_path); //delete file from server
                }

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
