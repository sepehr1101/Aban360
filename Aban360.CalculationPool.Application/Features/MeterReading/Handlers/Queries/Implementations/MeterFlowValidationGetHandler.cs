using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.Common.Timing;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Implementations
{
    internal sealed class MeterFlowValidationGetHandler : IMeterFlowValidationGetHandler
    {
        private readonly IMeterFlowQueryService _meterFlowService;
        public MeterFlowValidationGetHandler(IMeterFlowQueryService meterFlowService)
        {
            _meterFlowService = meterFlowService;
            _meterFlowService.NotNull(nameof(meterFlowService));
        }
        public async Task Handle(int id, CancellationToken cancellationToken)
        {
            MeterFlowValidationDto? meterFlowData = await _meterFlowService.GetMeterFlowValidation(id);
            if (meterFlowData is not null &&
                ((meterFlowData.RemovedDateTime is not null) ||
                 (meterFlowData.RemovedDateTime is null && meterFlowData.MeterFlowStepId == MeterFlowStepEnum.CalculationConfirmed)))
            {
                string insertDateJalali = ConvertDate.GregorianToJalali(meterFlowData.InsertDateTime);
                throw new ReadingException(ExceptionLiterals.InvalidDuplicateStepFlow(insertDateJalali));
            }
        }
        public async Task Handle(int id, MeterFlowStepEnum latestFlowId, CancellationToken cancellationToken)
        {
            MeterFlowValidationDto? meterFlowData = await _meterFlowService.GetMeterFlowValidation(id);
            if (meterFlowData is not null &&
                ((meterFlowData.RemovedDateTime is not null) ||
                 (meterFlowData.RemovedDateTime is null && meterFlowData.MeterFlowStepId == MeterFlowStepEnum.CalculationConfirmed)))
            {
                string insertDateJalali = ConvertDate.GregorianToJalali(meterFlowData.InsertDateTime);
                throw new ReadingException(ExceptionLiterals.InvalidDuplicateStepFlow(insertDateJalali));
            }
            if (meterFlowData.MeterFlowStepId != latestFlowId)
            {
                throw new ReadingException(ExceptionLiterals.NonAccessStepFlow);
            }
        }
    }
}
