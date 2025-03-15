using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.People.Entities;
using Aban360.ClaimPool.Persistence.Features.People.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Create.Implementations
{
    internal sealed class IndividualTagCreateHandler : IIndividualTagCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IIndividualTagCommandService _individualTagCommandService;
        public IndividualTagCreateHandler(
            IMapper mapper,
            IIndividualTagCommandService IndividualTagCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _individualTagCommandService = IndividualTagCommandService;
            _individualTagCommandService.NotNull(nameof(IndividualTagCommandService));
        }

        public async Task Handle(IndividualTagCreateDto createDto, CancellationToken cancellationToken)
        {
            IndividualTag individualTag = _mapper.Map<IndividualTag>(createDto);
            individualTag.InsertLogInfo = " ";//todo
            individualTag.ValidFrom=DateTime.Now;
            await _individualTagCommandService.Add(individualTag);
        }
    }
}
