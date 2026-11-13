using Aban360.LocationPool.Application.Features.MainHierarchy.Services;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts;

namespace Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Implementations
{
    internal sealed class HeadquartersAddhoc : IHeadquartersAddhoc
    {
        private readonly IHeadquarterService _service;

        public HeadquartersAddhoc(IHeadquarterService service)
        {
            service.NotNull(nameof(service));
            _service = service;
        }
        public async Task<string> Handle(short id, CancellationToken cancellationToken)
        {
            HeadquarterGetDto headquarter = await _service.GetSingleAsync(id, cancellationToken);
            return headquarter.Title;
        }
    }
}
