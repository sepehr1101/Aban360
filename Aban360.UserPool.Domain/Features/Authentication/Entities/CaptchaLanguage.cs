using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Features.Authentication;

public partial class CaptchaLanguage
{
    public short Id { get; set; }

    public string Name { get; set; } = null!;

    public string Title { get; set; } = null!;

    public virtual ICollection<Captcha> Captchas { get; set; } = new List<Captcha>();
}
