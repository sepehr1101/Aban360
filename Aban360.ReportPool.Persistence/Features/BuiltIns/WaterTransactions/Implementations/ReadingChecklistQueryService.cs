using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Extensions;
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

            IEnumerable<ReadingChecklistDataOutputDto> data = await _sqlReportConnection.QueryAsync<ReadingChecklistDataOutputDto>(ReadingChecklistQueryString, input, null, 180);
            ReadingChecklistHeaderOutputDto header = new()
            {
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = data.HasValue() ? data.Count() : 0,
                CustomerCount = data.HasValue() ? data.Count() : 0,
                ZoneTitle = data.HasValue() ? data.FirstOrDefault().ZoneTitle : "",
                Title= ReportLiterals.ReadingChecklist,
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
                                b.CounterStateTitle AS LastCounterStateTitle,
		                        b.CustomerNumber,
                                c.DeletionStateTitle,
                                c.MobileNo AS MobileNumber,
                                TRIM(c.Address) AS Address,
                                c.ReadingNumber,
                                c.BillId,                            
		                        RN=ROW_NUMBER() OVER (PARTITION BY b.BillId ORDER BY b.RegisterDay DESC)
                     
	                        FROM [CustomerWarehouse].dbo.Bills b
	                        Right JOIN [CustomerWarehouse].dbo.Clients c 
		                        on b.BillId=c.BillId
	                        WHERE 
	                            (b.CounterStateCode NOT IN(4,7,8) OR b.CounterStateCode IS NULL) AND
		                        c.ZoneId=@zoneId AND
                                (@FromReadingNumber IS NULL or
                            	@ToReadingNumber IS NULL or 
                            	c.ReadingNumber BETWEEN @FromReadingNumber and @ToReadingNumber) AND
            			        c.ToDayJalali IS NULL AND
								c.DeletionStateId NOT IN (1,2))
                        SELECT * FROM CTE
                        WHERE RN=1";
            //todo:
        }
    }
}