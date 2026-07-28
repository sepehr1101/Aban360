using Aban360.ReportPool.Domain.Constants;

namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs
{
    public record CustomerLegalDetailInputDto
    {
        public ICollection<int> ZoneIds { get; set; }
        public CustomerLegalDetailEnum Type { get; set; }
    }
}
