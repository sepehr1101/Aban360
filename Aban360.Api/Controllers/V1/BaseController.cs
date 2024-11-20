using Aban360.Common.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Runtime.InteropServices;

namespace Aban360.Api.Controllers.V1
{   
    [ApiController]
    [ApiVersion("0.0.1")]
    public abstract class BaseController : ControllerBase
    {
        [NonAction]
        public IActionResult Ok<D>(D data)
        {
            var envelope= new ApiResponseEnvelope<D>((int)HttpStatusCode.OK,data);
            return base.Ok(envelope);
        }

        [NonAction]
        public IActionResult ClientError(ICollection<ApiError> errors, [Optional]ApiMeta meta)
        {
            var envelope = new ApiResponseEnvelope<object>((int)HttpStatusCode.BadRequest, null, errors, null, meta);
            return BadRequest(envelope);
        }

        [NonAction]
        public IActionResult ClientError(ApiError error, [Optional] ApiMeta meta)
        {
            var envelope = new ApiResponseEnvelope<object>((int)HttpStatusCode.BadRequest, null, new List<ApiError> { error }, null, meta);
            return BadRequest(envelope);
        }

        [NonAction]
        public IActionResult ClientError(string errorMessage, [Optional] ApiMeta meta)
        {
            var envelope = new ApiResponseEnvelope<object>((int)HttpStatusCode.BadRequest, null, new List<ApiError> {new ApiError(errorMessage) }, null, meta);
            return BadRequest(envelope);
        }
    }
}
