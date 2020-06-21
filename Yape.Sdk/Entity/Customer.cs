using System.Text.Json.Serialization;

namespace Yape.Sdk.Entity
{
    public partial class Customer
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("response")]
        public CustomerResponse Response { get; set; }

        [JsonPropertyName("errors")]
        public object[] Errors { get; set; }
    }

    public partial class CustomerResponse
    {
        [JsonPropertyName("cashtag")]
        public string Cashtag { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("transferType")]
        public string TransferType { get; set; }
    }
}
