using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Implementations;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Implementations
{
    internal sealed class UsageGroup1InsertHandler : AbstractBaseConnection, IUsageGroup1InsertHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUsageGroup1QueryService _usageGroup1QueryService;
        private readonly IValidator<UsageGroup1InsertDto> _validator;
        public UsageGroup1InsertHandler(
            IHttpContextAccessor contextAccessor,
            IUsageGroup1QueryService usageGroup1QueryService,
            IValidator<UsageGroup1InsertDto> validator,
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
        public async Task Handle(UsageGroup1InsertDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await Validate(inputDto, cancellationToken);
            UsageGroup1GetDto? duplicateData = await _usageGroup1QueryService.Get(inputDto.Title);
            if (duplicateData is not null)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidDuplicateUsageGroup);
            }

            string opLogText = string.Format(OpLogLiterals.UsageGroup1InsertOpLog, inputDto.Title);
            await ExecSql(inputDto, appUser, opLogText);
        }
        private async Task ExecSql(UsageGroup1InsertDto usageGroup1InsertDto, IAppUser appUser, string opLogText)
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

                    await usageGroup1CommandService.Insert(usageGroup1InsertDto);
                    await opLogCommandService.Insert(opLogText, appUser);

                    transaction.Commit();
                }
            }
        }
        private async Task Validate(UsageGroup1InsertDto inputDto, CancellationToken cancellationToken)
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
