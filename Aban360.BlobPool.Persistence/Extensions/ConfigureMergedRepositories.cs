using Aban360.BlobPool.Persistence.Features.DMS.Commands.Contracts;
using Aban360.BlobPool.Persistence.Features.DMS.Queries.Contracts;
using Aban360.BlobPool.Persistence.Features.DMS.Repositories;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Commands.Contracts;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Queries.Contracts;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Aban360.BlobPool.Persistence.Extensions
{
    internal static class ConfigureMergedRepositories
    {
        private static readonly Type[] RepositoryTypes =
        {
            typeof(DocumentEntityRepository),
            typeof(DocumentCategoryRepository),
            typeof(DocumentRepository),
            typeof(DocumentTypeRepository),
            typeof(ExecutableMimetypeRepository),
            typeof(MimetypeCategoryRepository),
        };

        internal static bool IsMergedRepository(Type type) => RepositoryTypes.Contains(type);

        internal static void AddMergedPersistenceRepositories(this IServiceCollection services)
        {
            services.AddScoped<DocumentEntityRepository>();
            services.AddScoped<IDocumentEntityCommandService>(provider => provider.GetRequiredService<DocumentEntityRepository>());
            services.AddScoped<IDocumentEntityQueryService>(provider => provider.GetRequiredService<DocumentEntityRepository>());
            services.AddScoped<DocumentCategoryRepository>();
            services.AddScoped<IDocumentCategoryCommandService>(provider => provider.GetRequiredService<DocumentCategoryRepository>());
            services.AddScoped<IDocumentCategoryQueryService>(provider => provider.GetRequiredService<DocumentCategoryRepository>());
            services.AddScoped<DocumentRepository>();
            services.AddScoped<IDocumentCommandService>(provider => provider.GetRequiredService<DocumentRepository>());
            services.AddScoped<IDocumentQueryService>(provider => provider.GetRequiredService<DocumentRepository>());
            services.AddScoped<DocumentTypeRepository>();
            services.AddScoped<IDocumentTypeCommandService>(provider => provider.GetRequiredService<DocumentTypeRepository>());
            services.AddScoped<IDocumentTypeQueryService>(provider => provider.GetRequiredService<DocumentTypeRepository>());
            services.AddScoped<ExecutableMimetypeRepository>();
            services.AddScoped<IExecutableMimetypeCommandService>(provider => provider.GetRequiredService<ExecutableMimetypeRepository>());
            services.AddScoped<IExecutableMimetypeQueryService>(provider => provider.GetRequiredService<ExecutableMimetypeRepository>());
            services.AddScoped<MimetypeCategoryRepository>();
            services.AddScoped<IMimetypeCategoryCommandService>(provider => provider.GetRequiredService<MimetypeCategoryRepository>());
            services.AddScoped<IMimetypeCategoryQueryService>(provider => provider.GetRequiredService<MimetypeCategoryRepository>());
        }
    }
}
