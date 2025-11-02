namespace Aban360.CalculationPool.Domain.Features.Sale.Dto.Input
{
    public record InstallationAndEquipmentCreateDto
    {
        public bool IsWater { get; set; }
        public short DiameterId { get; set; }
        public long InstallationAmount { get; set; }
        public long EquipmentAmount { get; set; }
        public string FromDateJalali { get; set; } = null!;
        public string ToDateJalali { get; set; } = null!;
    }
}
