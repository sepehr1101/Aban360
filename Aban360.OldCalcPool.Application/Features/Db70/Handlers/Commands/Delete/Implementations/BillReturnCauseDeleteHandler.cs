using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Db70.Handlers.Commands.Delete.Contracts;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Commands;
using Aban360.OldCalcPool.Persistence.Features.Db70.Commands.Implementations;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.OldCalcPool.Application.Features.Db70.Handlers.Commands.Delete.Implementations
{
    internal sealed class BillReturnCauseDeleteHandler : AbstractBaseConnection, IBillReturnCauseDeleteHandler
    {
        private readonly IValidator<BillReturnCauseDeleteDto> _validator;
        private readonly IHttpContextAccessor _contextAccessor;
        public BillReturnCauseDeleteHandler(
            IHttpContextAccessor contextAccessor,
            IValidator<BillReturnCauseDeleteDto> validator,
            IConfiguration configuration)
                : base(configuration)
        {
            _validator = validator;
            _validator.NotNull(nameof(_validator));

            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(_contextAccessor));
        }
        public async Task Handle(BillReturnCauseDeleteDto deleteDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await InputValidate(deleteDto, cancellationToken);
            deleteDto.RemoveDateTime = DateTime.Now;
            deleteDto.RemoveByUserId = appUser.UserId;
            string opLogText = string.Format(OpLogLiterals.BillReturnCauseDeleteOpLog, deleteDto.Id);

            await ExecSql(deleteDto, appUser, opLogText);
        }
        private async Task ExecSql(BillReturnCauseDeleteDto deleteDto, IAppUser appUser, string opLogText)
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

                    await billReturnCauseCommandService.Delete(deleteDto);
                    await opLogCommandService.Insert(opLogText, appUser);

                    transaction.Commit();
                }
            }
        }
        private async Task InputValidate(BillReturnCauseDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(deleteDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
    }
}
