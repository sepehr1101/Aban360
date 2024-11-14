namespace Aban360.UserPool.Domain.Features.Auth.Entities;

public partial class CaptchaLanguage
{
    public short Id { get; set; }

    public string Name { get; set; } = null!;

    public string Title { get; set; } = null!;

    public virtual ICollection<Captcha> Captchas { get; set; } = new List<Captcha>();
}
