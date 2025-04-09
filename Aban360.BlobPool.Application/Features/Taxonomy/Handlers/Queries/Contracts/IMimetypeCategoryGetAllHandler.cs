using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Queries;

namespace Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Queries.Contracts
{
    public interface IMimetypeCategoryGetAllHandler
    {
        Task<ICollection<MimetypeCategoryGetDto>> Handle(CancellationToken cancellationToken);
    }
}
