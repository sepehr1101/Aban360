namespace Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries
{
    public record Table3GetDto
    {
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public int UsageGroupId { get; set; }
        public string UsageGroupTitle { get; set; }
        public int CompanyServiceId { get; set; }
        public string CompanyServiceTitle { get; set; }
        public int Price { get; set; }
    }
}