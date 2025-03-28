using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.TimeTable.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.TimeTable.Dto.Queries;
using Aban360.UserPool.Persistence.Features.TimeTable.Queries.Contracts;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.TimeTable.Handlers.Queries.Implementations
{
    internal sealed class UsageLevel4GetSingleHandler : IUsageLevel4GetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IUsageLevel4QueryService _usageLevel4QueryService;
        public UsageLevel4GetSingleHandler(
            IMapper mapper,
            IUsageLevel4QueryService usageLevel4QueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _usageLevel4QueryService = usageLevel4QueryService;
            _usageLevel4QueryService.NotNull(nameof(_usageLevel4QueryService));
        }

        public async Task<UsageLevel4GetDto> Handle(short id, CancellationToken cancellationToken)
        {
            var usageLevel4 = await _usageLevel4QueryService.Get(id);
            return _mapper.Map<UsageLevel4GetDto>(usageLevel4);
        }
    }
}
