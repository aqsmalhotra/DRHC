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
    public class ecardsController : Controller
    {
        private aqsmalhotraEntities db = new aqsmalhotraEntities();

        // GET: ecards
        public ActionResult Index()
        {
            try
            {
                if (Session["role"] != null)
                {
                    var ecards = db.ecards.Include(e => e.card_choices).Include(e => e.person);
                    return View(ecards.ToList());
                }

                return RedirectToAction("Create");
            }
            catch (Exception e)
            {
                ViewBag.errMsg = e.Message;
            }
            return View();
        }


        // GET: ecards/Details/5
        public ActionResult Details(int? id)
        {
            try
            {
                if (Session["role"] != null)
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    ecard ecard = db.ecards.Find(id);
                    if (ecard == null)
                    {
                        return HttpNotFound();
                    }
                    return View(ecard);
                }
                else
                {
                    return RedirectToAction("Create");
                }
            }
            catch (Exception e)
            {
                ViewBag.errMsg = e.Message;
            }
            return View();
        }

        // GET: ecards/Create
        public ActionResult Create()
        {
            try
            {

                person person = db.persons.Find(Convert.ToInt32(Session["id"]));
                Session["firstName"] = person.first_name;
                Session["lastName"] = person.last_name;
                Session["email"] = person.email;
                ViewBag.card_choice = new SelectList(db.card_choices, "id", "choice");
                return View();
            }
            catch (Exception e)
            {
                ViewBag.errMsg = e.Message;
            }
            return View();
        }

        // POST: ecards/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,sender_first_name,sender_last_name,sender_email,person_id,card_choice,card_message")] ecard ecard, string recipFirstName, string recipLastName)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (recipFirstName == "" || recipLastName == "")
                    {
                        ViewBag.lNameError = "Please enter a persons name.";
                    }
                    else
                    {
                        var person = db.persons.Where(p => p.first_name == recipFirstName && p.last_name == recipLastName);
                        if (person == null)
                        {
                            ViewBag.lNameError = "There are no people from the hospital with this name.";
                        }
                        foreach (var p in person)
                        {
                            ecard.person_id = p.id;
                        }

                        db.ecards.Add(ecard);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                ViewBag.lNameError = "There are no people from the hospital with this name.";
                ViewBag.card_choice = new SelectList(db.card_choices, "id", "choice");
                return View(ecard);
            }
            catch (Exception e)
            {
                ViewBag.errMsg = e.Message;
            }

            ViewBag.card_choice = new SelectList(db.card_choices, "id", "choice", ecard.card_choice);
            return View(ecard);
        }

        // GET: ecards/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                if (Convert.ToInt32(Session["role"]) == 1)
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    ecard ecard = db.ecards.Find(id);
                    if (ecard == null)
                    {
                        return HttpNotFound();
                    }
                    ViewBag.card_choice = new SelectList(db.card_choices, "id", "choice", ecard.card_choice);
                    ViewBag.person_id = new SelectList(db.persons, "id", "first_name", ecard.person_id);
                    return View(ecard);
                }
                else if (Session["role"] != null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("../home/Login");
                }
            }
            catch (Exception e)
            {
                ViewBag.errMsg = e.Message;
            }
            return View();
        }

        // POST: ecards/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,sender_first_name,sender_last_name,sender_email,person_id,card_choice,card_message")] ecard ecard)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(ecard).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.card_choice = new SelectList(db.card_choices, "id", "choice", ecard.card_choice);
                return View(ecard);
            }
            catch (Exception e)
            {
                ViewBag.errMsg = e.Message;
            }
            return View();
        }

        // GET: ecards/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {
                if (Session["role"] != null)
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    ecard ecard = db.ecards.Find(id);
                    if (ecard == null)
                    {
                        return HttpNotFound();
                    }
                    if (Convert.ToInt32(Session["role"]) != 1)
                    {
                        if (ecard.person_id != Convert.ToInt32(Session["id"]))
                        {
                            return RedirectToAction("Index");
                        }
                    }
                    return View(ecard);
                }
                else
                {
                    return RedirectToAction("Create");
                }
            }
            catch (Exception e)
            {
                ViewBag.errMsg = e.Message;
            }
            return View();
        }

        // POST: ecards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                ecard ecard = db.ecards.Find(id);
                if (Convert.ToInt32(Session["role"]) != 1 && ecard.person_id != Convert.ToInt32(Session["id"]))
                {
                    return RedirectToAction("Create");
                }
                db.ecards.Remove(ecard);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ViewBag.errMsg = e.Message;
            }
            return View();
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
