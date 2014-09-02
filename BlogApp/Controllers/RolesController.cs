using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AspNet.Identity.MongoDB;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using BlogApp.Extensions;
using BlogApp.Models;
using BlogApp.ViewModels;

namespace BlogApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private ApplicationRoleManager _roleManager;
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            set { _userManager = value; }
        }

        public ApplicationRoleManager RoleManager
        {
            get { return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>(); }
            private set { _roleManager = value; }
        }

        public ActionResult Index()
        {
            return View(RoleManager.Roles.OrderByDescending(c => c.DateCreated).ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RoleViewModel role)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await RoleManager.CreateAsync(
                    new ApplicationRole
                    {
                        Name = role.RoleName,
                        DateCreated = DateTime.UtcNow
                    });
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Error occured while creating new role");
            }
            return View(role);
        }

        public async Task<ActionResult> Edit(string id)
        {
            if (String.IsNullOrWhiteSpace(id))
            {
                return HttpNotFound();
            }
            ApplicationRole role = await GetRoleByIdAsyc(id);
            return View(role.ToViewModel());
        }

        private async Task<ApplicationRole> GetRoleByIdAsyc(string id)
        {
            ApplicationRole role = await RoleManager.FindByIdAsync(id);
            return role;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(RoleViewModel role)
        {
            if (ModelState.IsValid)
            {
                ApplicationRole model = await GetRoleByIdAsyc(role.Id);
                model.Name = role.RoleName;
                IdentityResult result = await RoleManager.UpdateAsync(model);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Error occured while updating role");
            }
            return View(role);
        }

        public async Task<ActionResult> Details(string id)
        {
            ApplicationRole role = await GetRoleByIdAsyc(id);
            if (role != null)
            {
                return View(role.ToViewModel());
            }
            return HttpNotFound();
        }

        public async Task<ActionResult> Delete(string id)
        {
            ApplicationRole role = await GetRoleByIdAsyc(id);
            if (role != null)
            {
                return View(role.ToViewModel());
            }
            return HttpNotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(RoleViewModel model)
        {
            ApplicationRole role = await GetRoleByIdAsyc(model.Id);
            if (role != null)
            {
                IdentityResult result = await RoleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    IQueryable<ApplicationUser> usersInRole = UserManager.Users.Where(c => c.Roles.Contains(role.Name));
                    foreach (ApplicationUser user in usersInRole)
                    {
                        await UserManager.RemoveFromRoleAsync(user.Id, role.Name);
                    }
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "An error occured while deleting the role");
                return View(model);
            }
            return View(model);
        }
    }
}