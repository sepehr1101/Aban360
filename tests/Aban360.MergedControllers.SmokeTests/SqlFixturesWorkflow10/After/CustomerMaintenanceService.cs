using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
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
using Aban360.OldCalcPools.Persistence.Features.WaterReturn.Queries.Contracts;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Validations;
using Aban360.ReportPool.Domain.Base;
using DNTPersianUtils.Core;
using FluentValidation;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Runtime.InteropServices;

namespace SqlAfterWorkflow10.Aban360.ClaimPool.Application.Features.Land.Services;
public interface ICustomerMaintenanceService
{
    Task UpdateAsync(CustomerUpdateInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    Task UpdateAsync(CustomerEstateUpdateDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    Task UpdateAsync(CustomerTechnicalUpdateDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    Task UpdateAsync(CustomerMobileUpdateInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    Task UpdateAsync(CustomerNecessaryInfoToUpdateInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    Task UpdateAsync(CustomerHouseholdUpdateInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    Task UpdateAsync(SubscriptionAssignmentInputUpdateDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    Task UpdateAsync(CustomerDeletionStateUpdateInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    Task UpdateAsync(MeterInstallationUpdateInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    Task UpdateAsync(CustomerBranchTypeUpdateInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    Task UpdateAsync(CustomerEmptyUnitUpdateInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    Task UpdateHouseholdAsync(HouseholdRegisterUpdateDto updateDto, CancellationToken cancellationToken);
    Task UpdateEmptyUnitAsync(EmptyUnitRegisterUpdateDto updateDto, CancellationToken cancellationToken);
    Task SetNormalBranchTypeAsync(BranchTypeToNormalUpdateInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
}

internal sealed class CustomerMaintenanceService : global::WorkflowRecordingConnectionBase, ICustomerMaintenanceService
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
    private readonly IValidator<CustomerNecessaryInfoToUpdateInputDto> _customerNecessaryInfoUpdateValidator;
    private readonly IValidator<CustomerHouseholdUpdateInputDto> _householdUpdateValidator;
    private readonly IValidator<SubscriptionAssignmentInputUpdateDto> _subscriptionAssignmentUpdateValidator;
    private readonly IValidator<MeterInstallationUpdateInputDto> _meterInstallationUpdateValidator;
    private readonly IValidator<CustomerBranchTypeUpdateInputDto> _customerBranchTypeUpdateValidator;
    private readonly IValidator<CustomerEmptyUnitUpdateInputDto> _customerEmptyUnitUpdateValidator;
    static int[] _allowedToSetConstructionType =
    {
        0,
        1
    };
    static int[] _domesticUsages =
    {
        (int)UsageEnum.Domestic,
        (int)UsageEnum.DomesticCommercial
    };
    static int[] _discountTypeIds =
    {
        (int)BranchTypeEnum.KomiteEmdad,
        (int)BranchTypeEnum.Behzisti
    };
    private string _currentDateJalali = DateTime.Now.ToShortPersianDateString();
    private int _constructionId = 4;
    private readonly IHouseholdRegisterCommandService _householdRegisterCommandService;
    private readonly IValidator<HouseholdRegisterUpdateDto> _validator;
    private readonly IEmptyUnitRegisterCommandService _emptyUnitRegisterCommandService;
    private readonly IValidator<EmptyUnitRegisterUpdateDto> _updateEmptyUnitValidator;
    public CustomerMaintenanceService(IHttpContextAccessor contextAccessor, ISubscriptionQueryService customerQueryService, ICommonMemberQueryService commonMemberQueryService, IMembersQueryService membersQueryService, ICommonZoneService commonZoneService, IValidator<CustomerMobileUpdateInputDto> mobileUpdatevalidator, IValidator<CustomerUpdateInputDto> allDataValidator, IValidator<CustomerEstateUpdateDto> estateValidator, IValidator<CustomerTechnicalUpdateDto> technicalUpdateValidator, IValidator<CustomerNecessaryInfoToUpdateInputDto> customerNecessaryInfoUpdateValidator, IValidator<CustomerHouseholdUpdateInputDto> householdUpdateValidator, IValidator<SubscriptionAssignmentInputUpdateDto> subscriptionAssignmentUpdateValidator, IValidator<MeterInstallationUpdateInputDto> meterInstallationUpdateValidator, IValidator<CustomerBranchTypeUpdateInputDto> customerBranchTypeUpdateValidator, IValidator<CustomerEmptyUnitUpdateInputDto> customerEmptyUnitUpdateValidator, IConfiguration configuration, IHouseholdRegisterCommandService householdRegisterCommandService, IValidator<HouseholdRegisterUpdateDto> validator, IEmptyUnitRegisterCommandService emptyUnitRegisterCommandService, IValidator<EmptyUnitRegisterUpdateDto> updateEmptyUnitValidator) : base(configuration)
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
        _allDataValidator = allDataValidator;
        _allDataValidator.NotNull(nameof(allDataValidator));
        _estateValidator = estateValidator;
        _estateValidator.NotNull(nameof(estateValidator));
        _technicalUpdateValidator = technicalUpdateValidator;
        _technicalUpdateValidator.NotNull(nameof(technicalUpdateValidator));
        _mobilevalidator = mobileUpdatevalidator;
        _mobilevalidator.NotNull(nameof(mobileUpdatevalidator));
        _customerNecessaryInfoUpdateValidator = customerNecessaryInfoUpdateValidator;
        _customerNecessaryInfoUpdateValidator.NotNull(nameof(customerNecessaryInfoUpdateValidator));
        _householdUpdateValidator = householdUpdateValidator;
        _householdUpdateValidator.NotNull(nameof(householdUpdateValidator));
        _subscriptionAssignmentUpdateValidator = subscriptionAssignmentUpdateValidator;
        _subscriptionAssignmentUpdateValidator.NotNull(nameof(subscriptionAssignmentUpdateValidator));
        _meterInstallationUpdateValidator = meterInstallationUpdateValidator;
        _meterInstallationUpdateValidator.NotNull(nameof(meterInstallationUpdateValidator));
        _customerBranchTypeUpdateValidator = customerBranchTypeUpdateValidator;
        _customerBranchTypeUpdateValidator.NotNull(nameof(customerBranchTypeUpdateValidator));
        _customerEmptyUnitUpdateValidator = customerEmptyUnitUpdateValidator;
        _customerEmptyUnitUpdateValidator.NotNull(nameof(customerEmptyUnitUpdateValidator));
        _householdRegisterCommandService = householdRegisterCommandService;
        _householdRegisterCommandService.NotNull(nameof(householdRegisterCommandService));
        _validator = validator;
        _validator.NotNull(nameof(validator));
        _emptyUnitRegisterCommandService = emptyUnitRegisterCommandService;
        _emptyUnitRegisterCommandService.NotNull(nameof(emptyUnitRegisterCommandService));
        _updateEmptyUnitValidator = updateEmptyUnitValidator;
        _updateEmptyUnitValidator.NotNull(nameof(updateEmptyUnitValidator));
    }

    public async Task UpdateAsync(CustomerUpdateInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        await UpdateInputValidate(inputDto, cancellationToken);
        MemberInfoGetDto memberInfo = await UpdateValidateAndGetMemberInfo(appUser, inputDto.BillId, inputDto.Id, inputDto.ReadingNumber);
        UpdateEmptyUnitValidate(inputDto.UsageSellId, inputDto.EmptyUnit);
        if (_discountTypeIds.Contains(inputDto.BranchTypeId))
        {
            UpdateDiscountValidate(inputDto.DomesticUnit, inputDto.CommertialUnit, inputDto.OtherUnit, inputDto.UsageSellId);
        }

        CustomerUpdateDto customerUpdate = UpdateGetCustomerUpdate(inputDto, memberInfo);
        string opLogText = string.Format(OpLogLiterals.CustomerFullUpdateOpLog, inputDto.BillId);
        await UpdateExecSql(customerUpdate, appUser, opLogText);
    }

    public async Task UpdateAsync(CustomerEstateUpdateDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        await UpdateInputValidate(inputDto, cancellationToken);
        MemberInfoGetDto memberInfo = await UpdateValidateAndGetMemberInfo(appUser, inputDto.BillId, inputDto.Id);
        UpdateEmptyUnitValidate(inputDto.UsageSellId, inputDto.EmptyUnit);
        string opLogText = string.Format(OpLogLiterals.CustomerEstateUpdateOpLog, inputDto.BillId);
        await UpdateExecSql(inputDto, appUser, opLogText);
    }

    public async Task UpdateAsync(CustomerTechnicalUpdateDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        await UpdateInputValidate(inputDto, cancellationToken);
        MemberInfoGetDto memberInfo = await UpdateValidateAndGetMemberInfo(appUser, inputDto.BillId, inputDto.Id);
        string opLogText = string.Format(OpLogLiterals.CustomerTechnicalUpdateOpLog, inputDto.BillId);
        await UpdateExecSql(inputDto, appUser, opLogText);
    }

    public async Task UpdateAsync(CustomerMobileUpdateInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        await UpdateInputValidate(inputDto, cancellationToken);
        MemberInfoGetDto memberInfo = await UpdateValidateAndGetMemberInfo(appUser, inputDto.BillId, inputDto.Id);
        CustomerMobileUpdateDto updateDto = new(inputDto.Id, inputDto.ZoneId, inputDto.CustomerNumber, inputDto.BillId, inputDto.MobileNumber, ReportLiterals.RayabOperator);
        string opLogText = string.Format(OpLogLiterals.CustomerMobileNumberUpdateOpLog, inputDto.BillId);
        await UpdateExecSql(updateDto, appUser, opLogText);
    }

    public async Task UpdateAsync(CustomerNecessaryInfoToUpdateInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        await UpdateInputValidate(inputDto, cancellationToken);
        MemberInfoGetDto memberInfo = await UpdateValidateAndGetMemberInfo(appUser, inputDto.BillId, inputDto.Id);
        if (memberInfo.UseStateId == (int)BranchTypeEnum.SakhtOSaz)
        {
            throw new InvalidCustomerCommandException(ExceptionLiterals.InvalidRepeatConstructionBranchType);
        }

        if (_allowedToSetConstructionType.Contains(memberInfo.UseStateId))
        {
            CustomerBranchTypeUpdateDto updateDto = new(inputDto.Id, inputDto.ZoneId, inputDto.CustomerNumber, inputDto.BillId, _constructionId);
            string opLogText = string.Format(OpLogLiterals.CustomerBranchTypeUpdateOpLog, inputDto.BillId);
            await UpdateExecSql(updateDto, appUser, opLogText);
        }
        else
        {
            throw new InvalidCustomerCommandException(ExceptionLiterals.InvalidBranchTypeId);
        }
    }

    public async Task UpdateAsync(CustomerHouseholdUpdateInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        await UpdateInputValidate(inputDto, cancellationToken);
        MemberInfoGetDto memberInfo = await UpdateValidateAndGetMemberInfo(appUser, inputDto.BillId, inputDto.Id);
        CustomerHouseholdUpdateDto updateDto = new(inputDto.Id, inputDto.ZoneId, inputDto.CustomerNumber, inputDto.BillId, inputDto.HouseholdNumber, inputDto.HouseholdDateJalali);
        string opLogText = string.Format(OpLogLiterals.CustomerHouseholdUpdateOpLog, inputDto.BillId);
        await UpdateExecSql(updateDto, appUser, opLogText);
    }

    public async Task UpdateAsync(SubscriptionAssignmentInputUpdateDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        await UpdateInputValidate(inputDto, cancellationToken);
        MemberInfoGetDto memberInfo = await UpdateValidateAndGetMemberInfo(appUser, inputDto.BillId, inputDto.Id, inputDto.ReadingNumber);
        SubscriptionAssignmentUpdateDto updateDto = new(inputDto.Id, inputDto.ZoneId, inputDto.CustomerNumber, inputDto.BillId, inputDto.ReadingNumber ?? string.Empty, inputDto.Address ?? string.Empty, inputDto.PostalCode ?? string.Empty);
        string opLogText = string.Format(OpLogLiterals.SubscriptionAssignmentUpdate, inputDto.BillId);
        await UpdateExecSql(updateDto, appUser, opLogText);
    }

    public async Task UpdateAsync(CustomerDeletionStateUpdateInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        MemberInfoGetDto memberInfo = await UpdateValidateAndGetMemberInfo(appUser, inputDto.BillId, inputDto.Id);
        if (memberInfo.DeletionStateId == (int)inputDto.DeletionStateType)
        {
            throw new InvalidCustomerCommandException(ExceptionLiterals.InvalidDuplicateDeletionState);
        }

        CustomerDeletionStateUpdateDto deletionStateUpdateDto = new(inputDto.Id, memberInfo.ZoneId, memberInfo.CustomerNumber, memberInfo.BillId, (int)inputDto.DeletionStateType);
        string opLogtText = string.Format(OpLogLiterals.CustomerBranchTypeUpdateOpLog, memberInfo.BillId);
        await UpdateExecSql(deletionStateUpdateDto, appUser, opLogtText);
    }

    public async Task UpdateAsync(MeterInstallationUpdateInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        await UpdateInputValidate(inputDto, cancellationToken);
        MemberInfoGetDto memberInfo = await UpdateValidateAndGetMemberInfo(appUser, inputDto.BillId, inputDto.Id);
        MeterInstallationUpdateDto updateDto = new(inputDto.Id, inputDto.ZoneId, inputDto.CustomerNumber, inputDto.BillId, inputDto.MeterInstallationDateJalali, inputDto.SiphonInstallationDateJalali);
        string opLogText = string.Format(OpLogLiterals.MeterInstallationUpdateOpLog, inputDto.BillId);
        await UpdateExecSql(updateDto, appUser, opLogText);
    }

    public async Task UpdateAsync(CustomerBranchTypeUpdateInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        await UpdateInputValidate(inputDto, cancellationToken);
        MemberInfoGetDto memberInfo = await UpdateValidateAndGetMemberInfo(appUser, inputDto.BillId, inputDto.Id);
        if (memberInfo.UseStateId == inputDto.BranchTypId)
        {
            throw new InvalidCustomerCommandException(ExceptionLiterals.InvalidDuplicateBranchType);
        }

        UpdateDiscountValidate(memberInfo.DomesticUnit, memberInfo.CommercialUnit, memberInfo.OtherUnit, memberInfo.UsageId);
        CustomerBranchTypeUpdateDto updateDto = new(inputDto.Id, inputDto.ZoneId, inputDto.CustomerNumber, inputDto.BillId, inputDto.BranchTypId);
        string opLogText = string.Format(OpLogLiterals.CustomerBranchTypeUpdateOpLog, inputDto.BillId);
        await UpdateExecSql(updateDto, appUser, opLogText);
    }

    public async Task UpdateAsync(CustomerEmptyUnitUpdateInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        await UpdateInputValidate(inputDto, cancellationToken);
        MemberInfoGetDto memberInfo = await UpdateValidateAndGetMemberInfo(appUser, inputDto.BillId, inputDto.Id);
        UpdateEmptyUnitValidate(memberInfo.UsageId, inputDto.EmptyUnit);
        CustomerEmptyUnitUpdateDto updateDto = new(inputDto.Id, inputDto.ZoneId, inputDto.CustomerNumber, inputDto.BillId, inputDto.EmptyUnit);
        string opLogText = string.Format(OpLogLiterals.CustomerEmptyUnitUpdateOpLog, inputDto.BillId);
        await UpdateExecSql(updateDto, appUser, opLogText);
    }

    private async Task UpdateExecSql(CustomerUpdateDto updateDto, IAppUser appUser, string opLogText)
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

    private async Task UpdateExecSql(CustomerEstateUpdateDto updateDto, IAppUser appUser, string opLogText)
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

    private async Task UpdateExecSql(CustomerTechnicalUpdateDto updateDto, IAppUser appUser, string opLogText)
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

    private async Task UpdateExecSql(CustomerMobileUpdateDto updateDto, IAppUser appUser, string opLogText)
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

    private async Task UpdateExecSql(CustomerBranchTypeUpdateDto updateDto, IAppUser appUser, string opLogText)
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

    private async Task UpdateExecSql(CustomerHouseholdUpdateDto updateDto, IAppUser appUser, string opLogText)
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

    private async Task UpdateExecSql(SubscriptionAssignmentUpdateDto updateDto, IAppUser appUser, string opLogText)
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

    private async Task UpdateExecSql(CustomerDeletionStateUpdateDto updateDto, IAppUser appUser, string opLogText)
    {
        ZoneIdAndCustomerNumber zoneIdAndCustomer = new(updateDto.ZoneId, updateDto.CustomerNumber);
        string dbName = GetDbName(updateDto.ZoneId);
        using (IDbConnection connection = _sqlReportConnection)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                MembersCommandService membersCommandService = new(connection, transaction);
                ArchMemCommandService archMemCommandService = new(connection, transaction);
                ClientsCommandService clientsCommandService = new(connection, transaction);
                OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);
                await membersCommandService.Update(updateDto, dbName);
                int archMemId = await archMemCommandService.Insert(updateDto, dbName, dbName);
                await clientsCommandService.UpdateToDayJalali(zoneIdAndCustomer, updateDto.ToDayDateJalali);
                await clientsCommandService.InsertByArchMemId(archMemId, dbName);
                await opLogCommandService.Insert(opLogText, appUser);
                transaction.Commit();
            }
        }
    }

    private async Task UpdateExecSql(MeterInstallationUpdateDto updateDto, IAppUser appUser, string opLogText)
    {
        ZoneIdAndCustomerNumber zoneIdAndCustomer = new(updateDto.ZoneId, updateDto.CustomerNumber);
        string dbName = GetDbName(updateDto.ZoneId);
        using (IDbConnection connection = _sqlReportConnection)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                MembersCommandService membersCommandService = new(connection, transaction);
                ArchMemCommandService archMemCommandService = new(connection, transaction);
                ClientsCommandService clientsCommandService = new(connection, transaction);
                OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);
                await membersCommandService.Update(updateDto, dbName);
                int archMemId = await archMemCommandService.Insert(updateDto, dbName);
                await clientsCommandService.UpdateToDayJalali(zoneIdAndCustomer, updateDto.ToDayDateJalali);
                await clientsCommandService.InsertByArchMemId(archMemId, dbName);
                await opLogCommandService.Insert(opLogText, appUser);
            //transaction.Commit();
            }
        }
    }

    private async Task UpdateExecSql(CustomerEmptyUnitUpdateDto updateDto, IAppUser appUser, string opLogText)
    {
        ZoneIdAndCustomerNumber zoneIdAndCustomer = new(updateDto.ZoneId, updateDto.CustomerNumber);
        string dbName = GetDbName(updateDto.ZoneId);
        using (IDbConnection connection = _sqlReportConnection)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                MembersCommandService membersCommandService = new(connection, transaction);
                ArchMemCommandService archMemCommandService = new(connection, transaction);
                ClientsCommandService clientsCommandService = new(connection, transaction);
                OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);
                await membersCommandService.Update(updateDto, dbName);
                int archMemId = await archMemCommandService.Insert(updateDto, dbName);
                await clientsCommandService.UpdateToDayJalali(zoneIdAndCustomer, updateDto.ToDayDateJalali);
                await clientsCommandService.InsertByArchMemId(archMemId, dbName);
                await opLogCommandService.Insert(opLogText, appUser);
                transaction.Commit();
            }
        }
    }

    private CustomerUpdateDto UpdateGetCustomerUpdate(CustomerUpdateInputDto inputDto, MemberInfoGetDto previousSubscription)
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
            HouseholdDateJalali = UpdateDateValidation(previousSubscription.HouseholdDateJalali, false),
            HouseholdNumber = previousSubscription.HouseholdNumber,
            MeterDiameterId = inputDto.MeterDiameterId,
            IsSpecial = inputDto.IsSpecial,
            ContractualCapacity = inputDto.ContractualCapacity,
            ImprovementCommertial = inputDto.ImprovementCommertial,
            ImprovementDomestic = inputDto.ImprovementDomestic,
            ImprovementOverall = inputDto.ImprovementOverall,
            Premises = inputDto.Premises,
            Operator = ReportLiterals.RayabOperator,
            SewageInstallationDateJalali = previousSubscription.SiphonInstallationDateJalali, // DateValidation(inputDto.SewageInstallationDateJalali, false),
            SewageRequestDateJalali = previousSubscription.SiphonRequestDateJalali, // DateValidation(inputDto.SewageRequestDateJalali, false),
            MeterInstallationDateJalali = previousSubscription.MeterInstallationDateJalali, //DateValidation(inputDto.MeterInstallationDateJalali, false),
            MeterRequestDateJalali = previousSubscription.MeterRequestDateJalali, //DateValidation(inputDto.MeterRequestDateJalali, false),
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
            MeterRegisterDateJalali = previousSubscription.MeterInstalltionRegisterDateJalali, //DateValidation(inputDto.MeterRegisterDateJalali, false),
            SewageRegisterDateJalali = previousSubscription.SiphonInstalltionRegisterDateJalali, //DateValidation(inputDto.SewageRegisterDateJalali, false),
            GuildId = inputDto.GuildId
        };
    }

    private async Task<MemberInfoGetDto> UpdateValidateAndGetMemberInfo(IAppUser appUser, string billId, int id, [Optional] string? readingNumber)
    {
        ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await _commonMemberQueryService.Get(billId);
        MemberInfoGetDto memberInfo = await _commonMemberQueryService.Get(zoneIdAndCustomerNumber);
        await _commonZoneService.IsUserInZone(appUser, memberInfo.ZoneId);
        if (memberInfo.Id != id)
        {
            throw new InvalidCustomerCommandException(ExceptionLiterals.InvalidId);
        }

        if (!string.IsNullOrWhiteSpace(readingNumber))
        {
            await UpdateDuplicateReadingNumberValidate(memberInfo.ZoneId, readingNumber, billId);
        }

        return memberInfo;
    }

    private async Task UpdateDuplicateReadingNumberValidate(int zoneId, string readingNumber, string billId)
    {
        IEnumerable<ZoneIdAndCustomerNumberAndBillId> readingNumbersInfo = await _commonMemberQueryService.GetFromClient(new ZoneIdsAndReadingNumber(new List<int> { zoneId }, readingNumber), false);
        if ((readingNumbersInfo?.Count() ?? 0) > 1)
        {
            throw new InvalidCustomerCommandException(ExceptionLiterals.InvalidDuplicateReadingNumber);
        }

        ZoneIdAndCustomerNumberAndBillId? firstCustomerInfo = readingNumbersInfo?.FirstOrDefault();
        if ((readingNumbersInfo?.Count() ?? 0) == 1 && (firstCustomerInfo?.BillId ?? string.Empty) != billId)
        {
            throw new InvalidCustomerCommandException(ExceptionLiterals.InvalidDuplicateReadingNumber);
        }
    }

    private string UpdateDateValidation(string? inputDate, bool hasException)
    {
        if (hasException)
        {
            return string.IsNullOrWhiteSpace(inputDate) || inputDate.Trim().Length != 10 ? throw new InvalidDateException(ExceptionLiterals.InvalidDate) : inputDate.Trim();
        }

        return string.IsNullOrWhiteSpace(inputDate) ? string.Empty : inputDate.Trim();
    }

    private void UpdateEmptyUnitValidate(int usageId, int emptyUnit)
    {
        if (!_domesticUsages.Contains(usageId) && emptyUnit > 0)
        {
            throw new InvalidCustomerCommandException(ExceptionLiterals.InvalidEmptyUnitForNonDomesticUnit);
        }
    }

    private void UpdateDiscountValidate(int domesticUnit, int commercialUnit, int otherUnit, int usageId)
    {
        if (domesticUnit != 1 || commercialUnit != 0 || otherUnit != 0)
        {
            throw new InvalidCustomerCommandException(ExceptionLiterals.InvalidDiscountUnits);
        }

        if (!_domesticUsages.Contains(usageId))
        {
            throw new InvalidCustomerCommandException(ExceptionLiterals.InvalidUsageInfo);
        }
    }

    private async Task UpdateInputValidate(CustomerMobileUpdateInputDto inputDto, CancellationToken cancellationToken)
    {
        var validationResult = await _mobilevalidator.ValidateAsync(inputDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new BaseException(message);
        }
    }

    private async Task UpdateInputValidate(CustomerUpdateInputDto inputDto, CancellationToken cancellationToken)
    {
        var validationResult = await _allDataValidator.ValidateAsync(inputDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new BaseException(message);
        }
    }

    private async Task UpdateInputValidate(CustomerNecessaryInfoToUpdateInputDto inputDto, CancellationToken cancellationToken)
    {
        var validationResult = await _customerNecessaryInfoUpdateValidator.ValidateAsync(inputDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new BaseException(message);
        }
    }

    private async Task UpdateInputValidate(CustomerEstateUpdateDto inputDto, CancellationToken cancellationToken)
    {
        var validationResult = await _estateValidator.ValidateAsync(inputDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new BaseException(message);
        }
    }

    private async Task UpdateInputValidate(CustomerTechnicalUpdateDto inputDto, CancellationToken cancellationToken)
    {
        var validationResult = await _technicalUpdateValidator.ValidateAsync(inputDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new BaseException(message);
        }
    }

    private async Task UpdateInputValidate(CustomerHouseholdUpdateInputDto inputDto, CancellationToken cancellationToken)
    {
        var validationResult = await _householdUpdateValidator.ValidateAsync(inputDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new BaseException(message);
        }
    }

    private async Task UpdateInputValidate(SubscriptionAssignmentInputUpdateDto inputDto, CancellationToken cancellationToken)
    {
        var validationResult = await _subscriptionAssignmentUpdateValidator.ValidateAsync(inputDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new BaseException(message);
        }
    }

    private async Task UpdateInputValidate(MeterInstallationUpdateInputDto inputDto, CancellationToken cancellationToken)
    {
        var validationResult = await _meterInstallationUpdateValidator.ValidateAsync(inputDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new BaseException(message);
        }
    }

    private async Task UpdateInputValidate(CustomerBranchTypeUpdateInputDto inputDto, CancellationToken cancellationToken)
    {
        var validationResult = await _customerBranchTypeUpdateValidator.ValidateAsync(inputDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new BaseException(message);
        }

        if (inputDto.BranchTypId != (int)BranchTypeEnum.Behzisti && inputDto.BranchTypId != (int)BranchTypeEnum.KomiteEmdad)
        {
            throw new InvalidCustomerCommandException(ExceptionLiterals.InvalidBranchTypeId);
        }
    }

    private async Task UpdateInputValidate(CustomerEmptyUnitUpdateInputDto inputDto, CancellationToken cancellationToken)
    {
        var validationResult = await _customerEmptyUnitUpdateValidator.ValidateAsync(inputDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new BaseException(message);
        }
    }

    public async Task UpdateHouseholdAsync(HouseholdRegisterUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        await _householdRegisterCommandService.Update(updateDto, DateTime.Now.ToShortPersianDateString());
    }

    public async Task UpdateEmptyUnitAsync(EmptyUnitRegisterUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateEmptyUnitValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        await _emptyUnitRegisterCommandService.Update(updateDto, DateTime.Now.ToShortPersianDateString());
    }

    public async Task SetNormalBranchTypeAsync(BranchTypeToNormalUpdateInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await _commonMemberQueryService.Get(inputDto.BillId);
        await _commonZoneService.IsUserInZone(appUser, zoneIdAndCustomerNumber.ZoneId);
        MemberInfoGetDto memberInfo = await _commonMemberQueryService.Get(zoneIdAndCustomerNumber);
        if (memberInfo.UseStateId == (int)BranchTypeEnum.Normal)
        {
            throw new InvalidCustomerCommandException(ExceptionLiterals.InvalidDuplicateBranchType);
        }

        CustomerBranchTypeUpdateDto branchTypeUpdateDto = new(memberInfo.Id, memberInfo.ZoneId, memberInfo.CustomerNumber, memberInfo.BillId, (int)BranchTypeEnum.Normal);
        string opLogtText = string.Format(OpLogLiterals.CustomerBranchTypeUpdateOpLog, memberInfo.BillId);
        await SetNormalBranchTypeExecSql(zoneIdAndCustomerNumber, branchTypeUpdateDto, appUser, opLogtText);
    }

    private async Task SetNormalBranchTypeExecSql(ZoneIdAndCustomerNumber zoneIdAndCustomerNumber, CustomerBranchTypeUpdateDto branchTypeUpdateDto, IAppUser appUser, string opLogText)
    {
        //string dbName = ReportLiterals.Atlas;
        string dbName = GetDbName(branchTypeUpdateDto.ZoneId);
        using (IDbConnection connection = _sqlReportConnection)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                MembersCommandService membersCommandService = new(connection, transaction);
                ArchMemCommandService archMemCommandService = new(connection, transaction);
                ClientsCommandService clientsCommandService = new(connection, transaction);
                OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);
                await membersCommandService.Update(branchTypeUpdateDto, dbName);
                int archMemId = await archMemCommandService.Insert(branchTypeUpdateDto, dbName, dbName);
                await clientsCommandService.UpdateToDayJalali(zoneIdAndCustomerNumber, branchTypeUpdateDto.ToDayDateJalali);
                await clientsCommandService.InsertByArchMemId(archMemId, dbName);
                await opLogCommandService.Insert(opLogText, appUser);
                transaction.Commit();
            }
        }
    }
}
