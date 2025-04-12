namespace Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto
{
    public record ResultEstateDto
    {
        public int Id { get; set; }
        public int Premises { get; set; }
        public string? X { get; set; }
        public string? Y { get; set; }
        public int ImprovementsCommercial { get; set; }
        public int ImprovementsDomestic { get; set; }
        public int ImprovementsOther { get; set; }
        public int ImprovementsOverall { get; set; }
    }
}
