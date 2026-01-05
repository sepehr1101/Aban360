using Aban360.ReportPool.Domain.Constants;

namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs
{
    public record CustomerMiniSearchInputDto
    {
        public CustomerMiniSearchInputEnum SearchType { get; set; }
        public string Input { get; set; }
    }
}
