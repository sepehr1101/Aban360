using Aban360.UserPool.Application.Features.Auth.Services.Contracts;
using Aban360.UserPool.Domain.Constants;
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
        private static void AddCustomJwtAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            services
               .AddAuthentication(options =>
               {
                   options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                   options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                   options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
               })
               .AddJwtBearer(cfg =>
               {
                   cfg.RequireHttpsMetadata = false;
                   cfg.SaveToken = true;
                   cfg.TokenValidationParameters = new TokenValidationParameters
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
                   cfg.Events = new JwtBearerEvents
                   {
                       OnAuthenticationFailed = context =>
                       {
                           //RawSqlHelper.InsertLogRequest(connectionString, CreateLogRequest(context.HttpContext));
                           var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(JwtBearerEvents));
                           logger.LogError("Authentication failed.", context.Exception);
                           return Task.CompletedTask;
                       },
                       OnTokenValidated = context =>
                       {
                           var endpoint = context.HttpContext.GetEndpoint();
                           if (!(endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() is object))
                           {
                               var tokenValidatorService = context.HttpContext.RequestServices.GetRequiredService<ITokenValidatorService>();
                               var validated = tokenValidatorService.ValidateAsync(context);
                               return validated;
                           }
                           return Task.CompletedTask;
                       },
                       OnMessageReceived = context =>
                       {
                           string accessTokenKey = "access_token";
                           var accessToken = context.Request.Query[accessTokenKey];
                           var path = context.HttpContext.Request.Path;
                           if (!string.IsNullOrEmpty(accessToken))
                           {
                               string? token = context.Request.Query[accessTokenKey][0];
                               if (!string.IsNullOrEmpty(token))
                               {
                                   if (!context.Request.Headers.ContainsKey(HeaderNames.Authorization))
                                   {
                                       context.Request.Headers.Add(HeaderNames.Authorization, $"Bearer {token}");
                                       context.Token = accessToken;
                                   }
                               }
                           }
                           /* if(context.Token is null)
                            {
                                //context.Fail(new Exception());
                            }*/
                           return Task.CompletedTask;
                       },
                       OnChallenge = context =>
                       {
                           var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(JwtBearerEvents));
                           logger.LogError("OnChallenge error", context.Error, context.ErrorDescription);
                           return Task.CompletedTask;
                       },
                       OnForbidden = context =>
                       {
                           var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(JwtBearerEvents));
                           logger.LogError("OnForbidden error", context.Request.Path, context.Request.Path);
                           return Task.CompletedTask;
                       }
                   };
               });
        }
    }
}
