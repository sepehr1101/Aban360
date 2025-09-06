namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs
{
    public record ClientValidationInputDto
    {
        public string? FromReadingNumber { get; set; }
        public string? ToReadingNumber { get; set; }

        public ICollection<int>? ZoneIds { get; set; }
    }   
}
