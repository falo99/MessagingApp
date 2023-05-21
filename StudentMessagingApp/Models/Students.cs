using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace StudentMessagingApp.Models
{
    [BsonIgnoreExtraElements]
    public class Students
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("Name")]
        [JsonPropertyName("Name")]
        public string Name { get; set; } = null!;

        [BsonElement("Surname")]
        [JsonPropertyName("Surname")]
        public string Surname { get; set; } = null!;

        [BsonElement("Messages")]
        [JsonPropertyName("Messages")]
        [BsonRepresentation(BsonType.String)]
        public List<Guid> Messages { get; set; } = null!;

    }
}
