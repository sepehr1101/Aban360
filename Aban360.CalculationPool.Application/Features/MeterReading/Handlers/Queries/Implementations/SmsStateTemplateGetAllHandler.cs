using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Implementations
{
    internal sealed class SmsStateTemplateGetAllHandler : ISmsStateTemplateGetAllHandler
    {
        private readonly ISmsStateTemplateQueryService _meterSmsStateService;
        public SmsStateTemplateGetAllHandler(ISmsStateTemplateQueryService meterSmsStateService)
        {
            _meterSmsStateService = meterSmsStateService;
            _meterSmsStateService.NotNull(nameof(meterSmsStateService));
        }

        public async Task<IEnumerable<SmsStateTemplateGetDto>> Handle(CancellationToken cancellationToken)
        {
            IEnumerable<SmsStateTemplateGetDto> meterSmsState = await _meterSmsStateService.Get(false);
            return meterSmsState;
        }
    }
}
