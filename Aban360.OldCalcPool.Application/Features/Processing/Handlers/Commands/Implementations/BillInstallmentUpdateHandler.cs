using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Implementations;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using DNTPersianUtils.Core;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Implementations
{
    internal sealed class BillInstallmentUpdateHandler : AbstractBaseConnection, IBillInstallmentUpdateHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly ICommonZoneService _commonZoneService;
        private readonly IGhestAbQueryService _ghestAbQueryService;
        private readonly IValidator<BillInstallmentUpdateInputDto> _validator;
        public BillInstallmentUpdateHandler(
            IHttpContextAccessor contextAccessor,
            ICommonMemberQueryService commonMemberQueryService,
            ICommonZoneService commonZoneService,
            IGhestAbQueryService ghestAbQueryService,
            IValidator<BillInstallmentUpdateInputDto> validator,
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

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<BillInstallmentDataOutputDto> Handle(BillInstallmentUpdateInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await Validate(inputDto, appUser, cancellationToken);
            BillInstallmentOutputDto billInstallmentInfo = await _ghestAbQueryService.Get(zoneIdAndCustomerNumber, inputDto.Id);
            DateValidate(billInstallmentInfo);

            BillInstallmentUpdateDto updateDto = new(zoneIdAndCustomerNumber.ZoneId, zoneIdAndCustomerNumber.CustomerNumber, inputDto.Id, inputDto.DeadLineDateJalali, inputDto.Amount);
            string logText = string.Format(OpLogLiterals.BillInstallmentUpdateOpLog, inputDto.BillId, inputDto.Id, billInstallmentInfo.Payable, inputDto.Amount, billInstallmentInfo.DeadLineDateJalali, inputDto.DeadLineDateJalali);

            await SqlCommands(updateDto, appUser, logText);
            return GetResult(inputDto, billInstallmentInfo);
        }
        private async Task SqlCommands(BillInstallmentUpdateDto updateDto, IAppUser appUser,string logText)
        {
            string dbName = GetDbName(updateDto.ZoneId);
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    GhestAbCommandService ghestAbCommandService = new(connection, transaction);
                    OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);

                    await ghestAbCommandService.Update(updateDto, dbName);
                    await opLogCommandService.Insert(logText, appUser);

                    transaction.Commit();
                }
            }
        }
        private async Task<ZoneIdAndCustomerNumber> Validate(BillInstallmentUpdateInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await InputValidate(inputDto, cancellationToken);
            ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await _commonMemberQueryService.Get(inputDto.BillId);
            await _commonZoneService.IsUserInZone(appUser, zoneIdAndCustomerNumber.ZoneId);

            return zoneIdAndCustomerNumber;
        }
        private async Task InputValidate(BillInstallmentUpdateInputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
        private void DateValidate(BillInstallmentOutputDto billInstallmentInfo)
        {
            if (billInstallmentInfo.RegisterDateJalali.CompareTo(DateTime.Now.ToShortPersianDateString()) != 0)
            {
                throw new InvalidInstallmentException(ExceptionLiterals.InvalidUpdateInstallmentNotCurrentDate);
            }
        }
        private BillInstallmentDataOutputDto GetResult(BillInstallmentUpdateInputDto inputDto, BillInstallmentOutputDto billInstallmentInfo)
        {
            return new BillInstallmentDataOutputDto()
            {
                DeadLineDateJalali = inputDto.DeadLineDateJalali,
                Payable = inputDto.Amount,
                QueueNumber = billInstallmentInfo.QueueNumber,
                BillId = inputDto.BillId,
                PaymentId = TransactionIdGenerator.GeneratePaymentId(inputDto.Amount, inputDto.BillId, $"10{billInstallmentInfo.QueueNumber}"),
                QueueNumberTitle = $"قسط {billInstallmentInfo.QueueNumber.NumberToText(Language.Persian)}"
            };
        }
    }
}
