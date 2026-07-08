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
    internal sealed class UsageGroup2UpdateHandler : AbstractBaseConnection, IUsageGroup2UpdateHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUsageGroup2QueryService _usageGroup2QueryService;
        private readonly IValidator<UsageGroup2UpdateDto> _validator;
        public UsageGroup2UpdateHandler(
            IHttpContextAccessor contextAccessor,
            IUsageGroup2QueryService usageGroup2QueryService,
            IValidator<UsageGroup2UpdateDto> validator,
            IConfiguration configuration)
                : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _usageGroup2QueryService = usageGroup2QueryService;
            _usageGroup2QueryService.NotNull(nameof(usageGroup2QueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }
        public async Task Handle(UsageGroup2UpdateDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await Validate(inputDto, cancellationToken);
            UsageGroup2GetDto previousInfo = await _usageGroup2QueryService.Get(inputDto.Id);
            string opLogText = string.Format(OpLogLiterals.UsageGroup2UpdateOpLog, previousInfo.Title, inputDto.Title, previousInfo.Group1Id, inputDto.Group1Id);

            await ExecSql(inputDto, appUser, opLogText);
        }
        private async Task ExecSql(UsageGroup2UpdateDto UsageGroup2UpdateDto, IAppUser appUser, string opLogText)
        {
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    UsageGroup2CommandService usageGroup2CommandService = new(connection, transaction);
                    OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);

                    await usageGroup2CommandService.Update(UsageGroup2UpdateDto);
                    await opLogCommandService.Insert(opLogText, appUser);

                    transaction.Commit();
                }
            }
        }
        private async Task Validate(UsageGroup2UpdateDto inputDto, CancellationToken cancellationToken)
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
