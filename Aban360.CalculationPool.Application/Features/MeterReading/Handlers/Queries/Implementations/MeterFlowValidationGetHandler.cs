using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.Common.Timing;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Implementations
{
    internal sealed class MeterFlowValidationGetHandler : IMeterFlowValidationGetHandler
    {
        private readonly IMeterFlowService _meterFlowService;
        public MeterFlowValidationGetHandler(IMeterFlowService meterFlowService)
        {
            _meterFlowService = meterFlowService;
            _meterFlowService.NotNull(nameof(meterFlowService));
        }
        public async Task Handle(int id, CancellationToken cancellationToken)
        {
            string? insertDateTime=await _meterFlowService.GetInsertDateTime(id);
            if (insertDateTime is not null)
            {
                string insertDateJalali = ConvertDate.GregorianToJalali(insertDateTime);
                throw new ReadingException(ExceptionLiterals.InvalidDuplicateStepFlow(insertDateJalali));
            }
        }
    }
}
