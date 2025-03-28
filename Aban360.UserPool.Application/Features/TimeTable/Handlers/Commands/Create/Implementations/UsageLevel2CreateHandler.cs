using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.TimeTable.Handlers.Commands.Create.Contracts;
using Aban360.UserPool.Domain.Features.TimeTable.Dto.Commands;
using Aban360.UserPool.Domain.Features.TimeTable.Entites;
using Aban360.UserPool.Persistence.Features.TimeTable.Commands.Contracts;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.TimeTable.Handlers.Commands.Create.Implementations
{
    internal sealed class UsageLevel2CreateHandler : IUsageLevel2CreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IUsageLevel2CommandService _usageLevel2CommandService;
        public UsageLevel2CreateHandler(
            IMapper mapper,
            IUsageLevel2CommandService usageLevel2CommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _usageLevel2CommandService = usageLevel2CommandService;
            _usageLevel2CommandService.NotNull(nameof(_usageLevel2CommandService));
        }

        public async Task Handle(UsageLevel2CreateDto createDto, CancellationToken cancellationToken)
        {
            var usageLevel2 = _mapper.Map<UsageLevel2>(createDto);
            await _usageLevel2CommandService.Add(usageLevel2);
        }
    }
}
