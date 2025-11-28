namespace Aban360.CalculationPool.Domain.Features.Sale.Dto.Input
{
    public record TankerWaterDistanceTariffCreateDto
    {
        public short FromDistance { get; set; }
        public short ToDistance { get; set; }
        public long Amount { get; set; }
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        public DateTime RegisterDateTime { get; set; }
        public Guid RegisterByUserId { get; set; }
        public DateTime? RemoveDateTime { get; set; }
        public Guid? RemoveByUserId { get; set; }

    }
}
