using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;

namespace Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts
{
    public interface IZaribCQueryService
    {
        Task<ZaribCQueryDto> GetZaribC(string from, string to);
    }
}