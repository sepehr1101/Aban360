namespace Aban360.UserPool.Domain.Constants
{
    public enum LogoutReasonEnum : short
    {
        ByUser = 1,
        ByAdmin = 2,
        PasswordChange = 3,
        EditByAdmin = 4,
        ExpiredToken = 5,
        ChangeIpInSession = 6,
        ChangeClientMeta = 7,
        ConcurrentLogin = 8,
    }
}
