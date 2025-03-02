using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Implementations
{
    public class ProfessionCreateHandler : IProfessionCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IProfessionCommandService _commandService;
        public ProfessionCreateHandler(
            IMapper mapper,
            IProfessionCommandService commandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _commandService = commandService;
            _commandService.NotNull(nameof(commandService));
        }

        public async Task Handle(ProfessionCreateDto createDto, CancellationToken cancellationToken)
        {
            var profession = _mapper.Map<Profession>(createDto);
            await _commandService.Add(profession);
        }
    }
}
