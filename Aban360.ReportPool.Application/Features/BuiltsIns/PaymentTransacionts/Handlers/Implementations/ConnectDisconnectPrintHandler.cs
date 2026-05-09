using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts;
using Aban360.ReportPool.Application.Features.Geo.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Domain.Features.Geo;
using Aban360.ReportPool.Infrastructure.Features.Geo;
using Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Implementations
{
    internal sealed class ConnectDisconnectPrintHandler : IConnectDisconnectPrintHandler
    {
        private readonly ICustomerGeneralInfoQueryService _customerGeneralInfoQueryService;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly ILocationInfoGetHandler _locationInfoService;
        public ConnectDisconnectPrintHandler(
            ICustomerGeneralInfoQueryService customerGeneralInfoQueryService,
            ICommonMemberQueryService commonMemberQueryService,
            ILocationInfoGetHandler locationInfoService)
        {
            _customerGeneralInfoQueryService = customerGeneralInfoQueryService;
            _customerGeneralInfoQueryService.NotNull(nameof(customerGeneralInfoQueryService));

            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));

            _locationInfoService = locationInfoService;
            _locationInfoService.NotNull(nameof(locationInfoService));
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

        private async Task<(double, double)> GetLocation(string billId, CancellationToken cancellationToken)
        {
            LocationInfoDto location = await _locationInfoService.Handle(billId, cancellationToken);
            var utm = UtmConverter.LatLonToUtm(double.Parse(location.X), double.Parse(location.Y));

            return (utm.Easting, utm.Northing);
        }

    }
}
