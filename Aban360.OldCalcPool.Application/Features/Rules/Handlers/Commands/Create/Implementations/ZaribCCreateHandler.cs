using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Create.Contracts;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;
using Aban360.OldCalcPool.Persistence.Features.Rules.Commands.Implementations;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Create.Implementations
{
    internal sealed class ZaribCCreateHandler : AbstractBaseConnection, IZaribCCreateHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IValidator<ZaribCCreateDto> _validator;
        public ZaribCCreateHandler(
            IHttpContextAccessor contextAccessor,
            IValidator<ZaribCCreateDto> validator,
            IConfiguration configuration)
                : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }
        public async Task Handle(ZaribCCreateDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await InputValidate(inputDto, cancellationToken);
            string opLogText = string.Format(OpLogLiterals.ZaribCInserstOpLog, inputDto.C, inputDto.FromDateJalali, inputDto.ToDateJalali);
            await ExecSql(inputDto, appUser, opLogText);
        }
        private async Task ExecSql(ZaribCCreateDto createDto, IAppUser appUser, string opLogText)
        {
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    ZaribCCommandService zaribCCommandService = new(connection, transaction);
                    OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);

                    await zaribCCommandService.Insert(createDto);
                    await opLogCommandService.Insert(opLogText, appUser);

                    transaction.Commit();
                }
            }
        }
        private async Task InputValidate(ZaribCCreateDto createDto, CancellationToken cancellationToken)
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
