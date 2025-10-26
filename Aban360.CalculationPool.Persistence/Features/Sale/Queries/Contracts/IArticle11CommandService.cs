using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;

namespace Aban360.CalculationPool.Persistence.Features.Sale.Queries.Contracts
{
    public interface IArticle11CommandService
    {
        Task Create(Article11InputDto input);
    }
}
