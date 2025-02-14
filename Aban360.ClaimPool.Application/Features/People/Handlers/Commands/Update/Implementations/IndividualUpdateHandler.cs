using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Update.Implementations
{
    public class IndividualUpdateHandler : IIndividualUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IIndividualQueryService _queryService;
        public IndividualUpdateHandler(
            IMapper mapper,
            IIndividualQueryService queryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task Handle(IndividualUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var individual = await _queryService.Get(updateDto.Id);
            if (individual == null)
            {
                throw new InvalidDataException();
            }
            _mapper.Map(updateDto, individual);
        }
    }
}
