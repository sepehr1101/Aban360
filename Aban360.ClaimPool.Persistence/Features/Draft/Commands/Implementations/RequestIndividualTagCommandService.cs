using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Draft.Commands.Implementations
{
    internal sealed class RequestIndividualTagCommandService : IRequestIndividualTagCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<RequestIndividualTag> _requestIndividualTag;
        public RequestIndividualTagCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _requestIndividualTag = _uow.Set<RequestIndividualTag>();
            _requestIndividualTag.NotNull(nameof(_requestIndividualTag));
        }

        public async Task Add(RequestIndividualTag requestIndividualTag)
        {
            await _requestIndividualTag.AddAsync(requestIndividualTag);
        }

        public async Task Remove(RequestIndividualTag requestIndividualTag)
        {
            _requestIndividualTag.Remove(requestIndividualTag);
        }
        public void Remove(ICollection<RequestIndividualTag> requestIndividualTags)
        {
            _requestIndividualTag.RemoveRange(requestIndividualTags);
        }
    }
}
