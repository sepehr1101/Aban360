namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input
{
    public record RemoveBillDto
    {
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public int previousNumber { get; set; }
        public int CurrentNumber { get; set; }
        public string PreviousDateJalali { get; set; }
        public string CurrentDateJalali { get; set; }
    }
}
