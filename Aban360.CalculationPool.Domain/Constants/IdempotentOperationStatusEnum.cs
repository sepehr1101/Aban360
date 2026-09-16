namespace Aban360.CalculationPool.Domain.Constants
{
    public enum IdempotentOperationStatusEnum : byte
    {
        Processing = 1,
        Completed = 2,
        Failed = 3
    }
}
