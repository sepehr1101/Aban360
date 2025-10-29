namespace Aban360.CalculationPool.Persistence.Features.Sale.Queries.Contracts
{
    public interface IOfferingQueryService
    {
        Task<string> Get(short id);
    }
}
