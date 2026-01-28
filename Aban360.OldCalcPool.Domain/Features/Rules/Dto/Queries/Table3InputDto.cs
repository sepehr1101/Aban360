namespace Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries
{
    public record Table3InputDto
    {
        public int ZoneId { get; set; }
        public int UsageId { get; set; }
        public IEnumerable<int> CompanyServiceIds { get; set; }
        public Table3InputDto(int zoneId,int usageId, IEnumerable<int> companyServiceIds)
        {
            ZoneId = zoneId;
            UsageId = usageId;
            CompanyServiceIds = companyServiceIds;
        }
    }
}