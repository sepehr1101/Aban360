using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Draft.Commands.Implementations
{
    internal sealed class RequestEstateCommandService : IRequestEstateCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<RequestEstate> _requestEstates;
        public RequestEstateCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _requestEstates = _uow.Set<RequestEstate>();
            _requestEstates.NotNull(nameof(_requestEstates));
        }
        public async Task Add(RequestEstate requestEstate)
        {
            await _requestEstates.AddAsync(requestEstate);
        }
        public async Task Add(IEnumerable<RequestEstate> requestEstates)
        {
            await _requestEstates.AddRangeAsync(requestEstates);
        }
        public async void Remove(RequestEstate requestEstate)
        {
            _requestEstates.Remove(requestEstate);
        }
    }
}
