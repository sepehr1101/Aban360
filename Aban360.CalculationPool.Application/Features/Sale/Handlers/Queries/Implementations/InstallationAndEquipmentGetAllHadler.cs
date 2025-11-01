using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.CalculationPool.Persistence.Features.Sale.Queries.Contracts;
using Aban360.Common.Extensions;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Implementations
{
    internal sealed class InstallationAndEquipmentGetAllHadler : IInstallationAndEquipmentGetAllHadler
    {
        private readonly IInstallationAndEquipmentQueryService _queryService;
        public InstallationAndEquipmentGetAllHadler(IInstallationAndEquipmentQueryService queryService)
        {
            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task<IEnumerable<InstallationAndEquipmentOutputDto>> Handle(CancellationToken cancellationToken)
        {
            IEnumerable<InstallationAndEquipmentOutputDto> result = await _queryService.Get();
            return result;
        }
    }
}
