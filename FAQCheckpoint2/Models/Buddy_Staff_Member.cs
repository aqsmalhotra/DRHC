using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Web;

namespace FAQCheckpoint2.Models
{
    [MetadataType(typeof(Staff_MemberMetadata))]
    public partial class Staff_Members
    {
        public string FullName { get { return this.First_Name + " " + this.Last_Name; } }
        public string SimpleDate { get { return this.Working_From.ToShortDateString(); } }
        class Staff_MemberMetadata
        {
            [Display(Name ="First Name")]
            public string First_Name { get; set; }
            [Display(Name = "Last Name")]
            public string Last_Name { get; set; }
            [Display(Name = "Biography")]
            public string Breaf_Bio { get; set; }
            [Display(Name = "Working since")]
            public System.DateTime Working_From { get; set; }
            public System.DateTime Created_At { get; set; }
            public string Image_Url { get; set; }
            [Display(Name = "Department")]
            public int Department_Id { get; set; }

        }

    }
}