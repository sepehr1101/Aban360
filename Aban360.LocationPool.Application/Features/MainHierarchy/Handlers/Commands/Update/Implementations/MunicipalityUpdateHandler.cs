using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Update.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Update.Implementations
{
    internal sealed class MunicipalityUpdateHandler : IMunicipalityUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IMunicipalityQueryService _municipalqueryService;
        public MunicipalityUpdateHandler(
            IMapper mapper,
            IMunicipalityQueryService municipalqueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _municipalqueryService = municipalqueryService;
            _municipalqueryService.NotNull(nameof(municipalqueryService));
        }

        public async Task Handel(MunicipalityUpdateDto updateDto, CancellationToken cancellationToken)
        {
            Municipality municipality = await _municipalqueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, municipality);
        }
    }
}
