using Aban360.BlobPool.Domain.Features.DMS.Dto.Queries;

namespace Aban360.BlobPool.Application.Features.DMS.Handlers.Queries.Contracts
{
    public interface IDocumentEntityGetAllHandler
    {
        Task<ICollection<DocumentEntityGetDto>> Handle(CancellationToken cancellationToken);
    }
}
