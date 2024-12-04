using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.UserPool.Domain.Features.Auth.Entities;

[Table(nameof(UserLogin))]
public class UserLogin
{
    public long Id { get; set; }
    public string Username { get; set; } = null!;
    public Guid? UserId { get; set; }
    public DateTime FirstStepDateTime { get; set; }
    public string Ip { get; set; } = null!;
    public bool FirstStepSuccess { get; set; }
    public string? WrongPassword { get; set; }
    public string AppVersion { get; set; } = null!;
    public string? TwoStepCode { get; set; }
    public DateTime? TwoStepExpireDateTime { get; set; }
    public DateTime? TwoStepInsertDateTime { get; set; }
    public bool? TwoStepWasSuccessful { get; set; }
    public bool PreviousFailureIsShown { get; set; }
    public DateTime? LogoutDateTime { get; set; }
    public int? LogoutReasonId { get; set; }
    public virtual User? User { get; set; }
}