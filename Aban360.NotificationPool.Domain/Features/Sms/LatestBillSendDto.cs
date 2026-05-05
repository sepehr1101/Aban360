namespace Aban360.NotificationPool.Domain.Features.Sms
{
    public record LatestBillSendDto
    {
        public string BillId { get; set; } = default!;
        public string? Mobile { get; set; }
    }
}
