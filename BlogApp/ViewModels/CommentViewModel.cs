using System;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.ViewModels
{
    public class CommentViewModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Please Enter the Name")]
        public string Name { get; set; }
        [EmailAddress(ErrorMessage = "Please Enter Valid Email address")]
        [Required(ErrorMessage = "Please Enter the Email address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please Enter the Message")]
        public string Message { get; set; }
        public DateTime DateCreated { get; set; } 
    }
}