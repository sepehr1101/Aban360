using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Contracts;
using Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts;

namespace Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Implementations
{
    public class HeadquartersAddhoc : IHeadquartersAddhoc
    {
        private readonly IHeadquarterGetSingleHandler _headquarterGetSingleHandler;
        public HeadquartersAddhoc(IHeadquarterGetSingleHandler headquarterGetSingleHandler)
        {
            _headquarterGetSingleHandler = headquarterGetSingleHandler;
            _headquarterGetSingleHandler.NotNull(nameof(headquarterGetSingleHandler));
        }
        public async Task<string> Handle(short id, CancellationToken cancellationToken)
        {
            var headquarter = await _headquarterGetSingleHandler.Handle(id, cancellationToken);
            return headquarter.Title;
        }
    }
}
