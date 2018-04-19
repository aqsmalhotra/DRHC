using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FAQCheckpoint2.Models
{
    [Table("volunteer")]
    public class Volunteers
    {
        [Key]

        [Required]
        [Display(Name = "ID")]
        public int volunteer_id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Volunteer Name field is required!")]
        [Display(Name = "Name")]
        public string volunteer_name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select the date of birth!")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "Date of Birth")]
        public DateTime volunteer_dob { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Email field is required!")]
        [Display(Name = "Email")]
        public string volunteer_email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Phone Number field is required!")]
        [MinLength(10, ErrorMessage = "Phone number must have 10 digits!")]
        [Display(Name = "Phone No")]
        public string volunteer_phone { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select areas of interest!")]
        [Display(Name = "Areas of Interest")]
        public string volunteer_interests { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select available shifts!")]
        [Display(Name = "Availability")]
        public string volunteer_shifts { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please give your statement!")]
        [Display(Name = "Why do you wish to volunteer with us?")]
        public string volunteer_message { get; set; }

        [Display(Name = "Upload CV (Optional)")]
        public string volunteer_cv { get; set; }
    }
}