using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Create.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Commands.Contracts;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Create.Implementations
{
    public class MunicipalityCreateHandler : IMunicipalityCreateHandler
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
            var municipality = _mapper.Map<Municipality>(createDto);
            await _municipalCommandService.Add(municipality);
        }
    }
}
