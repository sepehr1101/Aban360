using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Implementations
{
    internal sealed class MeterReplacementLifeDetailQueryService : AbstractBaseConnection, IMeterReplacementLifeDetailQueryService
    {
        public MeterReplacementLifeDetailQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<MeterReplacementLifeHeaderOutputDto, MeterReplacementLifeDataOutputDto>> Get(MeterReplacementLifeInputDto input)
        {
            string reportTitle = ReportLiterals.MeterReplacementLifeDetail;
            string query = GetQuery();

            IEnumerable<MeterReplacementLifeDataOutputDto> data = await _sqlReportConnection.QueryAsync<MeterReplacementLifeDataOutputDto>(query, input);
            MeterReplacementLifeHeaderOutputDto header = new()
            {
                FromChangeDateJalali = input.FromChangeDateJalali,
                ToChangeDateJalali = input.ToChangeDateJalali,

                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = data.Count(),
                CustomerCount = data.Count(),
                Title = reportTitle,
            };
            ReportOutput<MeterReplacementLifeHeaderOutputDto, MeterReplacementLifeDataOutputDto> result = new(reportTitle, header, data);
            return result;
        }

        private string GetQuery()
        {
            return $@";With ChangeMeter As(
                		Select 
                			mc.ZoneId,
                			mc.CustomerNumber,
                			MAX(mc.MeterNumber)MeterNumber,
                			MIN(mc.ChangeDateJalali)ChangeDateJalali,
                			MIN(mc.RegisterDateJalali)RegisterDateJalali,
                			MAX(mc.ChangeCauseTitle)ChangeCauseTitle,
                			MAx(DISTINCT mc.BodySerial) BodySerial
                		From CustomerWarehouse.dbo.MeterChange mc
                		Where 
                			mc.ZoneId IN (131211,13102) AND
                			mc.ChangeDateJalali BETWEEN @FromDateJalali AND @ToDateJalali AND
                			customerNumber in (3309,22263779,20589880)
                		Group By mc.zoneId, mc.Customernumber,TRIM(mc.bodySerial)
                    )
                    Select 
                    	c.BillId,
                    	c.ZoneTitle,
                    	c.ZoneId,
                    	c.CustomerNumber,
                    	(TRIM(c.firstName)+' '+ TRIM(c.SureName)) FullName,
                    	mc.ChangeDateJalali,
                    	c.PhysicalWaterInstallDateJalali,
                    	c.UsageId,
                    	c.UsageTitle,
                    	c.BranchType
                    From ChangeMeter mc
                    Join CustomerWarehouse.dbo.Clients c
                    	ON mc.ZoneId=c.ZoneId AND mc.CustomerNumber=c.CustomerNumber
                    Where c.ToDayJalali IS NULL
                    Order By c.BillId , mc.ChangeDateJalali ASC	";
        }
    }
}
