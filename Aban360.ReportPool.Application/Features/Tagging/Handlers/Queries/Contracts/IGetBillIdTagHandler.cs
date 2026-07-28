using Aban360.ReportPool.Domain.Features.Tagging.CustomerWarehouse.Application.DTOs;

namespace Aban360.ReportPool.Application.Features.Tagging.Handlers.Queries.Contracts
{
    public interface IGetBillIdTagHandler
    {
        Task<IEnumerable<BillIdTagDto>> Handle(string billId);
    }
}