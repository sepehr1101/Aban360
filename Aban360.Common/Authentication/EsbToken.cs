using System.Text.Json.Serialization;

namespace Aban360.Common.Authentication
{
    public sealed class EsbToken
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = default!;

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; } = default!;

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }
    }
}
