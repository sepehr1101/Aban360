namespace Aban360.UserPool.Domain.Features.Auth.Dto.Queries
{
    public sealed record CaptchaParams
    {
        public string ImageUrl { get; } = default!;
        public string TokenText { get; } = default!;
        public CaptchaParams(string imageUrl, string tokenText)
        {
            ImageUrl = imageUrl;
            TokenText = tokenText;
        }
    }
}
