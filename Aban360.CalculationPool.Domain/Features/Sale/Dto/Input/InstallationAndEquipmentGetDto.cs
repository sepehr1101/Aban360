namespace Aban360.CalculationPool.Domain.Features.Sale.Dto.Input
{
    public record InstallationAndEquipmentGetDto
    {
        public bool IsWater { get; set; }
        public short? MeterDiameterId { get; set; }
        public InstallationAndEquipmentGetDto(bool isWater, short? meterDiameterId)
        {
            IsWater = isWater;
            MeterDiameterId = meterDiameterId;
        }

    }
}
