using Newtonsoft.Json;

namespace Keyfactor.Extensions.Orchestrator.Fortanix.Models
{
    public class BearerToken
    {
        [JsonProperty("access_token")]
        public string Token { get; set; }

        [JsonProperty("entity_id")]
        public string EntityId { get; set; }
    }
}
