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
    public class ArticlesController : Controller
    {
        private aqsmalhotraEntities db = new aqsmalhotraEntities();

        // GET: Articles
        public ActionResult Index()
        {
            //check if we come from another page, which sent an error message
            if (TempData["error"] != null)
            {
                ViewBag.ErrorMessage = TempData["error"];//the error is sent to the view throgh the viebag
            }
            try
            {
                //a datetime variable from one month ago is created
                DateTime dateminus30 = DateTime.Now.AddDays(-30);
                
                string search = Request.QueryString["search"] ?? "";//we grab the text from the querystring
                ViewBag.Trending = db.Articles.Where(a => a.Timestamp_publiched < DateTime.Now)
                                                .OrderByDescending(a => a.clicks)
                                                .Take(5)
                                                .ToList() ;
                var articles = db.Articles.Where(a => a.Body.Contains(search) ||
                                            a.Title.Contains(search) || a.Category.Name.Contains(search)).OrderByDescending(a => a.Timestamp_publiched).ThenByDescending(a => a.Timestamp_created);
                List<Article> articlesList = new List<Article>();
                if (!(User.IsInRole("admin") || User.IsInRole("staff")))
                {
                    articlesList = articles.Where(a=> a.Timestamp_publiched<DateTime.Now).ToList();
                }
                else
                {
                    articlesList = articles.ToList();
                }
                return View(articlesList);
            }
            catch(Exception e)
            {
                ViewBag.RealErrorMessage = e.Message;
                ViewBag.ErrorMessage = "We are sorry, something went wrong with our content. Try Again please...";
                return View(new List<User>());
            }
        }

        // GET: Articles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = "No News was specified. Redirected to index.";
            }
            Article article = null;
            try
            {
                article = db.Articles.Find(id);
                article.clicks++;
                //ViewBag.Trending = db.Articles.Where(a => a.Timestamp_publiched < DateTime.Now
                //                                        && a.Timestamp_publiched > DateTime.Now.AddDays(-30)).OrderBy(a => a.clicks).Take(5).ToList();
                db.Entry(article).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch(Exception e)
            {
                ViewBag.RealErrorMessage = e.Message;
                ViewBag.ErrorMessage = "Sorry, there was a problem accessing our data. Please try again or contact us.";
            }

            if (article == null && ViewBag.ErrorMessage == null)
            {
                ViewBag.ErrorMessage = "The article you were trying to access was not found";
            }
            if(ViewBag.ErrorMessage != null)
            {
                TempData["error"] = ViewBag.ErrorMessage;
                return RedirectToAction("index");
            }
            return View(article);
        }

        // GET: Articles/Create
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            try
            {
                ViewBag.Categories = db.Categories.ToList();
            }
            catch
            {
                TempData["error"] = "Something is not working with the data access. Please try again later.";
                return RedirectToAction("Index");
            }
            return View();
        }

        // POST: Articles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Body,Timestamp_created,Timestamp_publiched1,Category_id,Author_id")] Article article, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string fileName = "";

                    //If a file has been sent to this action
                    if (file!=null && file.ContentLength > 0)
                    {
                        //Gets the filename of the file
                        fileName = Path.GetFileName(file.FileName);

                        //Assigning filename to the model property
                        article.image_url = fileName;
                    }

                    article.Timestamp_created = DateTime.Now;
                    article.clicks = 0;
                    article.Author_id = Convert.ToInt32(Session["Id"]);
                    db.Articles.Add(article);
                    db.SaveChanges();

                    //we add the file to a folder of the user id.
                    if (fileName != "")
                    {
                        //Find the path in the server to store the images then add in a directory with the custom directory
                        //Locally, this path is different than the eventual path on another server so this follows a path
                        string path = Path.Combine(Server.MapPath("~/Images/Articles/" + article.Id + "/"));

                        //C# requires you to create the directory. The server responds by creating directories that don't exist on top of the directories that do exist
                        Directory.CreateDirectory(path);

                        path = Path.Combine(Server.MapPath("~/Images/Articles/" + article.Id + "/"), fileName);
                        file.SaveAs(path);
                    }

                    return RedirectToAction("Details", "Articles", article.Id);
                }
                catch(Exception e)
                {
                    ViewBag.ErrorMessage = "Something went wrong with the data access. Try again...";
                }
                
            }
            try
            {
                ViewBag.Categories = db.Categories.ToList();
                return View(article);
            }
            catch
            {
                return RedirectToAction("Index");
            }
            
        }

        // GET: Articles/Edit/5
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = "No Article was specified. Redirected to index.";
            }
            Article article = null;
            try
            {
                article = db.Articles.Find(id);
                ViewBag.Categories = db.Categories.ToList();
            }
            catch
            {
                ViewBag.ErrorMessage = "Something went wrong with the data access. Try again...";
            }
            if (article == null && ViewBag.ErrorMessage == null)
            {
                ViewBag.ErrorMessage = "The article you were trying to access was not found"; ;
            }
            if(ViewBag.ErrorMessage != null)
            {
                TempData["error"] = ViewBag.ErrorMessage;
                return RedirectToAction("index");
            }
            return View(article);
        }

        // POST: Articles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Edit([Bind(Include = "Id,Title,Body,Timestamp_publiched1,Category_id,Author_id,image_url")] Article article, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                //replacing an image
                if (file!= null && file.ContentLength > 0)
                {
                    article.image_url = Path.GetFileName(file.FileName);
                    //Create the path if it didn't exist previously
                    string path = Path.Combine(Server.MapPath("~/Images/Articles/" + article.Id + "/"));
                    Directory.CreateDirectory(path);
                    
                    DirectoryInfo dirInfo = new DirectoryInfo(Request.MapPath("~/Images/Articles/" + article.Id));
                    foreach (FileInfo fi in dirInfo.GetFiles())
                    {
                        fi.Delete();
                    }

                    //Upload the new file
                    string pathToUpload = Path.Combine(Server.MapPath("~/Images/Articles/" + article.Id + "/" + article.image_url));
                    file.SaveAs(pathToUpload);
                }

                try {
                    //article.Timestamp_created = db.Articles.Find(article.Id).Timestamp_created;
                    db.Entry(article).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch(Exception e)
                {   
                    TempData["error"] = "Something went wrong with the data access. Try again...";
                    try
                    {
                        ViewBag.Categories=db.Categories.ToList();
                        return View(article);
                    }
                    catch
                    {
                        TempData["error"] = "Sorry, Something went REALLY wrong. Please try again later.";
                    }
                }

                return RedirectToAction("Index");
            }
            try
            {
                ViewBag.Categories = db.Categories.ToList();
                return View(article);
            }
            catch
            {
                TempData["error"] = "Sorry, Something went REALLY wrong. Please try again later.";
                return RedirectToAction("Index");
            }
            
        }

        // GET: Articles/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            Article article = null;
            if (id == null)
            {
                TempData["error"] = "No Article was not specified. Redirected to index.";
                return RedirectToAction("Index");
            }
            
            try
            {
                article = db.Articles.Find(id);
            }
            catch
            {
                TempData["error"] = "Something went wrong with the data access. Try again...";
                return RedirectToAction("Index");
            }
            

            if (article == null)
            {
                TempData["error"] = "The article you were trying to access was not found.";
                return RedirectToAction("Index");
            }
            return View(article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Article article = db.Articles.Find(id);
                List<Comment> comments = article.Comments.ToList();
                foreach(Comment comment in comments)
                {
                    db.Comments.Remove(comment);
                }

                db.Articles.Remove(article);
                string path = Path.Combine(Server.MapPath("~/Images/Articles/" + article.Id + "/"));
                Directory.CreateDirectory(path);

                DirectoryInfo dirInfo = new DirectoryInfo(Request.MapPath("~/Images/Articles/" + article.Id));
                foreach (FileInfo fi in dirInfo.GetFiles())
                {
                    fi.Delete();
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = "Something went wrong with the data access. Try again...";
                return RedirectToAction("Delete", "Articles",id);
            }
            
        }

        [Authorize(Roles ="admin")]
        public ActionResult DeleteImage(int id)
        { 
            //Get the article object with all its values
            Article article = db.Articles.Find(id);

            //Remove the flag_img value
            article.image_url = "";

            //Update the country as you normally would from the Edit action
            db.Entry(article).State = EntityState.Modified;
            db.SaveChanges();

            //Remove the flag image from the directory
            string pathToRemoveFiles = Request.MapPath("~/Images/Articles/" + article.Id);
            System.IO.DirectoryInfo DirInfo = new DirectoryInfo(pathToRemoveFiles);

            foreach (FileInfo file in DirInfo.GetFiles())
            {
                file.Delete();
            }

            return RedirectToAction("Details", new { id = article.Id});
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
