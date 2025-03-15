using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.People.Entities;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Update.Implementations
{
    internal sealed class IndividualTypeUpdateHandler : IIndividualTypeUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IIndividualTypeQueryService _queryService;
        public IndividualTypeUpdateHandler(
            IMapper mapper,
            IIndividualTypeQueryService queryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task Handle(IndividualTypeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            IndividualType individualType = await _queryService.Get(updateDto.Id);
            _mapper.Map(updateDto, individualType);
        }
    }
}
