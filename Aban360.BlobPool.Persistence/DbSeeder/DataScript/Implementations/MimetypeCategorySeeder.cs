using Aban360.BlobPool.Domain.Features.Taxonomy.Entities;
using Aban360.BlobPool.Persistence.Contexts.Contracts;
using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.BlobPool.Persistence.DbSeeder.DataScript.Implementations
{
    public class MimetypeCategorySeeder:IDataSeeder
    {
        public int Order { get; set; } = 10;
        private readonly IUnitOfwork _uow;
        private readonly DbSet<MimetypeCategory> _mimetypeCategories;
        public MimetypeCategorySeeder(
            IUnitOfwork uow,
            DbSet<MimetypeCategory> mimetypeCategories)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _mimetypeCategories = _uow.Set<MimetypeCategory>();
            _mimetypeCategories.NotNull(nameof(_mimetypeCategories));
        }


        public void SeedData()
        {
            
        }
    }
}
