namespace Aban360.OldCalcPool.Domain.Constants
{
    public enum NowGenerateBillErrorCodeEnum
    {
        Success = 0,
        Low = 1,
        High = 2,
        PreNumberHigherThanTodayNumber = 3,
        PreNumberEqualTodayNumber = 4,
        NotAvailableNow = 8,
        InvalidDeletionState = 9,
        BadRequest = 400,
    }
}
