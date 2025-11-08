namespace Aban360.CalculationPool.Domain.Features.Sale.Dto.Input
{
    public record InstallationAndEquipmentGetDto
    {
        public bool IsWater { get; set; }
        public short? DiameterId { get; set; }
        public string? CurrentDateJalali { get; set; }
        public InstallationAndEquipmentGetDto(bool isWater, short? diameterId,string? currentDateJalali)
        {
            IsWater = isWater;
            DiameterId = diameterId;
            CurrentDateJalali = currentDateJalali;
        }
    }
}
