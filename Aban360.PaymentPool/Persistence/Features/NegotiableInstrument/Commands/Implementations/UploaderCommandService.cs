using Aban360.Common.Extensions;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Entities;
using Aban360.PaymentPool.Persistence.Contexts.Contracts;
using Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Commands.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Commands.Implementations
{
    internal sealed class UploaderCommandService : IUploaderCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Uploader> _uploader;
        public UploaderCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _uploader = _uow.Set<Uploader>();
            _uploader.NotNull(nameof(_uploader));
        }

        public async Task Add(Uploader uploader)
        {
            await _uploader.AddAsync(uploader);
        }

        public async Task Remove(Uploader uploader)
        {
            _uploader.Remove(uploader);
        }
    }
}
