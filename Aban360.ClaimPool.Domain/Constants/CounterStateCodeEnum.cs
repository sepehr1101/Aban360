namespace Aban360.ClaimPool.Domain.Constants
{
    public enum CounterStateCodeEnum : int
    {
        Common = 0,
        Malfunction = 1,
        Change = 2,
        Reverse = 3,
        Close = 4,
        NextRound = 5,
        WithoutConsumption = 6,
        Block = 7,
        NonRead = 8,
        DesolateUnit = 9,
        Disconnection = 10,
    }
}
