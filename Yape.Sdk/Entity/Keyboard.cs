using System.Text.Json.Serialization;

namespace Yape.Sdk.Entity
{
    public class Keyboard
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("response")]
        public string[] Response { get; set; }
    }
}
