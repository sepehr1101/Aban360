using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Contracts
{
    public interface IMunicipalityGetSingleHandler
    {
        Task<MunicipalityGetDto> Handle(int id, CancellationToken cancellationToken);
    }
}
