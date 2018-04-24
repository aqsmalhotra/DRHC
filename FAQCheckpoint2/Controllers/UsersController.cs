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
    public class UsersController : Controller
    {
        private aqsmalhotraEntities db = new aqsmalhotraEntities();

        // GET: Users
        public ActionResult Index()
        {

            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            try
            {
                int count = db.Users.Where(u => u.Username == user.Username && u.Password == user.Password).Count();
                if (count == 1)
                {
                    var user1 = db.Users.Where(u => u.Username == user.Username);
                    foreach (var u in user1)
                    {
                        Session["username"] = u.Username;
                        Session["role"] = u.Role_id;
                        Session["id"] = u.person_id;

                    }

                    return RedirectToAction("../");
                }
                else
                {
                    ViewBag.errMsg = "Incorrect Username/Password combination.";
                }
            }
            catch (Exception e)
            {
                ViewBag.errMsg = e.Message;
            }
            return View(user);
        }

        public ActionResult Logout()
        {
            try
            {
                Session.Clear();
                return RedirectToAction("Login");
            }
            catch (Exception e)
            {
                ViewBag.errMsg = e.Message;
            }
            return View(); // CHANGE TO ERR MSG PAGE
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
