using Aban360.ClaimPool.Domain.Features.WasteWater.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.WasteWater.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.WasteWater.Commands.Implementation
{
    internal sealed class SiphonMaterialCommandService : ISiphonMaterialCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<SiphonMaterial> _siphonMaterial;
        public SiphonMaterialCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _siphonMaterial = _uow.Set<SiphonMaterial>();
            _siphonMaterial.NotNull(nameof(_siphonMaterial));
        }

        public async Task Add(SiphonMaterial siphonMaterial)
        {
            await _siphonMaterial.AddAsync(siphonMaterial);
        }

        public async Task Remove(SiphonMaterial siphonMaterial)
        {
            _siphonMaterial.Remove(siphonMaterial);
        }
    }
}
