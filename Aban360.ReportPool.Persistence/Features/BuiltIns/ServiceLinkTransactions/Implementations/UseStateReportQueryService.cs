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
    internal sealed class UseStateReportQueryService : AbstractBaseConnection, IUseStateReportQueryService
    {
        public UseStateReportQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<UseStateReportHeaderOutputDto, UseStateReportDataOutputDto>> GetInfo(UseStateReportInputDto input)
        {
            string useStateQueryString = GetUseStateDataQuery();
            IEnumerable<UseStateReportDataOutputDto> useStateData = await _sqlConnection.QueryAsync<UseStateReportDataOutputDto>(useStateQueryString, new { useStateId=input.UseStateId, fromDate=input.FromDate, toDate=input.ToDate , zoneIds = input.ZoneIds });
            UseStateReportHeaderOutputDto useStateHeader = new UseStateReportHeaderOutputDto()
            { 
                TotalDeptAmount=useStateData.Sum(useState=>Convert.ToInt64(useState.DeptAmount)).ToString(),
                FromDateJalali=input.FromDate,
                ToDateJalali = input.ToDate,
                RecordCount=useStateData.   Count(),
            };

            string useStateQuery = GetUseStateTitle();
            string useStateTitle=await _sqlConnection.QueryFirstAsync<string>(useStateQuery,new {useStateId=input.UseStateId});
            var result = new ReportOutput<UseStateReportHeaderOutputDto, UseStateReportDataOutputDto>(ReportLiterals.Report+" "+useStateTitle , useStateHeader, useStateData);
            
            return result;
        }

        private string GetUseStateDataQuery()
        {
            return @"select 
                    	c.CustomerNumber,
                    	c.ReadingNumber,
                    	c.FirstName AS FirstName,
                    	c.SureName As Surname,
                    	c.UsageTitle,
                    	c.WaterDiameterTitle,
                    	c.RegisterDayJalali AS EventDateJalali,
                    	'0' AS DeptAmount,--c.DeptAmount
                    	c.Address AS Address,
                    	c.ZoneTitle,
                    	c.DeletionStateTitle AS EventDateJalali
                    from Client1000 c
                    where 
                       c.FromDayJalali>=@fromDate and
                       c.ToDayJalali<=@toDate and
                       c.DeletionStateId=@useStateId and
                       c.ZoneId in @zoneIds;";
        }

        private string GetUseStateTitle()
        {
            return @"select Title
                     from ClaimPool.UseState 
                     where Id=@useStateId";
        }
    }
}
