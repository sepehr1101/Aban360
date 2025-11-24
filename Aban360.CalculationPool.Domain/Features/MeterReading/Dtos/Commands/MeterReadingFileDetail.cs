namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands
{
    public record MeterReadingFileDetail
    {
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public string ReadingNumber { get; set; }
        public int AgentCode { get; set; }
        public short CurrentCounterStateCode { get; set; }
        public string PreviousDateJalali { get; set; }
        public string CurrentDateJalali { get; set; }
        public int PreviousNumber { get; set; }
        public int CurrentNumber { get; set; }

        public Guid InsertByUserId { get; set; }
        public DateTime InsertDateTime { get; set; }

        public MeterReadingFileDetail(
            int zoneId, int customerNumber, string readingNumber, int agentCode, short currentCounterStateCode, string previousDateJalali,
            string currentDateJalali, int previousNumber, int currentNumber, Guid insertByUserId, DateTime insertDateTime)
        {
            ZoneId = zoneId;
            CustomerNumber = customerNumber;
            ReadingNumber = readingNumber;
            AgentCode = agentCode;
            CurrentCounterStateCode = currentCounterStateCode;
            PreviousDateJalali = previousDateJalali;
            CurrentDateJalali = currentDateJalali;
            PreviousNumber = previousNumber;
            CurrentNumber = currentNumber;
            InsertByUserId = insertByUserId;
            InsertDateTime = insertDateTime;
        }
    }
}