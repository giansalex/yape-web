using System;
using System.Text.Json.Serialization;

namespace Yape.Sdk.Entity
{
    public partial class Identity
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("response")]
        public AuthResponse Response { get; set; }

        [JsonPropertyName("errors")]
        public object[] Errors { get; set; }
    }

    public partial class AuthResponse
    {
        [JsonPropertyName("authToken")]
        public AuthToken AuthToken { get; set; }

        [JsonPropertyName("hasCard")]
        public bool HasCard { get; set; }

        [JsonPropertyName("updateTerms")]
        public bool UpdateTerms { get; set; }

        [JsonPropertyName("maxAmount")]
        public string MaxAmount { get; set; }

        [JsonPropertyName("hasEmail")]
        public bool HasEmail { get; set; }

        [JsonPropertyName("hasNews")]
        public bool HasNews { get; set; }

        [JsonPropertyName("userInfo")]
        public UserInfo UserInfo { get; set; }

        [JsonPropertyName("fullName")]
        public string FullName { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("documentType")]
        public object DocumentType { get; set; }

        [JsonPropertyName("ownerProvider")]
        public string OwnerProvider { get; set; }
    }

    public partial class AuthToken
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonPropertyName("expires_in")]
        public long ExpiresIn { get; set; }

        [JsonPropertyName("scope")]
        public string Scope { get; set; }
    }

    public partial class UserInfo
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }
    }
}
