using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using System.Reflection;

namespace Aban360.BlobPool.Persistence.Extensions
{
    public static class ConfigureServices
    {
        public static void AddBlobPoolPersistenceInjections(this IServiceCollection services)
        {
            // ثبت Repositoryها جداست تا دو قرارداد خواندن و نوشتن یک نمونه مشترک در هر Scope بگیرند.
            services.Scan(scan =>
                scan
                    .FromAssemblies(Assembly.GetExecutingAssembly())
                    .AddClasses(classes => classes.InNamespaces("Aban360.BlobPool.Persistence").Where(type => !ConfigureMergedRepositories.IsMergedRepository(type)), publicOnly: false)
                    .UsingRegistrationStrategy(RegistrationStrategy.Append)
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());

            services.AddMergedPersistenceRepositories();
        }
    }
}
