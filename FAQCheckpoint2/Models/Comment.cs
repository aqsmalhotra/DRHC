
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
    
public partial class Comment
{

    public int Id { get; set; }

    public System.DateTime Date_created { get; set; }

    public string Body { get; set; }

    public int Author_id { get; set; }

    public int Article_id { get; set; }



    public virtual Article Article { get; set; }

    public virtual User User { get; set; }

}

}
