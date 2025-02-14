using AutoMapper;
using Aban360.Common.Extensions;
using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.WasteWater;
using Aban360.ClaimPool.Persistence.Features.WasteWater.Commands.Contracts;
using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Create.Contracts;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Create.Implementations
{
    public class SiphonMaterialCreateHandler : ISiphonMaterialCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly ISiphonMaterialCommandService _commandService;
        public SiphonMaterialCreateHandler(
            IMapper mapper,
            ISiphonMaterialCommandService commandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _commandService = commandService;
            _commandService.NotNull(nameof(commandService));
        }

        public async Task Handle(SiphonMaterialCreateDto createDto, CancellationToken cancellationToken)
        {
            var SiphonMaterial = _mapper.Map<SiphonMaterial>(createDto);
            await _commandService.Add(SiphonMaterial);
        }
    }
}
