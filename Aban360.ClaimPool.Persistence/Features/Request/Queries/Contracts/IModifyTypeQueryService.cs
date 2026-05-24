using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;

namespace Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts
{
    public interface IModifyTypeQueryService
    {
        Task<IEnumerable<ModifyTypeGetDto>> Get();
        Task<ModifyTypeGetDto> GetByRequestBillDetails(int id);
        Task<ModifyTypeGetDto> GetByKarten75(int id);
    }
}
