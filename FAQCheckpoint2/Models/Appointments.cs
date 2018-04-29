namespace FAQCheckpoint2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Appointments
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Name")]
    
        public string Fullname { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9]+)*\\.([a-z]{2,4})$", ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [MinLength(10, ErrorMessage = "Phone number must have 10 digits")]
        public string Phone { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Department is required")]
        public int Department_id { get; set; }
        [NotMapped]
        public virtual Department Department { get; set; }


        [Required(ErrorMessage = "Message is required")]
        public string Message { get; set; }
    }
}
