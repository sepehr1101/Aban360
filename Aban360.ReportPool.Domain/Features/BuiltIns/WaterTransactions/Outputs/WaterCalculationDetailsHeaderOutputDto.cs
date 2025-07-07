namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record WaterCalculationDetailsHeaderOutputDto
    {
        public int CustomerNumber { get; set; }
        public string ReadingNumber { get; set; }
        public string BillId { get; set; }
        public string PayId { get; set; }
        public string ZoneTitle { get; set; }

        public string UsageTitle { get; set; }

        public int Consumption { get; set; }
        public int ConsumptionAverage { get; set; }
        public int Duration { get; set; }

        public int OtherUnit { get; set; }
        public int CommericialUnit { get; set; }
        public int DomesticUnit { get; set; }
        public int EmptyUnit { get; set; }

        public string RegisterDay { get; set; }
        public string BranchType { get; set; }
        public string ReadingStateTitle { get; set; }

        public string  CounterStateTitle { get; set; }

        public string PreviousDay { get; set; }
        public string CurrentDay { get; set; }

        public string PreviousNumber { get; set; }
        public string CurrentNumber { get; set; }

        public string Deadline { get; set; }

        public string  MeterDiameterTitle { get; set; }

    }
}
