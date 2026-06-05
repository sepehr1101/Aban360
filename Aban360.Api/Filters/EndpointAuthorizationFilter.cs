using Aban360.Common.ApplicationUser;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Constants;
using Aban360.UserPool.Persistence.Constants.Enums;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Aban360.Api.Filters
{
    public class EndpointAuthorizationFilter: IAsyncAuthorizationFilter
    {
        private readonly IUserClaimQueryService _userClaimQueryService;
        public EndpointAuthorizationFilter(IUserClaimQueryService userClaimQueryService)
        {
            _userClaimQueryService = userClaimQueryService;
            _userClaimQueryService.NotNull(nameof(_userClaimQueryService));
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            // Skip if [AllowAnonymous] exists
            var endpoint = context.HttpContext.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
            {               
                return;
            }           
            var actionName = GetControllerActionName(endpoint);
            if (string.IsNullOrWhiteSpace(actionName))
            {               
                return;
            }

            if (!context.HttpContext.User.Identity?.IsAuthenticated ?? true)
            {
                return;
            }
            IAppUser currentUser = new AppUser(context.HttpContext.User);
            if(!await _userClaimQueryService.HasValue(currentUser.UserId, ClaimType.Endpoint, actionName))
            {
                context.Result = new ObjectResult(CreateEnvelope())
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
                return;
            }
        }

        private string? GetControllerActionName(Endpoint? endpoint)
        {
            var actionDescriptor = endpoint?.Metadata.GetMetadata<ControllerActionDescriptor>();

            if (actionDescriptor == null)
                return null;

            return $"{actionDescriptor.ControllerName}.{actionDescriptor.ActionName}";
        }

        private ApiResponseEnvelope<object> CreateEnvelope()
        {
            ApiResponseEnvelope<object> envelope = new(
                (int)HttpStatusCode.Forbidden,
                null,
                null,
                new List<ApiError> { new ApiError(MessageResources.UnathorizedResource) }
            );
            return envelope;
        }
    }
}
