using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts;
using Aban360.ReportPool.Application.Features.Geo.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Domain.Features.Geo;
using Aban360.ReportPool.Infrastructure.Features.Geo;
using Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;
using DNTPersianUtils.Core;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Implementations
{
    internal sealed class ConnectDisconnectPrintHandler : IConnectDisconnectPrintHandler
    {
        private readonly ICustomerGeneralInfoQueryService _customerGeneralInfoQueryService;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly ILocationInfoGetHandler _locationInfoService;
        private readonly IMapService _mapService;
        public ConnectDisconnectPrintHandler(
            ICustomerGeneralInfoQueryService customerGeneralInfoQueryService,
            ICommonMemberQueryService commonMemberQueryService,
            ILocationInfoGetHandler locationInfoService,
            IMapService mapService)
        {
            _customerGeneralInfoQueryService = customerGeneralInfoQueryService;
            _customerGeneralInfoQueryService.NotNull(nameof(customerGeneralInfoQueryService));

            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));

            _locationInfoService = locationInfoService;
            _locationInfoService.NotNull(nameof(locationInfoService));

            _mapService = mapService;
            _mapService.NotNull(nameof(mapService));
        }

        public async Task<ReportOutput<ConnectDisconnectPrintHeaderOutputDto, ConnectDisconnectPrintDataOutputDto>> Handle(ConnectDisconnectPrintInputDto inputDto, bool isConnect ,CancellationToken cancellationToken)
        {
            string title = isConnect ? ReportLiterals.Connect : ReportLiterals.Disconnect;
            ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await _commonMemberQueryService.Get(inputDto.BillId);
            ReportOutput<CustomerGeneralInfoHeaderDto, CustomerGeneralInfoDataDto> customerInfo = await _customerGeneralInfoQueryService.Get(zoneIdAndCustomerNumber);

            ICollection<ConnectDisconnectPrintDataOutputDto> data = new List<ConnectDisconnectPrintDataOutputDto>();
            data.Add(new ConnectDisconnectPrintDataOutputDto()
            {
                CustomerNumber = customerInfo.ReportHeader?.CustomerNumber ?? 0,
                RegionTitle = customerInfo.ReportData?.FirstOrDefault()?.RegionTitle ?? string.Empty,
                ZoneTitle = customerInfo.ReportData?.FirstOrDefault()?.ZoneTitle ?? string.Empty,
                PostalCode = customerInfo.ReportData?.FirstOrDefault()?.PostalCode ?? string.Empty,
                NationalCode = customerInfo.ReportHeader?.NationalCode ?? string.Empty,
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
                CauseTitle = inputDto.Why.HasValue ? GetCasues().Where(c => c.Id == inputDto.Why).FirstOrDefault()?.Title ?? string.Empty : string.Empty,
                Base64 = await GetBase64Location(inputDto.BillId, cancellationToken)
            });
            ConnectDisconnectPrintHeaderOutputDto header = new()
            {
                CustomerCount = 1,
                RecordCount = 1,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                Title = title
            };
            return new ReportOutput<ConnectDisconnectPrintHeaderOutputDto, ConnectDisconnectPrintDataOutputDto>(title, header, data);
        }
        private async Task<string> GetBase64Location(string billId, CancellationToken cancellationToken)
        {
            LocationInfoDto location = await _locationInfoService.Handle(billId, cancellationToken);
            return await _mapService.GenerateMapBase64(location.X, location.Y);
        }
        public ICollection<NumericDictionary> GetCasues()
        {
            ICollection<NumericDictionary> causes = new List<NumericDictionary>()
            {
              new NumericDictionary(1,"بدهی آببها"),
              new NumericDictionary(2,"بدهی حق انشعاب"),
              new NumericDictionary(3,"بسته بیش از سه دوره"),
              new NumericDictionary(4,"انشعاب غیر مجاز آب"),
              new NumericDictionary(5,"انشعاب غیر مجاز فاضلاب"),
              new NumericDictionary(6,"به درخواست مشترک یا مصرف کننده"),
              new NumericDictionary(7,"نصب مستقیم پمپ"),
            };
            return causes;
        }
    }
}
