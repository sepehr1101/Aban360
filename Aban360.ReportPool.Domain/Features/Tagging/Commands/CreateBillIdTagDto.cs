namespace Aban360.ReportPool.Domain.Features.Tagging
{
    namespace CustomerWarehouse.Application.DTOs
    {
        public class CreateBillIdTagDto
        {
            public string BillId { get; set; } = string.Empty;
            public int TagId { get; set; }
            public string? ExpireDateJalali { get; set; }
        }
    }
}
