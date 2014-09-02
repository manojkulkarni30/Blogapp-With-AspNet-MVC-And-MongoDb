using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using BlogApp.Models;
using BlogApp.ViewModels;

namespace BlogApp.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private ApplicationRoleManager _roleManager;

        private ApplicationUserManager _userManager;

        public ApplicationRoleManager RoleManager
        {
            get { return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>(); }
            set { _roleManager = value; }
        }

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().Get<ApplicationUserManager>(); }
            set { _userManager = value; }
        }

        public ActionResult Index()
        {
            return View(UserManager.Users.OrderByDescending(c => c.DateCreated).ToList());
        }

        public ActionResult Create()
        {
            ViewBag.RoleId = GetRolesSelectList();
            return View();
        }

        [NonAction]
        private SelectList GetRolesSelectList()
        {
            return new SelectList(RoleManager.Roles.ToList(), "Name", "Name");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RegisterViewModel model, params string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    EmailConfirmed = true,
                    DateCreated = DateTime.UtcNow
                };
                IdentityResult adminResult = await UserManager.CreateAsync(user, model.Password);
                if (adminResult.Succeeded)
                {
                    IdentityResult claimResult = await
                        UserManager.AddClaimAsync(user.Id,
                            new Claim(ClaimTypes.GivenName, model.FirstName + " " + model.LastName));
                    if (claimResult.Succeeded)
                    {
                        IdentityResult roleResult = await UserManager.AddToRolesAsync(user.Id, selectedRoles);
                        if (roleResult.Succeeded)
                        {
                            return RedirectToAction("Index");
                        }
                        ModelState.AddModelError("", roleResult.Errors.First());
                    }
                    ModelState.AddModelError("", claimResult.Errors.First());
                }
                else
                {
                    ModelState.AddModelError("", adminResult.Errors.First());
                }
            }
            ViewBag.RoleId = GetRolesSelectList();
            return View(model);
        }

        public async Task<ActionResult> Edit(string id)
        {
            if (String.IsNullOrWhiteSpace(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = await UserManager.FindByIdAsync(id);
            if (user == null)
                return HttpNotFound();

            // Get Users Roles
            IList<string> roles = await UserManager.GetRolesAsync(user.Id);
            var model = new UserViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                RolesList = RoleManager.Roles.ToList().Select(c => new SelectListItem
                {
                    Selected = roles.Contains(c.Name),
                    Text = c.Name,
                    Value = c.Name
                })
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UserViewModel model, params string[] selectedRole)
        {
            IList<string> userRoles = await UserManager.GetRolesAsync(model.Id);
            if (ModelState.IsValid)
            {
                ApplicationUser user = await UserManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Email = model.Email;
                    user.UserName = model.Email;
                    selectedRole = selectedRole ?? new string[] { };
                    await UserManager.AddOrRemoveUserToRolesAsync(
                        user,
                        selectedRole.Except(userRoles).ToList(),
                        userRoles.Except(selectedRole).ToList());
                    return RedirectToAction("Index");
                }
            }
            model.RolesList = RoleManager.Roles.ToList().Select(c => new SelectListItem
            {
                Selected = userRoles.Contains(c.Name),
                Text = c.Name,
                Value = c.Name
            });
            return View(model);
        }

        public async Task<ActionResult> Details(string id)
        {
            if (String.IsNullOrWhiteSpace(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                return View(user);
            }
            return HttpNotFound();
        }

        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (String.IsNullOrWhiteSpace(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await UserManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", result.Errors.First());
                return View(user);
            }
            return HttpNotFound();
        }
    }
}