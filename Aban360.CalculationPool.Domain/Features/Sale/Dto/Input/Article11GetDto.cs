namespace Aban360.CalculationPool.Domain.Features.Sale.Dto.Input
{
    public record Article11GetDto
    {
        public string? BlockCode { get; set; }
        public int ZoneId { get; set; }
        public string? CurrentDateJalali { get; set; }
        public Article11GetDto(int zoneId,string? blockCode,string? currentDateJalali)
        {
            ZoneId = zoneId;
            BlockCode = blockCode;
            CurrentDateJalali = currentDateJalali;
        }
    }
}
