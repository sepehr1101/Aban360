using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.People.Entities;
using Aban360.ClaimPool.Persistence.Features.People.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Create.Implementations
{
    internal sealed class IndividualEstateRelationTypeCreateHandler : IIndividualEstateRelationTypeCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IIndividualEstateRelationTypeCommandService _commandService;
        public IndividualEstateRelationTypeCreateHandler(
            IMapper mapper,
            IIndividualEstateRelationTypeCommandService commandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _commandService = commandService;
            _commandService.NotNull(nameof(commandService));
        }

        public async Task Handle(IndividualEstateRelationTypeCreateDto createDto, CancellationToken cancellationToken)
        {
            IndividualEstateRelationType individualEstateRelationType = _mapper.Map<IndividualEstateRelationType>(createDto);
            await _commandService.Add(individualEstateRelationType);
        }
    }
}
