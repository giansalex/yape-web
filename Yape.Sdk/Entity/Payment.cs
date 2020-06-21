using System.Text.Json.Serialization;

namespace Yape.Sdk.Entity
{
    public class Payment
    {
        [JsonPropertyName("cashTag")]
        public string CashTag { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("creationTime")]
        public long CreationTime { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}
