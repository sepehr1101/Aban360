using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Implementations
{
    internal sealed class SmsStateTemplateGetHandler : ISmsStateTemplateGetHandler
    {
        private readonly ISmsStateTemplateQueryService _meterSmsStateService;
        public SmsStateTemplateGetHandler(ISmsStateTemplateQueryService meterSmsStateService)
        {
            _meterSmsStateService = meterSmsStateService;
            _meterSmsStateService.NotNull(nameof(meterSmsStateService));
        }

        public async Task<SmsStateTemplateGetDto> Handle(short id, CancellationToken cancellationToken)
        {
            SmsStateTemplateGetDto meterSmsState = await _meterSmsStateService.Get(id);
            return meterSmsState;
        }
    }
}
