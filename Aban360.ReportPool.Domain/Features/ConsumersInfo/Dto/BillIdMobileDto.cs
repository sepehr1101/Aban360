namespace Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto
{
    public record BillIdMobileDto
    {
        public string BillId { get; set; } = default!;
        public string MobileNo { get; set; } = default!;
    }
}
