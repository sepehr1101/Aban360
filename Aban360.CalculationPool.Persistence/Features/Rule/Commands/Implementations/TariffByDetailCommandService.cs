using Aban360.CalculationPool.Domain.Features.Rule.Entities;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.CalculationPool.Persistence.Features.Rule.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.CalculationPool.Persistence.Features.Rule.Commands.Implementations
{
    internal sealed class TariffByDetailCommandService : ITariffByDetailCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<TariffByDetail> _tariffByDetail;
        public TariffByDetailCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _tariffByDetail = _uow.Set<TariffByDetail>();
            _tariffByDetail.NotNull(nameof(_tariffByDetail));
        }

        public async Task Add(TariffByDetail tariffByDetail)
        {
            await _tariffByDetail.AddAsync(tariffByDetail);
        }

        public async Task Remove(TariffByDetail tariffByDetail)
        {
            _tariffByDetail.Remove(tariffByDetail);
        }
    }
}
