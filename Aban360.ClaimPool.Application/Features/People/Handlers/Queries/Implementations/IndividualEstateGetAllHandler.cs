using Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Implementations
{
    public class IndividualEstateGetAllHandler : IIndividualEstateGetAllHandler
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
            var individualEstate = await _queryService.Get();
            if (individualEstate == null)
            {
                throw new InvalidDataException();
            }
            return _mapper.Map<ICollection<IndividualEstateGetDto>>(individualEstate);
        }
    }
}
