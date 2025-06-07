namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs
{
    public record DebtorByDayInputDto
    {
        //Other
        public ICollection<int> ZoneIds { get; set; }
    }
}
