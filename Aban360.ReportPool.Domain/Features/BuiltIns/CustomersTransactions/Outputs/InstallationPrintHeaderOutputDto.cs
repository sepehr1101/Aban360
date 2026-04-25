using DNTPersianUtils.Core;

namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs
{
    public record InstallationPrintHeaderOutputDto
    {
        public string Title { get; set; }
        public string ReportDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
    }
}
