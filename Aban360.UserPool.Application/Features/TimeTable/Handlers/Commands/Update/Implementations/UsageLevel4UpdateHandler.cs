using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.TimeTable.Handlers.Commands.Update.Contracts;
using Aban360.UserPool.Domain.Features.TimeTable.Dto.Commands;
using Aban360.UserPool.Persistence.Features.TimeTable.Queries.Contracts;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.TimeTable.Handlers.Commands.Update.Implementations
{
    internal sealed class UsageLevel4UpdateHandler : IUsageLevel4UpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IUsageLevel4QueryService _usageLevel4QueryService;
        public UsageLevel4UpdateHandler(
            IMapper mapper,
            IUsageLevel4QueryService usageLevel4QueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _usageLevel4QueryService = usageLevel4QueryService;
            _usageLevel4QueryService.NotNull(nameof(_usageLevel4QueryService));
        }

        public async Task Handle(UsageLevel4UpdateDto updateDto, CancellationToken cancellationToken)
        {
            var usageLevel4 = await _usageLevel4QueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, usageLevel4);
        }
    }
}
