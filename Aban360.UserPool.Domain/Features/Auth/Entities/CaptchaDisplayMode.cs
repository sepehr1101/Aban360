namespace Aban360.UserPool.Domain.Features.Auth.Entities;

public partial class CaptchaDisplayMode
{
    public short Id { get; set; }

    public string Name { get; set; } = null!;

    public string Tite { get; set; } = null!;

    public virtual ICollection<Captcha> Captchas { get; set; } = new List<Captcha>();
}
