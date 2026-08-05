using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Timing;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using FluentValidation;
using static Aban360.Common.Timing.CalculationDistanceDate;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Implementations
{
    internal sealed class ConsumptionManagementByBillIdHandler : IConsumptionManagementByBillIdHandler
    {
        private readonly ICommonMemberQueryService _memberQueryService;
        private readonly ICommonZoneService _zoneService;
        private readonly IConsumptionManagementByBillIdQueryService _consumptionManagementByBillIdQueryService;
        private readonly IValidator<ConsumptionManagementByBillIdInputDto> _validator;
        public ConsumptionManagementByBillIdHandler(
            ICommonMemberQueryService memberQueryService,
            ICommonZoneService zoneService,
            IConsumptionManagementByBillIdQueryService consumptionManagementByBillIdQueryService,
            IValidator<ConsumptionManagementByBillIdInputDto> validator)
        {
            _memberQueryService = memberQueryService;
            _memberQueryService.NotNull(nameof(memberQueryService));

            _zoneService = zoneService;
            _zoneService.NotNull(nameof(zoneService));

            _consumptionManagementByBillIdQueryService = consumptionManagementByBillIdQueryService;
            _consumptionManagementByBillIdQueryService.NotNull(nameof(consumptionManagementByBillIdQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<FlatReportOutput<MemberInfoGetDto, CosnumptionManagementByBillIdDataOutputDto>> Handle(ConsumptionManagementByBillIdInputDto input, IAppUser appUser, CancellationToken cancellationToken)
        {
            await InputValidate(input, cancellationToken);
            ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await _memberQueryService.Get(input.BillId);
            MemberInfoGetDto memberInfo = await _memberQueryService.Get(zoneIdAndCustomerNumber);
            await _zoneService.IsUserInZone(appUser, memberInfo.ZoneId);
            ConsumptionManagementByBillIdDto inputDto = new(memberInfo.ZoneId, memberInfo.CustomerNumber, input.FromDateJalali, input.ToDateJalali);

            IEnumerable<ConsumptionManagementByBillIdGetDto> billDetails = await _consumptionManagementByBillIdQueryService.Get(inputDto);
            CosnumptionManagementByBillIdDataOutputDto data = GetData(input, billDetails);
            return new FlatReportOutput<MemberInfoGetDto, CosnumptionManagementByBillIdDataOutputDto>(ReportLiterals.ConsumptionManagerDetail, memberInfo, data);
        }
        private CosnumptionManagementByBillIdDataOutputDto GetData(ConsumptionManagementByBillIdInputDto input, IEnumerable<ConsumptionManagementByBillIdGetDto> billDetails)
        {
            float consumption = 0;
            int duration = 0;
            int billDetailsCount = billDetails?.Count() ?? 0;

            for (int i = 0; i < billDetailsCount; i++)
            {
                ConsumptionManagementByBillIdGetDto item = billDetails.ElementAt(i);
                if (i == 0)//اول 
                {
                    float dailyConsumption = (float)item.Consumption / item.Duration;
                    CalcDistanceResultDto calcDistanceResult = CalculationDistanceDate.CalcDistance(input.FromDateJalali, item.CurrentDateJalali, true);
                    float itemConsumption = dailyConsumption * (float)calcDistanceResult.Distance;

                    duration += calcDistanceResult.Distance;
                    consumption += itemConsumption;
                }
                else if (i == billDetailsCount - 1)//آخر
                {
                    float dailyConsumption = (float)item.Consumption / item.Duration;
                    CalcDistanceResultDto calcDistanceResult = CalculationDistanceDate.CalcDistance(item.PreviousDateJalali, input.ToDateJalali, true);
                    float itemConsumption = dailyConsumption * (float)calcDistanceResult.Distance;

                    duration += calcDistanceResult.Distance;
                    consumption += itemConsumption;
                }
                else//بین
                {
                    consumption += item.Consumption;
                    duration += item.Duration;
                }
            }
            return new CosnumptionManagementByBillIdDataOutputDto(input.FromDateJalali, input.ToDateJalali, (int)consumption, duration);
        }
        public async Task InputValidate(ConsumptionManagementByBillIdInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }

    }
}
