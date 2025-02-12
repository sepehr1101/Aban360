namespace Aban360.UserPool.Domain.Constants
{
    public enum TokenFailureTypeEnum : short
    {
        NoActionOrController = 1,
        NoClaims = 2,
        NoSerial = 3,
        NoUserId = 4,
        Expired = 5,
        NoTokenInDb = 6,
        NoAccess = 7,
        DeviceChanged = 8,
        InactiveSession = 9
    }
}
