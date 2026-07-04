using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;

namespace Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts
{
    public interface IAbonmanQueryService
    {
        Task<IEnumerable<AbonmanGetDto>> Get();
        Task<AbonmanGetDto> Get(int id);
    }
}