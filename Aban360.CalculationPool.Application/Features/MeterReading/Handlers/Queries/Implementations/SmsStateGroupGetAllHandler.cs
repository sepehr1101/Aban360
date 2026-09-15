using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Implementations
{
    internal sealed class SmsStateGroupGetAllHandler : ISmsStateGroupGetAllHandler
    {
        private readonly ISmsStateGroupService _smsStateGroupService;
        public SmsStateGroupGetAllHandler(ISmsStateGroupService smsStateGroupService)
        {
            _smsStateGroupService = smsStateGroupService;
            _smsStateGroupService.NotNull(nameof(smsStateGroupService));
        }

        public async Task<IEnumerable<NumericDictionary>> Handle(CancellationToken cancellationToken)
        {
            IEnumerable<NumericDictionary> SmsStateGroup = await _smsStateGroupService.Get();
            return SmsStateGroup;
        }
    }
}
