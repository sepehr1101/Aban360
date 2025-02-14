using Aban360.ClaimPool.Domain.Features.WasteWater;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.WasteWater.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.WasteWater.Queries.Implementation
{
    public class siphonMaterialQueryService : ISiphonMaterialQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<SiphonMaterial> _siphonMaterial;
        public siphonMaterialQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _siphonMaterial = _uow.Set<SiphonMaterial>();
            _siphonMaterial.NotNull(nameof(_siphonMaterial));
        }

        public async Task<SiphonMaterial> Get(short id)
        {
            return await _siphonMaterial
                .Include(i => i.Siphons)
                .Where(i => i.Id == id)
                .SingleAsync();
        }

        public async Task<ICollection<SiphonMaterial>> Get()
        {
            return await _siphonMaterial.ToListAsync();
        }
    }
}
