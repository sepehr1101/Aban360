using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Domain.Features.Tagging;
using Aban360.ReportPool.Domain.Features.Tagging.Commands;
using Aban360.ReportPool.Persistence.Features.Tagging.Queries.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.Tagging.Queries.Implementations
{
    internal sealed class MainTagGroupService : AbstractBaseConnection, IMainTagGroupService
    {
        public MainTagGroupService(IConfiguration configuration)
                : base(configuration)
        {
        }

        public async Task Insert(MainTagGroupInsertDto input)
        {
            int effectedRecords = await _sqlReportConnection.ExecuteAsync(GetInsertCommand(), input);
            if (effectedRecords < 0)
            {
                throw new CustomValidationException(ExceptionLiterals.InvalidInsertMainTagGroup);
            }
        }
        public async Task Update(MainTagGroupUpdateDto input)
        {
            int effectedRecords = await _sqlReportConnection.ExecuteAsync(GetUpdateCommand(), input);
            if (effectedRecords < 0)
            {
                throw new CustomValidationException(ExceptionLiterals.InvalidInsertMainTagGroup);
            }
        }
        public async Task Remove(MainTagGroupRemoveDto input)
        {
            int effectedRecords = await _sqlReportConnection.ExecuteAsync(GetRemoveCommand(), input);
            if (effectedRecords < 0)
            {
                throw new CustomValidationException(ExceptionLiterals.InvalidInsertMainTagGroup);
            }

        }
        public async Task<IEnumerable<MainTagGroupGetDto>> GetValid()
        {
            IEnumerable<MainTagGroupGetDto> data = await _sqlReportConnection.QueryAsync<MainTagGroupGetDto>(GetValidQuery());
            return data;
        }
        public async Task<MainTagGroupGetDto> GetValid(int id)
        {
            MainTagGroupGetDto data = await _sqlReportConnection.QueryFirstOrDefaultAsync<MainTagGroupGetDto>(GetValidByIdQuery(), new { id });
            return data;
        }

        private string GetInsertCommand()
        {
            return @"Insert Into CustomerWarehouse.dbo.MainTagGroup(Title , CreateDateTime)
                    Values(@Title , @CreateDateTime)";
        }
        private string GetRemoveCommand()
        {
            return @"Update CustomerWarehouse.dbo.MainTagGroup
                    Set DeleteDateTime = @RemoveDateTime 
                    Where Id = @Id";
        }
        private string GetUpdateCommand()
        {
            return @"Update CustomerWarehouse.dbo.MainTagGroup
                    Set Title = @Title
                    Where Id = @Id";
        }
        private string GetValidQuery()
        {
            return @"Select 
                    	Id,
                    	Title,
                    	CreateDateTime
                    From CustomerWarehouse.dbo.MainTagGroup
                    Where DeleteDateTime IS NULL";
        }
        private string GetValidByIdQuery()
        {
            return @"Select 
                    	Id,
                    	Title,
                    	CreateDateTime
                    From CustomerWarehouse.dbo.MainTagGroup
                    Where 
                        Id = @Id AND
                        DeleteDateTime IS NULL";
        }
    }
}
