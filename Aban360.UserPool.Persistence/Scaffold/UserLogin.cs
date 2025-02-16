using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class UserLogin
{
    public Guid Id { get; set; }

    public string Username { get; set; } = null!;

    public Guid? UserId { get; set; }

    public DateTime FirstStepDateTime { get; set; }

    public string Ip { get; set; } = null!;

    public bool FirstStepSuccess { get; set; }

    public short? InvalidLoginReasonId { get; set; }

    public string? WrongPassword { get; set; }

    public string AppVersion { get; set; } = null!;

    public string? TwoStepCode { get; set; }

    public DateTime? TwoStepExpireDateTime { get; set; }

    public DateTime? TwoStepInsertDateTime { get; set; }

    public bool? TwoStepWasSuccessful { get; set; }

    public bool PreviousFailureIsShown { get; set; }

    public DateTime? LogoutDateTime { get; set; }

    public short? LogoutReasonId { get; set; }

    public string LogInfo { get; set; } = null!;

    public virtual InvalidLoginReason? InvalidLoginReason { get; set; }

    public virtual LogoutReason? LogoutReason { get; set; }

    public virtual User? User { get; set; }
}
