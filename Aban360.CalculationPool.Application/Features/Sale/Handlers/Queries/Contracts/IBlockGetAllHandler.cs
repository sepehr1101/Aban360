namespace Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts
{
    public interface IBlockGetAllHandler
    {
        Task<IEnumerable<string>> Handle(CancellationToken cancellationToken);
    }
}
