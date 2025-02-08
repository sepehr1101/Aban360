using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.AceessTree.Entites;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.Features.UiElement.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Persistence.Features.UiElement.Queries.Implementations
{
    public sealed class EndpointCommandService : IEndpointQueryService
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
        public IQueryable<Endpoint> GetQuery()
        {
            return _endPoints.AsQueryable();
        }
        public async Task<ICollection<Endpoint>> Get()
        {
            return await _endPoints
                .Where(endpoint => endpoint.IsActive)
                .ToListAsync();
        }
        public async Task<ICollection<Endpoint>> GetInclude()
        {
            return await _endPoints
                .Include(endpoint => endpoint.SubModule)
                .Where(endpoint => endpoint.IsActive)
                .ToListAsync();
        }
        public async Task<Endpoint> Get(int id)
        {
            return await _uow.FindOrThrowAsync<Endpoint>(id);
        }
        public async Task<Endpoint> GetInclude(int id)
        {
            return await _endPoints
                .Include(endpoint => endpoint.SubModule)
                .Where(endpoint => endpoint.Id == id)
                .SingleAsync();
        }
    }
}