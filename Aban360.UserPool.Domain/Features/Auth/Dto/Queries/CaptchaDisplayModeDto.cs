namespace Aban360.UserPool.Domain.Features.Auth.Entities;

public record CaptchaDisplayModeDto
{
    public short Id { get; set; }
    public string Title { get; set; } = null!;
}
