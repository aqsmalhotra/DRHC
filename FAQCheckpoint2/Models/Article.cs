//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FAQCheckpoint2.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Article
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Article()
        {
            this.Comments = new HashSet<Comment>();
        }
    
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public System.DateTime Timestamp_created { get; set; }
        public int Category_id { get; set; }
        public int Author_id { get; set; }
        public Nullable<int> clicks { get; set; }
        public string image_url { get; set; }
        public Nullable<System.DateTime> Timestamp_published { get; set; }
        public Nullable<System.DateTime> Timestamp_publiched { get; set; }
    
        public virtual Category Category { get; set; }
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
