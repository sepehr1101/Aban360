namespace Aban360.BrdigeApi.Filters
{
    using Aban360.Common.ApplicationUser;
    using Aban360.Common.Categories.ApiResponse;
    using Aban360.Common.Db.QueryServices;
    using Aban360.UserPool.Domain.Constants;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using System.Net;

    public class ZoneAuthorizationFilter : IAsyncActionFilter
    {
        private readonly ICommonZoneService _zoneService;

        public ZoneAuthorizationFilter(ICommonZoneService zoneService)
        {
            _zoneService = zoneService;
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

            // 2. Extract zone IDs from query, route, body
            var requestZoneIds = ExtractZoneIds(context);

            if (requestZoneIds.Count == 0)
            {
                await next();
                return;
            }

            // 3. Get user's allowed zones
            IAppUser currentUser = new AppUser(context.HttpContext.User);
            var allowedZones = await _zoneService.GetMyZoneIds(currentUser);

            // 4. Authorization check
            if (!requestZoneIds.All(z => allowedZones.Contains(z)))
            {
                context.Result = new ObjectResult(CreateEnvelope())
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
                return;
            }

            await next();
        }

        private List<int> ExtractZoneIds(ActionExecutingContext ctx)
        {
            var zoneIds = new List<int>();

            // -------- QUERY STRING --------
            var query = ctx.HttpContext.Request.Query;

            if (query.TryGetValue("zoneId", out var single))
                if (int.TryParse(single, out var parsed))
                    zoneIds.Add(parsed);

            if (query.TryGetValue("zoneIds", out var multi))
            {
                foreach (var idStr in multi.ToString().Split(',', StringSplitOptions.RemoveEmptyEntries))
                    if (int.TryParse(idStr, out var parsed))
                        zoneIds.Add(parsed);
            }

            // -------- ROUTE DATA --------
            foreach (var rd in ctx.RouteData.Values)
            {
                if (rd.Key.Equals("zoneId", StringComparison.OrdinalIgnoreCase) &&
                    int.TryParse(rd.Value?.ToString(), out var parsed))
                    zoneIds.Add(parsed);
            }

            // -------- BODY/MODEL BINDING --------
            foreach (var arg in ctx.ActionArguments.Values)
            {
                if (arg != null)
                    ExtractZoneIdsFromObject(arg, zoneIds);
            }

            return zoneIds;
        }

        private void ExtractZoneIdsFromObject(object obj, List<int> result)
        {
            if (obj == null)
                return;

            var type = obj.GetType();

            // Iterate over properties
            foreach (var prop in type.GetProperties())
            {
                var name = prop.Name;

                // Only properties that actually contain "ZoneId"
                var isZoneProperty = name.Contains("zoneId", StringComparison.OrdinalIgnoreCase);

                if (!isZoneProperty)
                    continue;

                var value = prop.GetValue(obj);
                if (value == null)
                    continue;

                // single scalar zoneId
                if (value is int single)
                {
                    result.Add(single);
                    continue;
                }

                // list of zoneIds
                if (value is IEnumerable<int> enumerable)
                {
                    result.AddRange(enumerable);
                    continue;
                }

                // nested object (recursive)
                ExtractZoneIdsFromObject(value, result);
            }
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
