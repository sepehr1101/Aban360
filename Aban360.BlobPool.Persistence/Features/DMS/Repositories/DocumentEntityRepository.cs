using Aban360.BlobPool.Domain.Features.DMS.Entities;
using Aban360.BlobPool.Persistence.Contexts.Contracts;
using Aban360.BlobPool.Persistence.Features.DMS.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using Aban360.BlobPool.Persistence.Features.DMS.Queries.Contracts;

namespace Aban360.BlobPool.Persistence.Features.DMS.Repositories
{
    internal sealed class DocumentEntityRepository : IDocumentEntityCommandService, IDocumentEntityQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<DocumentEntity> _documentEntity;
        public DocumentEntityRepository(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));
            _documentEntity = _uow.Set<DocumentEntity>();
            _documentEntity.NotNull(nameof(_documentEntity));
        }

        public async Task Add(DocumentEntity documentEntity)
        {
            await _documentEntity.AddAsync(documentEntity);
        }

        public async Task Remove(DocumentEntity documentEntity)
        {
            _documentEntity.Remove(documentEntity);
        }

        public async Task<DocumentEntity> Get(long id)
        {
            return await _uow.FindOrThrowAsync<DocumentEntity>(id);
        }
        public async Task<ICollection<DocumentEntity>> Get(string id)
        {
            return await _documentEntity
                .Where(d=>d.BillId == id)
                .ToListAsync();
        }

        public async Task<ICollection<DocumentEntity>> Get()
        {
            return await _documentEntity.ToListAsync();
        }
    }
}
