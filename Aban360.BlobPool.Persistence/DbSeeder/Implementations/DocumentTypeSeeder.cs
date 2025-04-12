using Aban360.BlobPool.Domain.Features.Taxonomy.Entities;
using Aban360.BlobPool.Persistence.Contexts.Contracts;
using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.BlobPool.Persistence.DbSeeder.Implementations
{
    public class DocumentTypeSeeder : IDataSeeder
    {
        public int Order { get; set; } = 12;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<DocumentType> _documentTypes;
        public DocumentTypeSeeder(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _documentTypes = _uow.Set<DocumentType>();
            _documentTypes.NotNull(nameof(_documentTypes));
        }


        public void SeedData()
        {
            if (_documentTypes.Any())
            {
                return;
            }

            string sqlFilePath = GetSqlFilePath();
            _uow.ExecuteBatch(sqlFilePath);

        }
        private string GetSqlFilePath()
        {
            string basePath = AppContext.BaseDirectory;
            string relativePath = @"\DbSeeder\DataScript\DocumentType.sql";

            string path = string.Concat(basePath, relativePath);
            return path;
        }
    }
}
