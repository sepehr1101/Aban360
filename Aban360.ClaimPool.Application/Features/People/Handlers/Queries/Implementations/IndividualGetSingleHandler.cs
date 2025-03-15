using Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.People.Entities;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Implementations
{
    public class IndividualGetSingleHandler : IIndividualGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IIndividualQueryService _queryService;
        public IndividualGetSingleHandler(
            IMapper mapper,
            IIndividualQueryService queryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task<IndividualGetDto> Handle(int id, CancellationToken cancellationToken)
        {
            Individual individual = await _queryService.Get(id);
            return _mapper.Map<IndividualGetDto>(individual);
        }
    }
}
