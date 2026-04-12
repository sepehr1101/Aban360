using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.Common.BaseEntities;

namespace Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts
{
    public interface IMoshtrakQueryService
    {
        Task CheckOpenRequest(string nationalCode, int zoneId);
        Task CheckOpenRequest(int customerNumber, int zoneId);
        Task<IEnumerable<MoshtrakOutputDto>> Get(MoshtrakGetDto inputDto, MoshtrakSearchTypeEnum searchType);
        Task<IEnumerable<PreviousRequestGetDto>> GetAllRequestByCustomerNumber(ZoneIdAndCustomerNumber inputDto);
    }
}
