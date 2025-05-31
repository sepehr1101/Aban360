using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Draft.Commands.Implementations
{
    internal sealed class RequestTrackingCommandService : IRequestTrackingCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<RequestTracking> _requestTracking;
        public RequestTrackingCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _requestTracking = _uow.Set<RequestTracking>();
            _requestTracking.NotNull(nameof(_requestTracking));
        }

        public async Task Add(RequestTracking requestTracking)
        {
            await _requestTracking.AddAsync(requestTracking);
        }

        public async Task Remove(RequestTracking requestTracking)
        {
            _requestTracking.Remove(requestTracking);
        }
    }
}