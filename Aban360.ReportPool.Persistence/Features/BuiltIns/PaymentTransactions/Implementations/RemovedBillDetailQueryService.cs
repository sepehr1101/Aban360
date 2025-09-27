using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Implementations
{
    internal sealed class RemovedBillDetailQueryService : AbstractBaseConnection, IRemovedBillDetailQueryService
    {
        public RemovedBillDetailQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<RemovedBillHeaderOutputDto, RemovedBillDetailDataOutputDto>> GetInfo(RemovedBillInputDto input)
        {
            string RemovedBillQueryString = GetRemovedBillDataQuery(input.ZoneIds.Any());
            var @params = new
            {
                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,
                fromReadingNumber=input.FromReadingNumber,
                toReadingNumber=input.ToReadingNumber,
                fromAmount = input.FromAmount,
                toAmount = input.ToAmount,
                zoneIds = input.ZoneIds,
            };
            IEnumerable<RemovedBillDetailDataOutputDto> RemovedBillData = await _sqlReportConnection.QueryAsync<RemovedBillDetailDataOutputDto>(RemovedBillQueryString, @params);
            RemovedBillHeaderOutputDto RemovedBillHeader = new RemovedBillHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromReadingNumber= input.FromReadingNumber,
                ToReadingNumber= input.ToReadingNumber,
                FromAmount = input.FromAmount,
                ToAmount = input.ToAmount,
                CustomerCount = RemovedBillData is not null && RemovedBillData.Any() ? RemovedBillData.Count() : 0,
                RecordCount = RemovedBillData is not null && RemovedBillData.Any() ? RemovedBillData.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),

                SumAmount = RemovedBillData.Sum(x => x.Amount)
            };

            var result = new ReportOutput<RemovedBillHeaderOutputDto, RemovedBillDetailDataOutputDto>(ReportLiterals.RemovedBillDetail, RemovedBillHeader, RemovedBillData);

            return result;
        }

        private string GetRemovedBillDataQuery(bool hasZone)
        {
            string zoneQuery = hasZone ? "AND c.ZoneId IN @zoneIds" : string.Empty;
            return @$"Select
                	c.ZoneTitle,
                	c.ZoneId,
                	c.BillId,
                	rb.PreviousNumber AS PreviousMeterNumber,
                	rb.NextNumber CurrentMeterNumber,
                	rb.NextDay AS CurrentDateJalali,
                	rb.PreviousDay AS PreviousDateJalali,
                	rb.Consumption,
                	rb.SumItems AS Amount,
                	rb.RegisterDay AS RemovedDateJalali,
                	TRIM(c.FirstName) AS FirstName,
                	TRIM(c.SureName) AS Surname,
                	(TRIM(c.FirstName)+' ' +TRIM(c.SureName)) AS FullName,
                	TRIM(c.MobileNo) AS MobileNumber,
                	TRIM(c.NationalId) AS NationalCode,
                	TRIM(c.PostalCode) AS PostalCode,
                	c.UsageTitle
                From [CustomerWarehouse].dbo.RemovedBills rb
                Join [CustomerWarehouse].dbo.Clients c
                	on c.CustomerNumber=rb.CustomerNumber AND c.ZoneId=rb.ZoneId
                Where
                	(@fromDate IS NULL OR
                	@toDate IS NULL OR
                	rb.RegisterDay BETWEEN @fromDate AND @toDate) AND
                	(@fromAmount IS NULL OR
                	@toAmount IS NULL OR
                	rb.SumItems BETWEEN @fromAmount AND @toAmount) AND
                    (@fromReadingNumber IS NULL OR
                    @toReadingNumber IS NULL OR
                    c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) AND
					c.ToDayJalali IS NULL
                    {zoneQuery}";
        }

    }
}
