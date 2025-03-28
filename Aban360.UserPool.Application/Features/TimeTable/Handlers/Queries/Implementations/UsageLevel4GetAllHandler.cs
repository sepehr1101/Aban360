using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.TimeTable.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.TimeTable.Dto.Queries;
using Aban360.UserPool.Persistence.Features.TimeTable.Queries.Contracts;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.TimeTable.Handlers.Queries.Implementations
{
    internal sealed class UsageLevel4GetAllHandler : IUsageLevel4GetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly IUsageLevel4QueryService _usageLevel4QueryService;
        public UsageLevel4GetAllHandler(
            IMapper mapper,
            IUsageLevel4QueryService usageLevel4QueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _usageLevel4QueryService = usageLevel4QueryService;
            _usageLevel4QueryService.NotNull(nameof(_usageLevel4QueryService));
        }

        public async Task<ICollection<UsageLevel4GetDto>> Handle(CancellationToken cancellationToken)
        {
            var usageLevel4 = await _usageLevel4QueryService.Get();
            return _mapper.Map<ICollection<UsageLevel4GetDto>>(usageLevel4);
        }
    }
}
