using Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.People.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Implementations
{
    public class IndividualEstateRelationTypeGetSingelHandler : IIndividualEstateRelationTypeGetSingelHandler
    {
        private readonly IMapper _mapper;
        private readonly IIndividualEstateRelationTypeQueryService _queryService;
        public IndividualEstateRelationTypeGetSingelHandler(
            IMapper mapper,
            IIndividualEstateRelationTypeQueryService queryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task<IndividualEstateRelationTypeGetDto> Handle(IndividualEstateRelationEnum id, CancellationToken cancellationToken)
        {
            var individualEstateRelationType = await _queryService.Get(id);
            if (individualEstateRelationType == null)
            {
                throw new InvalidDataException();
            }
            return _mapper.Map<IndividualEstateRelationTypeGetDto>(individualEstateRelationType);
        }
    }
}
