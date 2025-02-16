using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class Captcha
{
    public int Id { get; set; }

    public short CaptchaLanguageId { get; set; }

    public short CaptchaDisplayModeId { get; set; }

    public bool ShowThousandSeperator { get; set; }

    public string FontName { get; set; } = null!;

    public int FontSize { get; set; }

    public string ForeColor { get; set; } = null!;

    public string BackColor { get; set; } = null!;

    public int ExpiresAfter { get; set; }

    public int RateLimit { get; set; }

    public string Noise { get; set; } = null!;

    public string EncryptionKey { get; set; } = null!;

    public string NonceKey { get; set; } = null!;

    public string Direction { get; set; } = null!;

    public int Min { get; set; }

    public int Max { get; set; }

    public string Title { get; set; } = null!;

    public bool IsSelected { get; set; }

    public virtual CaptchaDisplayMode CaptchaDisplayMode { get; set; } = null!;

    public virtual CaptchaLanguage CaptchaLanguage { get; set; } = null!;
}
