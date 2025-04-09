using Aban360.BlobPool.Domain.Features.Taxonomy.Entities;
using Aban360.BlobPool.Persistence.Contexts.Contracts;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.BlobPool.Persistence.Features.Taxonomy.Commands.Implementations
{
    internal sealed class DocumentCommandService : IDocumentCommandService
    {
        private readonly IUnitOfwork _uow;
        private readonly DbSet<Document> _document;
        public DocumentCommandService(IUnitOfwork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _document = _uow.Set<Document>();
            _document.NotNull(nameof(_document));
        }

        public async Task Add(Document document)
        {
            await _document.AddAsync(document);
        }

        public async Task Remove(Document document)
        {
            _document.Remove(document);
        }
    }

}
