namespace Aban360.Api.Filters
{
    using Aban360.Common.ApplicationUser;
    using Aban360.Common.BaseEntities;
    using Aban360.Common.Categories.ApiResponse;
    using Aban360.Common.Db.QueryServices;
    using Aban360.UserPool.Domain.Constants;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using System.Net;
    using System.Reflection;

    public class BillIdFromSearchInputAuthorizationFilter : IAsyncActionFilter
    {
        private readonly ICommonZoneService _zoneService;
        private readonly ICommonMemberQueryService _memberQueryService;

        public BillIdFromSearchInputAuthorizationFilter(
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

            // 2. Extract billId – first try from SearchInput parameter, then fallback to other sources
            string billId = ExtractBillIdFromSearchInput(context) ?? ExtractBillIdFromOtherSources(context);

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

        private string ExtractBillIdFromSearchInput(ActionExecutingContext ctx)
        {
            // Look for an action argument of type SearchInput
            foreach (var arg in ctx.ActionArguments.Values)
            {
                if (arg is SearchInput searchInput && searchInput.Input != null)
                {
                    return searchInput.Input;
                }
            }
            return null;
        }

        private string ExtractBillIdFromOtherSources(ActionExecutingContext ctx)
        {
            // -------- QUERY STRING --------
            var query = ctx.HttpContext.Request.Query;
            if (query.TryGetValue("billId", out var queryValues))
            {
                return queryValues.FirstOrDefault();
            }

            // -------- ROUTE DATA --------
            if (ctx.RouteData.Values.TryGetValue("billId", out var routeValue) && routeValue != null)
            {
                return routeValue.ToString();
            }

            // -------- ACTION ARGUMENTS (other than SearchInput) --------
            // 1. Direct argument named "billId"
            if (ctx.ActionArguments.TryGetValue("billId", out var argValue) && argValue != null)
            {
                return argValue.ToString();
            }

            // 2. Scan all arguments for a property named "billId"
            foreach (var arg in ctx.ActionArguments.Values)
            {
                if (arg == null || arg is SearchInput) continue; // Skip SearchInput (already handled)

                var prop = arg.GetType().GetProperty("billId",
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (prop != null && prop.CanRead)
                {
                    var val = prop.GetValue(arg);
                    if (val != null)
                        return val.ToString();
                }
            }

            return null;
        }

        private ApiResponseEnvelope<object> CreateEnvelope()
        {
            return new ApiResponseEnvelope<object>(
                (int)HttpStatusCode.Forbidden,
                null,
                null,
                new List<ApiError> { new ApiError(MessageResources.UnauthorizedZone) }
            );
        }
    }
}
