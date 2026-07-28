using Aban360.ReportPool.Domain.Features.Tagging.CustomerWarehouse.Application.DTOs;

namespace Aban360.ReportPool.Persistence.Features.Tagging.Queries.Contracts
{
    public interface IBillIdTagQueryService
    {
        Task<IEnumerable<BillIdTagDto>> GetByBillId(string billId);
        Task<IEnumerable<int>> GetIdsByBillId(string billId);
        Task<bool> HasBillIdTags(string billId, int tagId);
        Task<bool> HasBillId(string billId);
        Task<IEnumerable<BillIdTagDto>> GetByTagIds(IEnumerable<int> tagIds);
    }
}
