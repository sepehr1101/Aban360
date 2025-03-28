﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.UserPool.Domain.Features.Auth.Entities;

[Table(nameof(CaptchaDisplayMode))]
public class CaptchaDisplayMode
{
    public short Id { get; set; }
    public short DisplayModeEnumId { get; set; }
    public string Name { get; set; } = null!;

    public string Title { get; set; } = null!;

    public virtual ICollection<Captcha> Captchas { get; set; } = new List<Captcha>();
}
