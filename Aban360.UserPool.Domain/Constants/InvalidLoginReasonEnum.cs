namespace Aban360.UserPool.Domain.Constants
{
    public enum InvalidLoginReasonEnum : short
    {
        InvalidUsername = 1,
        InvalidPassword = 2,
        InvalidVerificationCode = 3,
        ExpiredVerificationCode = 4,
        LockedUser = 5,
        InactiveUser = 6,
    }
}
