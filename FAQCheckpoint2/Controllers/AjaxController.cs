using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using FAQCheckpoint2.Models;

namespace FAQCheckpoint2.Controllers
{
    public class AjaxController : Controller
    {
        aqsmalhotraEntities db = new aqsmalhotraEntities();


        [HttpPost]
        public JsonResult AddComment([Bind(Exclude = "Id")]Comment comment)
        {
            if (User.IsInRole("public") || User.IsInRole("staff"))
            {
                try
                {
                    comment.Author_id = Convert.ToInt32(Session["id"]);
                    if (ModelState.IsValid)
                    {
                        db.Comments.Add(new Comment
                        {
                            Article_id = Convert.ToInt32(comment.Article_id),
                            Body = comment.Body,
                            Author_id = Convert.ToInt32(comment.Author_id),
                            Date_created = DateTime.Now
                        });
                        db.SaveChanges();

                        return Json(new Dictionary<string, object>
                        {
                            { "message", "The comment was added. Updating comment list." },
                            { "comment", comment }
                        });
                    }
                    else
                    {
                        return Json(new Dictionary<string, object>
                        {
                            { "message", "Your was not saved due to a data restriction." }
                        });
                    }
                }
                catch(Exception e)
                {
                    return Json(new Dictionary<string, string>
                    {
                        { "message", "The comment wasn't added. Please try again later." }
                    });
                }
            }

            return Json(new Dictionary<string, string>
            {
                {"message", "You don't have privileges."}
            });
        }
        
        public JsonResult GetCommentList(int id)
        {
            try
            {
                var comments = db.Comments.Where(x => x.Article_id == id)
                    .Select(x => new
                    {
                        Username = x.faq_users.username,
                        x.Body
                    });
                return Json(new Dictionary<string, object>
                    {
                        { "message", "All the comments where updated" },
                        { "comments" , comments}
                    },
                    JsonRequestBehavior.AllowGet);
                //return Json(comments);
            }
            catch (Exception e)
            {
                return Json("Error, something went wrong with our data access. Try again later...");
            }
        }
        [HttpPost]
        public JsonResult GetCommentList(object o)
        {
            return Json("hola");
        }
    }
}