using System.Text.Json.Serialization;

namespace Yape.Sdk.Entity
{
    public class CustomerPhone
    {
        [JsonPropertyName("cashtag")]
        public string Cashtag { get; set; }
    }
}
