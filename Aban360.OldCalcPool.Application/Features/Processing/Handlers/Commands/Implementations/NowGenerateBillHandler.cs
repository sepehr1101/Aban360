using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Constants;
using Microsoft.Extensions.Configuration;
using Aban360.ClaimPool.Domain.Constants;
using DNTPersianUtils.Core;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.Common.Db.Services.Dtos;
using Aban360.Common.Db.Constants.Literals;
using Microsoft.AspNetCore.Http;
using Aban360.ReportPool.Domain.Base;
using FluentValidation;
using Aban360.Common.Exceptions;

namespace Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Implementations
{
    internal sealed class NowGenerateBillHandler : AbstractBaseConnection, INowGenerateBillHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IFreeGenerateBillHandler _freeGenerateBillHandler;
        private readonly IBedBesQueryService _bedBesQueryService;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly ICommonZoneService _commonZoneService;
        private readonly IValidator<NowGenerateBillInputDto> _validator;
        private static string _xApiKey = ReportLiterals.XApiKey;
        private static string _nowGenerateBillXApiKey = ReportLiterals.NowGenerateBillXApiKey;
        private static float _invalidGTConsumptionPercent = 1.5f;
        private static float _invalidLTConsumptionPercent = 0.5f;
        private static int[] _invalidDeletionStateCodes = { (int)DeletionStateEnum.JamAvari, (int)DeletionStateEnum.JabejaShode, (int)DeletionStateEnum.HazfMovaghat };
        private static int _invalidGTAmount = 50_000_000;
        private static int _invalidLTConsumption = 1;
        private static int _invalidLTDistanceDay = 20;

        public NowGenerateBillHandler(
            IHttpContextAccessor contextAccessor,
            IFreeGenerateBillHandler freeGenerateBillHandler,
            IBedBesQueryService bedBesQueryService,
            ICommonMemberQueryService commonMemberQueryService,
            ICommonZoneService commonZoneService,
            IValidator<NowGenerateBillInputDto> validator,
            IConfiguration configuration)
                : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _freeGenerateBillHandler = freeGenerateBillHandler;
            _freeGenerateBillHandler.NotNull(nameof(freeGenerateBillHandler));

