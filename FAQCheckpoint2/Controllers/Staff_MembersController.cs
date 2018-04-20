using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FAQCheckpoint2.Models;


namespace FAQCheckpoint2.Controllers
{
    public class Staff_MembersController : Controller
    {
        private aqsmalhotraEntities db = new aqsmalhotraEntities();

        // GET: Staff_Members
        public ActionResult Index()
        {
            try
            {
                var staff_Members = db.Staff_Members.Include(s => s.Department);
                return View(staff_Members.ToList());
            }
            catch(Exception genericException)
            {
                ViewBag.ExceptionMessage = genericException.Message;
                return View("~/Views/Error/Index.cshtml");
            }
            
            
        }

        // GET: Staff_Members/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                Staff_Members staff_Members = db.Staff_Members.Find(id);
                if (staff_Members == null)
                {
                    return HttpNotFound();
                }
                return View(staff_Members);
            }
            catch(Exception genericException) 
            {
                ViewBag.ExceptionMessage = genericException.Message;
                return View("~/Views/Error/Index.cshtml");
            }
        }

        // GET: Staff_Members/Create
        public ActionResult Create()
        {
            try
            {
                ViewBag.Department_Id = new SelectList(db.Departments, "Id", "Name");
                return View();
            }
            catch (Exception genericException)
            {
                ViewBag.ExceptionMessage = genericException.Message;
                return View("~/Views/Error/Index.cshtml");
            }
        }

        // POST: Staff_Members/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,First_Name,Last_Name,Breaf_Bio,Working_From,Created_At,Image_Url,Department_Id")] Staff_Members staff_Members, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                string fileName = "";

                //If a file has been sent to this action
                if (file != null && file.ContentLength > 0)
                {
                    //Gets the filename of the file
                    fileName = Path.GetFileName(file.FileName);
                }

                //Assigning filename to the model property
                staff_Members.Image_Url = fileName;

                try { 
                    db.Staff_Members.Add(staff_Members);
                    db.SaveChanges();
                    if (fileName != "")
                    {
                        //Find the path in the server to store the images then add in a directory with the custom directory
                        //Locally, this path is different than the eventual path on another server so this follows a path
                        string path = Path.Combine(Server.MapPath("~/Images/Staff_Members/" + staff_Members.Id + "/"));

                        //C# requires you to create the directory. The server responds by creating directories that don't exist on top of the directories that do exist
                        Directory.CreateDirectory(path);

                        path = Path.Combine(Server.MapPath("~/Images/Staff_Members/" + staff_Members.Id + "/"), fileName);
                        file.SaveAs(path);
                    }
                    return RedirectToAction("Index");
                }
                catch (Exception genericException)
                {
                    ViewBag.ExceptionMessage = genericException.Message;
                    return View("~/Views/Error/Index.cshtml");
                }
            }
            try
            {
                ViewBag.Department_Id = new SelectList(db.Departments, "Id", "Name", staff_Members.Department_Id);
                return View(staff_Members);
            }
            catch(Exception genericException)
            {
                ViewBag.ExceptionMessage = genericException.Message;
                return View("~/View/Error/Index.cshtml");
            }
            
            
        }

        // GET: Staff_Members/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                Staff_Members staff_Members = db.Staff_Members.Find(id);
                if (staff_Members == null)
                {
                    return HttpNotFound();
                }
                ViewBag.Department_Id = new SelectList(db.Departments, "Id", "Name", staff_Members.Department_Id);
                return View(staff_Members);
            }
            catch(Exception genericException)
            {
                ViewBag.ExceptionMessage = genericException.Message;
                return View("~/View/Error/Index.cshtml");
            }
            
            
        }

        // POST: Staff_Members/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,First_Name,Last_Name,Breaf_Bio,Working_From,Created_At,Image_Url,Department_Id")] Staff_Members staff_Members, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if(file!=null && file.ContentLength > 0)
                {
                    staff_Members.Image_Url = Path.GetFileName(file.FileName);
                    //Create the path if it didn't exist previously
                    string path = Path.Combine(Server.MapPath("~/Images/Staff_Members/" + staff_Members.Id + "/"));
                    Directory.CreateDirectory(path);

                    DirectoryInfo dirInfo = new DirectoryInfo(Request.MapPath("~/Images/Staff_Members/" + staff_Members.Id));
                    foreach (FileInfo fi in dirInfo.GetFiles())
                    {
                        fi.Delete();
                    }

                    //Upload the new file
                    string pathToUpload = Path.Combine(Server.MapPath("~/Images/Staff_Members/" + staff_Members.Id + "/" + staff_Members.Image_Url));
                    file.SaveAs(pathToUpload);
                }
                try
                {
                    db.Entry(staff_Members).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception genericException)
                {
                    ViewBag.ExceptionMessage = genericException.Message;
                    return View("~/View/Error/Index.cshtml");
                }

            }      
            try
            {
                ViewBag.Department_Id = new SelectList(db.Departments, "Id", "Name", staff_Members.Department_Id);
                return View(staff_Members);
            }
            catch (Exception genericException)
            {
                ViewBag.ExceptionMessage = genericException.Message;
                return View("~/View/Error/Index.cshtml");
            }

        }

        // GET: Staff_Members/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                Staff_Members staff_Members = db.Staff_Members.Find(id);
                if (staff_Members == null)
                {
                    return HttpNotFound();
                }
                return View(staff_Members);

            }
            catch(Exception genericException)
            {
                ViewBag.ExceptionMessage = genericException.Message;
                return View("~/View/Error/Index.cshtml");
            }
        }

        // POST: Staff_Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Staff_Members staff_Members = db.Staff_Members.Find(id);
                db.Staff_Members.Remove(staff_Members);
                db.SaveChanges();
                DirectoryInfo dirInfo = new DirectoryInfo(Request.MapPath("~/Images/Articles/" + staff_Members.Id));
                foreach (FileInfo fi in dirInfo.GetFiles())
                {
                    fi.Delete();
                }
                return RedirectToAction("Index");
            }
            catch(Exception genericException)
            {
                ViewBag.ExceptionMessage = genericException.Message;
                return View("~/View/Error/Index.cshtml");
            }
            
        }

        [Authorize(Roles = "admin")]
        public ActionResult DeleteImage(int id)
        {
            //Get the article object with all its values
            Staff_Members staff_Member = db.Staff_Members.Find(id);

            //Remove the flag_img value
            staff_Member.Image_Url = "";

            //Update the country as you normally would from the Edit action
            try
            {
                db.Entry(staff_Member).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception genericException)
            {
                ViewBag.ExceptionMessage = genericException.Message;
                return View("~/View/Error/Index.cshtml");
            }


            //Remove the image from the directory
            string pathToRemoveFiles = Request.MapPath("~/Images/Staff_Members/" + staff_Member.Id);
            System.IO.DirectoryInfo DirInfo = new DirectoryInfo(pathToRemoveFiles);

            foreach (FileInfo file in DirInfo.GetFiles())
            {
                file.Delete();
            }

            return RedirectToAction("Details", new { id = staff_Member.Id });
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
