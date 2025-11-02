namespace Aban360.CalculationPool.Domain.Features.Sale.Dto.Input
{
    public record Article11InputDto
    {
        public short Id { get; set; }
        public long DomesticWaterAmount { get; set; }
        public long NonDomesticWaterAmount { get; set; }
        public long? DomesticSewageAmount { get; set; }
        public long? NonDomesticSewageAmount { get; set; }
        public string? BlockCode { get; set; }
        public int ZoneId { get; set; }
        public string FromDateJalali { get; set; } = null!;
        public string ToDateJalali { get; set; } = null!;
        public DateTime RegisterDateTime { get; set; }
        public Guid RegisterByUserId { get; set; }
        public DateTime? RemoveDateTime { get; set; }
        public Guid? RemoveByUserId { get; set; }
    }
}
