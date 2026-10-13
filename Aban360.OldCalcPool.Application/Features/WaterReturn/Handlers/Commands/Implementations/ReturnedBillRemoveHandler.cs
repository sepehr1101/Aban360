using Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Implementations;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Aban360.OldCalcPool.Persistence.Features.WaterReturn.Command.Implementations;
using Aban360.OldCalcPools.Domain.Features.WaterReturn.Dto.Commands;
using Aban360.OldCalcPools.Domain.Features.WaterReturn.Dto.Queries;
using Aban360.OldCalcPools.Persistence.Features.WaterReturn.Command.Implementations;
using Aban360.OldCalcPools.Persistence.Features.WaterReturn.Queries.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.InvoiceInfo.Dto;
using Aban360.ReportPool.Persistence.Features.WaterInvoice.Contracts;
using DNTPersianUtils.Core;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Commands.Implementations
{
    internal sealed class ReturnedBillRemoveHandler : AbstractBaseConnection, IReturnedBillRemoveHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IVariabService _variabService;
        private readonly IRepairQueryService _repairQueryService;
        private readonly IBedBesQueryService _bedBesQueryService;
        private readonly IBillQueryService _billQueryService;
        private readonly ICommonZoneService _commonZoneService;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly IValidator<WaterReturedRemoveInputDto> _validator;
        public ReturnedBillRemoveHandler(
            IHttpContextAccessor contextAccessor,
            IVariabService variabService,
            IRepairQueryService repairQueryService,
            IBedBesQueryService bedBesQueryService,
            IBillQueryService billQueryService,
            ICommonZoneService commonZoneService,
            ICommonMemberQueryService commonMemberQueryService,
            IValidator<WaterReturedRemoveInputDto> validator,
            IConfiguration configuration)
                : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _variabService = variabService;
            _variabService.NotNull(nameof(variabService));

            _repairQueryService = repairQueryService;
            _repairQueryService.NotNull(nameof(repairQueryService));

            _bedBesQueryService = bedBesQueryService;
            _bedBesQueryService.NotNull(nameof(bedBesQueryService));

            _billQueryService = billQueryService;
            _billQueryService.NotNull(nameof(billQueryService));

            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(commonZoneService));

            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(WaterReturedRemoveInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            ZoneIdAndCustomerNumber customerBasicInfo = new(inputDto.ZoneId, inputDto.CustomerNumber);
            string billId = await Validate(inputDto, customerBasicInfo, appUser, cancellationToken);
            RepairGetDto repairInfo = await _repairQueryService.Get(customerBasicInfo, inputDto.Id, true);
            bool isDateValid = await _variabService.IsOperationValid((int)repairInfo.Town, repairInfo.DateBed);
            if (!isDateValid)
            {
                throw new ReturnedBillException(ExceptionLiterals.InvalidRemoveReturnedAfterLockDate);
            }
            IEnumerable<BedBesCreateDto> bedBesInfoList = await _bedBesQueryService.Get(new ZoneCustomerFromToDateDto((int)repairInfo.Town, (int)repairInfo.Radif, repairInfo.PriDate, repairInfo.TodayDate));
            if (bedBesInfoList.Where(b => b.Del == false).Any())
            {
                throw new ReturnedBillException(ExceptionLiterals.InvalidReturnedBill);
            }
            HRepairByRepairIdInsertDto hRepairInsertDto = new(repairInfo.Id, (int)repairInfo.Town, (int)repairInfo.Radif, (int)repairInfo.JalaseNo);
            IEnumerable<BedBesUpdateDelDto> bedBesDelUpdateDto = bedBesInfoList.Select(b => new BedBesUpdateDelDto((int)b.Town, b.Id, false)).ToList();
            RepairByInfoDeleteDto repairDeleteDto = new(repairInfo.Id, (int)repairInfo.Town, (int)repairInfo.Radif);
            BillFindReturnedInputDto billFindReturnDto = new((int)repairInfo.Town, (int)repairInfo.Radif, (int)repairInfo.PriNo, (int)repairInfo.TodayNo, repairInfo.PriDate, repairInfo.TodayDate);
            IEnumerable<BillsOldDbDelUpdateDto> billsDelUpdateDto = await GetBillToUpdate(billFindReturnDto, bedBesDelUpdateDto?.Count() ?? 0);
            BillsOldDbDelUpdateDto repairInbillInfo = await _billQueryService.GetReturnedBill(billFindReturnDto, ReportLiterals.RepairTypeCode);
            BillRemoveByIdDto billRemoveByIdDto = new(repairInbillInfo.Id, repairInbillInfo.ZoneId, repairInbillInfo.CustomerNumber);

            await ExecSql(hRepairInsertDto, bedBesDelUpdateDto, repairDeleteDto, billsDelUpdateDto, billRemoveByIdDto, (long)repairInfo.Jam, billId, appUser);
        }
        private async Task ExecSql(HRepairByRepairIdInsertDto hRepairInsertDto, IEnumerable<BedBesUpdateDelDto> bedBesDelUpdateDto, RepairByInfoDeleteDto repairDeleteDto, IEnumerable<BillsOldDbDelUpdateDto> billsDelUpdateDto, BillRemoveByIdDto billRemoveByIdDto, long amount, string billId, IAppUser appUser)
        {
            string dbName = GetDbName(hRepairInsertDto.ZoneId);

            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    BedBesCommandService bedBesCommandService = new(connection, transaction);
                    BillCommandService billCommandService = new(connection, transaction);
                    RepairCommandService repairCommandService = new(connection, transaction);
                    HRepairCommandService hRepairCommandService = new(connection, transaction);
                    WaterDebtCommandService waterDebtCommandService = new(connection, transaction);
                    MembersCommandService membersCommandService = new(connection, transaction);
                    AutoBackCommandService autoBackCommandService = new(connection, transaction);
                    OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);

                    await bedBesCommandService.UpdateDel(bedBesDelUpdateDto, dbName);
                    int hRepairId = await hRepairCommandService.Insert(hRepairInsertDto, dbName);
                    await repairCommandService.Delete(repairDeleteDto, dbName);
                    await waterDebtCommandService.UpdateAmount(billId, amount * -1);
                    await membersCommandService.UpdateBedbes(new ZoneIdAndCustomerNumber(hRepairInsertDto.ZoneId, hRepairInsertDto.CustomerNumber), -1 * amount, DateTime.Now.ToShortPersianDateString(), dbName);
                    await billCommandService.UpdateOldDbDel(billsDelUpdateDto);
                    await billCommandService.Delete(billRemoveByIdDto);
                    await autoBackCommandService.UpdateIsDeleted(hRepairInsertDto.MinutesNumber, ReportLiterals.Atlas);
                    string opLogText = string.Format(OpLogLiterals.BillReturnedDeletedOpLog, hRepairInsertDto.Id, hRepairId);
                    await opLogCommandService.Insert(opLogText, appUser);

                    transaction.Commit();
                }
            }
        }
        private async Task<IEnumerable<BillsOldDbDelUpdateDto>> GetBillToUpdate(BillFindReturnedInputDto billFindRepairDto, int returnedBillsCount)
        {
            IEnumerable<BillsOldDbDelUpdateDto> billsInfoList = await _billQueryService.GetReturnedId(billFindRepairDto);
            if (billsInfoList.Where(b => b.OldDbDel == false).Any() || (billsInfoList?.Count() ?? 0) != returnedBillsCount)
            {
                throw new ReturnedBillException(ExceptionLiterals.InvalidReturnedBill);
            }
            foreach (var item in billsInfoList)
            {
                item.OldDbDel = false;
            }
            return billsInfoList;
        }
        public async Task<string> Validate(WaterReturedRemoveInputDto inputDto, ZoneIdAndCustomerNumber customerBasicInfo, IAppUser appUser, CancellationToken cancellationToken)
        {
            await InputValidte(inputDto, cancellationToken);
            await _commonZoneService.IsUserInZone(appUser, inputDto.ZoneId);
            MemberInfoGetDto memberInfo = await _commonMemberQueryService.Get(customerBasicInfo);
            if (memberInfo.BillId != inputDto.BillId)
            {
                throw new ReturnedBillException(ExceptionLiterals.InvalidBillId);
            }

            return memberInfo.BillId;
        }
        public async Task InputValidte(WaterReturedRemoveInputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
    }
}
