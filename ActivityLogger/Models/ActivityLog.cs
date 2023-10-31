using ActivityLogger.Enums;
using ActivityLogger.Interfaces;
using MongoDB.Bson;
using Newtonsoft.Json.Linq;

namespace ActivityLogger.Models
{
    public class ActivityLog
    {
        public string Type { get; set; }
        public DateTime RecordDate { get; set; }
        public BsonDocument Data { get; set; }
    }
}
