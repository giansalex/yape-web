using System.Text.Json.Serialization;

namespace Yape.Sdk.Entity
{
    public partial class CustomerResult
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("response")]
        public Customer Response { get; set; }

        [JsonPropertyName("errors")]
        public string[] Errors { get; set; }
    }

    public partial class Customer
    {
        [JsonPropertyName("cashtag")]
        public string Cashtag { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("transferType")]
        public string TransferType { get; set; }
    }
}
