using Newtonsoft.Json;

namespace Keyfactor.Extensions.Orchestrator.Fortanix.Models
{
    public class APIError
    {
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
