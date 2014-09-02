using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BlogApp.ViewModels
{
    public class BlogViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Please Enter the Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please Enter the Summary")]
        [AllowHtml]
        public string Summary { get; set; }

        [Required(ErrorMessage = "Please Enter the Description")]
        [AllowHtml]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please Enter the Tags")]
        public string Tags { get; set; }

        public string SeoName { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public DateTime DatePublished { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Close Comments")]
        public bool IsCommentClosed { get; set; }

        public bool IsDeleted { get; set; }

        [Display(Name = "Allow Comments")]
        public bool IsCommentAllowed { get; set; }
        public string AuthorName { get; set; }
    }
}