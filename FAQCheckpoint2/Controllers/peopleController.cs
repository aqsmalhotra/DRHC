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
    public class peopleController : Controller
    {
        private aqsmalhotraEntities db = new aqsmalhotraEntities();

        

        // GET: people
        public ActionResult Index()
        {
            return View(db.persons.ToList());
        }

        // GET: people/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            person person = db.persons.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // GET: people/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: people/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,first_name,middle_name,last_name,email,gender,health_card")] person person, string usernameInput, string passwordInput, string passwordConfirmInput)
        {
            if (ModelState.IsValid)
            {
                if (usernameInput != "" && passwordInput != "" && passwordConfirmInput != "")
                {
                    if (passwordInput == passwordConfirmInput)
                    {
                        db.persons.Add(person);
                        db.SaveChanges();

                        User user = new Models.User();
                        var p = db.persons.Where(a => a.first_name == person.first_name && a.middle_name == person.middle_name && a.last_name == person.last_name && a.email == person.email && a.gender == person.gender && a.health_card == person.health_card);
                        foreach (var per in p)
                        {
                            user.person_id = per.id;
                        }
                        user.Username = usernameInput;
                        user.Password = passwordInput;
                        user.Role_id = 3;
                        db.Users.Add(user);

                        Session["role"] = user.Role_id;
                        Session["username"] = user.Username;
                        Session["id"] = user.person_id;
                        return RedirectToAction("Details/"+user.person_id.ToString(), user.person_id);
                    }
                    ViewBag.passwordErrorMsg = "Passwords must match.";
                    return View(person);
                }
                ViewBag.ErrorMsg = "Must fill in all required fields";
                return View(person);
            }

            return View(person);
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
