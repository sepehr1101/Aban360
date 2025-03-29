using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Implementations
{
    internal sealed class UsageLevel4CreateHandler : IUsageLevel4CreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IUsageLevel4CommandService _usageLevel4CommandService;
        public UsageLevel4CreateHandler(
            IMapper mapper,
            IUsageLevel4CommandService usageLevel4CommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _usageLevel4CommandService = usageLevel4CommandService;
            _usageLevel4CommandService.NotNull(nameof(_usageLevel4CommandService));
        }

        public async Task Handle(UsageLevel4CreateDto createDto, CancellationToken cancellationToken)
        {
            var usageLevel4 = _mapper.Map<UsageLevel4>(createDto);
            await _usageLevel4CommandService.Add(usageLevel4);
        }
    }
}
