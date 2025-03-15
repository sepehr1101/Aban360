using Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.People.Entities;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Implementations
{
    internal sealed class IndividualTypeGetSingleHandler : IIndividualTypeGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IIndividualTypeQueryService _queryService;
        public IndividualTypeGetSingleHandler(
            IMapper mapper,
            IIndividualTypeQueryService queryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task<IndividualTypeGetDto> Handle(short id, CancellationToken cancellationToken)
        {
            IndividualType individualType = await _queryService.Get(id);
            return _mapper.Map<IndividualTypeGetDto>(individualType);
        }
    }
}
