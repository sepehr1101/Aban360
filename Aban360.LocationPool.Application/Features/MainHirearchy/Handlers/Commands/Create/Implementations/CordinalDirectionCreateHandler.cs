using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHirearchy.Handlers.Commands.Create.Contracts;
using Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHirearchy.Entities;
using Aban360.LocationPool.Persistence.Features.MainHirearchy.Commands.Contracts;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHirearchy.Handlers.Commands.Create.Implementations
{
    public class CordinalDirectionCreateHandler : ICordinalDirectionCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly ICordinalDirectionCommandService _cordinalDirectionCommandService;
        public CordinalDirectionCreateHandler(
            IMapper mapper, 
            ICordinalDirectionCommandService commandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _cordinalDirectionCommandService = commandService;
            _cordinalDirectionCommandService.NotNull(nameof(commandService));
        }


        public async Task Handle(CordinalDirectionCreateDto createDto, CancellationToken cancellationToken)
        {
            var cordinalDirection = _mapper.Map<CordinalDirection>(createDto);
            await _cordinalDirectionCommandService.Add(cordinalDirection);
        }
    }
}
