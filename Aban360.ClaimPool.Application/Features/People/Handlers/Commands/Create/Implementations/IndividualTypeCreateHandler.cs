using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.People.Entities;
using Aban360.ClaimPool.Persistence.Features.People.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Create.Implementations
{
    public class IndividualTypeCreateHandler : IIndividualTypeCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IIndividualTypeCommandService _commandService;
        public IndividualTypeCreateHandler(
            IMapper mapper,
            IIndividualTypeCommandService commandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _commandService = commandService;
            _commandService.NotNull(nameof(commandService));
        }

        public async Task Handle(IndividualTypeCreateDto createDto, CancellationToken cancellationToken)
        {
            var individualType = _mapper.Map<IndividualType>(createDto);
            await _commandService.Add(individualType);
        }
    }
}
