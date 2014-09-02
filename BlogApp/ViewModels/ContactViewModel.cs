using System;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.ViewModels
{
    public class ContactViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Please Enter the Name")]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Please Enter the valid data")]
        [StringLength(60, ErrorMessage = "Maximum allowed length is 60 characters")]
        public string Name { get; set; }

        [StringLength(100, ErrorMessage = "Maximum allowed length is 100 characters")]
        [Required(ErrorMessage = "Please Enter the Subject")]
        [RegularExpression("^[a-zA-Z0-9 _]+$", ErrorMessage = "Please Enter the valid data")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Please Enter the Message")]
        public string Message { get; set; }

        [StringLength(60, ErrorMessage = "Maximum allowed length is 60 characters")]
        [Required(ErrorMessage = "Please Enter the Email Address")]
        [EmailAddress(ErrorMessage = "Please Enter valid Email Address")]
        public string Email { get; set; }
        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
    }
}