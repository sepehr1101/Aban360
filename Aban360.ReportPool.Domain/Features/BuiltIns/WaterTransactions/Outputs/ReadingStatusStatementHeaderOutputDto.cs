namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record ReadingStatusStatementHeaderOutputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }

        public string FromReadingNumber { get; set; }
        public string ToReadingNumber { get; set; }

        public string ReportDateJalali { get; set; }
        public int RecordCount { get; set; }

        public int SumReadingNet { get; set; }
        public int SumClosed { get; set; }
        public int SumObstacle { get; set; }
        public int SumTemporarily { get; set; }
        public int SumAll { get; set; }
        public int SumRuined { get; set; }
        //public int SumSettlement { get; set; }


    }
}
