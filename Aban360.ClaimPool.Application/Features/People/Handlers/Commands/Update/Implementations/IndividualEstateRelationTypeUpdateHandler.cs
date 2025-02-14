using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Update.Implementations
{
    public class IndividualEstateRelationTypeUpdateHandler : IIndividualEstateRelationTypeUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IIndividualEstateRelationTypeQueryService _queryService;
        public IndividualEstateRelationTypeUpdateHandler(
            IMapper mapper,
            IIndividualEstateRelationTypeQueryService queryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task Handle(IndividualEstateRelationTypeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var individualEstateRelationType = await _queryService.Get(updateDto.Id);
            if (individualEstateRelationType == null)
            {
                throw new InvalidDataException();
            }
            _mapper.Map(updateDto, individualEstateRelationType);
        }
    }
}
