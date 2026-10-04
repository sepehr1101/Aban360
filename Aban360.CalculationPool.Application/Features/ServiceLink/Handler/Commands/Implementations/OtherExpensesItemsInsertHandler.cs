using Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.CalculationPool.Persistence.Features.ServiceLink.Commands.Implementations;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Implementations
{
    internal sealed class OtherExpensesItemsInsertHandler : AbstractBaseConnection, IOtherExpensesItemsInsertHandler
    {
        private readonly IT100QueryService _t100QueryService;
        private readonly IT51QueryService _zoneQueryService;
        private readonly IT41QueryService _usageQueryService;
        private readonly IValidator<OtherExpensesItemsInsertInputDto> _validator;
        public OtherExpensesItemsInsertHandler(
            IT100QueryService t100QueryService,
            IT51QueryService zoneQueryService,
            IT41QueryService usageQueryService,
            IValidator<OtherExpensesItemsInsertInputDto> validator,
            IConfiguration configuration)
                : base(configuration)
        {
            _t100QueryService = t100QueryService;
            _t100QueryService.NotNull(nameof(t100QueryService));

            _zoneQueryService = zoneQueryService;
            _zoneQueryService.NotNull(nameof(zoneQueryService));

            _usageQueryService = usageQueryService;
            _usageQueryService.NotNull(nameof(usageQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task Handle(OtherExpensesItemsInsertInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await Validate(inputDto, cancellationToken);
            OtherExpensesItemsInsertDto insertDto = await GetInsertDto(inputDto, appUser);
            await ExecSql(insertDto);
        }
        private async Task ExecSql(OtherExpensesItemsInsertDto otherExpensesItmesInsertDto)
        {
            using (IDbConnection connection = _sqlConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    OtherExpensesItemsCommandService otherExpensesItmesCommandService = new(connection, transaction);
                    await otherExpensesItmesCommandService.Insert(otherExpensesItmesInsertDto);

                    transaction.Commit();
                }
            }
        }
        private async Task Validate(OtherExpensesItemsInsertInputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new BaseException(message);
            }
        }
        private async Task<OtherExpensesItemsInsertDto> GetInsertDto(OtherExpensesItemsInsertInputDto inputDto, IAppUser appUser)
        {
            return new OtherExpensesItemsInsertDto()
            {
                ZoneId = inputDto.ZoneId,
                ZoneTitle = (await _zoneQueryService.Get(inputDto.ZoneId, true)).Title,
                UsageId = inputDto.UsageId,
                UsageTitle = (await _usageQueryService.Get(inputDto.UsageId, true)).Title,
                ServiceId = inputDto.ServiceId,
                ServiceTitle = (await _t100QueryService.Get(inputDto.ServiceId, true)).Title,
                Amount = inputDto.Amount,
                InsertBy = appUser.UserId,
                InsertDateTime = DateTime.Now,
            };
        }
    }
}
