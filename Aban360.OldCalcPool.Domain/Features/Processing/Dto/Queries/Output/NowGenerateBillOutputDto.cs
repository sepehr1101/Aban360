namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output
{
    public record NowGenerateBillOutputDto
    {
        public string Message { get; set; }
        public int ErrorCode { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public Aban360.OldCalcPool.Domain.Constants.NowGenerateBillSmsCategoryEnum? SmsCategory { get; set; }
        public NowGenerateBillOutputDto(string message, int errorCode)
        {
            Message = message;
            ErrorCode = errorCode;
        }
        public NowGenerateBillOutputDto()
        {
        }
    }
}
