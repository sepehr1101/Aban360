using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Microsoft.Extensions.Configuration;
using System.Data;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations;
using Aban360.Common.Literals;
using DNTPersianUtils.Core;
using Aban360.Common.BaseEntities;
using FluentValidation;
using Aban360.OldCalcPools.Persistence.Features.WaterReturn.Queries.Contracts;
using Aban360.Common.Db.Services;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Constants.Literals;
using Microsoft.AspNetCore.Http;
using Aban360.ClaimPool.Domain.Constants;
using System.Threading;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Implementationsu
{
    internal sealed class CustomerUpdateHandler : AbstractBaseConnection, ICustomerUpdateHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ISubscriptionQueryService _customerQueryService;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly IMembersQueryService _membersQueryService;
        private readonly ICommonZoneService _commonZoneService;
        private readonly IValidator<CustomerUpdateInputDto> _allDataValidator;
        private readonly IValidator<CustomerEstateUpdateDto> _estateValidator;
        private readonly IValidator<CustomerTechnicalUpdateDto> _technicalUpdateValidator;
        private readonly IValidator<CustomerMobileUpdateInputDto> _mobilevalidator;
        private readonly IValidator<CustomerBranchTypeUpdateInputDto> _branchTypeUpdateValidator;
        private readonly IValidator<CustomerHouseholdUpdateInputDto> _householdUpdateValidator;
        static int[] _allowedToSetConstructionType = { 0, 1 };
        private string _currentDateJalali = DateTime.Now.ToShortPersianDateString();
        private int _constructionId = 4;
        private int _operator = 666;
        public CustomerUpdateHandler(
            IHttpContextAccessor contextAccessor,
            ISubscriptionQueryService customerQueryService,
            ICommonMemberQueryService commonMemberQueryService,
            IMembersQueryService membersQueryService,
            ICommonZoneService commonZoneService,
            IValidator<CustomerMobileUpdateInputDto> mobileUpdatevalidator,
            IValidator<CustomerUpdateInputDto> allDataValidator,
            IValidator<CustomerEstateUpdateDto> estateValidator,
            IValidator<CustomerTechnicalUpdateDto> technicalUpdateValidator,
            IValidator<CustomerBranchTypeUpdateInputDto> branchTypeUpdateValidator,
            IValidator<CustomerHouseholdUpdateInputDto> householdUpdateValidator,
            IConfiguration configuration)
            : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _customerQueryService = customerQueryService;
            _customerQueryService.NotNull(nameof(customerQueryService));

            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));

            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(commonZoneService));

            _membersQueryService = membersQueryService;
            _membersQueryService.NotNull(nameof(membersQueryService));

            _membersQueryService = membersQueryService;
            _membersQueryService.NotNull(nameof(membersQueryService));

            _allDataValidator = allDataValidator;
            _allDataValidator.NotNull(nameof(allDataValidator));

            _estateValidator = estateValidator;
            _estateValidator.NotNull(nameof(estateValidator));

            _technicalUpdateValidator = technicalUpdateValidator;
            _technicalUpdateValidator.NotNull(nameof(technicalUpdateValidator));

            _mobilevalidator = mobileUpdatevalidator;
            _mobilevalidator.NotNull(nameof(mobileUpdatevalidator));

            _branchTypeUpdateValidator = branchTypeUpdateValidator;
            _branchTypeUpdateValidator.NotNull(nameof(branchTypeUpdateValidator));

            _householdUpdateValidator = householdUpdateValidator;
            _householdUpdateValidator.NotNull(nameof(householdUpdateValidator));
        }

        public async Task Handle(CustomerUpdateInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await InputValidate(inputDto, cancellationToken);
            MemberInfoGetDto memberInfo = await ValidateAndGetMemberInfo(appUser, inputDto.BillId);
            CustomerUpdateDto customerUpdate = GetCustomerUpdate(inputDto, memberInfo);
            string opLogText = string.Format(OpLogLiterals.CustomerFullUpdateOpLog, inputDto.BillId);

            await ExecSql(customerUpdate, appUser, opLogText);
        }
        public async Task Handle(CustomerEstateUpdateDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await InputValidate(inputDto, cancellationToken);
            inputDto.Operator = _operator;
            MemberInfoGetDto memberInfo = await ValidateAndGetMemberInfo(appUser, inputDto.BillId);
            string opLogText = string.Format(OpLogLiterals.CustomerEstateUpdateOpLog, inputDto.BillId);

            await ExecSql(inputDto, appUser, opLogText);
        }
        public async Task Handle(CustomerTechnicalUpdateDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await InputValidate(inputDto, cancellationToken);
            inputDto.Operator = _operator;
            MemberInfoGetDto memberInfo = await ValidateAndGetMemberInfo(appUser, inputDto.BillId);
            string opLogText = string.Format(OpLogLiterals.CustomerTechnicalUpdateOpLog, inputDto.BillId);

            await ExecSql(inputDto, appUser, opLogText);
        }
        public async Task Handle(CustomerMobileUpdateInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await InputValidate(inputDto, cancellationToken);
            MemberInfoGetDto memberInfo = await ValidateAndGetMemberInfo(appUser, inputDto.BillId);
            CustomerMobileUpdateDto updateDto = new(memberInfo.Id, memberInfo.ZoneId, memberInfo.CustomerNumber, memberInfo.BillId, inputDto.MobileNumber, _operator);
            string opLogText = string.Format(OpLogLiterals.CustomerMobileNumberUpdateOpLog, inputDto.BillId);

            await ExecSql(updateDto, appUser, opLogText);
        }
        public async Task Handle(CustomerBranchTypeUpdateInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await InputValidate(inputDto, cancellationToken);

            MemberInfoGetDto memberInfo = await ValidateAndGetMemberInfo(appUser, inputDto.BillId);
            if (memberInfo.UseStateId == (int)BranchTypeEnum.SakhtOSaz)
            {
                throw new InvalidCustomerCommandException(ExceptionLiterals.InvalidRepeatConstructionBranchType);
            }
            if (_allowedToSetConstructionType.Contains(memberInfo.UseStateId))
            {
                CustomerBranchTypeUpdateDto updateDto = new(memberInfo.Id, memberInfo.ZoneId, memberInfo.CustomerNumber, memberInfo.BillId, _constructionId);
                string opLogText = string.Format(OpLogLiterals.CustomerBranchTypeUpdateOpLog, inputDto.BillId);
                await ExecSql(updateDto, appUser, opLogText);
            }
            else
            {
                throw new InvalidCustomerCommandException(ExceptionLiterals.InvalidBranchTypeId);
            }
        }
        public async Task Handle(CustomerHouseholdUpdateInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await InputValidate(inputDto, cancellationToken);

            MemberInfoGetDto memberInfo = await ValidateAndGetMemberInfo(appUser, inputDto.BillId);
            CustomerHouseholdUpdateDto updateDto = new(memberInfo.Id, memberInfo.ZoneId, memberInfo.CustomerNumber, memberInfo.BillId, inputDto.HouseholdNumber, inputDto.HouseholdDateJalali);
            string opLogText = string.Format(OpLogLiterals.CustomerHouseholdUpdateOpLog, inputDto.BillId);
            await ExecSql(updateDto, appUser, opLogText);
        }


        private async Task ExecSql(CustomerUpdateDto updateDto, IAppUser appUser, string opLogText)
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
                    ArchMemCommandService archMemCommandService = new(connection, transaction);
                    MembersCommandService membersCommandService = new(connection, transaction);
                    ClientsCommandService clientCommandService = new(connection, transaction);
                    OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);
                    string dbName = GetDbName(updateDto.ZoneId);
                    //string insertToDbName = "Atlas";

                    int rowId = await archMemCommandService.Insert(updateDto, dbName, dbName);
                    await membersCommandService.Update(updateDto, dbName);
                    await clientCommandService.UpdateToDayJalali(zoneIdAndCustomer, updateDto.ToDayDateJalali);
                    await clientCommandService.InsertByArchMemId(rowId, dbName);
                    await opLogCommandService.Insert(opLogText, appUser);

                    transaction.Commit();
                }
            }
        }
        private async Task ExecSql(CustomerEstateUpdateDto updateDto, IAppUser appUser, string opLogText)
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
                    ArchMemCommandService archMemCommandService = new(connection, transaction);
                    MembersCommandService membersCommandService = new(connection, transaction);
                    ClientsCommandService clientCommandService = new(connection, transaction);
                    OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);
                    string dbName = GetDbName(updateDto.ZoneId);

                    int rowId = await archMemCommandService.Insert(updateDto, dbName, dbName);
                    await membersCommandService.Update(updateDto, dbName);
                    await clientCommandService.UpdateToDayJalali(zoneIdAndCustomer, _currentDateJalali);
                    await clientCommandService.InsertByArchMemId(rowId, dbName);
                    await opLogCommandService.Insert(opLogText, appUser);

                    transaction.Commit();
                }
            }
        }
        private async Task ExecSql(CustomerTechnicalUpdateDto updateDto, IAppUser appUser, string opLogText)
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
                    ArchMemCommandService archMemCommandService = new(connection, transaction);
                    MembersCommandService membersCommandService = new(connection, transaction);
                    ClientsCommandService clientCommandService = new(connection, transaction);
                    OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);
                    string dbName = GetDbName(updateDto.ZoneId);

                    int rowId = await archMemCommandService.Insert(updateDto, dbName, dbName);
                    await membersCommandService.Update(updateDto, dbName);
                    await clientCommandService.UpdateToDayJalali(zoneIdAndCustomer, _currentDateJalali);
                    await clientCommandService.InsertByArchMemId(rowId, dbName);
                    await opLogCommandService.Insert(opLogText, appUser);

                    transaction.Commit();
                }
            }
        }
        private async Task ExecSql(CustomerMobileUpdateDto updateDto, IAppUser appUser, string opLogText)
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
                    ArchMemCommandService archMemCommandService = new(connection, transaction);
                    MembersCommandService membersCommandService = new(connection, transaction);
                    ClientsCommandService clientCommandService = new(connection, transaction);
                    OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);
                    string dbName = GetDbName(updateDto.ZoneId);

                    int rowId = await archMemCommandService.Insert(updateDto, dbName);
                    await membersCommandService.Update(updateDto, dbName);
                    await clientCommandService.UpdateToDayJalali(zoneIdAndCustomer, _currentDateJalali);
                    await clientCommandService.InsertByArchMemId(rowId, dbName);
                    await opLogCommandService.Insert(opLogText, appUser);

                    transaction.Commit();
                }
            }
        }
        private async Task ExecSql(CustomerBranchTypeUpdateDto updateDto, IAppUser appUser, string opLogText)
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
                    ArchMemCommandService archMemCommandService = new(connection, transaction);
                    MembersCommandService membersCommandService = new(connection, transaction);
                    ClientsCommandService clientCommandService = new(connection, transaction);
                    OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);
                    string dbName = GetDbName(updateDto.ZoneId);

                    int rowId = await archMemCommandService.Insert(updateDto, dbName);
                    await membersCommandService.Update(updateDto, dbName);
                    await clientCommandService.UpdateToDayJalali(zoneIdAndCustomer, _currentDateJalali);
                    await clientCommandService.InsertByArchMemId(rowId, dbName);
                    await opLogCommandService.Insert(opLogText, appUser);

                    transaction.Commit();
                }
            }
        }
        private async Task ExecSql(CustomerHouseholdUpdateDto updateDto, IAppUser appUser, string opLogText)
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
                    ArchMemCommandService archMemCommandService = new(connection, transaction);
                    MembersCommandService membersCommandService = new(connection, transaction);
                    ClientsCommandService clientCommandService = new(connection, transaction);
                    OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);
                    string dbName = GetDbName(updateDto.ZoneId);

                    int rowId = await archMemCommandService.Insert(updateDto, dbName);
                    await membersCommandService.Update(updateDto, dbName);
                    await clientCommandService.UpdateToDayJalali(zoneIdAndCustomer, _currentDateJalali);
                    await clientCommandService.InsertByArchMemId(rowId, dbName);
                    await opLogCommandService.Insert(opLogText, appUser);

                    transaction.Commit();
                }
            }
        }
     
        private CustomerUpdateDto GetCustomerUpdate(CustomerUpdateInputDto inputDto, MemberInfoGetDto previousSubscription)
        {
            return new CustomerUpdateDto()
            {
                Id = inputDto.Id,
                CustomerNumber = previousSubscription.CustomerNumber,
                ZoneId = previousSubscription.ZoneId,
                BillId = previousSubscription.BillId,
                X = inputDto.X,
                Y = inputDto.Y,
                ReadingNumber = inputDto.ReadingNumber,
                FirstName = inputDto.FirstName,
                Surname = inputDto.Surname,
                Address = inputDto.Address,
                PostalCode = inputDto.PostalCode ?? string.Empty,
                Plaque = inputDto.Plaque,
                NationalCode = inputDto.NationalCode ?? string.Empty,
                PhoneNumber = inputDto.PhoneNumber,
                MobileNumber = inputDto.MobileNumber ?? string.Empty,
                FatherName = inputDto.FatherName,
                BranchTypeId = inputDto.BranchTypeId,
                UsageSellId = inputDto.UsageSellId,
                UsageConsumptionId = inputDto.UsageConsumptionId,
                EmptyUnit = inputDto.EmptyUnit,
                CommertialUnit = inputDto.CommertialUnit,
                DomesticUnit = inputDto.DomesticUnit,
                OtherUnit = inputDto.OtherUnit,
                HouseholdDateJalali = DateValidation(previousSubscription.HouseholdDateJalali, false),
                HouseholdNumber = previousSubscription.HouseholdNumber,
                MeterDiameterId = inputDto.MeterDiameterId,
                IsSpecial = inputDto.IsSpecial,
                ContractualCapacity = inputDto.ContractualCapacity,
                ImprovementCommertial = inputDto.ImprovementCommertial,
                ImprovementDomestic = inputDto.ImprovementDomestic,
                ImprovementOverall = inputDto.ImprovementOverall,
                Premises = inputDto.Premises,
                Operator = _operator,
                SewageInstallationDateJalali = previousSubscription.SiphonInstallationDateJalali,// DateValidation(inputDto.SewageInstallationDateJalali, false),
                SewageRequestDateJalali = previousSubscription.SiphonRequestDateJalali,// DateValidation(inputDto.SewageRequestDateJalali, false),
                MeterInstallationDateJalali = previousSubscription.MeterInstallationDateJalali,//DateValidation(inputDto.MeterInstallationDateJalali, false),
                MeterRequestDateJalali = previousSubscription.MeterRequestDateJalali,//DateValidation(inputDto.MeterRequestDateJalali, false),
                Siphon100 = inputDto.Siphon100,
                Siphon125 = inputDto.Siphon125,
                Siphon150 = inputDto.Siphon150,
                Siphon200 = inputDto.Siphon200,
                Siphon5 = inputDto.Siphon5,
                Siphon6 = inputDto.Siphon6,
                Siphon7 = inputDto.Siphon7,
                Siphon8 = inputDto.Siphon8,
                MainSiphon = inputDto.MainSiphon,
                DeletionStateId = inputDto.DeletionStateId,
                BodySerial = inputDto.BodySerial ?? string.Empty,
                CommonSiphon = inputDto.CommonSiphon,
                MeterRegisterDateJalali = previousSubscription.MeterInstalltionRegisterDateJalali,//DateValidation(inputDto.MeterRegisterDateJalali, false),
                SewageRegisterDateJalali = previousSubscription.SiphonInstalltionRegisterDateJalali,//DateValidation(inputDto.SewageRegisterDateJalali, false),
                GuildId = inputDto.GuildId
            };
        }
        private async Task<MemberInfoGetDto> ValidateAndGetMemberInfo(IAppUser appUser, string billId)
        {
            ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await _commonMemberQueryService.Get(billId);
            MemberInfoGetDto memberInfo = await _commonMemberQueryService.Get(zoneIdAndCustomerNumber);
            await _commonZoneService.IsUserInZone(appUser, memberInfo.ZoneId);

            return memberInfo;
        }
        private string DateValidation(string? inputDate, bool hasException)
        {
            if (hasException)
            {
                return string.IsNullOrWhiteSpace(inputDate) || inputDate.Trim().Length != 10 ?
                    throw new InvalidDateException(ExceptionLiterals.InvalidDate) :
                    inputDate.Trim();
            }
            return string.IsNullOrWhiteSpace(inputDate) ? string.Empty : inputDate.Trim();
        }
        private async Task InputValidate(CustomerMobileUpdateInputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _mobilevalidator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new BaseException(message);
            }
        }
        private async Task InputValidate(CustomerUpdateInputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _allDataValidator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new BaseException(message);
            }
        }
        private async Task InputValidate(CustomerBranchTypeUpdateInputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _branchTypeUpdateValidator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new BaseException(message);
            }
        }
        private async Task InputValidate(CustomerEstateUpdateDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _estateValidator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new BaseException(message);
            }
        }
        private async Task InputValidate(CustomerTechnicalUpdateDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _technicalUpdateValidator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new BaseException(message);
            }
        }
        private async Task InputValidate(CustomerHouseholdUpdateInputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _householdUpdateValidator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new BaseException(message);
            }
        }
    }
}