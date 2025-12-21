using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Implementations
{
    internal sealed class ClosedSummaryQueryService : AbstractBaseConnection, IClosedSummaryQueryService
    {
        public ClosedSummaryQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<ClosedHeaderOutputDto, ClosedSummaryDataOutputDto>> Get(ClosedInputDto input)
        {
            string reportTitle = ReportLiterals.ClosedSummary;
            string groupField = input.IsZoneGrouped ? ReportLiterals.ZoneTitle : ReportLiterals.Usage;
            string query = GetQuery(groupField);
            IEnumerable<ClosedSummaryDataOutputDto> data = await _sqlReportConnection.QueryAsync<ClosedSummaryDataOutputDto>(query, input);
            ClosedHeaderOutputDto header = new()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                RecordCount = data.Count(),
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                Title = reportTitle,
                CustomerCount = data.Sum(x => x.CustomerCount),
            };

            ReportOutput<ClosedHeaderOutputDto, ClosedSummaryDataOutputDto> result = new(reportTitle, header, data);
            return result;
        }
        private string GetQuery(string groupField)
        {
            return @$";with Closed as
							(
								Select 
									ROW_NUMBER() OVER (PARTITION BY BillId ORDER BY RegisterDay DESC) AS rn,
									b.ZoneId,
									b.ZoneTitle,
									b.CustomerNumber,
									b.UsageId,
									b.UsageTitle,
									b.BillId,
									b.RegisterDay,
									b.CounterStateCode
								From CustomerWarehouse.dbo.Bills b
								Where	
									b.ZoneId IN @zoneIds AND
									b.UsageId IN @usageIds AND
									(b.RegisterDay BETWEEN @FromDateJalali AND @ToDateJalali) AND
									b.CounterStateCode IN (4)
							), StillClosed as
							(
								Select 
									c.ZoneId,
									c.ZoneTitle,
									c.CustomerNumber,
									c.UsageId,
									c.UsageTitle,
									c.BillId,
									c.RegisterDay,
									c.CounterStateCode	
								From Closed c
								Where
									c.rn=1 AND
									Not Exists
									(
										Select *
										From CustomerWarehouse.dbo.Bills b
										Where 
											b.BillId=c.BillId AND
											b.RegisterDay>c.RegisterDay AND b.RegisterDay<FORMAT(DATEADD(DAY,10,CustomerWarehouse.dbo.PersianToMiladi(c.RegisterDay)),'yyyy/MM/dd','fa-ir') AND
											b.CommercialCount NOT IN (4) 
											--رقم کنتور باید ثبت شده باشد -> لازم به شرط است؟
									)
							)
							Select 
								MAX(t46.C2) AS RegionTitle,
								c.{groupField} AS ItemTitle,
								c.{groupField},
								COUNT(c.{groupField}) AS CustomerCount
							From StillClosed c
							Join [Db70].dbo.T51 t51
								On t51.C0=c.ZoneId
							Join [Db70].dbo.T46 t46
								On t51.C1=t46.C0
							Group By c.{groupField}";
        }
    }
}