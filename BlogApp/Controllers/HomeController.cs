using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using BlogApp.Extensions;
using BlogApp.ViewModels;

namespace BlogApp.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationIdentityContext _context;

        public ApplicationIdentityContext Context
        {
            get { return _context ?? HttpContext.GetOwinContext().Get<ApplicationIdentityContext>(); ; }
            set { _context = value; }
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            if (User.Identity.IsAuthenticated)
            {
                var contact = new ContactViewModel
                {
                    Name = User.Identity.GetFullName(),
                    Email = User.Identity.GetUserName()
                };
                return View(contact);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(ContactViewModel contact)
        {
            if (ModelState.IsValid)
            {
                var model = contact.ToEntity();
                Context.Contact.Insert(model);
                return RedirectToAction("Success");
            }
            return View(contact);
        }

        public ActionResult Success()
        {
            return View();
        }
    }
}