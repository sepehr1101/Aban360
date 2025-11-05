using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;

namespace Aban360.OldCalcPool.Application.Features.Processing.Helpers
{
    internal static class NerkhInputFactory
    {
        const int constructionBranchType = 4;
        const int azadUsageId = 39;
        internal static NerkhByConsumptionInputDto CreateNerkhInput(MeterInfoInputDto input, CustomerInfoOutputDto customerInfo, MeterInfoOutputDto meterInfo, ConsumptionInfo consumptionInfo)
        {
            return new NerkhByConsumptionInputDto(
                customerInfo.ZoneId,
                customerInfo.BranchType == constructionBranchType ? azadUsageId : customerInfo.UsageId,
                meterInfo.PreviousDateJalali,
                input.CurrentDateJalali,
                consumptionInfo.MonthlyAverageConsumption);
        }
        internal static NerkhByConsumptionInputDto CreateNerkhInput(MeterInfoByPreviousDataInputDto input, CustomerInfoOutputDto customerInfo, ConsumptionInfo consumptionInfo)
        {
            return new NerkhByConsumptionInputDto(
                customerInfo.ZoneId,
                customerInfo.BranchType == constructionBranchType ? azadUsageId : customerInfo.UsageId,
                input.PreviousDateJalali,
                input.CurrentDateJalali,
                consumptionInfo.MonthlyAverageConsumption);
        }
        internal static NerkhByConsumptionInputDto CreateNerkhInput(BaseOldTariffEngineImaginaryInputDto input, CustomerInfoOutputDto customerInfo, ConsumptionInfo consumptionInfo)
        {
            return new NerkhByConsumptionInputDto(
                customerInfo.ZoneId,
                customerInfo.BranchType == constructionBranchType ? azadUsageId : customerInfo.UsageId,
                input.MeterPreviousData.PreviousDateJalali,
                input.MeterPreviousData.CurrentDateJalali,
                consumptionInfo.MonthlyAverageConsumption);
        }
    }
}
