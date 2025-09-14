using Aban360.ReportPool.Application.Features.Tagging.Contracts;
using Aban360.ReportPool.Domain.Features.Tagging.CustomerWarehouse.Application.DTOs;
using Aban360.ReportPool.Persistence.Features.Tagging;

namespace Aban360.ReportPool.Application.Features.Tagging.Implementations
{
    internal sealed class CreateBillIdTagHandler : ICreateBillIdTagHandler
    {
        private readonly IBillIdTagService _service;
        public CreateBillIdTagHandler(IBillIdTagService service) => _service = service;
        public async Task<long> Handle(CreateBillIdTagDto dto) => await _service.Create(dto);
    }


    internal sealed class GetBillIdTagHandler : IGetBillIdTagHandler
    {
        private readonly IBillIdTagService _service;
        public GetBillIdTagHandler(IBillIdTagService service) => _service = service;
        public async Task<IEnumerable<BillIdTagDto>> Handle(string billId) => await _service.GetByBillId(billId);
    }

    internal sealed class DeleteBillIdTagHandler : IDeleteBillIdTagHandler
    {
        private readonly IBillIdTagService _service;
        public DeleteBillIdTagHandler(IBillIdTagService service) => _service = service;
        public async Task<bool> Handle(long id) => await _service.Delete(id);
    }
}
