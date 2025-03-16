using Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.People.Entities;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Implementations
{
    internal sealed class IndividualEstateGetAllHandler : IIndividualEstateGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly IIndividualEstateQueryService _queryService;
        public IndividualEstateGetAllHandler(
            IMapper mapper,
            IIndividualEstateQueryService queryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task<ICollection<IndividualEstateGetDto>> Handle(CancellationToken cancellationToken)
        {
            ICollection<IndividualEstate> individualEstate = await _queryService.Get();
            return _mapper.Map<ICollection<IndividualEstateGetDto>>(individualEstate);
        }
    }
}
