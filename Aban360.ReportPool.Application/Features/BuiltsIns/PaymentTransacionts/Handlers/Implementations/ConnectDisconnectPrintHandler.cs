using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Application.Constant;
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
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Implementations
{
    internal sealed class ConnectDisconnectPrintHandler : AbstractBaseConnection, IConnectDisconnectPrintHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ICustomerGeneralInfoQueryService _customerGeneralInfoQueryService;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly ILocationInfoGetHandler _locationInfoService;
        private readonly IMapService _mapService;
        private int _connectTypeId = 1;
        private int _disconnectTypeId = 0;
        private string _connectTypeTitle = "صدور دستور وصل";
        private string _disconnectTypeTitle = "صدور دستور قطع";
        public ConnectDisconnectPrintHandler(
            IHttpContextAccessor contextAccessor,
            ICustomerGeneralInfoQueryService customerGeneralInfoQueryService,
            ICommonMemberQueryService commonMemberQueryService,
            ILocationInfoGetHandler locationInfoService,
            IMapService mapService,
            IConfiguration configuration)
                : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _customerGeneralInfoQueryService = customerGeneralInfoQueryService;
            _customerGeneralInfoQueryService.NotNull(nameof(customerGeneralInfoQueryService));

            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));

            _locationInfoService = locationInfoService;
            _locationInfoService.NotNull(nameof(locationInfoService));

            _mapService = mapService;
            _mapService.NotNull(nameof(mapService));
        }

        public async Task<ReportOutput<ConnectDisconnectPrintHeaderOutputDto, ConnectDisconnectPrintDataOutputDto>> Handle(ConnectDisconnectPrintInputDto inputDto, IAppUser appUser, bool isConnect, CancellationToken cancellationToken)
        {
            ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await _commonMemberQueryService.Get(inputDto.BillId);
            ReportOutput<CustomerGeneralInfoHeaderDto, CustomerGeneralInfoDataDto> customerInfo = await _customerGeneralInfoQueryService.Get(zoneIdAndCustomerNumber);
            NumericDictionary? connectDisconnectCause = GetCasues().Where(c => c.Id == (inputDto.Why ?? 0)).FirstOrDefault();
            var (messageText, opLogText, title) = GetStringsValue(customerInfo, inputDto, isConnect, connectDisconnectCause?.Title);

            ReportOutput<ConnectDisconnectPrintHeaderOutputDto, ConnectDisconnectPrintDataOutputDto> result = await GetResult(customerInfo, inputDto, title, messageText, cancellationToken);
            ConnectDisconnectInsertDto connectDisconnectInsertDto = GetConnectDisconnectInsertDto(customerInfo, inputDto, appUser, connectDisconnectCause?.Title ?? string.Empty, isConnect);

            await SqlCommands(connectDisconnectInsertDto, appUser, opLogText);

            return result;
        }
        private async Task SqlCommands(ConnectDisconnectInsertDto connectDisconnectInsertDto, IAppUser appUser, string logTex)
        {
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    ConnectDisconnectCommandService connectDisconnectCommandService = new(connection, transaction);
                    OpLogCommandService opLogCommandService = new(_contextAccessor, connection, transaction);

                    await connectDisconnectCommandService.Insert(connectDisconnectInsertDto);
                    await opLogCommandService.Insert(logTex, appUser);

                    transaction.Commit();
                }
            }
        }
        private async Task<ReportOutput<ConnectDisconnectPrintHeaderOutputDto, ConnectDisconnectPrintDataOutputDto>> GetResult(ReportOutput<CustomerGeneralInfoHeaderDto, CustomerGeneralInfoDataDto> customerInfo, ConnectDisconnectPrintInputDto inputDto, string title, string messageText, CancellationToken cancellationToken)
        {
            ICollection<ConnectDisconnectPrintDataOutputDto> data = new List<ConnectDisconnectPrintDataOutputDto>();
            var locInfo = await GetBase64Location(inputDto.BillId, cancellationToken);
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
                MeterDiameterId = customerInfo.ReportHeader?.MeterDiameterId ?? 0,
                MeterDiameterTitle = customerInfo.ReportHeader?.MeterDiameterTitle ?? string.Empty,
                BranchTypeTitle = customerInfo.ReportHeader?.BranchTypeTitle ?? string.Empty,
                CompanyTitle = inputDto.Who,
                CauseTitle = inputDto.Why.HasValue ? GetCasues().Where(c => c.Id == inputDto.Why).FirstOrDefault()?.Title ?? string.Empty : string.Empty,
                Base64 = locInfo.Item2,
                X = locInfo.Item1.X,
                Y = locInfo.Item1.Y
            });
            ConnectDisconnectPrintHeaderOutputDto header = new()
            {
                CustomerCount = 1,
                RecordCount = 1,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                Title = title,
                MessageText = messageText
            };
            return new ReportOutput<ConnectDisconnectPrintHeaderOutputDto, ConnectDisconnectPrintDataOutputDto>(title, header, data);
        }
        private ConnectDisconnectInsertDto GetConnectDisconnectInsertDto(ReportOutput<CustomerGeneralInfoHeaderDto, CustomerGeneralInfoDataDto> customerInfo, ConnectDisconnectPrintInputDto inputDto, IAppUser appUser, string causeTitle, bool isConnect)
        {
            return new ConnectDisconnectInsertDto()
            {
                ZoneId = customerInfo.ReportData?.FirstOrDefault()?.ZoneId ?? 0,
                ZoneTitle = customerInfo.ReportData?.FirstOrDefault()?.ZoneTitle ?? string.Empty,
                BillId = customerInfo.ReportHeader.BillId,
                WaterDebt = customerInfo.ReportData?.FirstOrDefault()?.WaterDebtAmount ?? 0,
                CommandDateTime = DateTime.Now,
                CommandBy = appUser.UserId,
                CommandCauseId = inputDto.Why ?? 0,
                CommandCauseTitle = causeTitle,
                ResultDateTime = null,
                ResultBy = null,
                ResultId = null,
                ResultTitle = null,
                MeterDiameterId = customerInfo.ReportHeader.MeterDiameterId,
                MeterDiameterTitle = customerInfo.ReportHeader.MeterDiameterTitle,
                CompanyTitle = inputDto.Who,
                TypeId = isConnect ? _connectTypeId : _disconnectTypeId,
                TypeTitle = isConnect ? _connectTypeTitle : _disconnectTypeTitle,
                Description=inputDto.Description ?? string.Empty,
            };
        }
        private (string, string, string) GetStringsValue(ReportOutput<CustomerGeneralInfoHeaderDto, CustomerGeneralInfoDataDto> customerInfo, ConnectDisconnectPrintInputDto inputDto, bool isConnect, string? causeTitle)
        {
            string disconnectText = string.Format(SmsTemplates.ServiceLinkDisconnectAlert, customerInfo.ReportData.FirstOrDefault().ZoneTitle, customerInfo.ReportHeader.BillId, causeTitle ?? string.Empty, inputDto.When, Environment.NewLine);
            string connectText = string.Format(SmsTemplates.ServiceLinkConnectAlert, customerInfo.ReportData.FirstOrDefault().ZoneTitle, customerInfo.ReportHeader.BillId, inputDto.When, Environment.NewLine);
            string messageText = isConnect ? connectText : disconnectText;

            string connectLog = string.Format(Literals.ServiceLinkConnectInsertOpLog, inputDto.BillId);
            string disconnectLog = string.Format(Literals.ServiceLinkDisconnectInsertOpLog, inputDto.BillId, causeTitle ?? string.Empty);
            string opLogText = isConnect ? connectLog : disconnectLog;

            string title = isConnect ? ReportLiterals.Connect : ReportLiterals.Disconnect;

            return (messageText, opLogText, title);
        }
        private async Task<(LocationInfoDto, string)> GetBase64Location(string billId, CancellationToken cancellationToken)
        {
            LocationInfoDto location = await _locationInfoService.Handle(billId, cancellationToken);
            string base64 = await _mapService.GenerateMapBase64(location.X, location.Y);
            return (location, base64);
        }
        public ICollection<NumericDictionary> GetCasues()
        {
            ICollection<NumericDictionary> causes = new List<NumericDictionary>()
            {
              new NumericDictionary(1,"بدهی آببها"),
              new NumericDictionary(2,"بدهی حق انشعاب"),
              new NumericDictionary(3,"عدم قرائت کنتور"),
              new NumericDictionary(4,"تخلفات"),
              new NumericDictionary(5,"سایر"),
              //new NumericDictionary(3,"بسته بیش از سه دوره"),
              //new NumericDictionary(4,"انشعاب غیر مجاز آب"),
              //new NumericDictionary(5,"انشعاب غیر مجاز فاضلاب"),
              //new NumericDictionary(6,"به درخواست مشترک یا مصرف کننده"),
              //new NumericDictionary(7,"نصب مستقیم پمپ"),
            };
            return causes;
        }
    }
}
