using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Implementations
{
    internal sealed class ServiceGroupGetAllHandler : IServiceGroupGetAllHandler
    {
        private readonly IT100QueryService _t100QueryService;
        public ServiceGroupGetAllHandler(IT100QueryService t100QueryService)
        {
            _t100QueryService = t100QueryService;
            _t100QueryService.NotNull(nameof(t100QueryService));
        }

        public async Task<IEnumerable<NumericDictionary>> Handle(CancellationToken cancellationToken)
        {
            return await _t100QueryService.Get();
        }
    }
}
