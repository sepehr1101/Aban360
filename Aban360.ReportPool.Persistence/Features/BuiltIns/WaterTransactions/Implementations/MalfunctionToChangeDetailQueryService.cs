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
    internal sealed class MalfunctionToChangeDetailQueryService : AbstractBaseConnection, IMalfunctionToChangeDetailQueryService
    {
        public MalfunctionToChangeDetailQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<MalfunctionToChangeHeaderOutputDto, MalfunctionToChangeDetailDataOutputDto>> Get(MalfunctionToChangeInputDto input)
        {
            string MalfunctionQueryString = GetMalfunctionToChangeQuery();
            var @params = new
            {
                fromDateJalali = input.FromDateJalali,
                toDateJalali = input.ToDateJalali,
            };
            IEnumerable<MalfunctionToChangeDetailDataOutputDto> MalfunctionData = await _sqlReportConnection.QueryAsync<MalfunctionToChangeDetailDataOutputDto>(MalfunctionQueryString, @params);
            MalfunctionToChangeHeaderOutputDto MalfunctionHeader = new MalfunctionToChangeHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = (MalfunctionData is not null && MalfunctionData.Any()) ? MalfunctionData.Count() : 0,
                CustomerCount = (MalfunctionData is not null && MalfunctionData.Any()) ? MalfunctionData.Count() : 0,
            };

            ReportOutput<MalfunctionToChangeHeaderOutputDto, MalfunctionToChangeDetailDataOutputDto> result = new ReportOutput<MalfunctionToChangeHeaderOutputDto, MalfunctionToChangeDetailDataOutputDto>(ReportLiterals.MalfunctionToChangeDetail, MalfunctionHeader, MalfunctionData);
            return result;
        }

        private string GetMalfunctionToChangeQuery()
        {
            return @";WITH LatestChange AS(
                    Select
                    	mc.ChangeDateJalali,
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
                    	*,
                    	ROW_NUMBER() OVER (PARTITION by m.BillId Order By m.RegisterDay desc ) AS RN
                    From LatestMalfunction m
                    Where 
                    	m.CounterStateCode=1 AND
                    	m.NextCounterState NoT IN (1,4,7,8)
                    )
                    Select 
                    	TRIM(c.FirstName) as FirstName,
                    	TRIM(c.SureName) as Surname,
                    	TRIM(c.FirstName) +' '+TRIM(c.SureName) as FullName,
                    	TRIM(c.Address) as Address,
                    	TRIM(c.NationalId) as NationalId,
                    	c.MobileNo as MobileNumber,
                    	c.UsageTitle,
                    	c.ZoneId,
                    	c.ZoneTitle,
                    	f.BillId,
                    	f.ChangeDateJalali,
                    	f.RegisterDay as LatestMalfunctinDateJalali
                    From [CustomerWarehouse].dbo.Clients c
                    Join Final f 
                    	On TRIM(c.BillId)=f.BillId
                    Where 
                    	f.RN=1 AND
                    	c.ToDayJalali IS NULL";
        }
    }
}
