using Aban360.ClaimPool.Application.Features.Registration.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Registration.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Registration.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Registration.Handlers.Queries.Implementations
{
    public class SubscriptionGetAllHandler : ISubscriptionGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly ISubscriptionQueryService _queryService;
        public SubscriptionGetAllHandler(
            IMapper mapper,
            ISubscriptionQueryService queryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task<ICollection<SubscriptionGetDto>> Handle(CancellationToken cancellationToken)
        {
            var subscription = await _queryService.Get();
            if (subscription == null)
            {
                throw new InvalidDataException();
            }
            return _mapper.Map<ICollection<SubscriptionGetDto>>(subscription);
        }
    }
}
