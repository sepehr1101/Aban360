using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using DNTPersianUtils.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Implementations
{
    internal sealed class ConnectDisconnectSetResultHandler : AbstractBaseConnection, IConnectDisconnectSetResultHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ISubscriptionQueryService _customerQueryService;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly ICommonZoneService _commonZoneService;
        private readonly IConCompanyQueryService _conCompanyQueryService;
        private readonly IConnectDisconnectQueryService _connectDisconnectQueryService;
        private static int[] _allowedSendMessageDisconnectResult = { 1, 2 };
        private static int _disconnectState = 5;
        private static int _connectState = 0;
        private static int _operator = 666;
        private static int _connectTypeId = 1;
        private static int _disconnectTypeId = 0;
        public ConnectDisconnectSetResultHandler(
            IHttpContextAccessor contextAccessor,
            ISubscriptionQueryService customerQueryService,
            ICommonMemberQueryService commonMemberQueryService,
            ICommonZoneService commonZoneService,
            IConCompanyQueryService conCompanyQueryService,
            IConnectDisconnectQueryService connectDisconnectQueryService,
            IConfiguration configuratio)
            : base(configuratio)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _customerQueryService = customerQueryService;
            _customerQueryService.NotNull(nameof(customerQueryService));

            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));

            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(commonZoneService));

            _conCompanyQueryService = conCompanyQueryService;
            _conCompanyQueryService.NotNull(nameof(conCompanyQueryService));

            _connectDisconnectQueryService = connectDisconnectQueryService;
            _connectDisconnectQueryService.NotNull(nameof(connectDisconnectQueryService));
        }

        public async Task<ConnectDisconnectSetResultOutputDto> Handle(ServiceLinkConnectionInput inputDto, bool isConnect, IAppUser appUser, CancellationToken cancellationToken)
        {
            int deletionStateId = isConnect ? _connectState : _disconnectState;
            ConnectDisconnectGetDto connectDisconnectInfo = await _connectDisconnectQueryService.Get(inputDto.Id, isConnect ? _connectTypeId : _disconnectTypeId);
            ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await _commonMemberQueryService.Get(connectDisconnectInfo.BillId);
            MemberInfoGetDto memberInfo = await _commonMemberQueryService.Get(zoneIdAndCustomerNumber);

            Validate(memberInfo, connectDisconnectInfo, deletionStateId, inputDto, isConnect);
            var (resultId, resultTitle, opLogText, message, isSendMessage) = GetConnectOrDisconnectValue(connectDisconnectInfo, memberInfo, inputDto, isConnect);

            CustomerUpdateDto customerUpdate = GetCustomerUpdate(inputDto, deletionStateId, memberInfo);
            ConnectDisconnectUpdateDto connectDiscnnectUpdateDto = new(inputDto.Id, appUser.UserId, resultId, resultTitle, null, string.Join("_", new string[] { connectDisconnectInfo.Description ?? string.Empty, inputDto.Description ?? string.Empty }));

            await SqlCommands(customerUpdate, connectDiscnnectUpdateDto, appUser, opLogText);

            return new ConnectDisconnectSetResultOutputDto(memberInfo.BillId, memberInfo.MobileNumber, message, isSendMessage);
        }
        private async Task SqlCommands(CustomerUpdateDto updateDto, ConnectDisconnectUpdateDto connectDiscnnectUpdateDto, IAppUser appUser, string opLogText)
        {
            ZoneIdAndCustomerNumber zoneIdAndCustomer = new(updateDto.ZoneId, updateDto.CustomerNumber);
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    ArchMemCommandService _archMemCommandService = new(connection, transaction);
                    MembersCommandService _membersCommandService = new(connection, transaction);
                    ClientsCommandService _clientCommandService = new(connection, transaction);
                    ConnectDisconnectCommandService _connectDisconnectCommandService = new(connection, transaction);
                    OpLogWithTransactionCommandService _opLogCommandService = new(_contextAccessor, connection, transaction);
                    string dbName = GetDbName(updateDto.ZoneId);
                    //string dbName = "Atlas";

                    int rowId = await _archMemCommandService.InsertByPreviousRecord(updateDto, dbName, dbName);
                    await _membersCommandService.Update(updateDto, dbName);
                    await _clientCommandService.UpdateToDayJalali(zoneIdAndCustomer, updateDto.ToDayDateJalali);
                    await _clientCommandService.InsertByArchMemId(rowId, dbName);
                    await _connectDisconnectCommandService.Update(connectDiscnnectUpdateDto);
                    await _opLogCommandService.Insert(opLogText, appUser);

                    transaction.Commit();
                }
            }
        }
        private CustomerUpdateDto GetCustomerUpdate(ServiceLinkConnectionInput inputDto, int deletionStateId, MemberInfoGetDto memberInfo)
        {
            return new CustomerUpdateDto()
            {
                Id = memberInfo.Id,
                CustomerNumber = memberInfo.CustomerNumber,
                ZoneId = memberInfo.ZoneId,
                BillId = memberInfo.BillId,
                X = memberInfo.X,
                Y = memberInfo.Y,
                ReadingNumber = memberInfo.ReadingNumber,
                FirstName = memberInfo.FirstName,
                Surname = memberInfo.Surname,
                Address = memberInfo.Address,
                PostalCode = memberInfo.PostalCode,
                Plaque = memberInfo.Plaque ?? string.Empty,
                NationalCode = memberInfo.NationalCode,
                PhoneNumber = memberInfo.PhoneNumber,
                MobileNumber = memberInfo.MobileNumber,
                FatherName = memberInfo.FatherName,
                BranchTypeId = memberInfo.UseStateId,
                UsageSellId = memberInfo.UsageId,
                UsageConsumptionId = memberInfo.UsageConsumptionId,
                EmptyUnit = memberInfo.EmptyUnit,
                CommertialUnit = memberInfo.CommercialUnit,
                DomesticUnit = memberInfo.DomesticUnit,
                OtherUnit = memberInfo.OtherUnit,
                HouseholdDateJalali = DateValidation(memberInfo.HouseholdDateJalali, false),
                HouseholdNumber = memberInfo.HouseholdNumber,
                MeterDiamterId = memberInfo.MeterDiameterId,
                IsSpecial = memberInfo.IsSpecial,
                ContractualCapacity = memberInfo.ContractualCapacity,
                ImprovementCommertial = memberInfo.CommercialImprovement,
                ImprovementDomestic = memberInfo.DomesticImprovement,
                ImprovementOverall = memberInfo.OverallImprovement,
                Premises = memberInfo.Premises,
                Operator = _operator,
                SewageInstallationDateJalali = DateValidation(memberInfo.SiphonInstallationDateJalali, false),
                SewageRequestDateJalali = DateValidation(memberInfo.SiphonRequestDateJalali, false),
                MeterInstallationDateJalali = DateValidation(memberInfo.MeterInstallationDateJalali, false),
                MeterRequestDateJalali = DateValidation(memberInfo.MeterRequestDateJalali, false),
                Siphon100 = memberInfo.Siphon100,
                Siphon125 = memberInfo.Siphon125,
                Siphon150 = memberInfo.Siphon150,
                Siphon200 = memberInfo.Siphon200,
                Siphon5 = memberInfo.Siphon5,
                Siphon6 = memberInfo.Siphon6,
                Siphon7 = memberInfo.Siphon7,
                Siphon8 = memberInfo.Siphon8,
                MainSiphon = int.Parse(memberInfo.MainSiphon),
                DeletionStateId = deletionStateId,
                BodySerial = memberInfo.BodySerial ?? string.Empty,
                CommonSiphon = memberInfo.CommonSiphon1,
                MeterRegisterDateJalali = DateValidation(memberInfo.MeterInstalltionRegisterDateJalali, false),
                SewageRegisterDateJalali = DateValidation(memberInfo.SiphonInstalltionRegisterDateJalali, false),
                GuildId = memberInfo.Guild,
                ToDayDateJalali = DateTime.Now.ToShortPersianDateString(),
                ToDayDateJalaliWithFragmentYear = DateTime.Now.ToShortPersianDateString().Substring(2, 8),
            };
        }
        private async Task<SubscriptionGetDto> GetCustomerPreviousInfo(IAppUser appUser, string billId)
        {
            SubscriptionGetDto previousSubscription = await _customerQueryService.GetInfo(billId);
            if (previousSubscription == null)
            {
                throw new BaseException("شناسه قبض یافت نشد");
            }
            await _commonZoneService.IsUserInZone(appUser, previousSubscription.ZoneId);

            return previousSubscription;
        }
        private string DateValidation(string? inputDate, bool hasException)
        {
            if (hasException)
            {
                return string.IsNullOrWhiteSpace(inputDate) || inputDate.Trim().Length != 10 ?
                    throw new InvalidDateException(ExceptionLiterals.InvalidDate) :
                    inputDate.Trim();
            }
            return string.IsNullOrWhiteSpace(inputDate) || inputDate.Trim().Length != 10 ? string.Empty : inputDate.Trim();
        }
        private (int, string, string, string, bool) GetConnectOrDisconnectValue(ConnectDisconnectGetDto connectDisconnectInfo, MemberInfoGetDto memberInfo, ServiceLinkConnectionInput inputDto,  bool isConnect)
        {
            ICollection<NumericDictionary> disconnectResults = GetDisconnectResults();
            NumericDictionary? result = isConnect ? new NumericDictionary(0, string.Empty) : disconnectResults.Where(d => d.Id == (inputDto.Why)).FirstOrDefault();
            if (result is null)
                throw new InvalidCustomerCommandException(ExceptionLiterals.InvalidEmptyDisconnectWhy);

            string connectLogText = string.Format(OpLogLiterals.ServiceLinkConnectSetResultOpLog, connectDisconnectInfo.BillId);
            string disconnectLogText = string.Format(OpLogLiterals.ServiceLinkDisconnectSetResultOpLog, connectDisconnectInfo.BillId, result.Title);
            string opLogText = isConnect ? connectLogText : disconnectLogText;

            string connectMessage = string.Format(SmsTemplates.ServiceLinkConnected, memberInfo.ZoneTitle, memberInfo.FullName, memberInfo.BillId, inputDto.When, connectDisconnectInfo.PersonnelName, Environment.NewLine);
            string disconnectMessage = string.Format(SmsTemplates.ServiceLinkDisconnected, memberInfo.ZoneTitle, memberInfo.FullName, memberInfo.BillId, inputDto.When, connectDisconnectInfo.PersonnelName, Environment.NewLine);
            string message = isConnect ? connectMessage : disconnectMessage;

            bool isSendMessage = isConnect ? true : _allowedSendMessageDisconnectResult.Contains(result.Id);

            return (result.Id, result.Title, opLogText, message, isSendMessage);
        }
        private void Validate(MemberInfoGetDto memberInfo, ConnectDisconnectGetDto connectDisconnectResult, int deletionStateId, ServiceLinkConnectionInput inputDto, bool isConnect)
        {
            if (!isConnect && (inputDto.Why == 0 || inputDto.Why is null))
            {
                throw new InvalidCustomerCommandException(ExceptionLiterals.InvalidEmptyDisconnectWhy);
            }
            if (connectDisconnectResult.ResultId is not null)
            {
                throw new InvalidCustomerCommandException(ExceptionLiterals.InvalidConnectDisconnectDuplicateResult);
            }
            if (memberInfo.DeletionStateId == deletionStateId)
            {
                throw new InvalidCustomerCommandException(ExceptionLiterals.InvalidDuplicateDeletionState);
            }
            if (connectDisconnectResult.CommandDateTime.ToShortPersianDateString().CompareTo(inputDto.When) > 0)
            {
                throw new InvalidCustomerCommandException(ExceptionLiterals.InvalidLessThanCommandDate);
            }
        }
        public ICollection<NumericDictionary> GetDisconnectResults()
        {
            ICollection<NumericDictionary> results = new List<NumericDictionary>()
            {
                new NumericDictionary(1,"انشعاب از طریق قطع و وصل، قطع گردید."),
                new NumericDictionary(2,"انشعاب از طریق حفاری قطع گردید."),
                new NumericDictionary(3,"محل انشعاب پیدا نشد."),
                new NumericDictionary(4,"مشترک از قطع جلوگیری نمود."),
                new NumericDictionary(5,"مشترک تسویه حساب نمود."),
                new NumericDictionary(6,"قرائت میسر شد."),
            };
            return results;
        }
    }
}
