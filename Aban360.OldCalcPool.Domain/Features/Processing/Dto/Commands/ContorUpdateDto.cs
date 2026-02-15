namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands
{
    public record ContorUpdateDto
    {
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        //public string PreviousDateJalali { get; set; }
        public string CurrentDateJalali{ get; set; }
        //public int PreviousNumber { get; set; }
        public int CurrentNumber { get; set; }
        public int Consumption { get; set; }
        public float ConsumptionAverage { get; set; }
        public string MeterChangeDateJalali { get; set; }
        public int MeterChangeNumber { get; set; }

    }
}
