using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FAQCheckpoint2.Models
{
    [MetadataType(typeof(ArticleMetadata))]
    public partial class Article
    {
        class ArticleMetadata
        {
            [Display(Name = "Article Id")]
            public int Id { get; set; }



            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            public Nullable<System.DateTime> Established { get; set; }

            
            public System.DateTime Timestamp_created { get; set; }

            [Display(Name ="Category")]
            public int Category_id { get; set; }
            
            public int Author_id { get; set; }
            

            [Display(Name = "Date to publish")]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            public Nullable<System.DateTime> Timestamp_published { get; set; }

            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            [Display(Name = "Date to publish")]
            public Nullable<System.DateTime> Timestamp_publiched { get; set; }
        }
    }
}