using Aban360.ClaimPool.Domain.Features.DMS.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.DMS.Handlers.Queries.Contracts
{
    public interface IDocumentEntityGetAllHandler
    {
        Task<ICollection<DocumentEntityGetDto>> Handle(CancellationToken cancellationToken);
    }
}
