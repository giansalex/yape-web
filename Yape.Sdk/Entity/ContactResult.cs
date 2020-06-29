using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Yape.Sdk.Entity
{
    public class ContactResult
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("response")]
        public PhoneResult Response { get; set; }

        [JsonPropertyName("errors")]
        public string[] Errors { get; set; }
    }

    public class PhoneResult
    {
        [JsonPropertyName("entities")]
        public string[] Phones { get; set; }

        [JsonPropertyName("size")]
        public int Size { get; set; }
    }
}
