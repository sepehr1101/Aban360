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
    internal sealed class NerkhCreateHandler : AbstractBaseConnection, INerkhCreateHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IValidator<NerkhCreateDto> _nerkhCreateValidator;
        public NerkhCreateHandler(
            IHttpContextAccessor contextAccessor,
            IValidator<NerkhCreateDto> nerkhCreateValidator,
            IConfiguration configuration)
                : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _nerkhCreateValidator = nerkhCreateValidator;
            _nerkhCreateValidator.NotNull(nameof(_nerkhCreateValidator));
        }
        public async Task Handle(NerkhCreateDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await InputValidate(inputDto, cancellationToken);
            string opLogText = string.Format(OpLogLiterals.NerkhInserstOpLog, inputDto.Cod, inputDto.Vaj, inputDto.Date1, inputDto.Date2);
            await ExecSql(inputDto, appUser, opLogText);
        }
        private async Task ExecSql(NerkhCreateDto createDto, IAppUser appUser, string opLogText)
        {
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    NerkhCommandService NerkhCommandService = new(connection, transaction);
                    OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);

                    await NerkhCommandService.Insert(createDto);
                    await opLogCommandService.Insert(opLogText, appUser);

                    transaction.Commit();
                }
            }
        }
        private async Task InputValidate(NerkhCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _nerkhCreateValidator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
    }
}
