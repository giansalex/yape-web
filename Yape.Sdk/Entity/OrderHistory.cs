using System.Text.Json.Serialization;

namespace Yape.Sdk.Entity
{
    public class OrderHistory
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("response")]
        public Payment[] Response { get; set; }

        [JsonPropertyName("errors")]
        public object[] Errors { get; set; }
    }
}
