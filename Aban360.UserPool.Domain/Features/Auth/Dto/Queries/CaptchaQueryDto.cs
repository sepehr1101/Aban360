namespace Aban360.UserPool.Domain.Features.Auth.Dto.Queries
{
    public sealed record CaptchaQueryDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public short LanguageId { get; set; }
        public short DisplayModeId { get; set; }
        public string DispalyModeTitle { get; set; } = default!;
        public string LanguageTitle { get; set; } = default!;
        public bool ShowThousandSeperator { get; set; }
        public string FontName { get; set; } = null!;
        public int FontSize { get; set; }
        public string ForeColor { get; set; } = null!;
        public string BackColor { get; set; } = null!;
        public int ExpiresAfter { get; set; }
        public string ValidationMessage { get; set; } = null!;
        public string ValidationMessageClass { get; set; } = null!;
        public int RateLimit { get; set; }
        public string Noise { get; set; } = null!;
        public string EncryptionKey { get; set; } = null!;
        public string NonceKey { get; set; } = null!;
        public string Direction { get; set; } = null!;
        public int Min { get; set; }
        public int Max { get; set; }
        public bool IsSelected { get; set; }
    }
}
