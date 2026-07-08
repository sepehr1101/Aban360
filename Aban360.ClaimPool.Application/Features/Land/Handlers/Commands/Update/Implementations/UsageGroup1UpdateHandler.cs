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
    internal sealed class UsageGroup1UpdateHandler : AbstractBaseConnection, IUsageGroup1UpdateHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUsageGroup1QueryService _usageGroup1QueryService;
        private readonly IValidator<UsageGroup1UpdateDto> _validator;
        public UsageGroup1UpdateHandler(
            IHttpContextAccessor contextAccessor,
            IUsageGroup1QueryService usageGroup1QueryService,
            IValidator<UsageGroup1UpdateDto> validator,
            IConfiguration configuration)
                : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _usageGroup1QueryService = usageGroup1QueryService;
            _usageGroup1QueryService.NotNull(nameof(usageGroup1QueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }
        public async Task Handle(UsageGroup1UpdateDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await Validate(inputDto, cancellationToken);
            UsageGroup1GetDto previousInfo = await _usageGroup1QueryService.Get(inputDto.Id);
            string opLogText = string.Format(OpLogLiterals.UsageGroup1UpdateOpLog, previousInfo.Title, inputDto.Title);

            await ExecSql(inputDto, appUser, opLogText);
        }
        private async Task ExecSql(UsageGroup1UpdateDto UsageGroup1UpdateDto, IAppUser appUser, string opLogText)
        {
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    UsageGroup1CommandService usageGroup1CommandService = new(connection, transaction);
                    OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);

                    await usageGroup1CommandService.Update(UsageGroup1UpdateDto);
                    await opLogCommandService.Insert(opLogText, appUser);

                    transaction.Commit();
                }
            }
        }
        private async Task Validate(UsageGroup1UpdateDto inputDto, CancellationToken cancellationToken)
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
