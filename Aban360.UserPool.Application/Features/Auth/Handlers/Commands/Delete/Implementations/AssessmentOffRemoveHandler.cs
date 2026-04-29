using Aban360.Common.Db.Dapper;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Delete.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Features.Auth.Commands.Implementations;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Delete.Implementations
{
    internal sealed class AssessmentOffRemoveHandler : AbstractBaseConnection, IAssessmentOffRemoveHandler
    {
        private readonly IUserQueryService _userQueryService;
        public AssessmentOffRemoveHandler(
            IUserQueryService userQueryService,
            IConfiguration configuration)
            : base(configuration)
        {
            _userQueryService = userQueryService;
            _userQueryService.NotNull(nameof(userQueryService));
        }

        public async Task Handle(Guid id, int userCode, CancellationToken cancellationToken)
        {
            User userInserterInfo = await _userQueryService.Get(userCode.ToString());

            DateTime currentDateTime = DateTime.Now;
            AssessmentOffRemoveDto assessmentOffRemoveDto = new()
            {
                Id= id,
                CancelDateTimeGregorian = currentDateTime,
                CancelTime = currentDateTime.ToString("HH:mm"),
                CancellerCode = userCode,
                CancellerName = userInserterInfo?.FullName??string.Empty,
            };
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    ExaminerOffCommandService examinerOffCommandService = new(connection, transaction);
                    await examinerOffCommandService.Remove(assessmentOffRemoveDto);
                    transaction.Commit();
                }
            }
        }
    }
}
