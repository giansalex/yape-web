using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Yape.Sdk.Entity
{
    public partial class Balance
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("response")]
        public BalanceResponse Response { get; set; }

        [JsonPropertyName("errors")]
        public object[] Errors { get; set; }
    }

    public partial class BalanceResponse
    {
        [JsonPropertyName("amount")]
        public string Amount { get; set; }

        [JsonPropertyName("currencyCode")]
        public string CurrencyCode { get; set; }
    }
}
