using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Implementations
{
    internal sealed class UsageCreateHandler : IUsageCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IUsageCommandSevice _usageCommandService;
        public UsageCreateHandler(
            IMapper mapper,
            IUsageCommandSevice usageCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _usageCommandService = usageCommandService;
            _usageCommandService.NotNull(nameof(usageCommandService));
        }

        public async Task Handle(UsageCreateDto createDto, CancellationToken cancellationToken)
        {
            var usage = _mapper.Map<Usage>(createDto);
            await _usageCommandService.Add(usage);
        }
    }
}
