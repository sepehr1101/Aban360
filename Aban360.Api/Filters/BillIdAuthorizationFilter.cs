namespace Aban360.Api.Filters
{
    using Aban360.Common.ApplicationUser;
    using Aban360.Common.Categories.ApiResponse;
    using Aban360.Common.Db.QueryServices;
    using Aban360.UserPool.Domain.Constants;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using System.Net;
    using System.Reflection;

    public class BillIdAuthorizationFilter : IAsyncActionFilter
    {
        private readonly ICommonZoneService _zoneService;
        private readonly ICommonMemberQueryService _memberQueryService;

        public BillIdAuthorizationFilter(
            ICommonZoneService zoneService,
            ICommonMemberQueryService memberQueryService)
        {
            _zoneService = zoneService;
            _memberQueryService = memberQueryService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // 1. Skip if [AllowAnonymous] exists
            var endpoint = context.HttpContext.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
            {
                await next();
                return;
            }

            // 2. Extract billId from query, route, body
            string billId = ExtractBillId(context);

            if (string.IsNullOrWhiteSpace(billId))
            {
                await next();
                return;
            }

            // 3. Get user's allowed zones
            IAppUser currentUser = new AppUser(context.HttpContext.User);
            var allowedZones = await _zoneService.GetMyZoneIds(currentUser);
            var billIdZone = await _memberQueryService.Get(billId);

            // 4. Authorization check
            if (!allowedZones.Contains(billIdZone.ZoneId))
            {
                context.Result = new ObjectResult(CreateEnvelope())
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
                return;
            }

            await next();
        }

        private string ExtractBillId(ActionExecutingContext ctx)
        {
            // -------- QUERY STRING --------
            var query = ctx.HttpContext.Request.Query;
            if (query.TryGetValue("billId", out var queryValues))
            {
                // Return the first value if multiple are present
                return queryValues.FirstOrDefault();
            }

            // -------- ROUTE DATA --------
            if (ctx.RouteData.Values.TryGetValue("billId", out var routeValue) && routeValue != null)
            {
                return routeValue.ToString();
            }

            // -------- ACTION ARGUMENTS --------
            // 1. Direct argument named "billId"
            if (ctx.ActionArguments.TryGetValue("billId", out var argValue) && argValue != null)
            {
                return argValue.ToString();
            }

            // 2. Scan all arguments for a property named "billId"
            foreach (var arg in ctx.ActionArguments.Values)
            {
                if (arg == null) continue;

                var prop = arg.GetType().GetProperty("billId",
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (prop != null && prop.CanRead)
                {
                    var val = prop.GetValue(arg);
                    if (val != null)
                        return val.ToString();
                }
            }

            return null; // No billId found
        }

        private ApiResponseEnvelope<object> CreateEnvelope()
        {
            ApiResponseEnvelope<object> envelope = new(
                (int)HttpStatusCode.Unauthorized,
                null,
                null,
                new List<ApiError> { new ApiError(MessageResources.UnauthorizedZone) }
            );
            return envelope;
        }
    }
}
