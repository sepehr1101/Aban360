using Aban360.ClaimPool.Domain.Features.WasteWater.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.WasteWater.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.WasteWater.Commands.Implementation
{
    public class SiphonDiameterCommandService : ISiphonDiameterCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<SiphonDiameter> _siphonDiameter;
        public SiphonDiameterCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _siphonDiameter = _uow.Set<SiphonDiameter>();
            _siphonDiameter.NotNull(nameof(_siphonDiameter));
        }

        public async Task Add(SiphonDiameter siphonDiameter)
        {
            await _siphonDiameter.AddAsync(siphonDiameter);
        }

        public async Task Remove(SiphonDiameter siphonDiameter)
        {
            _siphonDiameter.Remove(siphonDiameter);
        }
    }
}
