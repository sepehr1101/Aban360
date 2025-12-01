using Aban360.Common.Db.Dapper;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;
using Aban360.OldCalcPool.Persistence.Features.Rules.Commands.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.OldCalcPool.Persistence.Features.Rules.Commands.Implementations
{
    internal sealed class SCommandService : AbstractBaseConnection, ISCommandService
    {
        public SCommandService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task Update(SUpdateDto input)
        {
            string SUpdateQueryString = GetSUpdateQuery();
            var @params = new
            {
                id = input.Id,
                town = input.ZoneId,
                input.Olgo,
                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali
            };
            await _sqlReportConnection.ExecuteAsync(SUpdateQueryString, @params);

        }
        public async Task Create(SCreateDto input)
        {
            int latestId = await GetLatestId();
            
            string SCreateQueryString = GetSCreateQuery();
            var @params = new
            {
                id=latestId+1,
                town = input.ZoneId,
                input.Olgo,
                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali
            };
            await _sqlReportConnection.ExecuteAsync(SCreateQueryString, @params);
        }

        public async Task Delete(int id)
        {
            string SDeleteQueryString = GetSDeleteQuery();
            await _sqlReportConnection.ExecuteAsync(SDeleteQueryString, new { id });
        }
        private async Task<int> GetLatestId()
        {
            string query = GetLatestIdQuery();
            int latestId=await _sqlReportConnection.QueryFirstOrDefaultAsync<int>(query,null);

            return latestId;
        }

        private string GetSUpdateQuery()
        {
            return @"Update OldCalc.dbo.S
                    Set town=@town , olgo=@olgo , FromDate=@FromDate , ToDate=@FromDate
                    Where Id=@Id";
        }
        private string GetSCreateQuery()
        {
            return @"Insert Into OldCalc.dbo.S(id,town,olgo,FromDate,ToDate)
                    Values(@id,@town,@olgo,@FromDate,@ToDate)
";
        }
        private string GetSDeleteQuery()
        {
            return @"Delete from OldCalc.dbo.s
                    Where Id=@Id";
        }
        private string GetLatestIdQuery()
        {
            return @"Select top 1 Id
                    From OldCalc.dbo.S 
                    Order By Id Desc";
        }
    }
}
