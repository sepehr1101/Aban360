using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;

namespace Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts
{
    public interface ISQueryService
    {
        Task<IEnumerable<SGetDto>> Get(string from, string to);
        Task<IEnumerable<SGetDto>> Get();
        Task<SGetDto> Get(int id);
        Task<SGetDto> Get(string currentDateJalali);
    }
}
