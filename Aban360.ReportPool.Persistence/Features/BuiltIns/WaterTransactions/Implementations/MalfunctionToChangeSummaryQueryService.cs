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
    internal sealed class MalfunctionToChangeSummaryQueryService : AbstractBaseConnection, IMalfunctionToChangeSummaryQueryService
    {
        public MalfunctionToChangeSummaryQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<MalfunctionToChangeHeaderOutputDto, MalfunctionToChangeSummaryDataOutputFromDataBaseDto>> Get(MalfunctionToChangeInputDto input)
        {
            string MalfunctionQueryString = GetMalfunctionToChangeQuery();
            var @params = new
            {
                fromDateJalali = input.FromDateJalali,
                toDateJalali = input.ToDateJalali,
            };
            IEnumerable<MalfunctionToChangeSummaryDataOutputFromDataBaseDto> MalfunctionData = await _sqlReportConnection.QueryAsync<MalfunctionToChangeSummaryDataOutputFromDataBaseDto>(MalfunctionQueryString, @params);
            MalfunctionToChangeHeaderOutputDto MalfunctionHeader = new MalfunctionToChangeHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = (MalfunctionData is not null && MalfunctionData.Any()) ? MalfunctionData.Count() : 0,
                CustomerCount = (MalfunctionData is not null && MalfunctionData.Any()) ? MalfunctionData.Count() : 0,
            };

            ReportOutput<MalfunctionToChangeHeaderOutputDto, MalfunctionToChangeSummaryDataOutputFromDataBaseDto> result = new ReportOutput<MalfunctionToChangeHeaderOutputDto, MalfunctionToChangeSummaryDataOutputFromDataBaseDto>(ReportLiterals.MalfunctionToChangeSummary, MalfunctionHeader, MalfunctionData);
            return result;
        }

        private string GetMalfunctionToChangeQuery()
        {
            return @";WITH LatestChange AS(
                    Select
                    	mc.ChangeDateJalali,
                    	b.ZoneId,
                    	b.ZoneTitle,
                    	TRIM(b.BillId)AS BillId,
                    	RN=ROW_NUMBER() OVER (PARTITION By TRIM(b.BillId) Order By b.RegisterDay,mc.ChangeDateJalali Desc)
                    From [CustomerWarehouse].dbo.MeterChange mc
                    Join [CustomerWarehouse].dbo.Bills b
                    	On mc.CustomerNumber=b.CustomerNumber AND mc.ZoneId=b.ZoneId
                    Where 
                    	mc.ChangeDateJalali BETWEEN @fromDateJalali AND @toDateJalali
                    ),
                    
                    LatestMalfunction AS (
                    Select	
                    	l.BillId,
                    	l.ChangeDateJalali,
                    	b.CounterStateCode,
                    	b.ZoneId,
                    	b.ZoneTitle,
                    	b.RegisterDay,
                    	LEAD(b.CounterStateCode) OVER (PARTITION by b.BillId Order By b.RegisterDay DESC) AS NextCounterState
                    From [CustomerWarehouse].dbo.Bills b
                    Join LatestChange l
                    	On TRIM(b.BillId)=l.BillId
                    Where
                    	l.RN=1 AND
                    	b.RegisterDay<=l.ChangeDateJalali AND
                    	b.CounterStateCode NOT IN (4,7,8)
                    ),
                    Final as(
                    Select 
                    	m.ZoneId,
                    	m.ZoneTitle,
                    	m.BillId,
                    	m.ChangeDateJalali,
                    	m.RegisterDay AS LatestMalfunctionDateJalali,
                    	ROW_NUMBER() OVER (PARTITION by m.BillId Order By m.RegisterDay desc ) AS RN
                    From LatestMalfunction m
                    Where 
                    	m.CounterStateCode=1 AND
                    	m.NextCounterState NoT IN (1,4,7,8)
                    )
                    Select 
                    	f.ZoneId,
                    	f.ZoneTitle,
                    	f.BillId,
                    	f.ChangeDateJalali,
                    	f.LatestMalfunctionDateJalali
                    From Final f
                    Where f.RN=1";
        }
    }
}
