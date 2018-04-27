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
    using System.ComponentModel.DataAnnotations;

    public partial class ecard
    {
        [Key]
        public int id { get; set; }
        [Display(Name = "Sender First Name")]
        [Required(ErrorMessage = "Your first name must be provided.")]
        public string sender_first_name { get; set; }
        [Display(Name = "Sender Last Name")]
        [Required(ErrorMessage = "Your last name must be provided.")]
        public string sender_last_name { get; set; }
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email must be provided.")]
        public string sender_email { get; set; }
        [Display(Name = "Recipient")]
        [Required(ErrorMessage = "Person does not exist.")]
        public int person_id { get; set; }
        [Display(Name = "Card Choice")]
        [Required]
        public int card_choice { get; set; }
        [Display(Name = "Message")]
        [Required(ErrorMessage = "A message is required.")]
        public string card_message { get; set; }
    
        public virtual card_choices card_choices { get; set; }
        public virtual person person { get; set; }
    }
}
