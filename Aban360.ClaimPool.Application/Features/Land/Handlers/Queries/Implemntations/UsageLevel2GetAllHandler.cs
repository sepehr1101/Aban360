using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Implemntations
{
    internal sealed class UsageLevel2GetAllHandler : IUsageLevel2GetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly IUsageLevel2QueryService _usageLevel2QueryService;
        public UsageLevel2GetAllHandler(
            IMapper mapper,
            IUsageLevel2QueryService usageLevel2QueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _usageLevel2QueryService = usageLevel2QueryService;
            _usageLevel2QueryService.NotNull(nameof(_usageLevel2QueryService));
        }

        public async Task<ICollection<UsageLevel2GetDto>> Handle(CancellationToken cancellationToken)
        {
            var usageLevel2 = await _usageLevel2QueryService.Get();
            return _mapper.Map<ICollection<UsageLevel2GetDto>>(usageLevel2);
        }
    }
}
