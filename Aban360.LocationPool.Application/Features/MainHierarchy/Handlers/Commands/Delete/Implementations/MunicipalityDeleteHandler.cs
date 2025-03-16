using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Delete.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Commands.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Delete.Implementations
{
    internal sealed class MunicipalityDeleteHandler : IMunicipalityDeleteHandler
    {
        private readonly IMunicipalityQueryService _municipalqueryService;
        private readonly IMunicipalityCommandService _municipalCommandService;
        public MunicipalityDeleteHandler(
            IMunicipalityQueryService municipalqueryService,
            IMunicipalityCommandService municipalCommandService)
        {
            _municipalqueryService = municipalqueryService;
            _municipalqueryService.NotNull(nameof(municipalqueryService));

            _municipalCommandService = municipalCommandService;
            _municipalCommandService.NotNull(nameof(municipalCommandService));
        }

        public async Task Handel(MunicipalityDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            Municipality municipality = await _municipalqueryService.Get(deleteDto.Id);
            await _municipalCommandService.Remove(municipality);
        }
    }
}
