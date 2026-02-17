using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
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

namespace Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Implementations
{
    internal sealed class RemoveBillHandler : AbstractBaseConnection, IRemoveBillHandler
    {
        private readonly ICustomerInfoQueryService _customerInfoQueryService;
        private readonly IBedBesQueryService _billQueryService;
        private readonly IVariabService _variabService;
        private readonly IBedBesQueryService _bedBesQueryService;
        private readonly ITavizQueryService _tavizQueryService;

        public RemoveBillHandler(
            ICustomerInfoQueryService customerInfoQueryService,
            IBedBesQueryService billQueryService,
            IConfiguration configuration,
            IVariabService variabService,
            IBedBesQueryService bedBesQueryService,
            ITavizQueryService tavizQueryService)
                : base(configuration)
        {
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

        public async Task Handle(RemoveBillInputDto input, CancellationToken cancellationToken)
        {
            RemoveBillDataInputDto removeBill = await GetRemoveBillInputDto(input);
            if (!(await _variabService.IsOperationValid(removeBill.ZoneId, removeBill.RegisterDateJalali)))
            {
                throw new RemovedBillException(Exceptionliterals.InvalidRemoveBill_ClosedVariab);
            }
            removeBill.ToDayDateJalali = DateTime.Now.ToShortPersianDateString();
            RemoveBillDto removeBillDto = GetRemoveBillDto(removeBill);
            ContorUpdateDto controUpdate = await GetControUpdate(removeBill);
            var (zoneIdAndCustomerNumber_1, zoneIdAndCustomerNumber_2) = GetZoneIdAndCustomerNumber(removeBill);
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
                    MandeBedehiCommandService mandeBedehiCommandService = new(connection, transaction);
                    RemovedBillCommandService removedBillCommandService = new(connection,transaction);

                    await bedBesCommandService.Delete(removeBill.Id, removeBill.ZoneId);
                    if (removeBill.Discount > 0)
                    {
                        await kasrHaCommandService.Delete(removeBill);
                    }
                    await hbedBesCommandService.Insert(removeBill);
                    await billCommandService.Delete(removeBillDto);
                    await membersCommandService.UpdateBedbes(zoneIdAndCustomerNumber_2, amount, dbName);
                    await contorCommandService.Update(controUpdate, dbName, false);
                    await mandeBedehiCommandService.UpdateAmount(zoneIdAndCustomerNumber_1, amount, dbName);
                    await removedBillCommandService.Insert(zoneIdAndCustomerNumber_1, removeBill.Barge, dbName);


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
            };
        }
        private async Task<BedBesWithConsumptionOutputDto> GetPreviousBedBesData(RemoveBillDataInputDto input)
        {
            ZoneIdAndCustomerNumberOutputDto zoneIdAndCustomerNumber = new(input.ZoneId, input.CustomerNumber);
            BedBesWithConsumptionOutputDto previousBill = await _bedBesQueryService.GetPrevious(zoneIdAndCustomerNumber, input.PreviousDateJalali);
            return previousBill;
        }
        private (ZoneIdAndCustomerNumberOutputDto, ZoneIdCustomerNumber) GetZoneIdAndCustomerNumber(RemoveBillDataInputDto input)
        {
            ZoneIdAndCustomerNumberOutputDto result_1 = new ZoneIdAndCustomerNumberOutputDto(input.ZoneId, input.CustomerNumber);
            ZoneIdCustomerNumber result_2 = new ZoneIdCustomerNumber(input.ZoneId, input.CustomerNumber.ToString());
            return (result_1, result_2);
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