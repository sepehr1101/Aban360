using AutoMapper;
using Aban360.Common.Extensions;
using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.WasteWater.Commands.Contracts;
using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.WasteWater.Entities;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Create.Implementations
{
    internal sealed class SiphonTypeCreateHandler : ISiphonTypeCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly ISiphonTypeCommandService _commandService;
        public SiphonTypeCreateHandler(
            IMapper mapper,
            ISiphonTypeCommandService commandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _commandService = commandService;
            _commandService.NotNull(nameof(commandService));
        }

        public async Task Handle(SiphonTypeCreateDto createDto, CancellationToken cancellationToken)
        {
            SiphonType SiphonType = _mapper.Map<SiphonType>(createDto);
            await _commandService.Add(SiphonType);
        }
    }
}
