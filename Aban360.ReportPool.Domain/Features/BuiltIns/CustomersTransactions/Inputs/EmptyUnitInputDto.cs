namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs
{
    public record EmptyUnitInputDto
    {
        public int FromEmptyUnit { get; set; }
        public int ToEmptyUnit { get; set; }
        
        public string? FromReadingNumber { get; set; }
        public string? ToReadingNumber { get; set; }

        public ICollection<int>? UsageSellIds { get; set; }
        public ICollection<int>? ZoneIds { get; set; }
    } 
}
