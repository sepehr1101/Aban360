using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Db70.Handlers.Commands.Create.Contracts;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Commands;
using Aban360.OldCalcPool.Persistence.Features.Db70.Commands.Contracts;
using Aban360.OldCalcPool.Persistence.Features.Db70.Commands.Implementations;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.OldCalcPool.Application.Features.Db70.Handlers.Commands.Create.Implementations
{
    internal sealed class BillReturnCauseCreateHandler : AbstractBaseConnection, IBillReturnCauseCreateHandler
    {
        private readonly IValidator<BillReturnCauseCreateDto> _validator;
        private readonly IHttpContextAccessor _contextAccessor;

        public BillReturnCauseCreateHandler(
            IValidator<BillReturnCauseCreateDto> validator,
            IHttpContextAccessor contextAccessor,
            IConfiguration configuration)
                : base(configuration)
        {
            _validator = validator;
            _validator.NotNull(nameof(_validator));

            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));
        }
        public async Task Handle(BillReturnCauseCreateDto createDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await InputValidate(createDto, cancellationToken);

            createDto.RegisterDateTime = DateTime.Now;
            createDto.RegisterByUserId = appUser.UserId;
            string opLogText = string.Format(OpLogLiterals.BillReturnCauseInsertOpLog, createDto.Code, createDto.Title, createDto.IsInList, createDto.IsLastMeterValid, createDto.IsPartial);

            await ExecSql(createDto, appUser, opLogText);
        }
        private async Task ExecSql(BillReturnCauseCreateDto createDto, IAppUser appUser, string opLogText)
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

                    await billReturnCauseCommandService.Create(createDto);
                    await opLogCommandService.Insert(opLogText, appUser);

                    transaction.Commit();
                }
            }
        }
        private async Task InputValidate(BillReturnCauseCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
    }
}
