using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Implementations
{
    internal sealed class ConstructionTypeCreateHandler : IConstructionTypeCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IConstructionTypeCommandService _commandService;
        public ConstructionTypeCreateHandler(
            IMapper mapper,
            IConstructionTypeCommandService commandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _commandService = commandService;
            _commandService.NotNull(nameof(_commandService));
        }

        public async Task Handle(ConstructionTypeCreateDto createDto, CancellationToken cancellationToken)
        {
            ConstructionType constructionType = _mapper.Map<ConstructionType>(createDto);
            await _commandService.Add(constructionType);
        }
    }

}
