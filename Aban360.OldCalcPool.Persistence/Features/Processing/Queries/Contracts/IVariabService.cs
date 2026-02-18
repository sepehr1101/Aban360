namespace Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts
{
    public interface IVariabService
    {
        Task<decimal> GetAndRenew(int zoneId);
        Task<decimal[]> GetAndRenew(int zoneId, int count);
        Task<bool> IsOperationValid(int zoneId, string operationDate);
    }
}