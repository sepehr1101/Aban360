namespace Aban360.Api.Filters
{
    using Aban360.Common.Categories.ApiResponse;
    using Aban360.UserPool.Domain.Constants;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using System;
    using System.Net;
    using System.Threading.Tasks;

    public class AllowTimeWindowFilter : Attribute, IAsyncActionFilter
    {
        private readonly TimeSpan _start = new TimeSpan(15, 0, 0); // 15:00
        private readonly TimeSpan _end = new TimeSpan(7, 0, 0);    // 07:00

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var now = DateTime.Now.TimeOfDay;

            bool isAllowed =
                (now >= _start) ||     // از 15:00 تا 23:59
                (now <= _end);         // از 00:00 تا 07:00

            if (!isAllowed)
            {
                context.Result = new ObjectResult(CreateEnvelope())
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
                return;
            }

            await next();
        }
        private ApiResponseEnvelope<object> CreateEnvelope()
        {
            ApiResponseEnvelope<object> envelope = new(
                (int)HttpStatusCode.Unauthorized,
                null,
                null,
                new List<ApiError> { new ApiError(MessageResources.UnauthorizedTime) }
            );
            return envelope;
        }
    }

}
