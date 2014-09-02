using System;

namespace BlogAppWithMongoDB.Models
{
    public class Comment
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public DateTime DateCreated { get; set; }
    }
}