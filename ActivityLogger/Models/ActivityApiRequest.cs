using Newtonsoft.Json.Linq;
using System.Text.Json.Nodes;

namespace ActivityLogger.Models
{
    public class ActivityApiRequest
    {
        public int Type { get; set; }
        public JsonObject Data { get; set; }
    }
}
