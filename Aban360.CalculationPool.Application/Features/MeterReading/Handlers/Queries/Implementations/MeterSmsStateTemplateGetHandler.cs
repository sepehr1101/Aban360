using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Implementations
{
    internal sealed class MeterSmsStateTemplateGetHandler : IMeterSmsStateTemplateGetHandler
    {
        private readonly IMeterSmsStateTemplateQueryService _meterSmsStateService;
        public MeterSmsStateTemplateGetHandler(IMeterSmsStateTemplateQueryService meterSmsStateService)
        {
            _meterSmsStateService = meterSmsStateService;
            _meterSmsStateService.NotNull(nameof(meterSmsStateService));
        }

        public async Task<MeterSmsStateTemplateGetDto> Handle(short id, CancellationToken cancellationToken)
        {
            MeterSmsStateTemplateGetDto meterSmsState = await _meterSmsStateService.Get(id);
            return meterSmsState;
        }
    }
}
