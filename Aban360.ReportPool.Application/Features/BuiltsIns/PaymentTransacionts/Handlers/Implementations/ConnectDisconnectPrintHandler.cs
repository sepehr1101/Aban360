using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts;
using Aban360.ReportPool.Application.Features.Geo.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Domain.Features.Geo;
using Aban360.ReportPool.Infrastructure.Features.Geo;
using Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts;
using Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;
using DNTPersianUtils.Core;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Implementations
{
    internal sealed class ConnectDisconnectPrintHandler : IConnectDisconnectPrintHandler
    {
        private readonly ICustomerGeneralInfoQueryService _customerGeneralInfoQueryService;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly ILocationInfoGetHandler _locationInfoService;
        private readonly IPendingPaymentsQueryService _pendingPaymentsQueryService;
        public ConnectDisconnectPrintHandler(
            ICustomerGeneralInfoQueryService customerGeneralInfoQueryService,
            ICommonMemberQueryService commonMemberQueryService,
            ILocationInfoGetHandler locationInfoService,
            IPendingPaymentsQueryService pendingPaymentsQueryService)
        {
            _customerGeneralInfoQueryService = customerGeneralInfoQueryService;
            _customerGeneralInfoQueryService.NotNull(nameof(customerGeneralInfoQueryService));

            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));

            _locationInfoService = locationInfoService;
            _locationInfoService.NotNull(nameof(locationInfoService));

            _pendingPaymentsQueryService = pendingPaymentsQueryService;
            _pendingPaymentsQueryService.NotNull(nameof(pendingPaymentsQueryService));
        }

        public async Task<ConnectDisconnectPrintOutputDto> Handle(ConnectDisconnectPrintInputDto inputDto, CancellationToken cancellationToken)
        {
            ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await _commonMemberQueryService.Get(inputDto.BillId);
            ReportOutput<CustomerGeneralInfoHeaderDto, CustomerGeneralInfoDataDto> customerInfo = await _customerGeneralInfoQueryService.Get(zoneIdAndCustomerNumber);
            var (e, n) = await GetLocation(inputDto.BillId, cancellationToken);

            return new ConnectDisconnectPrintOutputDto()
            {
                CustomerNumber = customerInfo.ReportHeader?.CustomerNumber ?? 0,
                RegionTitle = customerInfo.ReportData?.FirstOrDefault()?.RegionTitle ?? string.Empty,
                ZoneTitle = customerInfo.ReportData?.FirstOrDefault()?.ZoneTitle ?? string.Empty,
                PostalCode = customerInfo.ReportData?.FirstOrDefault()?.PostalCode ?? string.Empty,
                N = e,
                E = n,
                Address = customerInfo.ReportData?.FirstOrDefault()?.Address ?? string.Empty,
                WaterDebtAmount = customerInfo.ReportData?.FirstOrDefault()?.WaterDebtAmount ?? 0,
                FirstName = customerInfo.ReportHeader?.FirstName ?? string.Empty,
                Surname = customerInfo.ReportHeader?.Surname ?? string.Empty,
                FatherName = customerInfo.ReportHeader?.FatherName ?? string.Empty,
                FullName = customerInfo.ReportHeader?.FullName ?? string.Empty,
                MobileNumber = customerInfo.ReportHeader?.MobileNumber ?? string.Empty,
                ReadingNumber = customerInfo.ReportHeader?.ReadingNumber ?? string.Empty,
                BillId = customerInfo.ReportHeader?.BillId ?? string.Empty,
                PaymentId = customerInfo.ReportData?.FirstOrDefault()?.PaymentId ?? string.Empty,
                UsageTitle = customerInfo.ReportHeader?.UsageTitle ?? string.Empty,
                MeterDiameterId = customerInfo.ReportHeader?.MeterDiameterId ?? string.Empty,
                MeterDiameterTitle = customerInfo.ReportHeader?.MeterDiameterTitle ?? string.Empty,
                BranchTypeTitle = customerInfo.ReportHeader?.BranchTypeTitle ?? string.Empty,
                CauseTitle = inputDto.CauseId,
            };
        }
        //public async Task<ReportOutput<ConnectDisconnectPrintstHeaderOutputDto, ConnectDisconnectPrintsDataOutputDto>> Handle(ConnectDisconnectPrintInputDto inputDto, CancellationToken cancellationToken)
        //{
        //    ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await _commonMemberQueryService.Get(inputDto.BillId);
        //    ReportOutput<CustomerGeneralInfoHeaderDto, CustomerGeneralInfoDataDto> customerInfo = await _customerGeneralInfoQueryService.Get(zoneIdAndCustomerNumber);
        //    var (e, n) = await GetLocation(inputDto.BillId, cancellationToken);

        //    ConnectDisconnectPrintsDataOutputDto singleData = new() 
        //    {
        //        CustomerNumber = customerInfo.ReportHeader?.CustomerNumber ?? 0,
        //        CustomerNumber2 = customerInfo.ReportHeader?.CustomerNumber ?? 0,
        //        ReadingNumber= customerInfo.ReportHeader?.ReadingNumber ?? string.Empty,
        //        DebtAmount= customerInfo.ReportData?.FirstOrDefault()?.WaterDebtAmount ?? 0,
        //        EmptyDueDateJalali=string.Empty,
        //        FirstName = customerInfo.ReportHeader?.FirstName ?? string.Empty,
        //        Surname = customerInfo.ReportHeader?.Surname ?? string.Empty,
        //        Address = customerInfo.ReportData?.FirstOrDefault()?.Address ?? string.Empty,
        //        UsageId = customerInfo.ReportHeader?.UsageId ?? string.Empty,
        //        PhoneNumber= customerInfo.ReportHeader?. ?? string.Empty,
        //        MobileNumber = customerInfo.ReportHeader?.MobileNumber ?? string.Empty,
        //        DebtPeriodCount =,
        //        PreviousReadingDateJalali= ,
        //        PreviousBillAmount=,
        //        DueDateJalali=,
        //        BillId = customerInfo.ReportHeader?.BillId ?? string.Empty,
        //        PayId =,
        //    };

        //    return new ConnectDisconnectPrintOutputDto()
        //    {
        //        CustomerNumber = customerInfo.ReportHeader?.CustomerNumber ?? 0,
        //        RegionTitle = customerInfo.ReportData?.FirstOrDefault()?.RegionTitle ?? string.Empty,
        //        ZoneTitle = customerInfo.ReportData?.FirstOrDefault()?.ZoneTitle ?? string.Empty,
        //        PostalCode = customerInfo.ReportData?.FirstOrDefault()?.PostalCode ?? string.Empty,
        //        N = e,
        //        E = n,
        //        WaterDebtAmount = ,
               
        //        FatherName = customerInfo.ReportHeader?.FatherName ?? string.Empty,
        //        FullName = customerInfo.ReportHeader?.FullName ?? string.Empty,
        //        ReadingNumber = ,
        //        PaymentId = customerInfo.ReportData?.FirstOrDefault()?.PaymentId ?? string.Empty,
        //        UsageTitle = customerInfo.ReportHeader?.UsageTitle ?? string.Empty,
        //        MeterDiameterId = customerInfo.ReportHeader?.MeterDiameterId ?? string.Empty,
        //        MeterDiameterTitle = customerInfo.ReportHeader?.MeterDiameterTitle ?? string.Empty,
        //        BranchTypeTitle = customerInfo.ReportHeader?.BranchTypeTitle ?? string.Empty,
        //        CauseTitle = inputDto.CauseId,
        //    };
        //}
        public async Task<ReportOutput<ConnectDisconnectPrintstHeaderOutputDto, ConnectDisconnectPrintsDataOutputDto>> Handle(ConnectDisconnectPrintDto input, CancellationToken cancellationToken)
        {
            //var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            //if (!validationResult.IsValid)
            //{
            //    var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            //    throw new CustomValidationException(message);
            //}

            PendingPaymentsInputDto pendingPaymentInputDto = new()
            {
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromAmount = input.FromAmount,
                ToAmount = input.ToAmount,
                FromDebtPeriodCount = input.FromDebtPeriodCount,
                ToDebtPeriodCount = input.ToDebtPeriodCount,
                UsageConsumptionIds = input.UsageConsumptionIds,
                UsageSellIds = input.UsageSellIds,
                ZoneIds = input.ZoneIds,
            };
            ReportOutput<PendingPaymentsHeaderOutputDto, PendingPaymentsDataOutputDto> pendingPayments = await _pendingPaymentsQueryService.GetInfo(pendingPaymentInputDto);

            return GetResult(pendingPayments, input);
        }
        private ReportOutput<ConnectDisconnectPrintstHeaderOutputDto, ConnectDisconnectPrintsDataOutputDto> GetResult(ReportOutput<PendingPaymentsHeaderOutputDto, PendingPaymentsDataOutputDto> pendingPayments, ConnectDisconnectPrintDto input)
        {
            IEnumerable<ConnectDisconnectPrintsDataOutputDto> data = pendingPayments
                 .ReportData
                 .Where(p => p.EndingDebt >= 0)
                 .Select(p => new ConnectDisconnectPrintsDataOutputDto()
                 {
                     CustomerNumber = p.CustomerNumber,
                     CustomerNumber2 = p.CustomerNumber,
                     ReadingNumber = p.ReadingNumber,
                     DebtAmount = p.EndingDebt,
                     FirstName = p.FirstName,
                     Surname = p.Surname,
                     Address = p.Address,
                     UsageId = p.UsageSellId,
                     PhoneNumber = p.PhoneNumber,
                     MobileNumber = p.MobileNumber,
                     DebtPeriodCount = (int)p.DebtPeriodCount,
                     PreviousReadingDateJalali = string.Empty,//todo
                     PreviousBillAmount = 0,//todo
                     DueDateJalali = string.Empty,//todo
                     BillId = p.BillId,
                     PayId = TransactionIdGenerator.GeneratePaymentId(p.EndingDebt, p.BillId, "100"),
                 });
            ConnectDisconnectPrintstHeaderOutputDto header = new()
            {
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromAmount = input.FromAmount,
                ToAmount = input.ToAmount,
                FromDebtPeriodCount = input.FromDebtPeriodCount,
                ToDebtPeriodCount = input.ToDebtPeriodCount,
                ZoneCount = input.ZoneIds.Count(),
                RecordCount = (data is not null && data.Any()) ? data.Count() : 0,
                TotalDebtPeriodCount = data.Sum(payment => payment.DebtPeriodCount),
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                Title = ReportLiterals.PendingPayments
            };
            return new ReportOutput<ConnectDisconnectPrintstHeaderOutputDto, ConnectDisconnectPrintsDataOutputDto>("قطع و وصل", header, data);
        }

        private async Task<(double, double)> GetLocation(string billId, CancellationToken cancellationToken)
        {
            LocationInfoDto location = await _locationInfoService.Handle(billId, cancellationToken);
            var utm = UtmConverter.LatLonToUtm(double.Parse(location.X), double.Parse(location.Y));

            return (utm.Easting, utm.Northing);
        }

    }
}
