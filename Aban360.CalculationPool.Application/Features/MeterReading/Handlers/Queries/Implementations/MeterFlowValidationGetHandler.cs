using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;

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

        public async Task Handle(string fileName, CancellationToken cancellation)
        {
            string? insertDateTime = await _meterFlowService.Get(fileName);
            if (insertDateTime is not null)
            {
                throw new ReadingException(ExceptionLiterals.invalidDuplicateFileName(insertDateTime));
            }
        }
    }
}
