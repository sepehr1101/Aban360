using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Dms.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.Dms;
using Aban360.ReportPool.Persistence.Features.Dms.Queries;

namespace Aban360.ReportPool.Application.Features.Dms.Queries.Implementations
{
    internal sealed class ClientDiscountGetAllHandler : IClientDiscountGetAllHandler
    {
        private readonly IRequestDiscountService _requestDiscountService;
        public ClientDiscountGetAllHandler(IRequestDiscountService requestDiscountService)
        {
            _requestDiscountService = requestDiscountService;
            _requestDiscountService.NotNull(nameof(requestDiscountService));
        }

        public async Task<IEnumerable<ClientDiscount>> Handle(CancellationToken cancellationToken)
        {
            var data = await _requestDiscountService.Get();
            return data;
        }
    }
}
