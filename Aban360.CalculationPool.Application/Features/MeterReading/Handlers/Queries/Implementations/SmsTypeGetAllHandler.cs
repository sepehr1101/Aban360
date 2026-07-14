using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Implementations
{
    internal sealed class SmsTypeGetAllHandler : ISmsTypeGetAllHandler
    {
        private readonly ISmsTypeService _smsTypeService;
        public SmsTypeGetAllHandler(ISmsTypeService smsTypeService)
        {
            _smsTypeService = smsTypeService;
            _smsTypeService.NotNull(nameof(smsTypeService));
        }

        public async Task<IEnumerable<NumericDictionary>> Handle(CancellationToken cancellationToken)
        {
            IEnumerable<NumericDictionary> SmsType = await _smsTypeService.Get();
            return SmsType;
        }
    }
}
