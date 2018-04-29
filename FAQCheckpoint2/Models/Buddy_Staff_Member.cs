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
        class Staff_MemberMetadata
        {

            


        }

    }
}