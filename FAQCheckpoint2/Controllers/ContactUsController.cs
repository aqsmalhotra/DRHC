using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using FAQCheckpoint2.Models;


namespace FAQCheckpoint2.Controllers
{
    public class ContactUsController : Controller
    {
        private HospitalContext db = new HospitalContext();

        // GET: Contact_messages
        [Authorize]
        public ActionResult Admin()
        {
            try
            {
                List<Contact_messages> messages = db.contactMessages.ToList();

                return View(messages);
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
            return View();
        }

        [HttpPost]
        public ActionResult Index(Contact_messages msg)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.contactMessages.Add(msg);
                    db.SaveChanges();
                    TempData["Thankyou"] = "Your message has been successfully sent. Thank you!";
                    return RedirectToAction("Index");
                }
                return View(msg);
            }
            catch (Exception genericException)
            {
                TempData["ExceptionMessage"] = genericException.Message;
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            try
            {
                Contact_messages message = db.contactMessages.Find(id);
                return View(message);
            }
            catch (Exception genericException)
            {
                TempData["ExceptionMessage"] = genericException.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize]

        public ActionResult Update(int? id)//contactmessages/update/{id}
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            try
            {
                Contact_messages message = db.contactMessages.Find(id);
                return View(message);
            }
            catch (Exception genericException)
            {
                TempData["ExceptionMessage"] = genericException.Message;
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize]
        public ActionResult Update(Contact_messages msg)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(msg).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (Exception genericException)
            {
                TempData["ExceptionMessage"] = genericException.Message;
            }


            return RedirectToAction("List");
        }

        [HttpGet]
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id==null)
            {
                return RedirectToAction("List");
            }

            try
            {
                Contact_messages message = db.contactMessages.Find(id);
                return View(message);
            }
            catch (Exception genericException)
            {
                TempData["ExceptionMessage"] = genericException.Message;
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        [Authorize]    
        public ActionResult Delete(int id)
        {
            try
            {
                Contact_messages message = db.contactMessages.Find(id);
                db.contactMessages.Remove(message);
                db.SaveChanges();
                return RedirectToAction("List");
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
        [Authorize]    
        public ActionResult Search_By_Name_Ajax(FormCollection frm) //search by question in Admin Page
        {
            try
            {
                string search = frm["search"];
                List<Contact_messages> messages;
                if (search == "")
                {
                    messages = db.contactMessages.ToList();
                }
                else
                {
                    messages = db.contactMessages.Where(q => q.Name.ToLower().Contains(search.ToLower())).ToList();              
                }
                return PartialView("~/Views/ContactUs/_Search_By_Name_Ajax.cshtml", messages);
            }
            catch (Exception e)
            {

                ViewBag.ExceptionMessage = e.Message;
            }
            return View("~/Views/Shared/_Error.cshtml");
        }

    }
}