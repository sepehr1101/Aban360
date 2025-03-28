using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.TimeTable.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.TimeTable.Dto.Queries;
using Aban360.UserPool.Persistence.Features.TimeTable.Queries.Contracts;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.TimeTable.Handlers.Queries.Implementations
{
    internal sealed class UsageLevel2GetSingleHandler : IUsageLevel2GetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IUsageLevel2QueryService _usageLevel2QueryService;
        public UsageLevel2GetSingleHandler(
            IMapper mapper,
            IUsageLevel2QueryService usageLevel2QueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _usageLevel2QueryService = usageLevel2QueryService;
            _usageLevel2QueryService.NotNull(nameof(_usageLevel2QueryService));
        }

        public async Task<UsageLevel2GetDto> Handle(short id, CancellationToken cancellationToken)
        {
            var usageLevel2 = await _usageLevel2QueryService.Get(id);
            return _mapper.Map<UsageLevel2GetDto>(usageLevel2);
        }
    }
}
