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
    internal sealed class MeterDuplicateChangeDetailQueryService : UseStateBase, IMeterDuplicateChangeDetailQueryService
    {
        public MeterDuplicateChangeDetailQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<MeterDuplicateChangeHeaderOutputDto, MeterDuplicateChangeDetailDataOutputDto>> GetInfo(MeterDuplicateChangeInputDto input)
        {
            string reportTitle = ReportLiterals.MeterDuplicateChangeDetail + (input.IsRegisterDate ? ReportLiterals.ByRegisterDate : ReportLiterals.ByChangeDate);
            string query = GetQuery();

            IEnumerable<MeterDuplicateChangeDetailDataOutputDto> data = await _sqlReportConnection.QueryAsync<MeterDuplicateChangeDetailDataOutputDto>(query, input);
            MeterDuplicateChangeHeaderOutputDto header = new MeterDuplicateChangeHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                Title = reportTitle,
                CustomerCount = data.Count(),
                MeterChangeCount = data.Sum(r => r.MeterChangeCount),
            };

            var result = new ReportOutput<MeterDuplicateChangeHeaderOutputDto, MeterDuplicateChangeDetailDataOutputDto>(reportTitle, header, data);

            return result;
        }

        private string GetQuery()
        {
            return @"Select 
                    	m.ZoneId,
                    	m.CustomerNumber,
                    	MAX(c.billId) BillId,
                    	(MAX(c.firstName)+' '+ MAX(c.SureName)) FullName,
                    	Count(1) MeterChangeCount,
                    	MAX(c.ZoneTitle) ZoneTitle,
                    	MAX(c.UsageTitle) UsageTitle,
                    	MAX(c.WaterDiameterTitle) MeterDiamterTitle	,
                    	MAX(c.BranchType) BranchTypeTitle
                    From CustomerWarehouse.dbo.MeterChange m
                    Join CustomerWarehouse.dbo.Clients c
                    	ON m.ZoneId=c.ZoneId AND m.CustomerNumber=c.CustomerNumber
                    Where 
                    	c.ToDayJalali IS NULL AND
                    	c.ZoneId IN @zoneIds AND
                    	c.UsageId IN @usageIds AND
                    	((@isRegisterDate=1 AND m.ChangeDateJalali BETWEEN @fromDateJalali AND @toDateJalali) OR
                    	(@isRegisterDate<>1 AND m.ChangeDateJalali BETWEEN @fromDateJalali AND @toDateJalali))
                    Group by m.ZoneId, m.CustomerNumber";
        }
    }
}
