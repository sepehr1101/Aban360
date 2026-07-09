using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
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
using Aban360.Common.Literals;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Implementations
{
    internal sealed class UsageGroup2InsertHandler : AbstractBaseConnection, IUsageGroup2InsertHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUsageGroup2QueryService _usageGroup2QueryService;
        private readonly IValidator<UsageGroup2InsertDto> _validator;
        public UsageGroup2InsertHandler(
            IHttpContextAccessor contextAccessor,
            IUsageGroup2QueryService usageGroup2QueryService,
            IValidator<UsageGroup2InsertDto> validator,
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
        public async Task Handle(UsageGroup2InsertDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await Validate(inputDto, cancellationToken);
            UsageGroup2GetDto? duplicateData = await _usageGroup2QueryService.Get(inputDto.Title, inputDto.Group1Id);
            if (duplicateData is not null)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidDuplicateUsageGroup);
            }
            string opLogText = string.Format(OpLogLiterals.UsageGroup2InsertOpLog, inputDto.Title);
            await ExecSql(inputDto, appUser, opLogText);
        }
        private async Task ExecSql(UsageGroup2InsertDto usageGroup2InsertDto, IAppUser appUser, string opLogText)
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

                    await usageGroup2CommandService.Insert(usageGroup2InsertDto);
                    await opLogCommandService.Insert(opLogText, appUser);

                    transaction.Commit();
                }
            }
        }
        private async Task Validate(UsageGroup2InsertDto inputDto, CancellationToken cancellationToken)
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
