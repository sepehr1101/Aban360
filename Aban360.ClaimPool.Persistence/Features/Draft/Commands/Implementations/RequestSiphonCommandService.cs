using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Draft.Commands.Implementations
{
    internal sealed class RequestSiphonCommandService : IRequestSiphonCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<RequestSiphon> _requestSiphon;
        public RequestSiphonCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _requestSiphon = _uow.Set<RequestSiphon>();
            _requestSiphon.NotNull(nameof(_requestSiphon));
        }

        public async Task Add(RequestSiphon requestSiphon)
        {
            await _requestSiphon.AddAsync(requestSiphon);
        }

        public void Remove(RequestSiphon requestSiphon)
        {
            _requestSiphon.Remove(requestSiphon);
        }
    }
}
