using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Application.Constant;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Implementations;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using DNTPersianUtils.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Reflection;

namespace Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Implementations
{
    internal sealed class BillInstallmentUpdateHandler : AbstractBaseConnection, IBillInstallmentUpdateHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly ICommonZoneService _commonZoneService;
        private readonly IGhestAbQueryService _ghestAbQueryService;
        public BillInstallmentUpdateHandler(
            IHttpContextAccessor contextAccessor,
            ICommonMemberQueryService commonMemberQueryService,
            ICommonZoneService commonZoneService,
            IGhestAbQueryService ghestAbQueryService,
            IConfiguration configuration)
            : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));

            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(CommonZoneService));

            _ghestAbQueryService = ghestAbQueryService;
            _ghestAbQueryService.NotNull(nameof(ghestAbQueryService));
        }

        public async Task<BillInstallmentDataOutputDto> Handle(BillInstallmentUpdateInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            //inputValidate
            ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await _commonMemberQueryService.Get(inputDto.BillId);
            await _commonZoneService.IsUserInZone(appUser, zoneIdAndCustomerNumber.ZoneId);
            BillInstallmentOutputDto billInstallmentInfo = await _ghestAbQueryService.Get(zoneIdAndCustomerNumber, inputDto.Id);
            if (billInstallmentInfo.RegisterDateJalali.CompareTo(DateTime.Now.ToShortPersianDateString()) != 0)
            {
                throw new InvalidInstallmentException(ExceptionLiterals.InvalidUpdateInstallmentNotCurrentDate);
            }

            BillInstallmentUpdateDto updateDto = new()
            {
                ZoneId = zoneIdAndCustomerNumber.ZoneId,
                CustomerNumber = zoneIdAndCustomerNumber.CustomerNumber,
                Amount = inputDto.Amount,
                DueDateJalali = inputDto.DueDateJalali,
                Id = inputDto.Id,
            };
            string logText = string.Format(Literals.BillInstallmentUpdateOpLog, inputDto.BillId, inputDto.Id, billInstallmentInfo.Payable, inputDto.Amount, billInstallmentInfo.DeadLineDateJalali, inputDto.DueDateJalali);

            string dbName = GetDbName(zoneIdAndCustomerNumber.ZoneId);
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    GhestAbCommandService ghestAbCommandService = new(connection, transaction);
                    OpLogCommandService opLogCommandService = new(_contextAccessor, connection, transaction);

                    await ghestAbCommandService.Update(updateDto, dbName);
                    await opLogCommandService.Insert(logText, appUser);

                    transaction.Commit();
                }
            }
            return new BillInstallmentDataOutputDto()
            {
                DeadLineDateJalali = inputDto.DueDateJalali,
                Payable = inputDto.Amount,
                QueueNumber = billInstallmentInfo.QueueNumber,
                BillId = inputDto.BillId,
                PaymentId = TransactionIdGenerator.GeneratePaymentId(inputDto.Amount, inputDto.BillId, $"10{billInstallmentInfo.QueueNumber}"),
                QueueNumberTitle = $"قسط {billInstallmentInfo.QueueNumber.NumberToText(Language.Persian)}"
            };
        }
    }
}
