using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Draft.Commands.Implementations
{
    internal sealed class RequestIndividualEstateCommandService : IRequestIndividualEstateCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<RequestIndividualEstate> _requestIndividualEstate;
        public RequestIndividualEstateCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _requestIndividualEstate = _uow.Set<RequestIndividualEstate>();
            _requestIndividualEstate.NotNull(nameof(_requestIndividualEstate));
        }

        public async Task Add(RequestIndividualEstate requestIndividualEstate)
        {
            await _requestIndividualEstate.AddAsync(requestIndividualEstate);
        }

        public async Task Remove(RequestIndividualEstate requestIndividualEstate)
        {
            _requestIndividualEstate.Remove(requestIndividualEstate);
        }
    }
}
