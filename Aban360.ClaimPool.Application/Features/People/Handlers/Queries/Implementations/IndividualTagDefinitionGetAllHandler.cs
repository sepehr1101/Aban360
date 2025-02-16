using Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.Common.Db.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Implementations
{
    public class IndividualTagDefinitionGetAllHandler : IIndividualTagDefinitionGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly IIndividualTagDefinitionQueryService _queryService;
        public IndividualTagDefinitionGetAllHandler(
            IMapper mapper,
            IIndividualTagDefinitionQueryService queryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task<ICollection<IndividualTagDefinitionGetDto>> Handle(CancellationToken cancellationToken)
        {
            var individualTagDefinition = await _queryService.Get();
            if (individualTagDefinition == null)
            {
                throw new InvalidIdException();//todo : exception
            }
            return _mapper.Map<ICollection<IndividualTagDefinitionGetDto>>(individualTagDefinition);
        }
    }
}
