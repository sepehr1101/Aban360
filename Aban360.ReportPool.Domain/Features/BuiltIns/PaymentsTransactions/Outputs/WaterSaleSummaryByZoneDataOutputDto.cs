namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs
{
    public record WaterSaleSummaryByZoneDataOutputDto
    {
        public string ZoneTitle { get; set; }
        public string? UsageTitle { get; set; }
        public int DifferentDay { get; set; }
        public int ConsumptionAverage { get; set; }
        public int BillCount { get; set; }
        public int CustomerCount { get; set; }
        public int DomesticUnit { get; set; }
        public int CommertialUnit { get; set; }
        public int OtherUnit { get; set; }
        public int TotalUnit { get; set; }
        public int SewageUnitCount { get; set; }
        public double AbBaha { get; set; }
        public double BazelabBaha { get; set; }
        public double AbonAb { get; set; }
        public double AbonFazelab { get; set; }
        public double Shahrdari { get; set; }
        public double Tabsare2 { get; set; }
        public double Jarime { get; set; }
        public double Abresani { get; set; }
        public double JavaniD { get; set; }
        public double HotSeason { get; set; }
        public double ZaribTadil { get; set; }
        public double Boodje { get; set; }
        public double Lavazem { get; set; }
    }
}
