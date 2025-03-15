using Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.People.Entities;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Implementations
{
    internal sealed class IndividualTagGetSinglBySearchInputeHandler : IIndividualTagGetSinglBySearchInputeHandler
    {
        private readonly IMapper _mapper;
        private readonly IIndividualTagQueryService _IndividualTagQueryService;
        public IndividualTagGetSinglBySearchInputeHandler(
            IMapper mapper,
            IIndividualTagQueryService IndividualTagQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _IndividualTagQueryService = IndividualTagQueryService;
            _IndividualTagQueryService.NotNull(nameof(IndividualTagQueryService));
        }

        public async Task<IndividualTagGetDto> Handle(string input, CancellationToken cancellationToken)
        {
            ICollection<IndividualTag> IndividualTag = await _IndividualTagQueryService.Get(input);
            return _mapper.Map<IndividualTagGetDto>(IndividualTag);
        }
    }
}
