using ActivityLogger.Enums;
using ActivityLogger.Interfaces;
using ActivityLogger.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ActivityLogger.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LogActivityController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<LogActivityController> _logger;
        private readonly IMongoClient _mongoClient;

        public LogActivityController(ILogger<LogActivityController> logger, IMongoClient mongoClient)
        {
            _logger = logger;
            _mongoClient = mongoClient;
        }

        [HttpGet]
        public async Task<IActionResult> GetLogs(int type, int limit)
        {
            try
            {
                bool isTypeDefined = Enum.IsDefined(typeof(ActivityTypes), type);
                if (!isTypeDefined)
                {
                    throw new Exception($"Activity type of number {type} does not exist");
                }

                string activityType = Enum.GetName(typeof(ActivityTypes), type);
                var db = _mongoClient.GetDatabase("Logs");
                var col = db.GetCollection<ActivityLog>("Activities");

                var typeFilter = Builders<ActivityLog>.Filter.Eq("Type", activityType);
                var sort = Builders<ActivityLog>.Sort.Descending("RecordDate");


                var filteredCol = await col.Find(typeFilter).Sort(sort).Limit(limit).ToListAsync();
                var serializedResult = JsonConvert.SerializeObject(filteredCol);
                return Ok(serializedResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest($"An error has occured: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task RecordActivity([FromBody] ActivityApiRequest activityApiRequest)
        {
            try
            {
                bool isTypeDefined = Enum.IsDefined(typeof(ActivityTypes), activityApiRequest.Type);
                if (!isTypeDefined)
                {
                    throw new Exception($"Activity type of number {activityApiRequest.Type} does not exist");
                }

                var json = activityApiRequest.Data.ToString();
                var bsonDocument = BsonDocument.Parse(json);
                string activityType = Enum.GetName(typeof(ActivityTypes), activityApiRequest.Type);

                ActivityLog activityLog = new ActivityLog()
                {
                    Type = activityType,
                    RecordDate = DateTime.Now,
                    Data = bsonDocument
                };
                var db = _mongoClient.GetDatabase("Logs");
                var col = db.GetCollection<ActivityLog>("Activities");
                await col.InsertOneAsync(activityLog);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}