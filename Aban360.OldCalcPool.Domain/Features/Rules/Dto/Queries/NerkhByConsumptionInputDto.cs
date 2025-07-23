namespace Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries
{
    public record NerkhByConsumptionInputDto
    {
        public int ZoneId { get; set; }
        public int UsageId { get; set; }
        public string PreviousDateJalali { get; set; }
        public string CurrentDateJalali { get; set; }
        public double AverageConsumption { get; set; }
        public NerkhByConsumptionInputDto(int _zoneId,int _usageId,string _previousDate,string _currentDate,double _average)
        {
            ZoneId = _zoneId;
            UsageId = _usageId;
            PreviousDateJalali = _previousDate;
            CurrentDateJalali = _currentDate;
            AverageConsumption = _average;
        }
    }
}
