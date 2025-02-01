using Aban360.Common.Categories.ApiResponse;
using Aban360.UserPool.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Runtime.InteropServices;

namespace Aban360.Api.Controllers.V1
{
    [ApiController]
    [ApiVersion("0.0.1")]
    //[Authorize]
    public abstract class BaseController : ControllerBase
    {
        [NonAction]
        public IActionResult Ok<D>(D data)
        {
            var envelope= new ApiResponseEnvelope<D>((int)HttpStatusCode.OK,data, MessageResources.SuccessfulProccess);
            return base.Ok(envelope);
        }

        [NonAction]
        public IActionResult Ok<D>(D data, string successfulMessage)
        {
            var envelope = new ApiResponseEnvelope<D>((int)HttpStatusCode.OK, data, successfulMessage);
            return base.Ok(envelope);
        }

        [NonAction]
        public IActionResult ClientError(ICollection<ApiError> errors, [Optional]ApiMeta meta)
        {
            var envelope = new ApiResponseEnvelope<object>((int)HttpStatusCode.BadRequest, null, null, errors, null, meta);
            return BadRequest(envelope);
        }

        [NonAction]
        public IActionResult ClientError(ApiError error, [Optional] ApiMeta meta)
        {
            var envelope = new ApiResponseEnvelope<object>((int)HttpStatusCode.BadRequest, null, null, new List<ApiError> { error }, null, meta);
            return BadRequest(envelope);
        }

        [NonAction]
        public IActionResult ClientError(string errorMessage, [Optional] ApiMeta meta)
        {
            var envelope = new ApiResponseEnvelope<object>((int)HttpStatusCode.BadRequest, null, null, new List<ApiError> {new ApiError(errorMessage) }, null, meta);
            return BadRequest(envelope);
        }

        [NonAction]
        public IActionResult Unauthorized([Optional]string? message)
        {
            message = message ?? MessageResources.UnathorizedResource;
            var envelope = new ApiResponseEnvelope<object>((int)HttpStatusCode.Unauthorized, null, null, new List<ApiError> { new ApiError(message) });
            return BadRequest(envelope);
        }
    }
}
