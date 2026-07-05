using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
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
    internal sealed class ZaribCreateHandler : AbstractBaseConnection, IZaribCreateHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IT51QueryService _t51QueryService;
        private readonly IValidator<ZaribCreateInputDto> _validator;
        public ZaribCreateHandler(
            IHttpContextAccessor contextAccessor,
            IT51QueryService t51QueryService,
            IValidator<ZaribCreateInputDto> validator,
            IConfiguration configuration)
                : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _t51QueryService = t51QueryService;
            _t51QueryService.NotNull(nameof(t51QueryService));

            _validator = validator;
            _validator.NotNull(nameof(_validator));
        }
        public async Task Handle(ZaribCreateInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await InputValidate(inputDto, cancellationToken);
            ZaribCreateDto createDto = await GetCreateDto(inputDto);
            string opLogText = string.Format(OpLogLiterals.ZaribInserstOpLog, createDto.ZoneTitle1, createDto.Zarib_baha);
            await ExecSql(createDto, appUser, opLogText);
        }
        private async Task ExecSql(ZaribCreateDto createDto, IAppUser appUser, string opLogText)
        {
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    ZaribCommandService zaribCommandService = new(connection, transaction);
                    OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);

                    await zaribCommandService.Insert(createDto);
                    await opLogCommandService.Insert(opLogText, appUser);

                    transaction.Commit();
                }
            }
        }
        private async Task<ZaribCreateDto> GetCreateDto(ZaribCreateInputDto inputDto)
        {
            NumericDictionary zoneInfo = await _t51QueryService.Get(inputDto.ZoneId1, true);
            return new ZaribCreateDto()
            {
                Town = inputDto.Town,
                ZoneTitle1 = zoneInfo.Title,
                ZoneTitle2 = zoneInfo.Title,
                Zarib_baha = inputDto.Zarib_baha,
                Date1 = inputDto.Date1,
                Date2 = inputDto.Date2,
                Zb = inputDto.Zb,
                Zb1 = inputDto.Zb1,
                Zb2 = inputDto.Zb2,
                Zb3 = inputDto.Zb3,
                Zb4 = inputDto.Zb4,
                Zb5 = inputDto.Zb5,
                Zb6 = inputDto.Zb6,
                Zb7 = inputDto.Zb7,
                Zb8 = inputDto.Zb8,
                Zb_r = inputDto.Zb_r,
            };
        }
        private async Task InputValidate(ZaribCreateInputDto createDto, CancellationToken cancellationToken)
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
