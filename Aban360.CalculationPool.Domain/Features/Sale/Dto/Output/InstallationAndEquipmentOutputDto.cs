namespace Aban360.CalculationPool.Domain.Features.Sale.Dto.Output
{
    public record InstallationAndEquipmentOutputDto
    {
        public short Id { get; set; }
        public bool IsWater { get; set; }
        public short MeterDiameterId { get; set; }
        public long InstallationAmount { get; set; }
        public long EquipmentAmount { get; set; }
        public string FromDateJalali { get; set; } = null!;
        public string ToDateJalali { get; set; } = null!;
    }
}
