using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.UserPool.Domain.Features.Auth.Entities;

[Table(nameof(Captcha))]
public class Captcha
{
    public int Id { get; set; }
    public short CaptchaLanguageId { get; set; }
    public short CaptchaDisplayModeId { get; set; }
    public bool ShowThousandSeperator { get; set; }

    //public bool ShowRefreshButton { get; set; }

    //public string RefreshButtonClass { get; set; } = null!;
    public string FontName { get; set; } = null!;
    public int FontSize { get; set; }
    public string ForeColor { get; set; } = null!;
    public string BackColor { get; set; } = null!;
    public int ExpiresAfter { get; set; }

    //public string ValidationMessage { get; set; } = null!;

    //public string ValidationMessageClass { get; set; } = null!;
    public int RateLimit { get; set; }

    //public string RateLimitMessage { get; set; } = null!;
    public string Noise { get; set; } = null!;
    public string EncryptionKey { get; set; } = null!;
    public string NonceKey { get; set; } = null!;
    public string Direction { get; set; } = null!;
    public int Min { get; set; }
    public int Max { get; set; }

    //public string InputPlaceholder { get; set; } = null!;

    //public string HiddenInputName { get; set; } = null!;

    //public string HiddenTokenName { get; set; } = null!;

    //public string InputName { get; set; } = null!;

    //public string InputClass { get; set; } = null!;

    //public string InputTemplate { get; set; } = null!;

    //public string Identifier { get; set; } = null!;

    public virtual CaptchaDisplayMode CaptchaDisplayMode { get; set; } = null!;

    public virtual CaptchaLanguage CaptchaLanguage { get; set; } = null!;
}
