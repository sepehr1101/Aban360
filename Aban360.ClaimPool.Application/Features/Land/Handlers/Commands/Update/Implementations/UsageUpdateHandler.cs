using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Implementations
{
    public class UsageUpdateHandler : IUsageUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IUsageQuerySevice _usageQueryService;
        public UsageUpdateHandler(
            IMapper mapper,
           IUsageQuerySevice usageQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _usageQueryService = usageQueryService;
            _usageQueryService.NotNull(nameof(usageQueryService));
        }

        public async Task Handle(UsageUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var usage = await _usageQueryService.Get(updateDto.Id);
            if (usage == null)
            {
                throw new InvalidDataException();
            }
            _mapper.Map(updateDto, usage);
        }
    }
}
