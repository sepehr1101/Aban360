using Microsoft.AspNetCore.Mvc;

namespace Aban360.BrdigeApi.Extensions
{
    public static class ConfigureAntiForgery
    {
        public static void AddCustomAntiforgery(this IServiceCollection services)
        {
            services.AddAntiforgery(x => x.HeaderName = "X-XSRF-TOKEN");
            services.AddMvc(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });
        }

    }
}