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
        private readonly IUnitOfwork _uow;
        private readonly DbSet<DocumentType> _documentTypes;
        public DocumentTypeSeeder(IUnitOfwork uow)
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

            ICollection<DocumentType> documentTypes = new List<DocumentType>()
            {
                new DocumentType(){Id=1,Title="",DocumentCategoryId=1,Icon="",Css=""},
                new DocumentType(){Id=2,Title="",DocumentCategoryId=1,Icon="",Css=""},
                new DocumentType(){Id=3,Title="",DocumentCategoryId=1,Icon="",Css=""},
                new DocumentType(){Id=4,Title="",DocumentCategoryId=1,Icon="",Css=""},
                new DocumentType(){Id=5,Title="",DocumentCategoryId=1,Icon="",Css=""},
            };
            _documentTypes.AddRange(documentTypes);
            _uow.SaveChanges();

        }
    }
}
