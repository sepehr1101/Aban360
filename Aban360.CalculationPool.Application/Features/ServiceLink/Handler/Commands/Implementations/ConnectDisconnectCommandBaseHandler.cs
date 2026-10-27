using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
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

namespace Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Implementations
{
    internal sealed class ConnectDisconnectCommandBaseHandler : AbstractBaseConnection, IConnectDisconnectCommandBaseHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IConnectDisconnectQueryService _connectDisconnectQueryService;
        private readonly ICustomerGeneralInfoQueryService _customerGeneralInfoQueryService;
        private readonly IConCompanyQueryService _conCompanyQueryService;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly ILocationInfoGetHandler _locationInfoService;
        private readonly IT51QueryService _t51QueryService;
        private readonly IMapService _mapService;
        private static int _waterDebtCauseId = 1;
        private static int _disconnectState = 5;
        private static int _connectState = 0;
        private string _connectStateTitle = "انشعاب برقرار";
        private string _disconnectStateTitle = "حذف موقت";
        public ConnectDisconnectCommandBaseHandler(
            IHttpContextAccessor contextAccessor,
            IConnectDisconnectQueryService connectDisconnectQueryService,
            ICustomerGeneralInfoQueryService customerGeneralInfoQueryService,
            IConCompanyQueryService conCompanyQueryService,
            ICommonMemberQueryService commonMemberQueryService,
            ILocationInfoGetHandler locationInfoService,
            IT51QueryService t51QueryService,
            IMapService mapService,
            IConfiguration configuration)
                : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _connectDisconnectQueryService = connectDisconnectQueryService;
            _connectDisconnectQueryService.NotNull(nameof(connectDisconnectQueryService));

            _customerGeneralInfoQueryService = customerGeneralInfoQueryService;
            _customerGeneralInfoQueryService.NotNull(nameof(customerGeneralInfoQueryService));

