using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Constants;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Queries;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Db70.Queries.Contracts;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Aban360.OldCalcPool.Persistence.Features.WaterReturn.Queries.Contracts;
using Aban360.OldCalcPools.Domain.Features.WaterReturn.Dto.Commands;

namespace Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Queries.Implementations
{
    internal sealed class ReturnBillByConfirmedNumberGetHandler : IReturnBillByConfirmedNumberGetHandler
    {
        private readonly IAutoBackQueryService _autoBackQueryService;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly IBillReturnCauseQueryService _billReturnCauseQueryService;
        private readonly IHBedBesQueryService _hBedBesQueryService;
        private static string _title = "برگشتی";

        public ReturnBillByConfirmedNumberGetHandler(
            IAutoBackQueryService autoBackQueryService,
            ICommonMemberQueryService commonMemberQueryService,
            IBillReturnCauseQueryService billReturnCauseQueryService,
            IHBedBesQueryService hBedBesQueryService)
        {
            _autoBackQueryService = autoBackQueryService;
            _autoBackQueryService.NotNull(nameof(autoBackQueryService));

            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));

            _billReturnCauseQueryService = billReturnCauseQueryService;
            _billReturnCauseQueryService.NotNull(nameof(billReturnCauseQueryService));

            _hBedBesQueryService = hBedBesQueryService;
            _hBedBesQueryService.NotNull(nameof(hBedBesQueryService));
        }

        public async Task<FlatReportOutput<ReturnBillHeaderOutputDto, ReturnBillOutputDto>> Handle(int confirmedNumber, CancellationToken cancellationToken)
        {
            IEnumerable<AutoBackGetByBargeDto> autoBackInfo = await _autoBackQueryService.GetByConfirmNumber(confirmedNumber);
            ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = new((int)(autoBackInfo?.FirstOrDefault()?.Town ?? 0), (int)(autoBackInfo?.FirstOrDefault()?.Radif ?? 0));
            MemberInfoGetDto customerInfo = await _commonMemberQueryService.Get(zoneIdAndCustomerNumber);
            bool hasReturned = await _hBedBesQueryService.Get(zoneIdAndCustomerNumber);
            return await GetResult(autoBackInfo, customerInfo, hasReturned);
        }
        private async Task<FlatReportOutput<ReturnBillHeaderOutputDto, ReturnBillOutputDto>> GetResult(IEnumerable<AutoBackGetByBargeDto> autoBackInfos, MemberInfoGetDto customerInfo, bool hasReturned)
        {
            AutoBackGetByBargeDto bedBesValue = autoBackInfos.ElementAt(0);
            AutoBackGetByBargeDto newCalculationValue = autoBackInfos.ElementAt(1);
            AutoBackGetByBargeDto differentValue = autoBackInfos.ElementAt(2);

            ReturnBillDataOutputDto previousValues = new ReturnBillDataOutputDto()
            {
                ZoneId = bedBesValue.Town,
                CustomerNumber = bedBesValue.Radif,
                ReadingNumber = bedBesValue.Eshtrak,
                PreviousNumber = bedBesValue.PriNo,
                CurrentNumber = bedBesValue.TodayNo,
                PreviousDateJalali = bedBesValue.PriDate,
                CurrentDateJalali = bedBesValue.TodayDate,
                MinutesNumber = (int)bedBesValue.JalaseNo,
                Item4 = bedBesValue.AbonFas,
                Item2 = bedBesValue.FasBaha,
                Item1 = bedBesValue.AbBaha,
                Item12 = bedBesValue.Ztadil,
                Consumption = bedBesValue.Masraf,
                Item5 = bedBesValue.Shahrdari,
                Duration = bedBesValue.Modat,
                RegisterDateJalali = bedBesValue.DateBed,
                SumItems = bedBesValue.Baha,
                Item3 = bedBesValue.AbonAb,
                PayableAmount = bedBesValue.Pard,
                CounterStateCode = bedBesValue.CodVas,
                BillsCount = "0",
                Removable = bedBesValue.Del,
                UsageId = bedBesValue.CodEnshab,
                MeterDiameterId = bedBesValue.Enshab,
                Cause = bedBesValue.Elat,
                BodySerial = bedBesValue.Serial,
                Item11 = bedBesValue.ZaribFasl,
                OtherUnit = bedBesValue.TedadVahd,
                DomesticUnit = bedBesValue.TedadMas,
                CommertialUnit = bedBesValue.TedadTej,
                BranchType = bedBesValue.NoeVa,
                Item8 = bedBesValue.Jarime,
                ConsumptionAverage = bedBesValue.Rate,
                Operator = bedBesValue.Operator,
                LastMeterChangeDateJalali = bedBesValue.TavizDate,
                Item9 = bedBesValue.Zabresani,
                Item10 = bedBesValue.ZaribD,
                Difference = bedBesValue.Tafavot,
                WastedWater = bedBesValue.AbHadar,
                WastedConsumption = bedBesValue.MasHadar,
                BillCount = bedBesValue.TedGhabs,
                Item18 = bedBesValue.Bodjeh,
                UsageConsumption = bedBesValue.Group1,
                HasSewage = bedBesValue.Faz,
                IsSpecial = bedBesValue.EdarehK,
                Lavazem = 0
            };
            ReturnBillDataOutputDto currentValues = new ReturnBillDataOutputDto()
            {
                ZoneId = newCalculationValue.Town,
                CustomerNumber = newCalculationValue.Radif,
                ReadingNumber = newCalculationValue.Eshtrak,
                PreviousNumber = newCalculationValue.PriNo,
                CurrentNumber = newCalculationValue.TodayNo,
                PreviousDateJalali = newCalculationValue.PriDate,
                CurrentDateJalali = newCalculationValue.TodayDate,
                MinutesNumber = (int)newCalculationValue.JalaseNo,
                Item4 = newCalculationValue.AbonFas,
                Item2 = newCalculationValue.FasBaha,
                Item1 = newCalculationValue.AbBaha,
                Item12 = newCalculationValue.Ztadil,
                Consumption = newCalculationValue.Masraf,
                Item5 = newCalculationValue.Shahrdari,
                Duration = newCalculationValue.Modat,
                RegisterDateJalali = newCalculationValue.DateBed,
                SumItems = newCalculationValue.Baha,
                Item3 = newCalculationValue.AbonAb,
                PayableAmount = newCalculationValue.Pard,
                CounterStateCode = newCalculationValue.CodVas,
                BillsCount = "0",
                Removable = newCalculationValue.Del,
                UsageId = newCalculationValue.CodEnshab,
                MeterDiameterId = newCalculationValue.Enshab,
                Cause = newCalculationValue.Elat,
                BodySerial = newCalculationValue.Serial,
                Item11 = newCalculationValue.ZaribFasl,
                OtherUnit = newCalculationValue.TedadVahd,
                DomesticUnit = newCalculationValue.TedadMas,
                CommertialUnit = newCalculationValue.TedadTej,
                BranchType = newCalculationValue.NoeVa,
                Item8 = newCalculationValue.Jarime,
                ConsumptionAverage = newCalculationValue.Rate,
                Operator = newCalculationValue.Operator,
                LastMeterChangeDateJalali = newCalculationValue.TavizDate,
                Item9 = newCalculationValue.Zabresani,
                Item10 = newCalculationValue.ZaribD,
                Difference = newCalculationValue.Tafavot,
                WastedWater = newCalculationValue.AbHadar,
                WastedConsumption = newCalculationValue.MasHadar,
                BillCount = newCalculationValue.TedGhabs,
                Item18 = newCalculationValue.Bodjeh,
                UsageConsumption = newCalculationValue.Group1,
                HasSewage = newCalculationValue.Faz,
                IsSpecial = newCalculationValue.EdarehK,
                Lavazem = 0
            };
            ReturnBillDataOutputDto returnValues = new ReturnBillDataOutputDto()
            {
                ZoneId = differentValue.Town,
                CustomerNumber = differentValue.Radif,
                ReadingNumber = differentValue.Eshtrak,
                PreviousNumber = differentValue.PriNo,
                CurrentNumber = differentValue.TodayNo,
                PreviousDateJalali = differentValue.PriDate,
                CurrentDateJalali = differentValue.TodayDate,
                MinutesNumber = (int)differentValue.JalaseNo,
                Item4 = differentValue.AbonFas,
                Item2 = differentValue.FasBaha,
                Item1 = differentValue.AbBaha,
                Item12 = differentValue.Ztadil,
                Consumption = differentValue.Masraf,
                Item5 = differentValue.Shahrdari,
                Duration = differentValue.Modat,
                RegisterDateJalali = differentValue.DateBed,
                SumItems = differentValue.Baha,
                Item3 = differentValue.AbonAb,
                PayableAmount = differentValue.Pard,
                CounterStateCode = differentValue.CodVas,
                BillsCount = "0",
                Removable = differentValue.Del,
                UsageId = differentValue.CodEnshab,
                MeterDiameterId = differentValue.Enshab,
                Cause = differentValue.Elat,
                BodySerial = differentValue.Serial,
                Item11 = differentValue.ZaribFasl,
                OtherUnit = differentValue.TedadVahd,
                DomesticUnit = differentValue.TedadMas,
                CommertialUnit = differentValue.TedadTej,
                BranchType = differentValue.NoeVa,
                Item8 = differentValue.Jarime,
                ConsumptionAverage = differentValue.Rate,
                Operator = differentValue.Operator,
                LastMeterChangeDateJalali = differentValue.TavizDate,
                Item9 = differentValue.Zabresani,
                Item10 = differentValue.ZaribD,
                Difference = differentValue.Tafavot,
                WastedWater = differentValue.AbHadar,
                WastedConsumption = differentValue.MasHadar,
                BillCount = differentValue.TedGhabs,
                Item18 = differentValue.Bodjeh,
                UsageConsumption = differentValue.Group1,
                HasSewage = differentValue.Faz,
                IsSpecial = differentValue.EdarehK,
                Lavazem = 0
            };

            string description = await GetDescription(customerInfo, bedBesValue);
            ReturnBillHeaderOutputDto header = new(description, customerInfo.ZoneTitle, previousValues.MinutesNumber, previousValues.MinutesNumber.ToString(), hasReturned);
            ReturnBillOutputDto data = new(previousValues, currentValues, returnValues);
            FlatReportOutput<ReturnBillHeaderOutputDto, ReturnBillOutputDto> result = new(_title, header, data);

            return result;
        }
        private async Task<string> GetDescription(MemberInfoGetDto customerInfo, AutoBackGetByBargeDto bedBesValue)
        {
            BillReturnCauseGetDto returnCause = await _billReturnCauseQueryService.Get(new SearchShortInputDto { Id = (short)bedBesValue.Elat });
            return string.Format(Literals.WaterReturnDescription, customerInfo.BillId, customerInfo.ReadingNumber, customerInfo.FullName, customerInfo.UsageTitle, returnCause.Title, bedBesValue.TedGhabs, bedBesValue.PriDate, bedBesValue.TodayDate, bedBesValue.Modat);
        }
    }
}
