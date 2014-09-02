using System.Web.Mvc;
using System.Web.Routing;
using Canonicalize;

namespace BlogApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.Canonicalize().Www().Lowercase().NoTrailingSlash();

            routes.MapRoute("CreateBlog", "Post/Create",
             new { controller = "Blog", action = "Create" }
             );

            routes.MapRoute("ViewPost", "Post/{SeoName}",
            new { controller = "Blog", action = "ViewPost" }
            );

            routes.MapRoute("MyPosts", "Posts/MyPost/{page}",
              new { controller = "Blog", action = "LoggedInUsersPost", page = UrlParameter.Optional }
            );

            routes.MapRoute("AllPost", "Posts/All/{page}",
            new { controller = "Blog", action = "Index", page = UrlParameter.Optional }
            );


            routes.MapRoute("Contact", "contact",
               new { controller = "Home", action = "Contact" }
               );
            routes.MapRoute("About", "About",
               new { controller = "Home", action = "About" }
               );

            routes.MapRoute("Default", "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                );
        }
    }
}