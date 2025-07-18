﻿using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Implementations
{
    internal sealed class ReadingChecklistQueryService : AbstractBaseConnection, IReadingChecklistQueryService
    {
        public ReadingChecklistQueryService(IConfiguration configuration)
            : base(configuration)
        { 
        }

        public async Task<ReportOutput<ReadingChecklistHeaderOutputDto, ReadingChecklistDataOutputDto>> Get(ReadingChecklistInputDto input)
        {
            string ReadingChecklistQueryString = GetReadingChecklistQuery();
            var @params = new
            {
                fromReadingNumber = input.FromReadingNumber,
                toReadingNumber = input.ToReadingNumber,
                zoneId = input.ZoneId,
                isShowLastNumber = input.IsShowLastNumber,
            };
            IEnumerable<ReadingChecklistDataOutputDto> data = await _sqlReportConnection.QueryAsync<ReadingChecklistDataOutputDto>(ReadingChecklistQueryString,@params);
            ReadingChecklistHeaderOutputDto header = new()
            {
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = (data is not null && data.Any()) ? data.Count() : 0,
                ZoneTitle = (data is not null && data.Any()) ? data.FirstOrDefault().ZoneTitle : "",
            };

            ReportOutput<ReadingChecklistHeaderOutputDto, ReadingChecklistDataOutputDto> result = new(ReportLiterals.ReadingChecklist, header, data);
            return result;
        }
        private string GetReadingChecklistQuery()
        {
            return @"Use CustomerWarehouse
                        ;WITH CTE AS(
	                        SELECT
		                        b.ZoneTitle,
		                        TRIM(c.FirstName) Firstname ,
		                        TRIM(c.SureName) AS Surname,
		                        TRIM(c.FirstName)+' '+TRIM(c.SureName) AS FullName,
		                        c.DomesticCount AS DomesticUnit,
		                        c.DomesticCount+c.OtherCount AS NonDomesticUnit,
		                        c.UsageTitle,
		                        c.WaterDiameterTitle AS MeterDiameterTitle,
		                        b.PreviousDay AS PreviousDateJalali,
		                        IIF(@isShowLastNumber=1,b.NextNumber,0) AS PreviousNumber,
		                        b.CounterStateCode AS LastCounterStateCode,
		                        b.CustomerNumber,
		                        RN=ROW_NUMBER() OVER (PARTITION BY b.BillId ORDER BY b.RegisterDay DESC)
                     
	                        FROM [CustomerWarehouse].dbo.Bills b
	                        JOIN [CustomerWarehouse].dbo.Clients c 
		                        on b.BillId=c.BillId
	                        WHERE 
	                            b.CounterStateCode NOT IN(4,7,8) AND
		                        b.ZoneId=@zoneId AND
		                        b.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber)
                        SELECT * FROM CTE
                        WHERE RN=1";
        }
    }
}