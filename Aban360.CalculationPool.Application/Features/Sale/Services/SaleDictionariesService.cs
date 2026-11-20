using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.CalculationPool.Persistence.Features.Sale.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;

namespace Aban360.CalculationPool.Application.Features.Sale.Services;
public interface ISaleDictionariesService
{
    Task<IEnumerable<BlockGetDto>> BlockGetAllAsync(CancellationToken cancellationToken);
    Task<IEnumerable<NumericDictionary>> SiphonDiameterGetAllAsync(CancellationToken cancellationToken);
    Task<IEnumerable<NumericDictionary>> WaterDiameterGetAllAsync(CancellationToken cancellationToken);
}

internal sealed class SaleDictionariesService : ISaleDictionariesService
{
    private readonly IBlockQueryService _queryService;
    private readonly ISiphonDiameterQueryService _siphonDiameterGetAllqueryService;
    private readonly IWaterDiameterQueryService _waterDiameterGetAllqueryService;
    public SaleDictionariesService(IBlockQueryService queryService, ISiphonDiameterQueryService siphonDiameterGetAllqueryService, IWaterDiameterQueryService waterDiameterGetAllqueryService)
    {
        _queryService = queryService;
        _queryService.NotNull(nameof(queryService));
        _siphonDiameterGetAllqueryService = siphonDiameterGetAllqueryService;
        _siphonDiameterGetAllqueryService.NotNull(nameof(siphonDiameterGetAllqueryService));
        _waterDiameterGetAllqueryService = waterDiameterGetAllqueryService;
        _waterDiameterGetAllqueryService.NotNull(nameof(waterDiameterGetAllqueryService));
    }

    public async Task<IEnumerable<BlockGetDto>> BlockGetAllAsync(CancellationToken cancellationToken)
    {
        IEnumerable<BlockGetDto> result = await _queryService.Get();
        return result;
    }

    public async Task<IEnumerable<NumericDictionary>> SiphonDiameterGetAllAsync(CancellationToken cancellationToken)
    {
        IEnumerable<NumericDictionary> result = await _siphonDiameterGetAllqueryService.Get();
        return result;
    }

    public async Task<IEnumerable<NumericDictionary>> WaterDiameterGetAllAsync(CancellationToken cancellationToken)
    {
        IEnumerable<NumericDictionary> result = await _waterDiameterGetAllqueryService.Get();
        return result;
    }
}
