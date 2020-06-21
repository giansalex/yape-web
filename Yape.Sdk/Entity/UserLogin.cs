using System.Text.Json.Serialization;

namespace Yape.Sdk.Entity
{
    public class UserLogin
    {
        [JsonPropertyName("pinText")]
        public string[] PinText { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("uuid")]
        public string Uuid { get; set; }
    }
}
