using Aban360.ReportPool.Application.Features.Tagging.Handlers.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.Tagging.CustomerWarehouse.Application.DTOs;
using Aban360.ReportPool.Persistence.Features.Tagging;

namespace Aban360.ReportPool.Application.Features.Tagging.Handlers.Queries.Implementations
{
    internal sealed class GetBillIdTagHandler : IGetBillIdTagHandler
    {
        private readonly IBillIdTagService _service;
        public GetBillIdTagHandler(IBillIdTagService service) => _service = service;
        public async Task<IEnumerable<BillIdTagDto>> Handle(string billId) => await _service.GetByBillId(billId);
    }
}
