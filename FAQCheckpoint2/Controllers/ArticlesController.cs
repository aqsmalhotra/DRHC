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
            //define variables
            string search = Request.QueryString["search"] ?? "";//we grab the text from the querystring
            int page = 1;
            Int32.TryParse(Request.QueryString["page"] ?? "1", out page);
            int totalPages = 1;

            //check if we come from another page, which sent an error message
            if (TempData["error"] != null)
            {
                ViewBag.ErrorMessage = TempData["error"];//the error is sent to the view throgh the viebag
            }
            try
            {
                List<Article> articles = Article.GetSearchResult(search, (User.IsInRole("admin") || User.IsInRole("staff")), page, out totalPages);

                //ViewBag.Trending = Article.GetTrending();
                ViewBag.Categories = db.Categories.ToList();
                ViewBag.Pages = totalPages;

                return View(articles);
            }
            catch (Exception e)
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
            catch (Exception e)
            {
                ViewBag.RealErrorMessage = e.Message;
                ViewBag.ErrorMessage = "Sorry, there was a problem accessing our data. Please try again or contact us.";
            }

            if (article == null && ViewBag.ErrorMessage == null)
            {
                ViewBag.ErrorMessage = "The article you were trying to access was not found";
            }
            if (ViewBag.ErrorMessage != null)
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
        public ActionResult Create([Bind(Include = "Id,Title,Body,Timestamp_created,Timestamp_publiched,Category_id,Author_id")] Article article, HttpPostedFileBase file)
        {
            article.Author_id = Convert.ToInt32(Session["id"]);
            if (ModelState.IsValid)
            {
                try
                {
                    string fileName = "";

                    //If a file has been sent to this action
                    if (file != null && file.ContentLength > 0)
                    {
                        fileName = Path.GetFileName(file.FileName);

                        article.image_url = fileName;
                    }

                    db.Articles.Add(article.addDefaults(Convert.ToInt32(Session["Id"])));
                    db.SaveChanges();


                    article.saveImage(fileName, file, Path.Combine(Server.MapPath("~/Images/Articles/" + article.Id + "/")));

                    return RedirectToAction("Details", "Articles", article.Id);
                }
                catch (Exception e)
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
                TempData["error"] = "There was an error when loading the categories for the articles. Please try again later.";
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
            if (ViewBag.ErrorMessage != null)
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
        public ActionResult Edit([Bind(Include = "Id,Title,Body,Timestamp_publiched,Category_id,Author_id,image_url")] Article article, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                //replacing an image
                if (file != null && file.ContentLength > 0)
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

                try
                {
                    //article.Timestamp_created = db.Articles.Find(article.Id).Timestamp_created;
                    db.Entry(article).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    TempData["error"] = "Something went wrong with the data access. Try again...";
                    try
                    {
                        ViewBag.Categories = db.Categories.ToList();
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
                foreach (Comment comment in comments)
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
                return RedirectToAction("Delete", "Articles", id);
            }

        }

        [Authorize(Roles = "admin")]
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

            return RedirectToAction("Details", new { id = article.Id });
        }

        //AJAX!!!
        [HttpPost]
        public ActionResult Search(FormCollection frm) //search by question
        {
            try
            {
                int totalNews = 0;
                string search = "";
                List<Article> f;
                if (frm["category"] != null)
                {
                    search = frm["category"];
                    f = Article.GetCategoryResult(search, (User.IsInRole("admin") || User.IsInRole("staff")), 1, out totalNews);
                }
                else
                {
                    if (frm["search"] != null)
                        search = frm["search"];
                    f = Article.GetSearchResult(search, (User.IsInRole("admin") || User.IsInRole("staff")), 1, out totalNews);
                }
                return PartialView("~/Views/Articles/View.cshtml", f);
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
