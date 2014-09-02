using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BlogAppWithMongoDB.Models
{
    public class Blog
    {
        public Blog()
        {
            Tags = new List<string>();
            Comments = new List<Comment>();
        }

        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Title { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public List<string> Tags { get; set; }
        public string SeoName { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public DateTime? DatePublished { get; set; }

        [BsonRepresentation(BsonType.Boolean)]
        public bool IsActive { get; set; }

        [BsonRepresentation(BsonType.Boolean)]
        public bool IsDeleted { get; set; }

        [BsonRepresentation(BsonType.Boolean)]
        public bool IsCommentAllowed { get; set; }

        [BsonRepresentation(BsonType.Boolean)]
        public bool IsCommentClosed { get; set; }
        public List<Comment> Comments { get; set; }
        public string AuthorName { get; set; }
    }
}