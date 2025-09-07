using Aban360.BlobPool.Domain.Features.DmsServices.Entities;
using Aban360.BlobPool.Persistence.Contexts.Contracts;
using Aban360.BlobPool.Persistence.Features.DmsServices.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.BlobPool.Persistence.Features.DmsServices.Queries.Implementations
{
    internal sealed class OpenKmMetaDataQueryServices : IOpenKmMetaDataQueryServices
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<OpenKmMetaData> _openKmMetaData;
        public OpenKmMetaDataQueryServices(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _openKmMetaData = _uow.Set<OpenKmMetaData>();
            _openKmMetaData.NotNull(nameof(_openKmMetaData));
        }

        public async Task<IEnumerable<OpenKmMetaData>> Get()
        {
            return await _openKmMetaData.ToListAsync();
        }
    }
}
