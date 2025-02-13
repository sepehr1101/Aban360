using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHirearchy.Handlers.Commands.Delete.Contracts;
using Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Commands;
using Aban360.LocationPool.Persistence.Features.MainHirearchy.Commands.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHirearchy.Queries.Contracts;

namespace Aban360.LocationPool.Application.Features.MainHirearchy.Handlers.Commands.Delete.Implementations
{
    public class MunicipalityDeleteHandler : IMunicipalityDeleteHandler
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
            var municipality = await _municipalqueryService.Get(deleteDto.Id);
            if (municipality == null)
            {
                throw new InvalidDataException();
            }
            await _municipalCommandService.Remove(municipality);
        }
    }
}
