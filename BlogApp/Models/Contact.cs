using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BlogApp.Models
{
    public class Contact
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        [BsonRepresentation(BsonType.Boolean)]
        public bool IsActive { get; set; }
        [BsonRepresentation(BsonType.Boolean)]
        public bool IsDeleted { get; set; }
        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }
    }
}