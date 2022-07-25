using System.Text.Json.Serialization;

namespace WebApp.Models
{
    public class Currency
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("rate")]
        public decimal Rate { get; set; }
    }
}
