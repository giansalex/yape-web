using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Yape.Sdk.Entity
{
    public partial class History
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("response")]
        public HistoryResponse Response { get; set; }

        [JsonPropertyName("errors")]
        public object[] Errors { get; set; }
    }

    public partial class HistoryResponse
    {
        [JsonPropertyName("entities")]
        public HistoryItem[] Entities { get; set; }

        [JsonPropertyName("size")]
        public int Size { get; set; }

        [JsonPropertyName("limit")]
        public int Limit { get; set; }

        [JsonPropertyName("offset")]
        public int Offset { get; set; }

        [JsonPropertyName("pageNumber")]
        public int PageNumber { get; set; }
    }

    public partial class HistoryItem
    {
        [JsonPropertyName("time")]
        public long Time { get; set; }

        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("contactFrom")]
        public Contact ContactFrom { get; set; }

        [JsonPropertyName("contactTo")]
        public Contact ContactTo { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("year")]
        public int Year { get; set; }

        [JsonPropertyName("month")]
        public int Month { get; set; }

        [JsonPropertyName("bonus")]
        public bool Bonus { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
    }

    public partial class Contact
    {
        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        [JsonPropertyName("cashtag")]
        public string Cashtag { get; set; }

        [JsonPropertyName("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
