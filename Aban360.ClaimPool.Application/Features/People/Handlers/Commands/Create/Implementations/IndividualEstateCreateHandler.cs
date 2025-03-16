using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Base;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.People.Entities;
using Aban360.ClaimPool.Persistence.Features.People.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Create.Implementations
{
    public class IndividualEstateCreateHandler : IIndividualEstateCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IIndividualEstateCommandService _commandService;
        public IndividualEstateCreateHandler(
            IMapper mapper,
            IIndividualEstateCommandService commandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _commandService = commandService;
            _commandService.NotNull(nameof(commandService));
        }

        public async Task Handle(IndividualEstateCreateDto createDto, CancellationToken cancellationToken)
        {
            var individualEstate = _mapper.Map<IndividualEstate>(createDto);
            await _commandService.Add(individualEstate);
        }
    }
}
