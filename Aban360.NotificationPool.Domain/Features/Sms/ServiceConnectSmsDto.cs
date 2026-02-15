namespace Aban360.NotificationPool.Domain.Features.Sms
{
    public record ServiceConnectSmsDto
    {
        public string BillId { get; set; } = default!;
        public int Hour { get; set; }
        public string? When { get; set; }
    }
}
