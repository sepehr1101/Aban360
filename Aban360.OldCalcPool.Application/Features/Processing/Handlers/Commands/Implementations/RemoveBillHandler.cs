using Aban360.Common.Db.Dapper;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Implementations;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.Transactions;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Implementations
{
    internal sealed class RemoveBillHandler : AbstractBaseConnection, IRemoveBillHandler
    {
        private readonly ICustomerInfoQueryService _customerInfoQueryService;
        //private readonly IBedBesCommandService _bedBesCommandService;
        //private readonly IHBedBesCommanddService _hbedBesCommanddService;
        //private readonly IKasrHaService _kasrHaService;
        private readonly IBedBesQueryService _billQueryService;
        private readonly IVariabService _variabService;

        public RemoveBillHandler(
            ICustomerInfoQueryService customerInfoQueryService,
            //IBedBesCommandService bedBesCommandService,
            //IHBedBesCommanddService hbedBesCommanddService,
            //IKasrHaCommandService kasrHaService,
            IBedBesQueryService billQueryService,
            IConfiguration configuration,
            IVariabService variabService)
                : base(configuration)
        {
            _customerInfoQueryService = customerInfoQueryService;
            _customerInfoQueryService.NotNull(nameof(customerInfoQueryService));

            _billQueryService = billQueryService;
            _billQueryService.NotNull(nameof(billQueryService));

            _variabService = variabService;
            _variabService.NotNull(nameof(_variabService));
        }

        public async Task Handle(RemoveBillInputDto input, CancellationToken cancellationToken)
        {            
            RemoveBillDataInputDto removeBill = await GetRemoveBillInputDto(input);
            await _variabService.GetAndRenew(removeBill.ZoneId);
            removeBill.ToDayDateJalali = DateTime.Now.ToShortPersianDateString();

            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    BedBesCommandService bedBesCommandService = new BedBesCommandService(_sqlReportConnection, transaction);
                    KasrHaCommandService kasrHaCommandService = new KasrHaCommandService(_sqlReportConnection, transaction);
                    HBedBesCommanddService hbedBesCommandService = new HBedBesCommanddService(_sqlReportConnection, transaction);

                    await bedBesCommandService.Delete(removeBill.Id, removeBill.ZoneId);
                    await kasrHaCommandService.Delete(removeBill);
                    await hbedBesCommandService.Insert(removeBill);
                    transaction.Commit();
                }
            }
        }
        public async Task<RemoveBillDataInputDto> GetRemoveBillInputDto(RemoveBillInputDto input)
        {
            ZoneIdAndCustomerNumberOutputDto zoneIdAndCustomerNumber = await _customerInfoQueryService.GetZoneIdAndCustomerNumber(input.BillId);
            RemoveBillGetDto removebillGet = new(input.Id, zoneIdAndCustomerNumber.ZoneId, zoneIdAndCustomerNumber.CustomerNumber);
            RemoveBillDataInputDto result = await _billQueryService.GetToRemove(removebillGet);
            return result;
        }
    }
}