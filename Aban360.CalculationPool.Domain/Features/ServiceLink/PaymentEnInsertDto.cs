namespace Aban360.CalculationPool.Domain.Features.ServiceLink
{
    public record PaymentEnInsertDto
    {
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public int CustomerNumber { get; set; }
        public string BillId { get; set; }
        public long Amount { get; set; }
        public string RegisterDay { get; set; }
        public DateTime RegisterDayGregorian { get; set; }
        public string BankName { get; set; }
        public int BankBranchCode { get; set; }
        public string PaymentGateway { get; set; }
        public long? BillTableId { get; set; }
        public string VillageId { get; set; }
        public string VillageName { get; set; }
        public int IsVillage { get; set; }
        public string PayId { get; set; }
        public string BankCode { get; set; }
        public string PayDateJalali { get; set; }
        public long? TempId { get; set; }
    }
}
