using Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Implementations;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using ReportPoolDomain = Aban360.ReportPool.Domain.Features.Transactions;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;
using System.Data;
using Aban360.Common.Exceptions;
using Aban360.OldCalcPool.Persistence.Constants;
using Aban360.Common.BaseEntities;
using Microsoft.AspNetCore.Http;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Services;
using Aban360.OldCalcPool.Application.Constant;

namespace Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Implementations
{
    internal sealed class RemoveBillHandler : AbstractBaseConnection, IRemoveBillHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ICustomerInfoQueryService _customerInfoQueryService;
        private readonly IBedBesQueryService _billQueryService;
        private readonly IVariabService _variabService;
        private readonly IBedBesQueryService _bedBesQueryService;
        private readonly ITavizQueryService _tavizQueryService;

        public RemoveBillHandler(
            IHttpContextAccessor contextAccessor,
            ICustomerInfoQueryService customerInfoQueryService,
            IBedBesQueryService billQueryService,
            IConfiguration configuration,
            IVariabService variabService,
            IBedBesQueryService bedBesQueryService,
            ITavizQueryService tavizQueryService)
                : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _customerInfoQueryService = customerInfoQueryService;
            _customerInfoQueryService.NotNull(nameof(customerInfoQueryService));

            _billQueryService = billQueryService;
            _billQueryService.NotNull(nameof(billQueryService));

            _variabService = variabService;
            _variabService.NotNull(nameof(_variabService));

            _bedBesQueryService = bedBesQueryService;
            _bedBesQueryService.NotNull(nameof(bedBesQueryService));

            _tavizQueryService = tavizQueryService;
            _tavizQueryService.NotNull(nameof(tavizQueryService));
        }

        public async Task Handle(RemoveBillInputDto input, IAppUser appUser, CancellationToken cancellationToken)
        {
            RemoveBillDataInputDto removeBill = await GetRemoveBillInputDto(input);
            if (!(await _variabService.IsOperationValid(removeBill.ZoneId, removeBill.RegisterDateJalali)))
            {
                throw new RemovedBillException(Exceptionliterals.InvalidRemoveBill_ClosedVariab);
            }
            removeBill.ToDayDateJalali = DateTime.Now.ToShortPersianDateString();
            RemoveBillDto removeBillDto = GetRemoveBillDto(removeBill);
            ContorUpdateDto controUpdate = await GetControUpdate(removeBill);
            ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = GetZoneIdAndCustomerNumber(removeBill);
            string logText = string.Format(Literals.RemoveBillOpLog, input.Id, input.BillId);

            await SqlCommands(removeBill, removeBillDto, controUpdate, zoneIdAndCustomerNumber, appUser, logText);

        }
        private async Task SqlCommands(RemoveBillDataInputDto removeBill, RemoveBillDto removeBillDto, ContorUpdateDto controUpdate, ZoneIdAndCustomerNumber zoneIdAndCustomerNumber, IAppUser appUser, string logText)
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
                    OpLogCommandService opLogCommandService = new(_contextAccessor, connection, transaction);

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
        private async Task<ContorUpdateDto> GetControUpdate(RemoveBillDataInputDto input)
        {
            BedBesWithConsumptionOutputDto previousBill = await GetPreviousBedBesData(input);
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
        private async Task<BedBesWithConsumptionOutputDto> GetPreviousBedBesData(RemoveBillDataInputDto input)
        {
            ZoneIdAndCustomerNumberOutputDto zoneIdAndCustomerNumber = new(input.ZoneId, input.CustomerNumber);
            BedBesWithConsumptionOutputDto previousBill = await _bedBesQueryService.GetPrevious(zoneIdAndCustomerNumber, input.PreviousDateJalali);
            return previousBill;
        }
        private ZoneIdAndCustomerNumber GetZoneIdAndCustomerNumber(RemoveBillDataInputDto input)
        {
            return new ZoneIdAndCustomerNumber()
            {
                ZoneId = input.ZoneId,
                CustomerNumber = input.CustomerNumber,
            };
        }
        private RemoveBillDto GetRemoveBillDto(RemoveBillDataInputDto input)
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
        public async Task<RemoveBillDataInputDto> GetRemoveBillInputDto(RemoveBillInputDto input)
        {
            ReportPoolDomain.ZoneIdAndCustomerNumberOutputDto zoneIdAndCustomerNumber = await _customerInfoQueryService.GetZoneIdAndCustomerNumber(input.BillId);
            RemoveBillGetDto removebillGet = new(input.Id, zoneIdAndCustomerNumber.ZoneId, zoneIdAndCustomerNumber.CustomerNumber);
            RemoveBillDataInputDto result = await _billQueryService.GetToRemove(removebillGet);
            return result;
        }
    }
}