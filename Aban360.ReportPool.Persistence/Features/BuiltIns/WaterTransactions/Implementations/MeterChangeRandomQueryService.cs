using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Implementations
{
    internal sealed class MeterChangeRandomQueryService : ChangeHistoryBase, IMeterChangeRandomQueryService
    {
        public MeterChangeRandomQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<MeterChangeRandomHeaderOutputDto, MeterChangeRandomDataOutputDto>> GetInfo(MeterChangeRandomInputDto input)
        {
            string title = ReportLiterals.MeterChangeRandom;
            string query = GetRandomQuery(input.ChangeCount);

            IEnumerable<MeterChangeRandomDataOutputDto> data = await _sqlReportConnection.QueryAsync<MeterChangeRandomDataOutputDto>(query, input);
            MeterChangeRandomHeaderOutputDto header = new MeterChangeRandomHeaderOutputDto()
            {
                ChangeDateJalali = input.ChangeDateJalali,
                RegisterDateJalali = input.RegisterDateJalali,
                ChangeCount = input.ChangeCount,

                CustomerCount = data is not null && data.Any() ? data.Count() : 0,
                RecordCount = data is not null && data.Any() ? data.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                Title = title,
            };

            var result = new ReportOutput<MeterChangeRandomHeaderOutputDto, MeterChangeRandomDataOutputDto>(title, header, data);
            return result;
        }
        internal string GetRandomQuery(int changeCount)
        {
            return $@"use CustomerWarehouse
                    ;with cte  as(
                    select top {changeCount} * from MeterChange
                    where 
                    	ChangeDateJalali>@ChangeDateJalali 
                    	order by NEWID() 
                    )
                    select 
                    	b.ZoneTitle, b.BillId, c.FirstName+ ' '+c.SureName FullName, b.WaterDiameterTitle, b.UsageTitle,
                    	b.Consumption, b.ConsumptionAverage, b.Duration, b.DomesticCount+ b.CommercialCount+ b.OtherCount TotalUnit,
                    	b.PreviousDay, b.NextDay, b.PreviousNumber, b.NextNumber, b.CounterStateTitle,
                    	cte.MeterNumber, cte.RegisterDateJalali, cte.ChangeDateJalali
                    from cte
                    join Bills b
                    join Clients c
                    on b.BillId=c.BillId and c.ToDayJalali is null
                    on cte.CustomerNumber=b.CustomerNumber and cte.ZoneId= b.ZoneId
                    where	
                    	b.RegisterDay>@RegisterDateJalali and
                    	b.CounterStateCode not in (4,7,8)
                    order by BillId, b.RegisterDay";
        }
    }
}

