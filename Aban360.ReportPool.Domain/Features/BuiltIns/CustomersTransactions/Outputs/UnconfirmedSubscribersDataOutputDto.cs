namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs
{
    public record UnconfirmedSubscribersDataOutputDto
    {
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string FullName { get; set; }
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public long FinalAmount { get; set; }
        public long PreInstallmentAmount { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public int ContractualCapacity { get; set; }
        public string TrackNumber { get; set; }
        public string RequestDateJalali { get; set; }
    }
}
