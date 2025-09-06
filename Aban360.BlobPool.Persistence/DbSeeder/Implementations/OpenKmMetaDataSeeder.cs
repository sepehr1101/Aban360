using Aban360.BlobPool.Domain.Features.DmsServices.Entities;
using Aban360.BlobPool.Domain.Features.Taxonomy.Entities;
using Aban360.BlobPool.Persistence.Contexts.Contracts;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.BlobPool.Persistence.DbSeeder.Implementations
{
    public class OpenKmMetaDataSeeder :IDataSeeder
    {
        public int Order { get; set; } = 12;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<OpenKmMetaData> _openKmMetaDatas;
        public OpenKmMetaDataSeeder(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _openKmMetaDatas = _uow.Set<OpenKmMetaData>();
            _openKmMetaDatas.NotNull(nameof(_openKmMetaDatas));
        }


        public void SeedData()
        {
            if (_openKmMetaDatas.Any())
            {
                return;
            }

            string sqlFilePath = GetSqlFilePath();
            _uow.ExecuteBatch(sqlFilePath);

        }
        private string GetSqlFilePath()
        {
            string basePath = AppContext.BaseDirectory;
            string relativePath = @"\DbSeeder\DataScript\OpenKmMetaData.sql";

            string path = string.Concat(basePath, relativePath);
            return path;
        }
    }
}
