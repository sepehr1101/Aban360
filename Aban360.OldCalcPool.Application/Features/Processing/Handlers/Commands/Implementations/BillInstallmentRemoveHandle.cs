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
using Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Implementations;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Implementations
{
    internal sealed class BillInstallmentRemoveHandle : AbstractBaseConnection, IBillInstallmentRemoveHandle
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ICommonMemberQueryService _memberQueryService;
        private readonly ICommonZoneService _zoneService;
        private readonly IValidator<BillInstallmentRemoveInputDto> _validator;
        public BillInstallmentRemoveHandle(
            IHttpContextAccessor contextAccessor,
            ICommonMemberQueryService memberQueryService,
            ICommonZoneService zoneService,
            IValidator<BillInstallmentRemoveInputDto> validator,
            IConfiguration configuration)
               : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _memberQueryService = memberQueryService;
            _memberQueryService.NotNull(nameof(memberQueryService));

            _zoneService = zoneService;
            _zoneService.NotNull(nameof(zoneService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task Handle(BillInstallmentRemoveInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await InputValidate(inputDto, cancellationToken);

            ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await _memberQueryService.Get(inputDto.BillId);
            await _zoneService.IsUserInZone(appUser, zoneIdAndCustomerNumber.ZoneId);
            if (zoneIdAndCustomerNumber.CustomerNumber != inputDto.CustomerNumber)
            {
                throw new InvalidInstallmentException(ExceptionLiterals.InvalidCustomerNumber);
            }
            string logText = string.Format(OpLogLiterals.BillInstallmentRemoveOpLog, inputDto.BillId, inputDto.Id);
            await ExecSql(inputDto, appUser, logText);
        }
        private async Task ExecSql(BillInstallmentRemoveInputDto inputDto, IAppUser appUser, string logText)
        {
            string dbName = GetDbName(inputDto.ZoneId);
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

                    await ghestAbCommandService.Remove(inputDto, dbName);
                    await opLogCommandService.Insert(logText, appUser);

                    transaction.Commit();
                }
            }
        }
        private async Task InputValidate(BillInstallmentRemoveInputDto inputDto, CancellationToken cancellationToken)
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
