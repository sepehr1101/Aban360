namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs
{
    public record ConsumptionManagerHeaderOutputDto
    {
        public string FromBaseDateJalali { get; set; }
        public string ToBaseDateJalali { get; set; }

        public string FromComparisonDateJalali { get; set; }
        public string ToComparisonDateJalali { get; set; }
        
        public float FromMultiplier { get; set; }
        public float ToMultiplier { get; set; }

        public int? FromConsumptionAverage { get; set; }
        public int? ToConsumptionAverage { get; set; }
        
        public bool IsOlgoo { get; set; }

        public int CustomerCount { get; set; }
        public int RecordCount { get; set; }
        public string ReportDateJalali { get; set; }
        public string Title { get; set; }

        public float AverageSumtItemsPercent { get; set; }
        public float AverageConsumptionPercent { get; set; }

    }
}
