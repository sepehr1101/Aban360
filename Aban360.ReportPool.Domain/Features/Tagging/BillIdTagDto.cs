namespace Aban360.ReportPool.Domain.Features.Tagging
{
    namespace CustomerWarehouse.Application.DTOs
    {
        public class BillIdTagDto
        {
            public long Id { get; set; }
            public string BillId { get; set; } = string.Empty;
            public string ExpireDateJalali { get; set; }
            public int TagId { get; set; }
            public string TagTitle { get; set; } = string.Empty;
            public DateTime CreateDateTime { get; set; }
            public DateTime? DeleteDateTime { get; set; }
        }

        public class CreateBillIdTagDto
        {
            public string BillId { get; set; } = string.Empty;
            public int TagId { get; set; }
            public string? ExpireDateJalali { get; set; }
        }
    }

}
