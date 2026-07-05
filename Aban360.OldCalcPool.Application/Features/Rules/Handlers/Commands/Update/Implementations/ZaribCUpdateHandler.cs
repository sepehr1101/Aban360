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
    internal sealed class ZaribCUpdateHandler : AbstractBaseConnection, IZaribCUpdateHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IZaribCQueryService _zaribCQueryService;
        private readonly IValidator<ZaribCUpdateDto> _validator;
        public ZaribCUpdateHandler(
            IHttpContextAccessor contextAccessor,
            IZaribCQueryService zaribCQueryService,
            IValidator<ZaribCUpdateDto> validator,
            IConfiguration configuration)
                : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _zaribCQueryService = zaribCQueryService;
            _zaribCQueryService.NotNull(nameof(zaribCQueryService));

            _validator = validator;
            _validator.NotNull(nameof(_validator));
        }
        public async Task Handle(ZaribCUpdateDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await InputValidate(inputDto, cancellationToken);
            ZaribCQueryDto zaribCInfo = await _zaribCQueryService.Get(inputDto.Id);
            string opLogText = string.Format(OpLogLiterals.ZaribCUpdateOpLog, zaribCInfo.C, inputDto.C, zaribCInfo.FromDateJalali, inputDto.FromDateJalali, zaribCInfo.ToDateJalali, inputDto.ToDateJalali);
            await ExecSql(inputDto, appUser, opLogText);
        }
        private async Task ExecSql(ZaribCUpdateDto updateDto, IAppUser appUser, string opLogText)
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

                    await zaribCCommandService.Update(updateDto);
                    await opLogCommandService.Insert(opLogText, appUser);

                    transaction.Commit();
                }
            }
        }
        private async Task InputValidate(ZaribCUpdateDto UpdateDto, CancellationToken cancellationToken)
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
