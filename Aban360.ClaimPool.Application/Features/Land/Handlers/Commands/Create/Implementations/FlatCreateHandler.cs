using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Implementations
{
    public class FlatCreateHandler : IFlatCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IFlatCommandService _commandService;
        public FlatCreateHandler(
            IMapper mapper,
            IFlatCommandService commandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _commandService = commandService;
            _commandService.NotNull(nameof(commandService));
        }

        public async Task Handle(FlatCreateDto createDto, CancellationToken cancellationToken)
        {
            var flat = _mapper.Map<Flat>(createDto);
            await _commandService.Add(flat);
        }
    }
}
