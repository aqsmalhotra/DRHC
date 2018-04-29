using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using FAQCheckpoint2.Models;


namespace FAQCheckpoint2.Controllers
{
    public class MakeAppointmentController : Controller
    {
        private HospitalContext db = new HospitalContext();

        // GET: Contact_messages
        [Authorize(Roles = "admin,staff")]
        public ActionResult Admin()
        {
            try
            {
                List<Appointments> appointments = db.appointments.ToList();

                return View(appointments);
            }
            catch (SqlException sqlException)
            {
                ViewBag.ErrorMessage = sqlException.Message;

            }
            catch (Exception genericException)
            {
                ViewBag.ErrorMessage += " " + genericException.Message;
            }
            return RedirectToAction("Index");        
        }

        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Department_id = new SelectList(db.Departments, "Id", "Name");

            return View();
        }

        [HttpPost]
        public ActionResult Index([Bind(Include = "Fullname,Email,Phone,Date,Department_id,Message")]Appointments appt)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.appointments.Add(appt);
                    db.SaveChanges();
                    TempData["Thankyou"] = "Your appointment has been successfully saved. Thank you!";
                    return RedirectToAction("Index");
                }
                ViewBag.Department_id = db.Departments.ToList();
                return View(appt);
            }
            catch (Exception genericException)
            {
                TempData["ExceptionMessage"] = genericException.Message;
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "admin,staff")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Admin");
            }

            try
            {
                Appointments appointment = db.appointments.Find(id);
                return View(appointment);
            }
            catch (Exception genericException)
            {
                TempData["ExceptionMessage"] = genericException.Message;
            }

            return RedirectToAction("Admin");
        }

        [HttpGet]
        [Authorize(Roles = "admin,staff")]

        public ActionResult Update(int? id)//contactmessages/update/{id}
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            try
            {
                Appointments appointment = db.appointments.Find(id);
                if (appointment == null)
                {
                    //return HttpNotFound();
                    RedirectToAction("Admin");
                }
                ViewBag.dept = db.Departments.ToList();
                return View(appointment);
            }
            catch (Exception genericException)
            {
                TempData["ExceptionMessage"] = genericException.Message;
            }
            return RedirectToAction("Admin");
        }

        [HttpPost]
        [Authorize(Roles = "admin,staff")]
        public ActionResult Update(Appointments appt)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(appt).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Admin");             
                }
                ViewBag.dept = db.Departments.ToList();
                return View(appt);
            }
            catch (Exception genericException)
            {
                TempData["ExceptionMessage"] = genericException.Message;
            }


            return RedirectToAction("Admin");
        }

        [HttpGet]
        [Authorize(Roles = "admin,staff")]
        public ActionResult Delete(int? id)
        {
            if (id==null)
            {
                return RedirectToAction("Admin");
            }

            try
            {
                Appointments appointment = db.appointments.Find(id);
                return View(appointment);
            }
            catch (Exception genericException)
            {
                TempData["ExceptionMessage"] = genericException.Message;
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        [Authorize(Roles = "admin,staff")]    
        public ActionResult Delete(int id)
        {
            try
            {
                Appointments appointment = db.appointments.Find(id);
                db.appointments.Remove(appointment);
                db.SaveChanges();
                return RedirectToAction("Admin");
            }
            catch (SqlException sqlException)
            {
                TempData["SqlExceptionMessage"] = sqlException.Message;
            }
            catch (Exception genericException)
            {
                TempData["ExceptionMessage"] = genericException.Message;
            }

            return RedirectToAction("Delete", new { id = id });
        }

        [HttpPost]
        [Authorize(Roles = "admin,staff")]    
        public ActionResult Search_By_Name(FormCollection frm) //search by question in Admin Page
        {
            try
            {
                string search = frm["search"];
                List<Appointments> appointments;
                if (search == "")
                {
                    appointments = db.appointments.ToList();
                }
                else
                {
                    appointments = db.appointments.Where(q => q.Fullname.ToLower().Contains(search.ToLower())).ToList();              
                }
                return PartialView("~/Views/MakeAppointment/_Search_By_Name.cshtml", appointments);
            }
            catch (Exception e)
            {

                ViewBag.ExceptionMessage = e.Message;
            }
            return View("~/Views/Shared/_Error.cshtml");
        }

    }
}