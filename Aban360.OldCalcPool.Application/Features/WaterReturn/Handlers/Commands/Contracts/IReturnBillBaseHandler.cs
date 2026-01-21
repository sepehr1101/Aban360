using Aban360.Common.BaseEntities;
using Aban360.OldCalcPool.Domain.Constants;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;
using Aban360.OldCalcPools.Domain.Features.WaterReturn.Dto.Commands;

namespace Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Commands.Contracts
{
    public interface IReturnBillBaseHandler
    {
        Task<FlatReportOutput<ReturnBillHeaderOutputDto, ReturnBillOutputDto>> GetReturn(AutoBackCreateDto bedBes, AutoBackCreateDto newCalculation, AutoBackCreateDto different, CustomerInfoOutputDto customerInfo, int billCount, bool isConfirm);
        AutoBackCreateDto GetFullNewCalculation(BedBesCreateDto bedBes, int returnCauseId, int bedbesCount, int jalaseNumber);
        AutoBackCreateDto GetNewCalculation(AbBahaCalculationDetails tariffInfo, BedBesCreateDto bedBes, int returnCauseId, int bedbesCount, float? consumptionHadar, long? abHadarAmount, int jalaseNumber);
        AutoBackCreateDto GetDifferent(BedBesCreateDto bedBes, AutoBackCreateDto repair, int jalaseNumber);
        Task<IEnumerable<BedBesCreateDto>> GetBedBesList(CustomerInfoOutputDto customerInfo, string fromDateJalali, string toDateJalali);
        BedBesCreateDto GetBedbes(IEnumerable<BedBesCreateDto> input, CustomerInfoOutputDto customerInfo);
        AutoBackCreateDto GetBedBes(BedBesCreateDto bedBes, int bedBesCount, int jalaseNumber, int returnCauseId);
        Task<int> GetJalaliNumber(int? minutesNumber, int zoneId, int customerNumber);
        Task FullValidation(ReturnBillFullInputDto input, CancellationToken cancellationToken);
        Task PartialValidation(ReturnBillPartialInputDto input, CancellationToken cancellationToken);
        void ValidationAmount(decimal repairSumItems, decimal previousSumItems);
        Task<CustomerInfoOutputDto> Validation(string billId, string fromDateJalali, string toDateJalali);
        Task<float> GetConsumptionAverage(string fromDateJalali, ReturnedBillCalculationTypeEnum calculationType, float? userInput, CustomerInfoOutputDto customerInfo);
        bool IsDomestic(int customerNumber);
    }
}
