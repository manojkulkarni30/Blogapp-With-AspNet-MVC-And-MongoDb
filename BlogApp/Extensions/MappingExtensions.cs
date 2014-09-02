using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Web;
using BlogApp.Models;
using BlogApp.ViewModels;
using BlogAppWithMongoDB.Models;

namespace BlogApp.Extensions
{
    public static class MappingExtensions
    {
        #region User details
        public static string GetFullName(this IIdentity identity)
        {
            var currrenPricinpal = (ClaimsPrincipal)Thread.CurrentPrincipal;

            return
                currrenPricinpal.Claims.
                    Where(c => c.Type == ClaimTypes.GivenName).
                    Select(c => c.Value).FirstOrDefault();
        }

        #endregion

        #region RoleManager

        public static RoleViewModel ToViewModel(this ApplicationRole role)
        {
            var model = new RoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name,
                DateCreated = role.DateCreated
            };
            return model;
        }

        #endregion

        #region Contact
        public static Contact ToEntity(this ContactViewModel contact)
        {
            var model = new Contact
            {
                Name = contact.Name,
                Email = contact.Email,
                Subject = contact.Subject,
                Message = contact.Message,
                DateCreated = DateTime.UtcNow,
                IsActive = true,
                IsDeleted = false
            };
            return model;
        }
        #endregion

        #region Blog

        public static Blog ToEntity(this BlogViewModel blog)
        {
            var model = new Blog
            {
                Title = blog.Title,
                Summary = HttpUtility.HtmlDecode(blog.Summary),
                Description = HttpUtility.HtmlDecode(blog.Description),
                Tags = blog.Tags.Split(',').ToList(),
                IsActive = blog.IsActive,
                IsCommentAllowed = blog.IsCommentAllowed,
                IsCommentClosed = blog.IsCommentClosed,
                IsDeleted = false,
                AuthorName = blog.AuthorName,
                SeoName = blog.SeoName
            };
            return model;
        }

        public static BlogViewModel ToViewModel(this Blog blog)
        {
            var model = new BlogViewModel
            {
                Id = blog.Id,
                Title = blog.Title,
                Summary = HttpUtility.HtmlDecode(blog.Summary),
                Description = HttpUtility.HtmlDecode(blog.Description),
                Tags = String.Join(",", blog.Tags),
                IsActive = blog.IsActive,
                IsCommentAllowed = blog.IsCommentAllowed,
                IsCommentClosed = blog.IsCommentClosed,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                DatePublished = DateTime.UtcNow,
                IsDeleted = false,
                AuthorName = blog.AuthorName,
                SeoName = blog.SeoName
            };
            return model;
        }
        #endregion

        #region Comments

        public static Comment ToEntity(this CommentViewModel comment)
        {
            var model = new Comment
            {
                Name = comment.Name,
                Email = comment.Email,
                Message = comment.Message,
                DateCreated = DateTime.UtcNow
            };
            return model;
        }
        #endregion
    }
}