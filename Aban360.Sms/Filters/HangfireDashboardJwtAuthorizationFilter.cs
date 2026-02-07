using Aban360.Common.ApplicationUser;
using Hangfire.Dashboard;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Aban360.Sms.Filters
{
    public class HangfireDashboardJwtAuthorizationFilter : IDashboardAuthorizationFilter
    {        
        private static readonly string HangFireCookieName = "HangFireSmsCookie";
        private static readonly int CookieExpirationMinutes = 60;
        private TokenValidationParameters tokenValidationParameters;
        private readonly string[] _roles;

        internal HangfireDashboardJwtAuthorizationFilter(TokenValidationParameters tokenValidationParameters, string[] roles = null)
        {
            this.tokenValidationParameters = tokenValidationParameters;
            _roles = roles;
        }

        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            var access_token = string.Empty;
            var setCookie = false;
            var path = httpContext.Request.Path;
            var pathBase = httpContext.Request.PathBase;

            // try to get token from query string
            if (httpContext.Request.Query.ContainsKey("access_token") &&
                (path.StartsWithSegments("/main/admin/hangfire") || path.StartsWithSegments("/sms/main/admin/hangfire") ||
                 pathBase.StartsWithSegments("/main/admin/hangfire") || pathBase.StartsWithSegments("/sms/main/admin/hangfire")))
            {
                access_token = httpContext.Request.Query["access_token"].FirstOrDefault();
                setCookie = true;
            }
            else
            {
                access_token = httpContext.Request.Cookies[HangFireCookieName];
            }

            if (string.IsNullOrEmpty(access_token))
            {
                return false;
            }

            try
            {
                SecurityToken validatedToken = null;
                JwtSecurityTokenHandler hand = new JwtSecurityTokenHandler();
                var claims = hand.ValidateToken(access_token, tokenValidationParameters, out validatedToken);

                if (!CheckIsInRole(_roles, claims))
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                //logger.Error(e, "Error during dashboard hangfire jwt validation process");
                throw e;
            }

            if (setCookie)
            {
                httpContext.Response.Cookies.Append(HangFireCookieName,
                access_token,
                new CookieOptions()
                {
                    Expires = DateTime.Now.AddMinutes(CookieExpirationMinutes)
                });
            }
            return true;
        }
        private bool CheckIsInRole(string[] roles, ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal is null)
            {
                return false;
            }
            if (roles is null)
            {
                return false;
            }
            IAppUser user = new AppUser(claimsPrincipal);
            var r = user.Roles;
            foreach (string role in roles)
            {
                if (string.IsNullOrEmpty(role))
                {
                    return false;
                }
                if (claimsPrincipal.IsInRole(role))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
