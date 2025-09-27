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
    internal sealed class ReadingDailyStatementQueryService : AbstractBaseConnection, IReadingDailyStatementQueryService
    {
        public ReadingDailyStatementQueryService(IConfiguration configuration)
            : base(configuration)
        { 
        }

        public async Task<ReportOutput<ReadingDailyStatementHeaderOutputDto, ReadingDailyStatementDataOutputDto>> GetInfo(ReadingDailyStatementInputDto input)
        {
            string readingDailyStatements = GetReadingDailyStatementQuery();
            var @params = new
            {
                fromAmount=input.FromAmount,
                toAmount=input.ToAmount,
                fromReadingNumber = input.FromReadingNumber,
                toReadingNumber = input.ToReadingNumber,
                fromConsumption = input.FromConsumption,
                toConsumption = input.ToConsumption,
                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,
                zoneIds = input.ZoneIds,
            };
            IEnumerable<ReadingDailyStatementDataOutputDto> data = await _sqlReportConnection.QueryAsync<ReadingDailyStatementDataOutputDto>(readingDailyStatements, @params);
            ReadingDailyStatementHeaderOutputDto header = new ReadingDailyStatementHeaderOutputDto()
            {
                FromConsumption = input.FromConsumption,
                ToConsumption = input.ToConsumption,
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                FromAmount=input.FromAmount,
                ToAmount=input.ToAmount,

                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = (data is not null && data.Any()) ? data.Count() : 0,
                CustomerCount = (data is not null && data.Any()) ? data.Count() : 0,
            };

            var result = new ReportOutput<ReadingDailyStatementHeaderOutputDto, ReadingDailyStatementDataOutputDto>(ReportLiterals.ReadingDailyStatement, header, data);
            return result;
        }

        private string GetReadingDailyStatementQuery()
        {
            return @"Select
						b.ZoneId,
						b.ZoneTitle,
                    	b.ReadingNumber,
                    	b.CustomerNumber,
                    	TRIM(c.FirstName) FirstName,
                    	TRIM(c.SureName) AS Surname,
                    	TRIM(c.FirstName) + ' ' + TRIM(c.SureName) AS FullName,
                    	c.WaterDiameterTitle AS MeterDiameterTitle,
                    	b.Consumption,
                        b.UsageTitle,
                        TRIM(b.BillId) AS BillId,
                    	b.ConsumptionAverage,
                    	b.SumItems AS InvoiceAmount,
                    	TRIM(c.Address) AS Address,
                        b.PayId AS PaymentId,
						b.SumItems,
						TRIM(c.MobileNo) AS MobileNumber,
						TRIM(c.PhoneNo) AS PhoneNumber,
						TRIM(c.NationalId) AS NationalCode,
						TRIM(c.PostalCode) AS PostalCode,
						b.PreviousDay AS FromReadingDateJalali,
						b.NextDay AS ToReadingDateJalali,
						b.RegisterDay AS RegisterBillDateJalali
                    From [CustomerWarehouse].dbo.Bills b
                    Join [CustomerWarehouse].dbo.Clients c on b.BillId=c.BillId
                    Where 
                    	c.ToDayJalali IS NULL AND
                    	(@fromReadingNumber IS NULL OR
                        @toReadingNumber IS NULL OR
                        b.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) AND
                    	b.NextDay BETWEEN @fromDate AND @toDate AND
                        b.Consumption BETWEEN @fromConsumption AND @toConsumption AND
                    	b.ZoneId IN @zoneIds AND
						(@fromAmount IS NULL OR
						@toAmount IS NULL OR
						b.SumItems BETWEEN @fromAmount AND @toAmount)";
        }
    }
}
