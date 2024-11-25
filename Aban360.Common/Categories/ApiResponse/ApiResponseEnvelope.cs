using System.Runtime.InteropServices;

namespace Aban360.Common.Categories.ApiResponse
{
    public sealed class ApiResponseEnvelope<D>
    {
        public bool Success { get; }
        public int HttpStatusCode { get; }
        public string? SuccessMessage { get; }
        public D? Data { get; }
        public ICollection<ApiError> Errors { get; }
        public ICollection<ApiError> Warnings { get; }
        public ApiMeta Meta { get; }
        public ApiResponseEnvelope(int httpStatusCode, D? data, [Optional] string? successMessage, [Optional] ICollection<ApiError> errors, [Optional] ICollection<ApiError> warnings, [Optional] ApiMeta meta)
        {
            Success = httpStatusCode == 200;
            SuccessMessage = httpStatusCode == 200 ? successMessage : null;
            HttpStatusCode = httpStatusCode;
            Data = data;
            Errors = errors;
            Warnings = warnings;
            Meta = meta ?? new ApiMeta();
        }
    }
}