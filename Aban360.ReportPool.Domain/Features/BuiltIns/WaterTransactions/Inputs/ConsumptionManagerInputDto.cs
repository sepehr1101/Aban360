namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs
{
    public record ConsumptionManagerInputDto
    {
        public ICollection<int> ZoneIds { get; set; }
        public bool IsOlgoo { get; set; }
      
        public string FromBaseDateJalali { get; set; }
        public string ToBaseDateJalali { get; set; }
        
        public string FromComparisonDateJalali { get; set; }
        public string ToComparisonDateJalali { get; set; }
       
        public float FromMultiplier { get; set; }
        public float ToMultiplier { get; set; }
        
        public int? FromConsumptionAverage { get; set; }
        public int? ToConsumptionAverage { get; set; }
    }
}
