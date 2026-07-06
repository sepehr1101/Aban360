using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;

namespace Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts
{
    public interface IZaribCQueryService
    {
        Task<ZaribCQueryDto> Get(string from, string to);
        Task<ZaribCQueryDto> GetLatest(string @from, string @to);
        Task<IEnumerable<ZaribCQueryDto>> GetZaribC();
        Task<ZaribCQueryDto> Get(string currentDateJalali);
        Task<ZaribCQueryDto> Get(int id);
    }
}