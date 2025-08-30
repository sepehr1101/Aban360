using Aban360.UserPool.Application.Features.Auth.Services.Contracts;
using Aban360.UserPool.Domain.Constants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System.Text;

namespace Aban360.Api.Extensions
{
    public static class ConfigureAuth
    {
        public static void AddAuth(this IServiceCollection services, IConfiguration configuration)
        {
            // Only needed for custom roles.
            AddCustomAuthorization(services);

            // Needed for jwt auth.
            AddCustomJwtAuthentication(services, configuration);
        }
        private static void AddCustomAuthorization(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(BaseRoles.Admin, policy => policy.RequireRole(BaseRoles.Admin));
                options.AddPolicy(BaseRoles.Programmer, policy => policy.RequireRole(BaseRoles.Programmer));
            });
        }
       
        public static void AddCustomJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddAuthentication(options => ConfigureAuthenticationDefaults(options))
                .AddJwtBearer(cfg =>
                {
                    ConfigureJwtBearer(cfg, configuration);
                });
        }

        private static void ConfigureAuthenticationDefaults(AuthenticationOptions options)
        {
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        }

        private static void ConfigureJwtBearer(JwtBearerOptions cfg, IConfiguration configuration)
        {
            cfg.RequireHttpsMetadata = false;
            cfg.SaveToken = true;
            cfg.TokenValidationParameters = GetTokenValidationParameters(configuration);
            cfg.Events = CreateJwtBearerEvents();
        }

        private static TokenValidationParameters GetTokenValidationParameters(IConfiguration configuration)
        {
            return new TokenValidationParameters
            {
                ValidIssuer = configuration["BearerTokens:Issuer"],
                ValidateIssuer = false, // TODO: change to true to avoid forwarding attacks
                ValidAudience = configuration["BearerTokens:Audience"],
                ValidateAudience = false, // TODO: change to true to avoid forwarding attacks
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["BearerTokens:Key"])),
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        }

        private static JwtBearerEvents CreateJwtBearerEvents()
        {
            return new JwtBearerEvents
            {
                OnAuthenticationFailed = HandleAuthenticationFailed,
                OnTokenValidated = HandleTokenValidated,
                OnMessageReceived = HandleMessageReceived,
                OnChallenge = HandleChallenge,
                OnForbidden = HandleForbidden
            };
        }

        private static Task HandleAuthenticationFailed(AuthenticationFailedContext context)
        {
            var logger = context.HttpContext.RequestServices
                .GetRequiredService<ILoggerFactory>()
                .CreateLogger(nameof(JwtBearerEvents));

            logger.LogError("Authentication failed.", context.Exception);
            return Task.CompletedTask;
        }

        private static Task HandleTokenValidated(TokenValidatedContext context)
        {
            var endpoint = context.HttpContext.GetEndpoint();
            if (!(endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() is object))
            {
                var tokenValidatorService = context.HttpContext.RequestServices
                    .GetRequiredService<ITokenValidatorService>();

                return tokenValidatorService.ValidateAsync(context);
            }

            return Task.CompletedTask;
        }

        private static Task HandleMessageReceived(MessageReceivedContext context)
        {
            string accessTokenKey = "access_token";
            var accessToken = context.Request.Query[accessTokenKey];

            if (!string.IsNullOrEmpty(accessToken))
            {
                string? token = context.Request.Query[accessTokenKey][0];
                if (!string.IsNullOrEmpty(token) &&
                    !context.Request.Headers.ContainsKey(HeaderNames.Authorization))
                {
                    context.Request.Headers.Add(HeaderNames.Authorization, $"Bearer {token}");
                    context.Token = accessToken;
                }
            }

            return Task.CompletedTask;
        }

        private static Task HandleChallenge(JwtBearerChallengeContext context)
        {
            var logger = context.HttpContext.RequestServices
                .GetRequiredService<ILoggerFactory>()
                .CreateLogger(nameof(JwtBearerEvents));

            logger.LogError("OnChallenge error: {Error}, {Description}", context.Error, context.ErrorDescription);
            return Task.CompletedTask;
        }

        private static Task HandleForbidden(ForbiddenContext context)
        {
            var logger = context.HttpContext.RequestServices
                .GetRequiredService<ILoggerFactory>()
                .CreateLogger(nameof(JwtBearerEvents));

            logger.LogError("OnForbidden error at path {Path}", context.Request.Path);
            return Task.CompletedTask;
        }
    }
}
