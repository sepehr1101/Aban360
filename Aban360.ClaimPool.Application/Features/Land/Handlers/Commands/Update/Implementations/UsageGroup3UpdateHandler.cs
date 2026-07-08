using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Implementations
{
    internal sealed class UsageGroup3UpdateHandler : AbstractBaseConnection, IUsageGroup3UpdateHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUsageGroup3QueryService _usageGroup3QueryService;
        private readonly IValidator<UsageGroup3UpdateDto> _validator;
        public UsageGroup3UpdateHandler(
            IHttpContextAccessor contextAccessor,
            IUsageGroup3QueryService usageGroup3QueryService,
            IValidator<UsageGroup3UpdateDto> validator,
            IConfiguration configuration)
                : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _usageGroup3QueryService = usageGroup3QueryService;
            _usageGroup3QueryService.NotNull(nameof(usageGroup3QueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }
        public async Task Handle(UsageGroup3UpdateDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await Validate(inputDto, cancellationToken);
            UsageGroup3GetDto previousInfo = await _usageGroup3QueryService.Get(inputDto.Id);
            string opLogText = string.Format(OpLogLiterals.UsageGroup3UpdateOpLog, previousInfo.UsageId, inputDto.UsageId, previousInfo.Group2Id, inputDto.Group2Id);

            await ExecSql(inputDto, appUser, opLogText);
        }
        private async Task ExecSql(UsageGroup3UpdateDto UsageGroup3UpdateDto, IAppUser appUser, string opLogText)
        {
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    UsageGroup3CommandService usageGroup3CommandService = new(connection, transaction);
                    OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);

                    await usageGroup3CommandService.Update(UsageGroup3UpdateDto);
                    await opLogCommandService.Insert(opLogText, appUser);

                    transaction.Commit();
                }
            }
        }
        private async Task Validate(UsageGroup3UpdateDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new BaseException(message);
            }
        }
    }
}
