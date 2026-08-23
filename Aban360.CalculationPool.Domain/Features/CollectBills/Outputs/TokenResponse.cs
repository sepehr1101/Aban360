using System.Text.Json.Serialization;

namespace Aban360.CalculationPool.Domain.Features.CollectBills.Outputs
{
    public class TokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = default!;

        [JsonPropertyName("scope")]
        public string Scope { get; set; } = default!;

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; } = default!;

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }
    }
}