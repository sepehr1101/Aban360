using Aban360.Common.BaseEntities;
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
    internal sealed class MeterDuplicateChangeSummaryQueryService : UseStateBase, IMeterDuplicateChangeSummaryQueryService
    {
        public MeterDuplicateChangeSummaryQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<MeterDuplicateChangeHeaderOutputDto, MeterDuplicateChangeSummaryDataOutputDto>> GetInfo(MeterDuplicateChangeSummaryInputDto input)
        {
            string groupField = input.IsZoneGroup ? ReportLiterals.ZoneTitle : ReportLiterals.UsageTitle;
            string reportTitle = ReportLiterals.MeterDuplicateChangeSummary + '-' + (input.IsRegisterDate ? ReportLiterals.ByRegisterDate : ReportLiterals.ByChangeDate) + '-' + (input.IsZoneGroup ? ReportLiterals.ByZone : ReportLiterals.ByUsage);
            string query = GetQuery(groupField);

            IEnumerable<MeterDuplicateChangeSummaryDataOutputDto> data = await _sqlReportConnection.QueryAsync<MeterDuplicateChangeSummaryDataOutputDto>(query, input);
            MeterDuplicateChangeHeaderOutputDto header = new MeterDuplicateChangeHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                Title = reportTitle,
                CustomerCount = data.Sum(r => r.CustomerCount),
                RecordCount = data.Count(),
                MeterChangeCount = data.Sum(r => r.MeterChangeCount),
                FirstChange=data.Sum(r => r.FirstChange),
                SecondChange=data.Sum(r => r.SecondChange),
                MoreThanThirdChange=data.Sum(r=>r.MoreThanThirdChange),
            };

            var result = new ReportOutput<MeterDuplicateChangeHeaderOutputDto, MeterDuplicateChangeSummaryDataOutputDto>(reportTitle, header, data);

            return result;
        }

        private string GetQuery(string groupField)
        {
            return $@";With cte as(
                    	Select 
                    		m.ZoneId,
                    		m.CustomerNumber,
                    		MAX(c.billId) BillId,
                    		Count(1) MeterChangeCount,
                    		MAX(c.ZoneTitle) ZoneTitle,
                    		MAX(c.UsageTitle) UsageTitle
                    	From CustomerWarehouse.dbo.MeterChange m
                    	Join CustomerWarehouse.dbo.Clients c
                    		ON m.ZoneId=c.ZoneId AND m.CustomerNumber=c.CustomerNumber
                    	Where 
                    		c.ToDayJalali IS NULL AND
                            c.ZoneId IN @zoneIds AND
                            c.UsageId IN @usageIds AND
                            ((@isRegisterDate=1 AND m.ChangeDateJalali BETWEEN @fromDateJalali AND @toDateJalali) OR
                            (@isRegisterDate<>1 AND m.ChangeDateJalali BETWEEN @fromDateJalali AND @toDateJalali))
                    	Group by m.ZoneId, m.CustomerNumber
                    )
                    Select 
                    	{groupField},
                    	{groupField} ItemTitle,
                    	COUNT({groupField}) CustomerCount,
                    	SUM(MeterChangeCount) MeterChangeCount,
                    	COUNT(Case When MeterChangeCount=1 Then 1 Else null End)FirstChange,
                    	COUNT(Case When MeterChangeCount=2 Then 1 Else null End)SecondChange,
                    	COUNT(Case When MeterChangeCount>2 Then 1 Else null End)MoreThanThirdChange
                    From cte
                    Group By {groupField}";
        }
    }
}
