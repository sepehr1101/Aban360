using Aban360.Common.Db.Dapper;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Create.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Features.Auth.Commands.Implementations;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Create.Implementations
{
    internal sealed class AssessmentOffInsertHandler : AbstractBaseConnection, IAssessmentOffInsertHandler
    {
        private readonly IUserQueryService _userQueryService;
        public AssessmentOffInsertHandler(
            IUserQueryService userQueryService,
            IConfiguration configuration)
            : base(configuration)
        {
            _userQueryService = userQueryService;
            _userQueryService.NotNull(nameof(userQueryService));
        }

        public async Task Handle(AssessmentOffInsertInputDto inputDto, int userCode, CancellationToken cancellationToken)
        {
            //validation
            User? inserterInfo = await _userQueryService.Get(userCode.ToString());
            User? assessmentInfo = await _userQueryService.Get(inputDto.AssessmentCode.ToString());
            DateTime currentDate = DateTime.Now;

            AssessmentOffInsertDto assessmentOffIsertDto = new()
            {
                AssessmentCode = Convert.ToInt32(assessmentInfo.Username),
                AssessmentId = assessmentInfo.Id,
                AssessmentName = assessmentInfo.FullName,
                OffDateJalali = inputDto.OffDateJalali,
                InsertedByUserCode = Convert.ToInt32(inserterInfo.Username),
                InsertedByUserName = inserterInfo.FullName,
                InsertDateGregorian = currentDate,
                InsertDateJalali = currentDate.ToShortPersianDateString(),
                InsertTime = currentDate.ToString("HH:mm")
            };
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    ExaminerOffCommandService examinerOffCommandService = new(connection, transaction);
                    await examinerOffCommandService.Insert(assessmentOffIsertDto);
                    transaction.Commit();
                }
            }
        }
    }

}
