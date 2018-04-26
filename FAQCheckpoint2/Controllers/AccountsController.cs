using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FAQCheckpoint2.Models;
using HospitalProject.LIB;
using FAQCheckpoint2.LIBs;
using System.Web.Security;

namespace FAQCheckpoint2.Controllers
{

    public class AccountsController : Controller
    {
        private HospitalContext db = new HospitalContext();
        private Categories ca = new Categories();

        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {

                    try
                    {
                        return View(db.Accounts.ToList());
                    }
                    catch (Exception e)
                    {

                        ViewBag.ExceptionMessage = e.Message;
                    }
                    return View("~/Views/Errors/Details.cshtml");

            
        }

        [Authorize(Roles = "admin")]
        public ActionResult Details(int? id)
        {

                    try
                    {
                        if (id == null)
                        {
                            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                        }
                        Accounts accounts = db.Accounts.Find(id);
                        if (accounts == null)
                        {
                            return HttpNotFound();
                        }
                        return View(accounts);
                    }
                    catch (Exception e)
                    {

                        ViewBag.ExceptionMessage = e.Message;
                    }
                    return View("~/Views/Errors/Details.cshtml");

        }

        public ActionResult Search_By_User(FormCollection frm) //search by user in Admin Page
        {
            try
            {
                string search = frm["search"];
                List<Accounts> a = new List<Accounts>();
                a = db.Accounts.Where(acc => acc.username.ToLower().Contains(search.ToLower())).ToList();
                return PartialView("~/Views/Accounts/_Search_User_Results.cshtml", a);
            }
            catch (Exception e)
            {

                ViewBag.ExceptionMessage = e.Message;
            }
            return View("~/Views/Errors/Details.cshtml");
        }


        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    if(User.IsInRole("admin"))
                    {
                        return RedirectToAction("Index","Accounts");
                    }
                    else
                    {
                        return RedirectToAction("Admin","FAQs");
                    }
                }
                else
                {
                    return View();
                }
            }
            catch (Exception e)
            {

                ViewBag.ExceptionMessage = e.Message;
            }
            return View("~/Views/Errors/Details.cshtml");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Accounts users)
        {
            try
            {
                users.password = HashPass.SHA512(users.password);
                var user = db.Accounts.Where(u => u.username == users.username && u.password == users.password).FirstOrDefault();
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(users.username, false);
                    Session["id"] = user.id;
                    string url = Request.QueryString["ReturnUrl"];
                    if(url!= null && Url.IsLocalUrl(url))
                    {
                        return Redirect(url);
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                    
                }
                else
                {

                    ModelState.AddModelError("", "Authentication failed. Please try again!");
                }
                return View();

            }
            catch (Exception e)
            {

                ViewBag.ExceptionMessage = e.Message;
            }
            return View("~/Views/Errors/Details.cshtml");
        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public ActionResult Create()
        {

                    try
                    {
                        ViewBag.Roles = ca.User_Roles();
                        return View();
                    }
                    catch (Exception e)
                    {

                        ViewBag.ExceptionMessage = e.Message;
                    }
                    return View("~/Views/Errors/Details.cshtml");
                  
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,fullname,username,password,email,role")] Accounts accounts)
        {
            try
            {
                ViewBag.Roles = ca.User_Roles();
                if (ModelState.IsValid)
                {
                    accounts.password = HashPass.SHA512(accounts.password);
                    db.Accounts.Add(accounts);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(accounts);
            }
            catch (Exception e)
            {

                ViewBag.ExceptionMessage = e.Message;
            }
            return View("~/Views/Errors/Details.cshtml");
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public ActionResult Edit(int? id)
        {

                    try
                    {
                        ViewBag.Roles = ca.User_Roles();
                        if (id == null)
                        {
                            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                        }
                        Accounts accounts = db.Accounts.Find(id);
                        if (accounts == null)
                        {
                            return HttpNotFound();
                        }
                        return View(accounts);
                    }
                    catch (Exception e)
                    {

                        ViewBag.ExceptionMessage = e.Message;
                    }
                    return View("~/Views/Errors/Details.cshtml");

                   
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,fullname,username,password,email,role")] Accounts accounts)
        {
            try
            {
                ViewBag.Roles = ca.User_Roles();
                if (ModelState.IsValid)
                {
                    accounts.password = HashPass.SHA512(accounts.password);
                    db.Entry(accounts).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(accounts);
            }
            catch (Exception e)
            {

                ViewBag.ExceptionMessage = e.Message;
            }
            return View("~/Views/Errors/Details.cshtml");
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public ActionResult Delete(int? id)
        {

                    try
                    {
                        if (id == null)
                        {
                            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                        }
                        Accounts accounts = db.Accounts.Find(id);
                        if (accounts == null)
                        {
                            return HttpNotFound();
                        }
                        return View(accounts);
                    }
                    catch (Exception e)
                    {

                        ViewBag.ExceptionMessage = e.Message;
                    }
                    return View("~/Views/Errors/Details.cshtml");
                   
        }

        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Accounts accounts = db.Accounts.Find(id);
                db.Accounts.Remove(accounts);
                db.SaveChanges();
                return RedirectToAction("Index");
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