            _bedBesQueryService = bedBesQueryService;
            _bedBesQueryService.NotNull(nameof(commonMemberQueryService));

            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));

            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(commonZoneService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<NowGenerateBillOutputDto> Handle(NowGenerateBillInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await InputValidate(inputDto, cancellationToken);
            var (zoneIdAndCustomerNumber, customerInfo, customerValidateResult) = await CustomerValidate(inputDto);
            if (customerValidateResult is not null)
            {
                return customerValidateResult;
            }
            NowGenerateBillOutputDto? previousInfoValidate = PreviousInfoValidate(inputDto, customerInfo);
            if (previousInfoValidate is not null)
            {
                return previousInfoValidate;
            }

            NewBillOutputDto freeBillResultTemp = await CalcFreeBill(inputDto, customerInfo, false, appUser, cancellationToken);
            NowGenerateBillOutputDto? result = BillValidate(freeBillResultTemp, customerInfo);

            if (result is null)
            {
                NewBillOutputDto freeBillResult = await CalcFreeBill(inputDto, customerInfo, true, appUser, cancellationToken);
                return new NowGenerateBillOutputDto("موفق", (int)NowGenerateBillErrorCodeEnum.Success);
            }

            return result;
        }
        private async Task InputValidate(NowGenerateBillInputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
        private async Task<(ZoneIdAndCustomerNumber?, CustomerLastInfoGetDto?, NowGenerateBillOutputDto?)> CustomerValidate(NowGenerateBillInputDto inputDto)
        {
            string returnMessage = string.Empty;
            IHeaderDictionary requestHeader = _contextAccessor.HttpContext.Request.Headers;
            if (!requestHeader.Where(r => r.Key == _xApiKey && r.Value == _nowGenerateBillXApiKey).Any())
            {
                returnMessage = "احراز هویت با خطا روبرو شد.";
                NowGenerateBillOutputDto result = new(returnMessage, (int)NowGenerateBillErrorCodeEnum.BadRequest);
                return (null, null, result);
            }

            ZoneIdAndCustomerNumber? zoneIdAndCustomerNumber = await _commonMemberQueryService.Get(inputDto.BillId);
            if (zoneIdAndCustomerNumber == null)
            {
                returnMessage = "شناسه قبض معتبر نیست.";
                NowGenerateBillOutputDto result = new(returnMessage, (int)NowGenerateBillErrorCodeEnum.BadRequest);
                return (null, null, result);
            }

            CustomerLastInfoGetDto? customerInfo = await _commonMemberQueryService.GetMembersBedBesTavizFlatInfo(zoneIdAndCustomerNumber.ZoneId, zoneIdAndCustomerNumber.CustomerNumber, false);
            if (customerInfo is null)
            {
                returnMessage = "اطلاعات قبض قبلی به درستی یافت نشد.";
                NowGenerateBillOutputDto result = new(returnMessage, (int)NowGenerateBillErrorCodeEnum.BadRequest);
                return (null, null, result);
            }

            return (zoneIdAndCustomerNumber, customerInfo, null);
        }
        private NowGenerateBillOutputDto? PreviousInfoValidate(NowGenerateBillInputDto inputDto, CustomerLastInfoGetDto customerInfo)
        {
            string returnMessage = string.Empty;
            if (_invalidDeletionStateCodes.Contains(customerInfo.DeletionStateId))
            {
                returnMessage = "انشعاب جمع‌آوری شده است.";
                return new NowGenerateBillOutputDto(returnMessage, (int)NowGenerateBillErrorCodeEnum.InvalidDeletionState);
            }
            if (customerInfo.LastMeterNumber > inputDto.CurrentMeterNumber)
            {
                returnMessage = "رقم فعلی کمتر از رقم قبلی است.";
                return new NowGenerateBillOutputDto(returnMessage, (int)NowGenerateBillErrorCodeEnum.PreNumberHigherThanTodayNumber);
            }
            if (customerInfo.LastMeterNumber == inputDto.CurrentMeterNumber)
            {
                returnMessage = "رقم فعلی برابر با رقم قبلی است.";
                return new NowGenerateBillOutputDto(returnMessage, (int)NowGenerateBillErrorCodeEnum.PreNumberEqualTodayNumber);
            }
            if (customerInfo.LastMeterDateJalali.CompareTo(DateTime.Now.ToShortPersianDateString().ToString()) <= _invalidLTDistanceDay)
            {
                returnMessage = "برای مشترک در کمتر از 20 روز قبض دیگری صادر شده.";
                return new NowGenerateBillOutputDto(returnMessage, (int)NowGenerateBillErrorCodeEnum.Low);
            }

            return null;
        }
        private NowGenerateBillOutputDto? BillValidate(NewBillOutputDto freeBillResult, CustomerLastInfoGetDto customerInfo)
        {
            string returnMessage = string.Empty;

            if (freeBillResult.AbBahaCalculationDetail.MonthlyConsumption >= customerInfo.LastMonthlyConsumption * _invalidGTConsumptionPercent)
            {
                returnMessage = "میانگین مصرف دوره جاری بیش از 50درصد دوره قبلی است.";
                return new NowGenerateBillOutputDto(returnMessage, (int)NowGenerateBillErrorCodeEnum.High);
            }
            if (freeBillResult.AbBahaCalculationDetail.MonthlyConsumption <= customerInfo.LastMonthlyConsumption * _invalidLTConsumptionPercent)
            {
                returnMessage = "میانگین مصرف دوره جاری کمتر از 50درصد دوره قبلی است.";
                return new NowGenerateBillOutputDto(returnMessage, (int)NowGenerateBillErrorCodeEnum.Low);
            }
            if (freeBillResult.AbBahaCalculationDetail.SumItems >= _invalidGTAmount)
            {
                returnMessage = "مبلغ دوره جاری بیش از 5میلیون تومان است.";
                return new NowGenerateBillOutputDto(returnMessage, (int)NowGenerateBillErrorCodeEnum.High);
            }
            if (freeBillResult.AbBahaCalculationDetail.Consumption <= _invalidLTConsumption)
            {
                returnMessage = "مصرف دوره جاری نمی‌تواند کمتر از 2مترمکعب باشد.";
                return new NowGenerateBillOutputDto(returnMessage, (int)NowGenerateBillErrorCodeEnum.Low);
            }

            return null;
        }
        private async Task<NewBillOutputDto> CalcFreeBill(NowGenerateBillInputDto inputDto, CustomerLastInfoGetDto customerInfo, bool isConfirm, IAppUser appUser, CancellationToken cancellationToken)
        {
            string detailLogText = OpLogLiterals.NowGenerateBillOpLog;
            FreeGenerateBillInputDto freeGenerateBillDto = new()
            {
                BillId = inputDto.BillId,
                CurrentMeterNumber = inputDto.CurrentMeterNumber,
                PreviousMeterNumber = customerInfo.LastMeterNumber ?? 0,
                CurrentDateJalali = DateTime.Now.ToShortPersianDateString(),
                PreviousDateJalali = customerInfo.LastMeterDateJalali,
                ConsumptionAverage = null,
                CounterStateCode = (int)CounterStateCodeEnum.Common,
                OperatorCode = FreeGenerateBillOperatorCodeEnum.Customer,
                IsConfirm = isConfirm,
            };
            NewBillOutputDto freeResult = await _freeGenerateBillHandler.Handle(freeGenerateBillDto, detailLogText, appUser, cancellationToken);

            return freeResult;
        }
    }
}
