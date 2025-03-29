using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Implemntations
{
    internal sealed class UsageLevel1GetSingleHandler : IUsageLevel1GetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IUsageLevel1QueryService _usageLevel1QueryService;
        public UsageLevel1GetSingleHandler(
            IMapper mapper,
            IUsageLevel1QueryService usageLevel1QueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _usageLevel1QueryService = usageLevel1QueryService;
            _usageLevel1QueryService.NotNull(nameof(_usageLevel1QueryService));
        }

        public async Task<UsageLevel1GetDto> Handle(short id, CancellationToken cancellationToken)
        {
            var usageLevel1 = await _usageLevel1QueryService.Get(id);
            return _mapper.Map<UsageLevel1GetDto>(usageLevel1);
        }
    }
}
