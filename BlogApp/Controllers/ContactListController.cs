using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using BlogApp.Models;

namespace BlogApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ContactListController : Controller
    {
        private ApplicationIdentityContext _context;

        public ApplicationIdentityContext Context
        {
            get { return _context ?? HttpContext.GetOwinContext().Get<ApplicationIdentityContext>(); }
            set { _context = value; }
        }

        public ActionResult Index()
        {
            return View(Context.Contact.AsQueryable<Contact>().OrderByDescending(c => c.DateCreated).ToList());
        }

        public ActionResult Details(string id)
        {
            Contact contact = GetContactById(id);
            if (contact != null)
            {
                return View(contact);
            }
            return HttpNotFound();
        }

        public ActionResult Delete(string id)
        {
            Contact contact = GetContactById(id);
            if (contact != null)
            {
                return View(contact);
            }
            return HttpNotFound();
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfimed(string id)
        {
            Contact contact = GetContactById(id);
            if (contact != null)
            {
                Context.Contact.Remove(Query.EQ("_id", new ObjectId(contact.Id)));
                return RedirectToAction("Index");
            }
            return View();
        }

        [NonAction]
        private Contact GetContactById(string id)
        {
            return Context.Contact.FindOneByIdAs<Contact>(new ObjectId(id));
        }
    }
}