namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands
{
    public record MeterReadingFileCreateOutputDto
    {
        public int FirstFlowId { get; set; }
        public int SmsFlowId { get; set; }
        public MeterReadingFileCreateOutputDto(int firstFlowId, int smsFlowId)
        {
            FirstFlowId = firstFlowId;
            SmsFlowId = smsFlowId;
        }
        public MeterReadingFileCreateOutputDto()
        {
        }
    }
}
