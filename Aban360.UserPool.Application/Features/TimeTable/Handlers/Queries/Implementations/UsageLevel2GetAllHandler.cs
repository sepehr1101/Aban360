using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.TimeTable.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.TimeTable.Dto.Queries;
using Aban360.UserPool.Persistence.Features.TimeTable.Queries.Contracts;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.TimeTable.Handlers.Queries.Implementations
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
