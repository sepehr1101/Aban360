using Aban360.ClaimPool.Application.Features.Registration.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Registration.Dto.Command;
using Aban360.ClaimPool.Persistence.Features.Registration.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Registration.Handlers.Commands.Update.Implementations
{
    public class SubscriptionUpdateHandler : ISubscriptionUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly ISubscriptionQueryService _queryService;
        public SubscriptionUpdateHandler(
            IMapper mapper,
            ISubscriptionQueryService queryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task Handle(SubscriptionUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var subscription = await _queryService.Get(updateDto.Id);
            if (subscription == null)
            {
                throw new InvalidDataException();
            }
            _mapper.Map(updateDto, subscription);
        }
    }
}
