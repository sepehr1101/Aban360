namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands
{
    public record MeterReadingFileCreateOutputDto
    {
        public int ZoneId { get; set; }
        public int FirstFlowId { get; set; }
        public int SmsFlowId { get; set; }
        public MeterReadingFileCreateOutputDto(int zoneId, int firstFlowId, int smsFlowId)
        {
            ZoneId = zoneId;
            FirstFlowId = firstFlowId;
            SmsFlowId = smsFlowId;
        }
        public MeterReadingFileCreateOutputDto()
        {
        }
    }
}
