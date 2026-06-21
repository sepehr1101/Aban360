using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;

namespace Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts
{
    public interface IConCompanyQueryService
    {
        Task<IEnumerable<ConCompanyGetDto>> Get();
        Task<ConCompanyGetDto> Get(int id);
        Task<ConCompanyPersonnelGetDto> GetPersonnel(int id);
        Task<IEnumerable<ConCompanyPersonnelGetDto>> GetPersonnel();
        Task<ConCompanyPersonnelPersonalGetDto> GetPersonnelById(int companyId, Guid personnelId);
        Task<int> GetPersonnelIndex(int companyId, Guid personnelId);
    }
}
