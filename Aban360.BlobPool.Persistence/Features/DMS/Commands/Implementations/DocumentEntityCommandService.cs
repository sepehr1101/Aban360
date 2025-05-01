using Aban360.BlobPool.Domain.Features.DMS.Entities;
using Aban360.BlobPool.Persistence.Contexts.Contracts;
using Aban360.BlobPool.Persistence.Features.DMS.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.BlobPool.Persistence.Features.DMS.Commands.Implementations
{
    internal sealed class DocumentEntityCommandService : IDocumentEntityCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<DocumentEntity> _documentEntity;
        public DocumentEntityCommandService(IUnitOfWork uow)
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
    }
}
