using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BlogApp.ViewModels
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Please Enter the Rolename")]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }
    }

    public class UserViewModel
    {
        public string Id { get; set; }
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Please Enter the Firstname")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Please Enter the Lastname")]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        public IEnumerable<SelectListItem> RolesList { get; set; }

    }
}