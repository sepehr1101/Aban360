namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output
{
    public record NowGenerateBillOutputDto
    {
        public string Message { get; set; }
        public int ErrorCode { get; set; }
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
