using ActivityLogger.Enums;
using ActivityLogger.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json.Linq;

namespace ActivityLogger.Models
{
    public class ActivityLog
    {
        [BsonElement("_id")]
        public ObjectId Id { get; set; }
        public string Type { get; set; }
        public DateTime RecordDate { get; set; }
        public BsonDocument Data { get; set; }
    }
}
