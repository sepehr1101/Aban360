namespace Aban360.CalculationPool.Domain.Features.Sale.Dto.Output
{
    public record TankerWaterDistanceTariffOutputDto
    {
        public short Id { get; set; }
        public short FromDistance { get; set; }
        public short ToDistance { get; set; }
        public long Amount { get; set; }
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        public DateTime RegisterDateTime { get; set; }
        public Guid RegisterByUserId { get; set; }
        public DateTime RemoveDateTime { get; set; }
        public Guid RemoveByUserId { get; set; }

    }
}
