using Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Persistence.Constants;
using Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Implementations;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using DNTPersianUtils.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using ReportPoolDomain = Aban360.ReportPool.Domain.Features.Transactions;
using System.Data;

namespace Aban360.OldCalcPool.Application.Features.Processing.Services;
public interface IBillRemovalService
{
    Task RemoveBillAsync(RemoveBillInputDto input, IAppUser appUser, CancellationToken cancellationToken);
    Task<RemoveBillDataInputDto> RemoveBillGetRemoveBillInputDto(RemoveBillInputDto input);
    Task<IEnumerable<BillsCanRemoveOutputDto>> BillsCanRemovedGetAsync(SearchInput inputDto, CancellationToken cancellationToken);
}

internal sealed class BillRemovalService : AbstractBaseConnection, IBillRemovalService
{
    public BillRemovalService(IHttpContextAccessor contextAccessor, ICustomerInfoQueryService customerInfoQueryService, IBedBesQueryService billQueryService, IConfiguration configuration, IVariabService variabService, ITavizQueryService tavizQueryService, ICommonMemberQueryService customerInfoService) : base(configuration)
    {
        _contextAccessor = contextAccessor;
        _contextAccessor.NotNull(nameof(contextAccessor));
        _customerInfoQueryService = customerInfoQueryService;
        _customerInfoQueryService.NotNull(nameof(customerInfoQueryService));
        _billQueryService = billQueryService;
        _billQueryService.NotNull(nameof(billQueryService));
        _variabService = variabService;
        _variabService.NotNull(nameof(_variabService));
        _bedBesQueryService = billQueryService;
        _bedBesQueryService.NotNull(nameof(billQueryService));
        _tavizQueryService = tavizQueryService;
        _tavizQueryService.NotNull(nameof(tavizQueryService));
        _customerInfoService = customerInfoService;
        _customerInfoService.NotNull(nameof(customerInfoService));
    }

    private readonly IHttpContextAccessor _contextAccessor;
    private readonly ICustomerInfoQueryService _customerInfoQueryService;
    private readonly IBedBesQueryService _billQueryService;
    private readonly IVariabService _variabService;
    private readonly IBedBesQueryService _bedBesQueryService;
    private readonly ITavizQueryService _tavizQueryService;
    private int _operator = 666;
    public async Task RemoveBillAsync(RemoveBillInputDto input, IAppUser appUser, CancellationToken cancellationToken)
    {
        RemoveBillDataInputDto removeBill = await RemoveBillGetRemoveBillInputDto(input);
        if (!(await _variabService.IsOperationValid(removeBill.ZoneId, removeBill.RegisterDateJalali)))
        {
            throw new RemovedBillException(Exceptionliterals.InvalidRemoveBill_ClosedVariab);
        }

        removeBill.ToDayDateJalali = DateTime.Now.ToShortPersianDateString();
        RemoveBillDto removeBillDto = RemoveBillGetRemoveBillDto(removeBill);
        ContorUpdateDto controUpdate = await RemoveBillGetControUpdate(removeBill);
        ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = RemoveBillGetZoneIdAndCustomerNumber(removeBill);
        string logText = string.Format(OpLogLiterals.RemoveBillOpLog, input.Id, input.BillId);
        await RemoveBillExecSql(removeBill, removeBillDto, controUpdate, zoneIdAndCustomerNumber, appUser, logText);
    }

    private async Task RemoveBillExecSql(RemoveBillDataInputDto removeBill, RemoveBillDto removeBillDto, ContorUpdateDto controUpdate, ZoneIdAndCustomerNumber zoneIdAndCustomerNumber, IAppUser appUser, string logText)
    {
        string dbName = GetDbName(removeBill.ZoneId);
        long amount = removeBill.Baha * -1;
        using (IDbConnection connection = _sqlReportConnection)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                BedBesCommandService bedBesCommandService = new(connection, transaction);
                KasrHaCommandService kasrHaCommandService = new(connection, transaction);
                HBedBesCommanddService hbedBesCommandService = new(connection, transaction);
                BillCommandService billCommandService = new(connection, transaction);
                MembersCommandService membersCommandService = new(connection, transaction);
                ContorCommandService contorCommandService = new(connection, transaction);
                WaterDebtCommandService waterDebtCommandService = new(connection, transaction);
                RemovedBillCommandService removedBillCommandService = new(connection, transaction);
                OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);
                await bedBesCommandService.Delete(removeBill.Id, removeBill.ZoneId, dbName);
                if (removeBill.Discount > 0)
                {
                    await kasrHaCommandService.Delete(removeBill, dbName);
                }

