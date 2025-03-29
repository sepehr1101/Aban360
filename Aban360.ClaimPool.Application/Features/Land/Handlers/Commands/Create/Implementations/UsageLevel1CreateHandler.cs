using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Implementations
{
    internal sealed class UsageLevel1CreateHandler : IUsageLevel1CreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IUsageLevel1CommandService _usageLevel1CommandService;
        public UsageLevel1CreateHandler(
            IMapper mapper,
            IUsageLevel1CommandService usageLevel1CommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _usageLevel1CommandService = usageLevel1CommandService;
            _usageLevel1CommandService.NotNull(nameof(_usageLevel1CommandService));
        }

        public async Task Handle(UsageLevel1CreateDto createDto, CancellationToken cancellationToken)
        {
            var usageLevel1 = _mapper.Map<UsageLevel1>(createDto);
            await _usageLevel1CommandService.Add(usageLevel1);
        }
    }
}
