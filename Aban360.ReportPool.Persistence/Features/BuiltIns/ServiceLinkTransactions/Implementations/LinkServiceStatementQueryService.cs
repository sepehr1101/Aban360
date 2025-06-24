using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Implementations
{
    internal sealed class LinkServiceStatementQueryService : AbstractBaseConnection, ILinkServiceStatementQueryService
    {
        public LinkServiceStatementQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<LinkServiceStatementHeaderOutputDto, LinkServiceStatementDataOutputDto>> GetInfo(LinkServiceStatementInputDto input)
        {
            string linkServiceStatementDataInfoQuery = GetLinkServiceStatementDataQuery();
            var @params = new 
            {
                FromDateJalali=input.FromDateJalali,
                ToDateJalali=input.ToDateJalali,
                ZoneIds=input.ZoneIds,
            };
            IEnumerable<LinkServiceStatementDataOutputDto> linkServiceStatementData = await _sqlConnection.QueryAsync<LinkServiceStatementDataOutputDto>(linkServiceStatementDataInfoQuery,@params);
            LinkServiceStatementHeaderOutputDto linkServiceStatementHeader = new LinkServiceStatementHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                RecordCount = linkServiceStatementData.Count(),
                ReportDate = DateTime.Now.ToShortPersianDateString()
            };

            var result = new ReportOutput<LinkServiceStatementHeaderOutputDto, LinkServiceStatementDataOutputDto>(ReportLiterals.LinkServiceStatement, linkServiceStatementHeader, linkServiceStatementData);
            return result;
        }

        private string GetLinkServiceStatementDataQuery()
        {
            return @"Select 
                    	SUM(r.FinalAmount) AS FinalAmount,
                    	SUM(r.Amount) AS Amount,
                    	SUM(r.OffAmount) AS OffAmount,
                    	MAX(r.TypeId) AS TypeTitle,
                    	MAX(r.ZoneTitle) AS ZoneTitle
                    From [InvoiceBranchConvert].dbo.RequestBillDetails r
                    Where 
                    	(r.RegisterDate BETWEEN @FromDateJalali AND @ToDateJalali) AND
                    	r.ZoneId In @ZoneIds
                    Group By 
                    	r.ZoneId,r.ItemId";
        }
    }
}