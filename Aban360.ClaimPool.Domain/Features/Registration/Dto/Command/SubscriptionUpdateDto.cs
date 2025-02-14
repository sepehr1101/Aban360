namespace Aban360.ClaimPool.Domain.Features.Registration.Dto.Command
{
    public record SubscriptionUpdateDto
    {
        public int Id { get; set; }
        public string? ReadingNumber { get; set; }
        public int CustomerNumber { get; set; }
        public string BillId { get; set; } = null!;
        public int EstateId { get; set; }
        public int WaterMeterId { get; set; }
        public short UseStateId { get; set; }
        public Guid UserId { get; set; }
        public int? PreviousId { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public string InsertLogInfo { get; set; } = null!;
    }
}
