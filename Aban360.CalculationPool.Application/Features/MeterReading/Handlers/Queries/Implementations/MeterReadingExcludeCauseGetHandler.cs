using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.ReportPool.Domain.Base;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Implementations
{
    internal sealed class MeterReadingExcludeCauseGetHandler : IMeterReadingExcludeCauseGetHandler
    {
        public IEnumerable<MeterReadingExcludeCauseGetDto> Handle(bool isSelectable, CancellationToken cancellationToken)
        {
            IEnumerable<MeterReadingExcludeCauseGetDto> userCause = new List<MeterReadingExcludeCauseGetDto>()
            {
                new MeterReadingExcludeCauseGetDto((int)ExcludedCauseEnum.PriGTCurrent,ReportLiterals.PriGTCurrent,true),
                new MeterReadingExcludeCauseGetDto((int)ExcludedCauseEnum.NeedEvaluate,ReportLiterals.NeedEvaluate,true),
                new MeterReadingExcludeCauseGetDto((int)ExcludedCauseEnum.Error,ReportLiterals.Error,true),
            };
            IEnumerable<MeterReadingExcludeCauseGetDto> backendCause = new List<MeterReadingExcludeCauseGetDto>()
            {
                new MeterReadingExcludeCauseGetDto((int)ExcludedCauseEnum.DuplicateBill,ReportLiterals.DuplicateBill,false),
                new MeterReadingExcludeCauseGetDto((int)ExcludedCauseEnum.Deleted_Close,ReportLiterals.Deleted_Close,false),
                new MeterReadingExcludeCauseGetDto((int)ExcludedCauseEnum.DuplicateBill5,ReportLiterals.DuplicateBill5,false),
                new MeterReadingExcludeCauseGetDto((int)ExcludedCauseEnum.Deleted_Block,ReportLiterals.Deleted_Block,false),
            };

            return isSelectable ? userCause : userCause.Union(backendCause);
        }
    }
}
