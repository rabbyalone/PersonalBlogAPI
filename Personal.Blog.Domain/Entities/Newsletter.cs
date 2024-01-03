using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Personal.Blog.Domain.Entities
{
    public class Newsletter
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string? Email { get; set; }
    }
}
