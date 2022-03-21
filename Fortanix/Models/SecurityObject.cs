using Newtonsoft.Json;

namespace Keyfactor.Extensions.Orchestrator.Fortanix.Models
{
    public class SecurityObject
    {
        [JsonProperty("obj_type")]
        public string Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Certificate { get; set; }
    }
}
