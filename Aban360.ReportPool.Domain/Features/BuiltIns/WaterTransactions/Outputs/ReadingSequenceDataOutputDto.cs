namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record ReadingSequenceDataOutputDto
    {
        public int Id { get; set; }
        public int CustomerNumber { get; set; }
        public string BillId { get; set; }
        public string ReadingNumber { get; set; }
        public string PreviousDateJalali { get; set; }
        public string CurrentDateJalali { get; set; }
        public string PreviousCurrentDateJalali { get; set; }

        public int PreviousNumber { get; set; }
        public int CurrentNumber { get; set; }
        public int PreviousCurrentNumber { get; set; }

        public int CounterStateCode { get; set; }
        public string CounterStateTitle { get; set; }

    }
}
