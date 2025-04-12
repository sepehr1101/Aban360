using Aban360.ClaimPool.Domain.Features.DMS.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.DMS.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.DMS.Queries.Implementations
{
    internal sealed class DocumentEntityQueryService : IDocumentEntityQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<DocumentEntity> _documentEntity;
        public DocumentEntityQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _documentEntity = _uow.Set<DocumentEntity>();
            _documentEntity.NotNull(nameof(_documentEntity));
        }

        public async Task<DocumentEntity> Get(long id)
        {
            return await _uow.FindOrThrowAsync<DocumentEntity>(id);
        }

        public async Task<ICollection<DocumentEntity>> Get()
        {
            return await _documentEntity.ToListAsync();
        }
    }
}
