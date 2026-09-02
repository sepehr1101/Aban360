using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Db70.Handlers.Commands.Update.Contracts;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Db70.Commands.Implementations;
using Aban360.OldCalcPool.Persistence.Features.Db70.Queries.Contracts;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.OldCalcPool.Application.Features.Db70.Handlers.Commands.Update.Implementations
{
    internal sealed class BillReturnCauseUpdateHandler : AbstractBaseConnection, IBillReturnCauseUpdateHandler
    {
        private readonly IBillReturnCauseQueryService _billReturnCauseQueryService;
        private readonly IValidator<BillReturnCauseUpdateDto> _validator;
        private readonly IHttpContextAccessor _contextAccessor;
        public BillReturnCauseUpdateHandler(
            IBillReturnCauseQueryService billReturnCauseQueryService,
            IValidator<BillReturnCauseUpdateDto> validator,
            IHttpContextAccessor contextAccessor,
            IConfiguration configuration)
                : base(configuration)
        {
            _billReturnCauseQueryService = billReturnCauseQueryService;
            _billReturnCauseQueryService.NotNull(nameof(_billReturnCauseQueryService));

            _validator = validator;
            _validator.NotNull(nameof(_validator));

            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(_contextAccessor));
        }
        public async Task Handle(BillReturnCauseUpdateDto updateDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await InputValidate(updateDto, cancellationToken);
            BillReturnCauseGetDto billReturnGetDto = await _billReturnCauseQueryService.Get(updateDto.Id);
            string opLogText = string.Format(OpLogLiterals.BillReturnCauseUpdateOpLog, updateDto.Id, billReturnGetDto.Code, updateDto.Code, billReturnGetDto.Title, updateDto.Title, billReturnGetDto.IsInList, updateDto.IsInList, billReturnGetDto.IsLastMeterValid, updateDto.IsLastMeterValid, billReturnGetDto.IsPartial, updateDto.IsPartial);

            await ExecSql(updateDto, appUser, opLogText);
        }
        private async Task ExecSql(BillReturnCauseUpdateDto updateDto, IAppUser appUser, string opLogText)
        {
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    BillReturnCauseCommandService billReturnCauseCommandService = new(connection, transaction);
                    OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);

                    await billReturnCauseCommandService.Update(updateDto);
                    await opLogCommandService.Insert(opLogText, appUser);

                    transaction.Commit();
                }
            }
        }
        private async Task InputValidate(BillReturnCauseUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
    }
}
