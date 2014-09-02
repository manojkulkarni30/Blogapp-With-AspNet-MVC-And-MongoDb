using System;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNet.Identity.MongoDB;
using Microsoft.AspNet.Identity;

namespace BlogApp.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            ClaimsIdentity userIdentity =
                await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            // Add custom user claims here
            userIdentity.AddClaim(new Claim(ClaimTypes.GivenName, this.FirstName + " " + this.LastName));
            return userIdentity;
        }
        [Display(Name = "Date Created")]
        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }
    }

    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole()
            : base()
        {

        }
        public ApplicationRole(string name)
            : base(name)
        {

        }
        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }
    }
}