using Aban360.BlobPool.Domain.Features.DmsServices.Entities;
using Aban360.BlobPool.Persistence.Contexts.Contracts;
using Aban360.BlobPool.Persistence.Features.DmsServices.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.BlobPool.Persistence.Features.DmsServices.Queries.Implementations
{
    internal sealed class OpenKmMetaDataQueryServices : IOpenKmMetaDataQueryServices
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<OpenKmMetaData> _openKmMetaData;
        private string _fileTypes = "okp:moshtarakin.title";
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

        public async Task<IEnumerable<NumericDictionary>> GetFileTitles()
        {
            return await _openKmMetaData
                    .Where(o => o.Section == _fileTypes)
                    .Select(o => new NumericDictionary(o.Id, o.Label))
                    .ToListAsync();
        }
    }
}
