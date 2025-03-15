using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Create.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Commands.Contracts;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Create.Implementations
{
    internal sealed class CordinalDirectionCreateHandler : ICordinalDirectionCreateHandler
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
            CordinalDirection cordinalDirection = _mapper.Map<CordinalDirection>(createDto);
            await _cordinalDirectionCommandService.Add(cordinalDirection);
        }
    }
}
