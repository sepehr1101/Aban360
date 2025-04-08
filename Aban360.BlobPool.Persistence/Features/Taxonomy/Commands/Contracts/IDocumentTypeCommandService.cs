using Aban360.BlobPool.Domain.Features.Classification;

namespace Aban360.BlobPool.Persistence.Features.Taxonomy.Commands.Contracts
{
    public interface IDocumentTypeCommandService
    {
        Task Add(DocumentType documentType);
        Task Add(ICollection<DocumentType> documentTypes);
        void AddSync(DocumentType documentType);
        void AddSync(ICollection<DocumentType> documentTypes);
        void Remove(DocumentType documentType);
        void Remove(ICollection<DocumentType> documentTypes);
    }
}