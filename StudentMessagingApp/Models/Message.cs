using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace StudentMessagingApp.Models
{
    [BsonIgnoreExtraElements]
    public class Message
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? StudentId { get; set; }

        [BsonElement("Title")]
        [JsonPropertyName("Title")]
        public string Title { get; set; } = null!;

        [BsonElement("Content")]
        [JsonPropertyName("Content")]
        public string Content { get; set; } = null!;
    }
}
