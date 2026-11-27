using Aban360.Common.Extensions;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Entities;
using Aban360.PaymentPool.Persistence.Contexts.Contracts;
using Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Queries.Impelmentations
{
    internal sealed class UploaderQueryService : IUploaderQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Uploader> _uploader;
        public UploaderQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _uploader = _uow.Set<Uploader>();
            _uploader.NotNull(nameof(_uploader));
        }

        public async Task<Uploader> Get(int id)
        {
            return await _uow.FindOrThrowAsync<Uploader>(id);
        }

        public async Task<ICollection<Uploader>> Get()
        {
            return await _uploader.ToListAsync();
        }
    }
}
