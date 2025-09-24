using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Application.Features.Tagging.Contracts;
using Aban360.ReportPool.Domain.Features.Tagging.CustomerWarehouse.Application.DTOs;
using Aban360.ReportPool.Persistence.Features.Tagging;

namespace Aban360.ReportPool.Application.Features.Tagging.Implementations
{
    internal sealed class CreateBillIdTagHandler : ICreateBillIdTagHandler
    {
        private readonly IBillIdTagService _service;
        public CreateBillIdTagHandler(IBillIdTagService service) => _service = service;
        public async Task<long> Handle(CreateBillIdTagDto dto)
        {
            BillIdTagValidation(dto);
            return await _service.Create(dto);
        }

        private async void BillIdTagValidation(CreateBillIdTagDto dto)
        {
            bool hasBillIdTag = await _service.HasBillIdTags(dto.BillId, dto.TagId);
            if (hasBillIdTag)
            {
                throw new DuplicateEntityException(ExceptionLiterals.DuplicateBillIdTags);
            }

            bool hasBillId = await _service.HasBillId(dto.BillId);
            if (!hasBillId)
            {
                throw new InvalidBillIdException(ExceptionLiterals.BillIdNotFound);
            }
        }
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
