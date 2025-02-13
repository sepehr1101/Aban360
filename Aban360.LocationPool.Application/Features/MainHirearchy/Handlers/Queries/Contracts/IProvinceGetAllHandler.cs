using Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Queries;

namespace Aban360.LocationPool.Application.Features.MainHirearchy.Handlers.Queries.Contracts
{
    public interface IProvinceGetAllHandler
    {
        Task<ICollection<ProvinceGetDto>> Handle(CancellationToken cancellationToken);
    }
}
