namespace Aban360.Common.Entities.ApiResponse
{
    public record ApiError
    {
        public ApiError(string message, int code = 0)
        {
            Code = code;
            Message = message;
        }
        public int Code { get; }
        public string Message { get; } = null!;
    }
}
