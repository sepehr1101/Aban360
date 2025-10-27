namespace Aban360.CalculationPool.Domain.Features.Sale.Dto.Input
{
    public record InstallationAndEquipmentInputDto
    {
        public bool IsWater { get; set; }
        public short MeterDiameterId { get; set; }
        public long InstallationAmount { get; set; }
        public long EquipmentAmount { get;  set; }
        public string? RegisterDateJalali { get; set; }
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        public string? RemovedDateJalali { get; set; }
    }
}
