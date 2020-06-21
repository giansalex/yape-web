using System;
using System.Collections.Generic;
using System.Text;
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
        public long Amount { get; set; }

        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("creationTime")]
        public long CreationTime { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}
