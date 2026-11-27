using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Commands.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.LocationPool.Persistence.Features.MainHierarchy.Commands.Implementations
{
    internal sealed class ProvinceCommandService : IProvinceCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Province> _province;
        public ProvinceCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _province = _uow.Set<Province>();
            _province.NotNull(nameof(_province));
        }

        public async Task Add(Province province)
        {
            await _province.AddAsync(province);
        }

        public async Task Remove(Province province)
        {
            _province.Remove(province);
        }
    }
}
