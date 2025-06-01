using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Implementations
{
    internal sealed class UseStateReportQueryService : AbstractBaseConnection, IUseStateReportQueryService
    {
        public UseStateReportQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<UseStateReportHeaderOutputDto, UseStateReportDataOutputDto>> GetInfo(UseStateReportInputDto input)
        {
            string useStateQueryString = GetUseStateDataQuery();
            IEnumerable<UseStateReportDataOutputDto> useStateData = await _sqlConnection.QueryAsync<UseStateReportDataOutputDto>(useStateQueryString, new { input.UseStateId, input.FromDate, input.ToDate });
            UseStateReportHeaderOutputDto useStateHeader = new UseStateReportHeaderOutputDto()
            { };

            string useStateQuery = GetUseStateTitle();
            string useStateTitle=await _sqlConnection.QueryFirstAsync<string>(useStateQuery,new {useStateId=input.UseStateId});
            var result = new ReportOutput<UseStateReportHeaderOutputDto, UseStateReportDataOutputDto>(ReportLiterals.Report+" "+useStateTitle , useStateHeader, useStateData);
            
            return result;
        }

        private string GetUseStateDataQuery()
        {
            return " ";
        }

        private string GetUseStateTitle()
        {
            return @"select Title
                     from ClaimPool.UseState 
                     where Id=@useStateId";
        }
    }
}
