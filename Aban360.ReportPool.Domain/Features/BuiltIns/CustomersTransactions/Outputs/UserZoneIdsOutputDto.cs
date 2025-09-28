namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs
{
    public record UserZoneIdsOutputDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public bool IsVillage { get; set; }
    }
}
