using Aban360.CalculationPool.Domain.Features.Rule.Entities;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.CalculationPool.Persistence.Features.Rule.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.CalculationPool.Persistence.Features.Rule.Commands.Implementations
{
    public class TariffConstantCommandService : ITariffConstantCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<TariffConstant> _tariffConstant;
        public TariffConstantCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _tariffConstant = _uow.Set<TariffConstant>();
            _tariffConstant.NotNull(nameof(_tariffConstant));
        }

        public async Task Add(TariffConstant tariffConstant)
        {
            await _tariffConstant.AddAsync(tariffConstant);
        }

        public async Task Remove(TariffConstant tariffConstant)
        {
            _tariffConstant.Remove(tariffConstant);
        }
    }
}