                await hbedBesCommandService.Insert(removeBill, dbName);
                await billCommandService.Delete(removeBillDto);
                await membersCommandService.UpdateBedbes(zoneIdAndCustomerNumber, amount, dbName);
                await contorCommandService.Update(controUpdate, dbName, false);
                await waterDebtCommandService.UpdateAmount(removeBill.BillId, amount);
                await removedBillCommandService.Insert(zoneIdAndCustomerNumber, removeBill.Barge, dbName);
                await opLogCommandService.Insert(logText, appUser);
                transaction.Commit();
            }
        }
    }

    private async Task<ContorUpdateDto> RemoveBillGetControUpdate(RemoveBillDataInputDto input)
    {
        BedBesWithConsumptionOutputDto previousBill = await RemoveBillGetPreviousBedBesData(input);
        return new ContorUpdateDto()
        {
            ZoneId = previousBill.ZoneId,
            CustomerNumber = previousBill.CustomerNumber,
            CurrentDateJalali = previousBill.CurrentDateJalali,
            CurrentNumber = previousBill.CurrentNumber,
            Consumption = previousBill.Consumption,
            ConsumptionAverage = previousBill.ConsumptionAverage,
            PreviousCounterState = 0
        };
    }

    private async Task<BedBesWithConsumptionOutputDto> RemoveBillGetPreviousBedBesData(RemoveBillDataInputDto input)
    {
        ZoneIdAndCustomerNumberOutputDto zoneIdAndCustomerNumber = new(input.ZoneId, input.CustomerNumber);
        BedBesWithConsumptionOutputDto previousBill = await _bedBesQueryService.GetPrevious(zoneIdAndCustomerNumber, input.PreviousDateJalali);
        return previousBill;
    }

    private ZoneIdAndCustomerNumber RemoveBillGetZoneIdAndCustomerNumber(RemoveBillDataInputDto input)
    {
        return new ZoneIdAndCustomerNumber()
        {
            ZoneId = input.ZoneId,
            CustomerNumber = input.CustomerNumber,
        };
    }

    private RemoveBillDto RemoveBillGetRemoveBillDto(RemoveBillDataInputDto input)
    {
        return new RemoveBillDto()
        {
            ZoneId = input.ZoneId,
            CustomerNumber = input.CustomerNumber,
            previousNumber = input.PreviousNumber,
            PreviousDateJalali = input.PreviousDateJalali,
            CurrentNumber = input.CurrentNumber,
            CurrentDateJalali = input.CurrentDateJalali,
        };
    }

    public async Task<RemoveBillDataInputDto> RemoveBillGetRemoveBillInputDto(RemoveBillInputDto input)
    {
        ReportPoolDomain.ZoneIdAndCustomerNumberOutputDto zoneIdAndCustomerNumber = await _customerInfoQueryService.GetZoneIdAndCustomerNumber(input.BillId);
        RemoveBillGetDto removebillGet = new(input.Id, zoneIdAndCustomerNumber.ZoneId, zoneIdAndCustomerNumber.CustomerNumber);
        RemoveBillDataInputDto result = await _billQueryService.GetToRemove(removebillGet);
        result.Operator = _operator;
        return result;
    }

    private readonly ICommonMemberQueryService _customerInfoService;
    public async Task<IEnumerable<BillsCanRemoveOutputDto>> BillsCanRemovedGetAsync(SearchInput inputDto, CancellationToken cancellationToken)
    {
        //validation
        ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await _customerInfoService.Get(inputDto.Input);
        RemovedBillSearchDto removedBillSearchDto = new(zoneIdAndCustomerNumber.ZoneId, zoneIdAndCustomerNumber.CustomerNumber);
        IEnumerable<BillsCanRemoveOutputDto> billsCanRemoved = await _billQueryService.GetToRemove(removedBillSearchDto);
        billsCanRemoved.Select(b => b.ZoneTitle = zoneIdAndCustomerNumber.ZoneTitle);
        return billsCanRemoved;
    }
}