            _conCompanyQueryService = conCompanyQueryService;
            _conCompanyQueryService.NotNull(nameof(conCompanyQueryService));

            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));

            _locationInfoService = locationInfoService;
            _locationInfoService.NotNull(nameof(locationInfoService));

            _t51QueryService = t51QueryService;
            _t51QueryService.NotNull(nameof(t51QueryService));

            _mapService = mapService;
            _mapService.NotNull(nameof(mapService));
        }

        public async Task<(ConCompanyGetDto, ConCompanyPersonnelPersonalGetDto)> GetCompanyInfo(int companyId, Guid personnelId)
        {
            ConCompanyGetDto companyInfo = await _conCompanyQueryService.GetValid(companyId);
            ConCompanyPersonnelPersonalGetDto personnelInfo = await _conCompanyQueryService.GetPersonnelById(companyId, personnelId);

            return (companyInfo, personnelInfo);
        }
        public async Task ExecSql(IDbConnection connection, IDbTransaction transaction, ConnectDisconnectInsertDto connectDisconnectInsertDto, IAppUser appUser, string logTex)
        {
            ConnectDisconnectCommandService connectDisconnectCommandService = new(connection, transaction);
            OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);

            await connectDisconnectCommandService.Insert(connectDisconnectInsertDto);
            await opLogCommandService.Insert(logTex, appUser);
        }
        public async Task<ReportOutput<ConnectDisconnectPrintHeaderOutputDto, ConnectDisconnectPrintDataOutputDto>> GetResult(ReportOutput<CustomerGeneralInfoHeaderDto, CustomerGeneralInfoDataDto> customerInfo, string billId, int? why, string title, string messageText, string companyName, CancellationToken cancellationToken)
        {
            string? zoneAddress = await _t51QueryService.GetAddress(customerInfo.ReportData?.FirstOrDefault()?.ZoneId ?? 0, true);
            ICollection<ConnectDisconnectPrintDataOutputDto> data = new List<ConnectDisconnectPrintDataOutputDto>();
            var locInfo = await GetBase64Location(billId, cancellationToken);
            long devidDebt = (customerInfo.ReportData?.FirstOrDefault()?.WaterDebtAmount ?? 0) / 1000;

            data.Add(new ConnectDisconnectPrintDataOutputDto()
            {
                CustomerNumber = customerInfo.ReportHeader?.CustomerNumber ?? 0,
                RegionTitle = customerInfo.ReportData?.FirstOrDefault()?.RegionTitle ?? string.Empty,
                ZoneTitle = customerInfo.ReportData?.FirstOrDefault()?.ZoneTitle ?? string.Empty,
                ZoneAddress = zoneAddress,
                PostalCode = customerInfo.ReportData?.FirstOrDefault()?.PostalCode ?? string.Empty,
                NationalCode = customerInfo.ReportHeader?.NationalCode ?? string.Empty,
                Address = customerInfo.ReportData?.FirstOrDefault()?.Address ?? string.Empty,
                WaterDebtAmount = devidDebt * 1000,
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
                CompanyTitle = companyName,
                CauseTitle = why.HasValue ? GetCasues().Where(c => c.Id == why).FirstOrDefault()?.Title ?? string.Empty : string.Empty,
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
        public ConnectDisconnectInsertDto GetConnectDisconnectInsertDto(ReportOutput<CustomerGeneralInfoHeaderDto, CustomerGeneralInfoDataDto> customerInfo, int? why, string? description, ConCompanyGetDto companyInfo, ConCompanyPersonnelPersonalGetDto personnelInfo, IAppUser appUser, string causeTitle, int typeId, string typeTitle)
        {
            return new ConnectDisconnectInsertDto()
            {
                ZoneId = customerInfo.ReportData?.FirstOrDefault()?.ZoneId ?? 0,
                ZoneTitle = customerInfo.ReportData?.FirstOrDefault()?.ZoneTitle ?? string.Empty,
                BillId = customerInfo.ReportHeader.BillId,
                WaterDebt = customerInfo.ReportData?.FirstOrDefault()?.WaterDebtAmount ?? 0,
                CommandDateTime = DateTime.Now,
                CommandBy = appUser.UserId,
                CommandCauseId = why ?? 0,
                CommandCauseTitle = causeTitle,
                ResultDateTime = null,
                ResultBy = null,
                ResultId = null,
                ResultTitle = null,
                MeterDiameterId = customerInfo.ReportHeader.MeterDiameterId,
                MeterDiameterTitle = customerInfo.ReportHeader.MeterDiameterTitle,
                CompanyId = companyInfo.Id,
                CompanyTitle = companyInfo.CompanyName,
                PersonnelId = personnelInfo.Id,
                PersonnelName = personnelInfo.FullName,
                TypeId = typeId,
                TypeTitle = typeTitle,
                Description = description ?? string.Empty,
            };
        }
        private async Task<(LocationInfoDto, string)> GetBase64Location(string billId, CancellationToken cancellationToken)
        {
            LocationInfoDto location = await _locationInfoService.Handle(billId, cancellationToken);
            if (location is null ||
                string.IsNullOrWhiteSpace(location.X) || location.X.Trim() == "0" ||
                string.IsNullOrWhiteSpace(location.Y) || location.Y.Trim() == "0")
            {
                return (location, await Base64Operation.GetNotFoundBase64(cancellationToken));
            }
            string base64 = await _mapService.GenerateMapBase64(location.X, location.Y, cancellationToken);
            return (location, base64);
        }
        public async Task<ReportOutput<CustomerGeneralInfoHeaderDto, CustomerGeneralInfoDataDto>> ValidateAndGetCustomerGeneral(string billId, int? why, bool isConnect)
        {
            int typeId = isConnect ? ReportLiterals.ConnectId : ReportLiterals.DisconnectId;
            int deletionStateId = isConnect ? _connectState : _disconnectState;
            string deletionStateTitle = isConnect ? _connectStateTitle : _disconnectStateTitle;
            bool isWaterCause = (why ?? 0) == _waterDebtCauseId;

            await CheckDuplicateRequest(billId, typeId);
            ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await _commonMemberQueryService.Get(billId);
            ReportOutput<CustomerGeneralInfoHeaderDto, CustomerGeneralInfoDataDto> customerInfo = await _customerGeneralInfoQueryService.Get(zoneIdAndCustomerNumber);

            if ((customerInfo?.ReportData?.FirstOrDefault()?.DeletionStateId ?? 0) == deletionStateId)
            {
                throw new InvalidCustomerCommandException(ExceptionLiterals.InvalidConnectDisconnectByCustomerInfo(deletionStateTitle));
            }
            if ((customerInfo?.ReportData?.FirstOrDefault()?.WaterDebtAmount ?? 0) <= 0 && isWaterCause)
            {
                throw new InvalidCustomerCommandException(ExceptionLiterals.InvalidDebtAmountLessThanZero);
            }
            return customerInfo;
        }
        private async Task CheckDuplicateRequest(string billId, int typeId)
        {
            ConnectDisconnectGetDto? previousRequest = await _connectDisconnectQueryService.Get(new ConnectDisconnectGetWithConditionDto(billId, typeId, false, false));
            if (previousRequest is not null)
            {
                throw new InvalidCustomerCommandException(ExceptionLiterals.InvalidConnectDisconnectDuplicateRequest);
            }
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
