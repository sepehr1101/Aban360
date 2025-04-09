using Aban360.BlobPool.Domain.Features.Taxonomy.Entities;
using Aban360.BlobPool.Persistence.Contexts.Contracts;
using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;
using System.Net.NetworkInformation;

namespace Aban360.BlobPool.Persistence.DbSeeder.Implementations
{
    public class MimetypeCategorySeeder : IDataSeeder
    {
        public int Order { get; set; } = 10;
        private readonly IUnitOfwork _uow;
        private readonly DbSet<MimetypeCategory> _mimetypeCategories;
        public MimetypeCategorySeeder(IUnitOfwork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _mimetypeCategories = _uow.Set<MimetypeCategory>();
            _mimetypeCategories.NotNull(nameof(_mimetypeCategories));
        }


        public void SeedData()
        {
            if (_mimetypeCategories.Any())
            {
                return;
            }

            ICollection<MimetypeCategory> mimetypeCategories = new List<MimetypeCategory>()
            {
                new MimetypeCategory(){Id=1,Name="عکس",Title="Image"},
                new MimetypeCategory(){Id=2,Name="فیلم",Title="Video"},
                new MimetypeCategory(){Id=3,Name="متن",Title="Text"},
                new MimetypeCategory(){Id=4,Name="سند متنی",Title="Word"},
                new MimetypeCategory(){Id=5,Name="اکسل",Title="Excel"},
                new MimetypeCategory(){Id=6,Name="prd",Title="pdf"},
            };
            _mimetypeCategories.AddRange(mimetypeCategories);
            _uow.SaveChanges();

        }
    }
}
