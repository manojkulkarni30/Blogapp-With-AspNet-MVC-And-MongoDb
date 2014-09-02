using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using BlogApp.Extensions;
using BlogApp.ViewModels;
using BlogAppWithMongoDB.Models;
using Microsoft.AspNet.Identity.Owin;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using PagedList;

namespace BlogApp.Controllers
{
    [Authorize]
    public class BlogController : Controller
    {
        private const int PageSize = 5;
        private ApplicationIdentityContext _context;

        private ApplicationIdentityContext Context
        {
            get
            {
                return _context ?? System.Web.HttpContext.Current.GetOwinContext().Get<ApplicationIdentityContext>();
            }
            set { _context = value; }
        }

        [AllowAnonymous]
        public ActionResult Index(int? page)
        {
            int pageNumber = page ?? 1;
            IPagedList<Blog> blogs = Context.Blog.AsQueryable<Blog>().Where(c => c.IsActive).
                OrderByDescending(c => c.DatePublished).ToPagedList(pageNumber, PageSize);
            return View(blogs);
        }

        public ActionResult LoggedInUsersPost(int? page)
        {
            int pageNumber = page ?? 1;
            IOrderedQueryable<Blog> blogs = Context.Blog.AsQueryable<Blog>().
                OrderByDescending(c => c.DatePublished);
            return
                View(
                    blogs.Where(c => c.AuthorName.ToLower() == User.Identity.GetFullName().ToLower())
                        .ToPagedList(pageNumber, PageSize));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BlogViewModel blog)
        {
            if (ModelState.IsValid)
            {
                blog.SeoName = GetSeoName(blog.Title);
                blog.AuthorName = User.Identity.GetFullName();
                Blog model = blog.ToEntity();
                model.DateCreated = DateTime.UtcNow;
                model.DatePublished = DateTime.UtcNow;
                model.DateUpdated = DateTime.UtcNow;
                Context.Blog.Insert(model);
                return RedirectToAction("Index", "Blog", new { page = 1 });
            }
            return View(blog);
        }

        public ActionResult Edit(string id)
        {
            if (String.IsNullOrWhiteSpace(id))
            {
                return HttpNotFound();
            }
            Blog blog = GetBlogById(id);
            return View(blog.ToViewModel());
        }

        [NonAction]
        private Blog GetBlogById(string id)
        {
            return Context.Blog.FindOneByIdAs<Blog>(new ObjectId(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BlogViewModel blog)
        {
            if (ModelState.IsValid)
            {
                Blog model = GetBlogById(blog.Id);
                model.Title = blog.Title;
                model.Summary = HttpUtility.HtmlDecode(blog.Summary);
                model.Description = HttpUtility.HtmlDecode(blog.Description);
                model.Tags = blog.Tags.Split(',').ToList();
                model.IsActive = blog.IsActive;
                model.IsCommentAllowed = blog.IsCommentAllowed;
                model.DatePublished = model.IsActive == false && blog.IsActive ? DateTime.UtcNow : model.DatePublished;
                model.DateUpdated = DateTime.UtcNow;
                model.SeoName = GetSeoName(blog.Title);
                Context.Blog.Save(model);
                return RedirectToAction("Index");
            }
            return View(blog);
        }

        public ActionResult ViewPost(string seoName)
        {
            if (String.IsNullOrWhiteSpace(seoName))
            {
                return RedirectToAction("Index");
            }
            return View(Context.Blog.FindOneAs<Blog>(Query.EQ("SeoName", new BsonString(seoName))));
        }

        public JsonResult IsBlogNameExists(string blogTitle)
        {
            bool result = false;
            string err = String.Empty;
            if (String.IsNullOrWhiteSpace(blogTitle))
            {
                err = "Blog Title can not be empty";
            }
            string seoName = GetSeoName(blogTitle);
            if (Context.Blog.Exists())
            {
                var blog = Context.Blog.FindOneAs<Blog>(Query.EQ("SeoName", new BsonString(seoName)));
                result = blog != null;
            }
            else
            {
                result = true;
            }
            return Json(new { output = result, error = err }, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        private static string GetSeoName(string blogTitle)
        {
            if (String.IsNullOrWhiteSpace(blogTitle))
            {
                return String.Empty;
            }
            string removeSpecialCharacters = Regex.Replace(Regex.Replace(blogTitle.ToLower(), "[^a-zA-Z0-9]+$", " "),
                "[.$',\"\"{}()#?:+*<> &#@!`~]", "-");
            string removeDuplicateDash = Regex.Replace(removeSpecialCharacters, "[-]+", "-");
            if (removeDuplicateDash.LastIndexOf('-') == (removeDuplicateDash.Length - 1))
            {
                return removeDuplicateDash.Remove((removeDuplicateDash.Length - 1), 1);
            }
            return removeDuplicateDash;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddComment(CommentViewModel comment)
        {
            if (ModelState.IsValid)
            {
                var model = comment.ToEntity();
                var modifiedDoc = Update<Blog>.Push(c => c.Comments, model);
                Context.Blog.Update(Query.EQ("_id", new ObjectId(comment.Id)), modifiedDoc);
            }
            return PartialView("_AddComment", comment);
        }

        public ActionResult GetComments(string id)
        {
            var results = Context.Blog.AsQueryable<Blog>().FirstOrDefault(c => c.Id == id);
            if (results != null) return PartialView("_Comments", results.Comments.OrderByDescending(c => c.DateCreated).ToList());
            return null;
        }
    }
}