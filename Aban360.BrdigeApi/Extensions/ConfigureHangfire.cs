using Hangfire;

namespace Aban360.BrdigeApi.Extensions
{
    public static class ConfigureHangfire
    {
        internal static void AddHangfire(this WebApplicationBuilder builder)
        {
            builder.Services.AddHangfire(x =>
            {
                x.UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
        }
    }
}
