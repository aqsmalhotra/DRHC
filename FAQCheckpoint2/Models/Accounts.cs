using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FAQCheckpoint2.Models
{
    [Table("faq_users")]
    public class Accounts
    {
        [Key]

        [Required]
        [Display(Name = "ID")]
        public int id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Full name is required!")]
        [Display(Name = "Full Name")]
        public string fullname { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Username is required!")]
        [Display(Name = "Username")]
        public string username { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required!")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required!")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string email { get; set; }


        [Display(Name = "User Role")]
        public string role { get; set; }
    }
}