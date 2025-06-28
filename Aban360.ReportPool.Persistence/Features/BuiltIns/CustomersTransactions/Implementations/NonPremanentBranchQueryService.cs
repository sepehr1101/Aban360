using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Implementations
{
    internal sealed class NonPremanentBranchQueryService : AbstractBaseConnection, INonPremanentBranchQueryService
    {
        public NonPremanentBranchQueryService(IConfiguration configuration)
            : base(configuration)
        { }
        public async Task<ReportOutput<NonPremanentBranchHeaderOutputDto, NonPremanentBranchDataOutputDto>> GetInfo(NonPremanentBranchInputDto input)
        {
            string nonPremanentBranchQuery = GetNonPremanentBranchQuery();
            var @params = new
            {
                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,

                zoneIds = input.ZoneIds
            };

            IEnumerable<NonPremanentBranchDataOutputDto> nonPremanentBranchData = await _sqlReportConnection.QueryAsync<NonPremanentBranchDataOutputDto>(nonPremanentBranchQuery, @params);
            NonPremanentBranchHeaderOutputDto nonPremanentBranchHeader = new NonPremanentBranchHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                RecordCount = nonPremanentBranchData.Count(),
                ReportDate = DateTime.Now.ToShortPersianDateString()
            };

            var result = new ReportOutput<NonPremanentBranchHeaderOutputDto, NonPremanentBranchDataOutputDto>(ReportLiterals.NonPremanentBranch, nonPremanentBranchHeader, nonPremanentBranchData);

            return result;
        }

        private string GetNonPremanentBranchQuery()
        {
            return @"";
        }
    }
}
