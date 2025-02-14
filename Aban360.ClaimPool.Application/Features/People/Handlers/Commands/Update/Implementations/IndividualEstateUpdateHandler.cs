using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Update.Implementations
{
    public class IndividualEstateUpdateHandler : IIndividualEstateUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IIndividualEstateQueryService _queryService;
        public IndividualEstateUpdateHandler(
            IMapper mapper,
            IIndividualEstateQueryService queryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task Handle(IndividualEstateUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var individualEstate = await _queryService.Get(updateDto.Id);
            if (individualEstate == null)
            {
                throw new InvalidDataException();
            }
            _mapper.Map(updateDto, individualEstate);
        }
    }
}
