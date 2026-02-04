using Hangfire;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Aban360.Sms.Filters;

namespace Aban360.Sms.Extensions
{
    internal static class ConfigureHangfire
    {
        private const string _dashboardRoute = "/main/admin/hangfire";
        internal static void AddHangfire(this WebApplicationBuilder builder)
        {
            builder.Services.AddHangfire(x =>
            {
                x.UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddHangfireServer();
        }
        internal static void AddHangfireDashboard(this IApplicationBuilder app, IConfiguration configuration)
        {
            var dashboardOptions = new DashboardOptions
            {
                Authorization =
                [
                    new HangfireDashboardJwtAuthorizationFilter(GetTokenValidationParameters(configuration), ["Admin", "Programmer"])
                ],
                IgnoreAntiforgeryToken = true,
                DashboardTitle = "داشبورد پیامک",
            };
            app.UseHangfireDashboard(_dashboardRoute, dashboardOptions);
        }
        private static TokenValidationParameters GetTokenValidationParameters(IConfiguration configuration)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = configuration["BearerTokens:Issuer"], // site that makes the token
                ValidateIssuer = false, // TODO: change this to avoid forwarding attacks
                ValidAudience = configuration["BearerTokens:Audience"], // site that consumes the token
                ValidateAudience = false, // TODO: change this to avoid forwarding attacks
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["BearerTokens:Key"])),
                ValidateIssuerSigningKey = true, // verify signature to avoid tampering
                ValidateLifetime = true, // validate the expiration
                ClockSkew = TimeSpan.Zero // tolerance for the expiration date
            };
            return tokenValidationParameters;
        }
    }
}
