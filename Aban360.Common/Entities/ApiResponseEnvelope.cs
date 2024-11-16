using System.Runtime.InteropServices;

namespace Aban360.Common.Entities
{
    public sealed class ApiResponseEnvelope<D>
    {
        public bool Success { get; }
        public int HttpStatusCode { get;}        
        public D? Data { get; }
        public ICollection<ApiError> Errors { get;}
        public ICollection<ApiError> Warnings { get;}
        public ApiMeta Meta { get;}
        public ApiResponseEnvelope(int httpStatusCode, D? data, [Optional] ICollection<ApiError> errors, [Optional] ICollection<ApiError> warnings, [Optional] ApiMeta meta)
        {
            Success = httpStatusCode == 200;
            HttpStatusCode = httpStatusCode;
            Data = data;
            Errors = errors;
            Warnings = warnings;
            Meta = meta?? new ApiMeta() ;
        }       
    }
}