namespace Aban360.UserPool.Domain.Features.Auth.Dto.Commands
{
    public record RefreshToken
    {
        public string Value { get; set; }
        public RefreshToken(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new ArgumentNullException(nameof(refreshToken));
            }
            Value = refreshToken;
        }
        public RefreshToken()
        {
                
        }
    }
}
