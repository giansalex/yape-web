using System.Text.Json.Serialization;

namespace Yape.Sdk.Entity
{
    public class Order
    {
        [JsonPropertyName("amount")]
        public string Amount { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("cashTag")]
        public string CashTag { get; set; }
    }
}
