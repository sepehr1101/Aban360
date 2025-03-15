using Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.People.Entities;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Implementations
{
    internal sealed class IndividualGetAllHandler : IIndividualGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly IIndividualQueryService _queryService;
        public IndividualGetAllHandler(
            IMapper mapper,
            IIndividualQueryService queryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task<ICollection<IndividualGetDto>> Handle(CancellationToken cancellationToken)
        {
            ICollection<Individual> individual = await _queryService.Get();
            return _mapper.Map<ICollection<IndividualGetDto>>(individual);
        }
    }
}
