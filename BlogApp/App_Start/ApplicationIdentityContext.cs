using System;
using System.Configuration;
using AspNet.Identity.MongoDB;
using BlogAppWithMongoDB.Models;
using MongoDB.Driver;
using BlogApp.Models;

namespace BlogApp
{
    public class ApplicationIdentityContext : IdentityContext, IDisposable
    {
        public MongoCollection Contact { get; private set; }
        public MongoCollection Blog { get; private set; }
        public ApplicationIdentityContext(MongoCollection users, MongoCollection roles) :
            base(users, roles)
        {

        }

        private ApplicationIdentityContext(
            MongoCollection users,
            MongoCollection roles,
            MongoCollection contact, MongoCollection blog)
            : base(users, roles)
        {
            Contact = contact;
            Blog = blog;
        }
        public static ApplicationIdentityContext Create()
        {
            var dbName = ConfigurationManager.AppSettings["DbName"];
            var client = new MongoClient("mongodb://localhost:27017");
            var server = client.GetServer();
            var database = server.GetDatabase(dbName);
            var users = database.GetCollection<IdentityUser>("users");
            var roles = database.GetCollection<ApplicationRole>("roles");
            var contact = database.GetCollection<Contact>("contact");
            var blog = database.GetCollection<Blog>("blog");
            blog.CreateIndex(new[] { "Title", "SeoName" });
            return new ApplicationIdentityContext(users, roles, contact, blog);
        }

        public void Dispose()
        {

        }
    }
}