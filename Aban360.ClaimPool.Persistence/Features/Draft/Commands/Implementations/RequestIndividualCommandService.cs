using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Draft.Commands.Implementations
{
    internal sealed class RequestIndividualCommandService : IRequestIndividualCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<RequestIndividual> _requestIndividual;
        public RequestIndividualCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _requestIndividual = _uow.Set<RequestIndividual>();
            _requestIndividual.NotNull(nameof(_requestIndividual));
        }

        public async Task Add(RequestIndividual requestIndividual)
        {
            await _requestIndividual.AddAsync(requestIndividual);
        }

        public async Task Remove(RequestIndividual requestIndividual)
        {
            _requestIndividual.Remove(requestIndividual);
        }
    }
}
