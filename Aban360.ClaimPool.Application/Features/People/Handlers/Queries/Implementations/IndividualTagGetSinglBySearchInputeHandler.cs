using Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.Common.Db.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Implementations
{
    public class IndividualTagGetSinglBySearchInputeHandler : IIndividualTagGetSinglBySearchInputeHandler
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
            var IndividualTag = await _IndividualTagQueryService.Get(input);
            if (IndividualTag == null)
            {
                throw new InvalidIdException();
            }
            return _mapper.Map<IndividualTagGetDto>(IndividualTag);
        }
    }
}
