namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output
{
    public record NowGenerateBillBySmsOutputDto
    {
        public string Message { get; set; }
        public int ErrorCode { get; set; }
        public string TextSms { get; set; }
        public string MobileNumber { get; set; }
        public NowGenerateBillBySmsOutputDto(string message, int errorCode, string? textSms,string? mobileNumber)
        {
            Message = message;
            ErrorCode = errorCode;
            TextSms = textSms ?? string.Empty;
            MobileNumber = mobileNumber ?? string.Empty;
        }
        public NowGenerateBillBySmsOutputDto()
        {
        }
    }
}
