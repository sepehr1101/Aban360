using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Create.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Commands.Contracts;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Create.Implementations
{
    internal sealed class MunicipalityCreateHandler : IMunicipalityCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IMunicipalityCommandService _municipalCommandService;
        public MunicipalityCreateHandler(
            IMapper mapper,
            IMunicipalityCommandService municipalCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _municipalCommandService = municipalCommandService;
            _municipalCommandService.NotNull(nameof(municipalCommandService));
        }

        public async Task Handle(MunicipalityCreateDto createDto, CancellationToken cancellationToken)
        {
            Municipality municipality = _mapper.Map<Municipality>(createDto);
            await _municipalCommandService.Add(municipality);
        }
    }
}
