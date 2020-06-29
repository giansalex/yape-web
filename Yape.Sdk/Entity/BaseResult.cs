using System.Text.Json.Serialization;

namespace Yape.Sdk.Entity
{
    public class BaseResult
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("errors")]
        public string[] Errors { get; set; }
    }
}
