using Aban360.BlobPool.Domain.Features.DmsServices.Entities;

namespace Aban360.BlobPool.Persistence.Features.DmsServices.Queries.Contracts
{
    public interface IOpenKmMetaDataQueryServices
    {
        Task<IEnumerable<OpenKmMetaData>> Get();
    }
}
