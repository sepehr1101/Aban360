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
    internal sealed class ReadingChecklistQueryService : AbstractBaseConnection, IReadingChecklistQueryService
    {
        public ReadingChecklistQueryService(IConfiguration configuration)
            : base(configuration)
        { }

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
            IEnumerable<ReadingChecklistDataOutputDto> data = await _sqlReportConnection.QueryAsync<ReadingChecklistDataOutputDto>(ReadingChecklistQueryString,@params);//todo:Params
            ReadingChecklistHeaderOutputDto header = new()
            {
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = data.Count()
            };

            ReportOutput<ReadingChecklistHeaderOutputDto, ReadingChecklistDataOutputDto> result = new(ReportLiterals.ReadingChecklist, header, data);
            return result;
        }
        private string GetReadingChecklistQuery()
        {
            return @"Select
                     	b.ZoneTitle,
                     	TRIM(c.FirstName) ,
                     	TRIM(c.SureName) AS Surname,
                     	TRIM(c.FirstName)+' '+TRIM(c.SureName) AS FullName,
                     	c.DomesticCount AS DomesticUnit,
                     	c.DomesticCount+c.OtherCount AS NonDomesticUnit,
                     	c.UsageTitle,
                     	c.WaterDiameterTitle AS MeterDiameterTitle,
                     	b.PreviousDay AS PreviousDateJalali,
                     	IIF(@isShowLastNumber=1,b.PreviousNumber,0) AS PreviousNumber,
                     	b.CounterStateCode AS LastCounterStateCode,
                     	b.CustomerNumber	
                     
                     From [CustomerWarehouse].dbo.Bills b
                     join [CustomerWarehouse].dbo.Clients c on b.BillId=c.BillId
                     Where 
                     	b.ZoneId=@zoneId AND
                     	b.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber
                     Order By b.RegisterDay Desc";
        }
    }
}
