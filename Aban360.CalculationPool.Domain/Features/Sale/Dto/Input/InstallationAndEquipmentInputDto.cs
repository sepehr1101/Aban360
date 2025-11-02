namespace Aban360.CalculationPool.Domain.Features.Sale.Dto.Input
{
    public record InstallationAndEquipmentInputDto
    {
        public short Id { get; set; }
        public bool IsWater { get; set; }
        public short DiameterId { get; set; }
        public long InstallationAmount { get; set; }
        public long EquipmentAmount { get; set; }
        public string FromDateJalali { get; set; } = null!;
        public string ToDateJalali { get; set; } = null!;
        public DateTime RegisterDateTime { get; set; }
        public Guid RegisterByUserId { get; set; }
        public DateTime? RemoveDateTime { get; set; }
        public Guid? RemoveByUserId { get; set; }
    }

}
