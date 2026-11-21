using Aban360.Common.Db.Dapper;
using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Features.Auth.Commands.Implementations;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.UserPool.Application.Features.Auth.Services;

public interface IAssessmentOffService
{
    Task CreateAsync(AssessmentOffInsertInputDto inputDto, int userCode, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, int userCode, CancellationToken cancellationToken);
    Task<IEnumerable<AssessmentOffGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<IEnumerable<AssessmentOffGetDto>> GetByAssessmentCodeAsync(int assessmentCode, CancellationToken cancellationToken);
}

internal sealed class AssessmentOffService : AbstractBaseConnection, IAssessmentOffService
{
    private readonly IUserQueryService _userQueryService;
    private readonly IExaminerOffQueryService _examinerOffQueryService;
    public AssessmentOffService(
        IUserQueryService userQueryService,
        IConfiguration configuration,
        IExaminerOffQueryService examinerOffQueryService) : base(configuration)
    {
        _userQueryService = userQueryService;
        _userQueryService.NotNull(nameof(userQueryService));
        _examinerOffQueryService = examinerOffQueryService;
        _examinerOffQueryService.NotNull(nameof(examinerOffQueryService));
    }

    public async Task CreateAsync(AssessmentOffInsertInputDto inputDto, int userCode, CancellationToken cancellationToken)
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

    public async Task DeleteAsync(Guid id, int userCode, CancellationToken cancellationToken)
    {
        User userInserterInfo = await _userQueryService.Get(userCode.ToString());
        DateTime currentDateTime = DateTime.Now;
        AssessmentOffRemoveDto assessmentOffRemoveDto = new()
        {
            Id = id,
            CancelDateTimeGregorian = currentDateTime,
            CancelTime = currentDateTime.ToString("HH:mm"),
            CancellerCode = userCode,
            CancellerName = userInserterInfo?.FullName ?? string.Empty,
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

    public async Task<IEnumerable<AssessmentOffGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        IEnumerable<AssessmentOffGetDto> data = await _examinerOffQueryService.Get();
        return data;
    }

    public async Task<IEnumerable<AssessmentOffGetDto>> GetByAssessmentCodeAsync(int assessmentCode, CancellationToken cancellationToken)
    {
        IEnumerable<AssessmentOffGetDto> data = await _examinerOffQueryService.Get(assessmentCode);
        return data;
    }
}
