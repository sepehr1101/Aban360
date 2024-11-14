namespace Aban360.UserPool.Domain.Features.Auth.Dto.Queries
{
    public record CaptchaApiResponse
    {
        public string ImageUrl { get; } = null!;
        private string Id { get; } = null!;
        public string CaptchaText { get; } = null!;
        public string CaptchaToken { get; } = null!;
        public CaptchaApiResponse(string imgUrl, string id, string text, string token)
        {
            ImageUrl= imgUrl;
            Id=id;
            CaptchaToken = token;
            CaptchaText=text;
        }
    }
}
