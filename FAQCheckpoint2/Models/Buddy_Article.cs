using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace FAQCheckpoint2.Models
{
    [MetadataType(typeof(ArticleMetadata))]
    public partial class Article
    {
        private static aqsmalhotraEntities db = new aqsmalhotraEntities();

        class ArticleMetadata
        {
            //public System.DateTime Timestamp_created { get; set; }

            [Display(Name = "Category")]
            public int Category_id { get; set; }

            //public int Author_id { get; set; }
            public System.DateTime Timestamp_created { get; set; }


            [Display(Name = "Date to publish")]
            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
            public Nullable<System.DateTime> Timestamp_published { get; set; }

            [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
            [DataType(DataType.Date)]
            [Display(Name = "Date to publish")]
            public Nullable<System.DateTime> Timestamp_publiched { get; set; }


        }

        public static List<Article> GetTrending()
        {
            //add in the last 30 days.
            return db.Articles.Where(a => a.Timestamp_publiched < DateTime.Now)
                                                          .OrderByDescending(a => a.clicks)
                                                          .Take(5)
                                                          .ToList();
        }

        public static List<Article> GetSearchResult(string search, bool admin, int page, out int totalNews)
        {
            totalNews = 0;
            int articlesPerPage = 10;
            var articles = db.Articles.Where(a => a.Body.Contains(search) ||
                                            a.Title.Contains(search) || a.Category.Name.Contains(search)).OrderByDescending(a => a.Timestamp_publiched).ThenByDescending(a => a.Timestamp_publiched);


            if (!admin)
            {
                var finalList = articles.Where(a => a.Timestamp_publiched < DateTime.Now);
                var final2 = finalList;
                totalNews = final2.Count() / articlesPerPage;
                return finalList.Skip(articlesPerPage * (page - 1)).Take(10).ToList();
            }
            else
            {
                var finalList = articles;
                var final2 = finalList;
                totalNews = final2.Count() / articlesPerPage;
                return finalList.Skip(articlesPerPage * (page - 1)).Take(10).ToList();
            }

        }

        public Article addDefaults(int AuthorId)
        {
            this.Timestamp_created = DateTime.Now;
            this.clicks = 0;
            this.Author_id = AuthorId;
            return this;
        }

        public string saveImage(string fileName, HttpPostedFileBase file, string path)
        {
            //we add the file to a folder of the user id.
            if (fileName != "")
            {
                //C# requires you to create the directory. The server responds by creating directories that don't exist on top of the directories that do exist
                Directory.CreateDirectory(path);
                path = Path.Combine(path, fileName);
                file.SaveAs(path);
            }
            return null;
        }

    }
}