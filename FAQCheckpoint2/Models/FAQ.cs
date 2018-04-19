using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FAQCheckpoint2.Models
{
    
    [Table("faq")]
    public class FAQ
    {
        //DataAnnotations for required properties
        [Key]

        //DataAnnotations for specific formats(Email, Date, Character limits)
        [Required]
        [Display(Name = "ID")]
        public int Faq_id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Question field is required!")]
        [MinLength(10, ErrorMessage = "Please put atleast a sentence in question field!")]
        [Display(Name = "Question")]
        public string Faq_question { get; set; }

        [Display(Name = "Answer")]
        public string Faq_answer { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select the current date!")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "Created/ Last Updated")]
        public DateTime Faq_last_update { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Author field is required!")]
        [MinLength(3, ErrorMessage = "You need to mention atleast the first name!")]
        [Display(Name = "Author")]
        public string Faq_author { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select a category!")]
        [Display(Name ="Category")]
        public string Faq_category { get; set; }

    }
}