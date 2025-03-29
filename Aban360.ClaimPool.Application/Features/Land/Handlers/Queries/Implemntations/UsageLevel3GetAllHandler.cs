using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Implemntations
{
    internal sealed class UsageLevel3GetAllHandler : IUsageLevel3GetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly IUsageLevel3QueryService _usageLevel3QueryService;
        public UsageLevel3GetAllHandler(
            IMapper mapper,
            IUsageLevel3QueryService usageLevel3QueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _usageLevel3QueryService = usageLevel3QueryService;
            _usageLevel3QueryService.NotNull(nameof(_usageLevel3QueryService));
        }

        public async Task<ICollection<UsageLevel3GetDto>> Handle(CancellationToken cancellationToken)
        {
            var usageLevel3 = await _usageLevel3QueryService.Get();
            return _mapper.Map<ICollection<UsageLevel3GetDto>>(usageLevel3);
        }
    }
}
