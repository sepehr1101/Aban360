using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.People.Entities;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Update.Implementations
{
    internal sealed class IndividualTagUpdateHandler : IIndividualTagUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IIndividualTagQueryService _IndividualTagQueryService;
        public IndividualTagUpdateHandler(
            IMapper mapper,
            IIndividualTagQueryService IndividualTagQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _IndividualTagQueryService = IndividualTagQueryService;
            _IndividualTagQueryService.NotNull(nameof(IndividualTagQueryService));
        }

        public async Task Handle(IndividualTagUpdateDto updateDto, CancellationToken cancellationToken)
        {
            IndividualTag individualTag = await _IndividualTagQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, individualTag);
        }
    }
}
