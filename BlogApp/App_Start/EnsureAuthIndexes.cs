using System.Web;
using AspNet.Identity.MongoDB;
using Microsoft.AspNet.Identity.Owin;

namespace BlogApp
{
    public class EnsureAuthIndexes
    {
        public static void Exists()
        {
            var context = ApplicationIdentityContext.Create();
            IndexChecks.EnsureUniqueIndexOnUserName(context.Users);
            IndexChecks.EnsureUniqueIndexOnRoleName(context.Roles);
            IndexChecks.EnsureUniqueIndexOnEmail(context.Users);
        }
    }
}