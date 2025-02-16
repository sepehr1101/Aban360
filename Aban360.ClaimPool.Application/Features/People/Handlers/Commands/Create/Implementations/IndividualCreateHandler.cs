using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.People.Entities;
using Aban360.ClaimPool.Persistence.Features.People.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Create.Implementations
{
    public class IndividualCreateHandler : IIndividualCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IIndividualCommandService _commandService;
        public IndividualCreateHandler(
            IMapper mapper,
            IIndividualCommandService commandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _commandService = commandService;
            _commandService.NotNull(nameof(commandService));
        }

        public async Task Handle(IndividualCreateDto createDto, CancellationToken cancellationToken)
        {
            var individual = _mapper.Map<Individual>(createDto);
            individual.Hash = " "; //todo Hash in not null
            await _commandService.Add(individual);
        }
    }
}
