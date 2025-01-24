using Microsoft.OpenApi.Models;
namespace Aban360.Api.Extensions
{
    internal static class ConfigureSwagger
    {
        internal static void AddSwaggerService(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Aban360", Version = "0.0.1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Sms Hub", Version = "0.0.1" });
            //    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme()
            //    {
            //        Type = SecuritySchemeType.ApiKey,
            //        In = ParameterLocation.Header,
            //        Name = HeaderKeys.ApiKeyHeaderName,
            //        Description = "Authorize Using Api-Key, this key will be sent in the request header",
            //        Scheme = "ApiKey"
            //    });
            //    var key = new OpenApiSecurityScheme()
            //    {
            //        Reference = new OpenApiReference
            //        {
            //            Type = ReferenceType.SecurityScheme,
            //            Id = "ApiKey"
            //        },
            //        In = ParameterLocation.Header
            //    };
            //    var requirement = new OpenApiSecurityRequirement
            //        {
            //            { key, new List<string>() }
            //        };
            //    c.AddSecurityRequirement(requirement);
            //});
        }
        internal static void AddSwaggerApp(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }
}
