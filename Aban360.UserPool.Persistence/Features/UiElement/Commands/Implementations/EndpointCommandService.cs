using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.AceessTree.Entites;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.Features.UiElement.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Persistence.Features.UiElement.Commands.Implementations
{
    public sealed class EndpointCommandService : IEndpointCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Endpoint> _endPoints;
        public EndpointCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _endPoints = _uow.Set<Endpoint>();
            _endPoints.NotNull(nameof(_endPoints));
        }
        public async Task Add(Endpoint endpoint)
        {
            await _endPoints.AddAsync(endpoint);
        }
        public void Remove(Endpoint endpoint)
        {
            endpoint.IsActive = false;
        }
    }
}