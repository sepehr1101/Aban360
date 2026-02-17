using Aban360.BlobPool.Domain.Features.DmsServices.Entities;
using Aban360.Common.BaseEntities;

namespace Aban360.BlobPool.Persistence.Features.DmsServices.Queries.Contracts
{
    public interface IOpenKmMetaDataQueryServices
    {
        Task<IEnumerable<OpenKmMetaData>> Get();
        Task<IEnumerable<NumericDictionary>> GetFileTitles();
    }
}
