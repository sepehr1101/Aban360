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
    internal sealed class UsageGroup3InsertHandler : AbstractBaseConnection, IUsageGroup3InsertHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IValidator<UsageGroup3InsertDto> _validator;
        public UsageGroup3InsertHandler(
            IHttpContextAccessor contextAccessor,
            IValidator<UsageGroup3InsertDto> validator,
            IConfiguration configuration)
                : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }
        public async Task Handle(UsageGroup3InsertDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await Validate(inputDto, cancellationToken);
            await ExecSql(inputDto, appUser);
        }
        private async Task ExecSql(UsageGroup3InsertDto usageGroup3InsertDto, IAppUser appUser)
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

                    int removedEffectedRecord = await usageGroup3CommandService.RemoveByUsageGroup2(usageGroup3InsertDto.Group2Id, false);
                    int insertRecordCount = await usageGroup3CommandService.Insert(usageGroup3InsertDto);
                    string insertOpLogText = string.Format(OpLogLiterals.UsageGroup3InsertOpLog, usageGroup3InsertDto.Group2Id, insertRecordCount);
                    string removeOpLogText = string.Format(OpLogLiterals.UsageGroup3DeleteListOpLog, usageGroup3InsertDto.Group2Id, removedEffectedRecord);
                    await opLogCommandService.Insert(removeOpLogText, appUser);
                    await opLogCommandService.Insert(insertOpLogText, appUser);

                    transaction.Commit();
                }
            }
        }
        private async Task Validate(UsageGroup3InsertDto inputDto, CancellationToken cancellationToken)
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
