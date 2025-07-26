namespace Aban360.Api.Extensions
{
    internal static class ConfigureCors
    {
        private static string _corsPolicyName = "CorsPolicy";
        internal static void AddCustomCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(_corsPolicyName,
                    builder => builder
                        .AllowAnyOrigin()
                        //.WithOrigins("http://*.aban360.ir","https://*.aban360.ir", "http://*.5ch.ir", "http://192.168.13.50") //Note:  The URL must be specified without a trailing slash (/).
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .SetIsOriginAllowed((host) => true)
                        //.AllowCredentials()
                        .SetPreflightMaxAge(TimeSpan.FromMinutes(120))
                        .WithExposedHeaders("Content-Disposition"));
            });
        }

        internal static void UseCustomCors(this IApplicationBuilder app)
        {
            app.UseCors(_corsPolicyName);
        }
    }
}
