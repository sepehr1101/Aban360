using Aban360.BlobPool.Domain.Features.Taxonomy.Entities;
using Aban360.BlobPool.Persistence.Contexts.Contracts;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Queries.Contracts;

namespace Aban360.BlobPool.Persistence.Features.Taxonomy.Repositories
{
    internal sealed class DocumentTypeRepository : IDocumentTypeCommandService, IDocumentTypeQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<DocumentType> _documentTypes;
        public DocumentTypeRepository(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));
            _documentTypes = _uow.Set<DocumentType>();
            _documentTypes.NotNull(nameof(uow));
        }
        public async Task Add(DocumentType documentType)
        {
            await _documentTypes.AddAsync(documentType);
        }
        public void AddSync(DocumentType documentType)
        {
            _documentTypes.Add(documentType);
        }
        public async Task Add(ICollection<DocumentType> documentTypes)
        {
            await _documentTypes.AddRangeAsync(documentTypes);
        }
        public void AddSync(ICollection<DocumentType> documentTypes)
        {
            _documentTypes.AddRange(documentTypes);
        }
        public void Remove(DocumentType documentType)
        {
            _documentTypes.Remove(documentType);
        }
        public void Remove(ICollection<DocumentType> documentTypes)
        {
            _documentTypes.RemoveRange(documentTypes);
        }

        public async Task<ICollection<DocumentType>> Get()
        {
            return await _documentTypes
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<DocumentType> Get(short id)
        {
            return await _uow
                .FindOrThrowAsync<DocumentType>(id);
        }
    }
}
