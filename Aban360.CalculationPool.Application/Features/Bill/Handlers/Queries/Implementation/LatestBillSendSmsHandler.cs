using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.NotificationPool.Application.Features.Sms;
using Aban360.ReportPool.Application.Features.WaterInvoice.Handler.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Hangfire;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Implementation
{
    internal sealed class LatestBillSendSmsHandler : ILatestBillSendSmsHandler
    {
        private readonly ICommonMemberQueryService _customerInfoHandler;
        private readonly IWaterInvoiceHandler _waterInvoiceHandler;
        private readonly ICommonZoneService _zoneService;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly ISmsOldHandler _smsOldHandler;
        public LatestBillSendSmsHandler(
            ICommonMemberQueryService customerInfoHandler,
            IWaterInvoiceHandler waterInvoiceHandler,
            ICommonZoneService zoneService,
            IBackgroundJobClient backgroundJobClient,
            ISmsOldHandler smsOldHandler)
        {
            _customerInfoHandler = customerInfoHandler;
            _customerInfoHandler.NotNull(nameof(customerInfoHandler));

            _waterInvoiceHandler = waterInvoiceHandler;
            _waterInvoiceHandler.NotNull(nameof(waterInvoiceHandler));

            _zoneService = zoneService;
            _zoneService.NotNull(nameof(zoneService));

            _backgroundJobClient = backgroundJobClient;
            _backgroundJobClient.NotNull(nameof(backgroundJobClient));

            _smsOldHandler = smsOldHandler;
            _smsOldHandler.NotNull(nameof(smsOldHandler));
        }

        public async Task Handle(string billId, string? mobileNumber, IAppUser appUser, CancellationToken cancellationToken)
        {
            ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await _customerInfoHandler.Get(billId, true);
            MemberInfoGetDto customerInfo = await _customerInfoHandler.Get(zoneIdAndCustomerNumber);
            await _zoneService.IsUserInZone(appUser, zoneIdAndCustomerNumber.ZoneId);

            string text = await GetText(billId, cancellationToken);
            string finalMobileNumber = string.IsNullOrWhiteSpace(mobileNumber) ? customerInfo.MobileNumber : mobileNumber;

            _backgroundJobClient.Enqueue(() => _smsOldHandler.SendNew(finalMobileNumber, text, Guid.NewGuid()));

        }
        private async Task<string> GetText(string billId, CancellationToken cancellationToken)
        {
            ReportOutput<WaterInvoiceDto, LineItemsDto> result = await _waterInvoiceHandler.Handle_WithLastDb(billId, cancellationToken);
            return string.Format(SmsTemplates.SimpleBill,
                result.ReportHeader.FullName,
                result.ReportHeader.ZoneTitle,
                result.ReportHeader.PreviousMeterDateJalali,
                result.ReportHeader.CurrentMeterDateJalali,
                result.ReportHeader.PreviousMeterNumber,
                result.ReportHeader.CurrentMeterNumber,
                result.ReportHeader.Sum,
                result.ReportHeader.PayableAmount,
                result.ReportHeader.BillId,
                result.ReportHeader.PayId,
                result.ReportHeader.PaymentDeadline//TODO: اگه jam-baha>10000 بود بشه فوری و در غیر اینصورت مهلت فعلی
                );
        }
    }
}
