using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Update.Contracts;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Rules.Commands.Implementations;
using Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Update.Implementations
{
    internal sealed class AbonmanUpdateHandler : AbstractBaseConnection, IAbonmanUpdateHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IAbonmanQueryService _abonmanQueryService;
        private readonly IValidator<AbonmanUpdateDto> _validator;
        public AbonmanUpdateHandler(
            IHttpContextAccessor contextAccessor,
            IAbonmanQueryService abonmanQueryService,
            IValidator<AbonmanUpdateDto> validator,
            IConfiguration configuration)
                : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _abonmanQueryService = abonmanQueryService;
            _abonmanQueryService.NotNull(nameof(abonmanQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }
        public async Task Handle(AbonmanUpdateDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await InputValidate(inputDto, cancellationToken);
            AbonmanGetDto previousInfo = await _abonmanQueryService.Get(inputDto.Id);
            string opLogText = string.Format(OpLogLiterals.AbonmanUpdateOpLog, previousInfo.Code, inputDto.Code, previousInfo.Vaj, inputDto.Vaj, previousInfo.Date1, inputDto.Date1, previousInfo.Date2, inputDto.Date2);
            await ExecSql(inputDto, appUser, opLogText);
        }
        private async Task ExecSql(AbonmanUpdateDto UpdateDto, IAppUser appUser, string opLogText)
        {
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    AbonmanCommandService abonmanCommandService = new(connection, transaction);
                    OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);

                    await abonmanCommandService.Update(UpdateDto);
                    await opLogCommandService.Insert(opLogText, appUser);

                    transaction.Commit();
                }
            }
        }
        public async Task InputValidate(AbonmanUpdateDto UpdateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(UpdateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
    }
}
