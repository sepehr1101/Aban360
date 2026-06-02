namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record WaterDiscountDetailDataOutputDto
    {
        public int ZoneId { get; set; }
        public string ZoneTitle { get; }
        public int RegionId { get; }
        public string RegionTitle { get; }
        public int CustomerNumber { get; set; }
        public string BillId { get; set; }
        public string ReadingNumber { get; set; }
        public string FirstName { get; }
        public string Surname { get; }
        public string FullName { get; }
        public string? FatherName { get; }
        public string? PostalCode { get; }
        public string? Address { get; }
        public string? PhoneNumber { get; }
        public string? MobileNumber { get; }
        public int UsageId { get; }
        public string UsageTitle { get; }
        public string BranchTypeTitle { get; }
        public int BranchTypeId { get; }
        public int CommercialUnit { get; }
        public int DomesticUnit { get; }
        public int OtherUnit { get; }
        public int TotalUnit { get; }
        public int BillUnit { get; }
        public int Consumption { get; }
        public float ConsumptionAverage { get; }
        public int CounterStateCode { get; }
        public string? CounterStateTitle { get; }

        public long AbBaha { get; set; }
        public long FazelabBaha { get; set; }
        public long AbonmanAb { get; set; }
        public long AbonmanFazelab { get; set; }
        public long Maliat { get; set; }
        public long Tabsare2 { get; set; }
        public long Tabsare2_3 { get; set; }
        public long Jarime { get; set; }
        public long Abresani { get; set; }
        public long JavaniJamiat { get; set; }
        public long FaslGarm { get; set; }
        public long ZaribTadil { get; set; }
        public long Tabsare3Ab { get; set; }
        public long Tabsare3Fazelab { get; set; }
        public long TabsareAbonmanFazelab { get; set; }
        public long GhanonBoodje { get; set; }
        public long JavazemKahande { get; set; }
        public long AvarezSanati { get; set; }
    }
}
