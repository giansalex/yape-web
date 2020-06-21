using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Yape.Sdk.Entity
{
    public class OrderResult
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("response")]
        public Payment Response { get; set; }

        [JsonPropertyName("errors")]
        public object[] Errors { get; set; }
    }
}
