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
    internal sealed class NonPermanentBranchQueryService : AbstractBaseConnection, INonPermanentBranchQueryService
    {
        public NonPermanentBranchQueryService(IConfiguration configuration)
            : base(configuration)
        { }
        public async Task<ReportOutput<NonPermanentBranchHeaderOutputDto, NonPermanentBranchDataOutputDto>> GetInfo(NonPermanentBranchInputDto input)
        {
            string nonPremanentBranchQuery = GetNonPermanentBranchQuery();
            var @params = new
            {
                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,

                zoneIds = input.ZoneIds
            };

            IEnumerable<NonPermanentBranchDataOutputDto> nonPremanentBranchData = await _sqlReportConnection.QueryAsync<NonPermanentBranchDataOutputDto>(nonPremanentBranchQuery, @params);
            NonPermanentBranchHeaderOutputDto nonPremanentBranchHeader = new NonPermanentBranchHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                RecordCount = nonPremanentBranchData.Count(),
                ReportDate = DateTime.Now.ToShortPersianDateString()
            };

            var result = new ReportOutput<NonPermanentBranchHeaderOutputDto, NonPermanentBranchDataOutputDto>(ReportLiterals.NonPermanentBranch, nonPremanentBranchHeader, nonPremanentBranchData);

            return result;
        }

        private string GetNonPermanentBranchQuery()
        {
            return @"";
        }
    }
}
