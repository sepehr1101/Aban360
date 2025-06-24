using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Implementations
{
    internal sealed class PendingPaymentsQueryService : AbstractBaseConnection, IPendingPaymentsQueryService
    {
        public PendingPaymentsQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<PendingPaymentsHeaderOutputDto, PendingPaymentsDataOutputDto>> GetInfo(PendingPaymentsInputDto input)
        {
            string usageTitleQuery = GetUsageTitleById();

            string pendingPaymentsQueryString = GetPendingPaymentsDataQuery();
            var @params = new
            {
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                FromDate = input.FromDateJalali,
                ToDate = input.ToDateJalali,
                FromAmount = input.FromAmount,
                ToAmount = input.ToAmount,
                FromDebtPeriodCount = input.FromDebtPeriodCount,
                ToDebtPeriodCount = input.ToDebtPeriodCount,
                UsageConsumptionIds =  input.UsageConsumptionIds,
                UsageSellIds = input.UsageSellIds,
                ZoneId = input.ZoneId
            };
            IEnumerable<PendingPaymentsDataOutputDto> pendingPaymentsData = await _sqlConnection.QueryAsync<PendingPaymentsDataOutputDto>(pendingPaymentsQueryString,@params);//todo: parameters
            PendingPaymentsHeaderOutputDto pendingPaymentsHeader = new PendingPaymentsHeaderOutputDto()
            {
				FromReadingNumber=input.FromReadingNumber,
				ToReadingNumber=input.ToReadingNumber,
				FromDateJalali=input.FromDateJalali,
				ToDateJalali=input.ToDateJalali,
				FromAmount=input.FromAmount,
				ToAmount=input.ToAmount,
				FromDebtPeriodCount=input.FromDebtPeriodCount,
				ToDebtPeriodCount=input.ToDebtPeriodCount,
				ZoneTitle=pendingPaymentsData.First().ZoneTitle,
                RecordCount = pendingPaymentsData.Count(),
                TotalBeginDebt = pendingPaymentsData.Sum(payment => payment.BeginDebt),
                TotalDebtPeriodCount = pendingPaymentsData.Sum(payment => payment.DebtPeriodCount),
                TotalEndingDebt = pendingPaymentsData.Sum(payment => payment.EndingDebt),
                TotalPayedAmount = pendingPaymentsData.Sum(payment => payment.PayedAmount),
            };

            var result = new ReportOutput<PendingPaymentsHeaderOutputDto, PendingPaymentsDataOutputDto>(ReportLiterals.PendingPayments, pendingPaymentsHeader, pendingPaymentsData);

            return result;
        }

        private string GetUsageTitleById()
        {
            return @"Select u.Title
					 From  [Aban360].ClaimPool.Usage u
					 Where u.Id In @UsageId";
        }

        private string GetPendingPaymentsDataQuery()
        {
            return @"-- مشتریان هدف
						WITH FilteredClients AS (
							SELECT ZoneId,ZoneTitle, VillageName, CustomerNumber,BillId, ReadingNumber,
								   UsageTitle AS UsageSellTitle , UsageTitle2 AS UsageConsumptionTitle,  TRIM(FirstName) As FirstName,TRIM(SureName) Surname,
								   MobileNo AS MobileNumber, PhoneNo AS PhoneNumber, DeletionStateTitle AS UseStateTitle, '--' AS HeadquarterTitle , '--' AS RegionTitle
							FROM [CustomerWarehouse].dbo.Clients 
							WHERE ToDayJalali IS NULL
							  AND ZoneId=@ZoneId
							  AND TRIM(ReadingNumber) BETWEEN @FromReadingNumber AND @ToReadingNumber
							  AND UsageId IN @UsageSellIds--IN (SELECT value FROM STRING_SPLIT(@UsageSellIds, ','))
							  AND UsageId2 IN @UsageConsumptionIds--IN (SELECT value FROM STRING_SPLIT(@UsageConsumptionIds, ','))
						),
						
						-- تجمیعی قبض‌ها
						BillAgg AS (
							SELECT ZoneId, CustomerNumber,
								SUM(CASE WHEN RegisterDay < @FromDate THEN SumItems ELSE 0 END) AS BillBefore,
								SUM(CASE WHEN RegisterDay BETWEEN @FromDate AND @ToDate THEN SumItems ELSE 0 END) AS BillBetween,
								SUM(CASE WHEN RegisterDay > @ToDate THEN SumItems ELSE 0 END) AS BillAfter
							FROM [CustomerWarehouse].dbo.Bills
							WHERE ZoneId =@ZoneId
							GROUP BY ZoneId, CustomerNumber
						),
						
						-- تجمیعی پرداخت‌ها + آخرین پرداخت
						PaymentAgg AS (
							SELECT ZoneId, CustomerNumber,
								SUM(CASE WHEN RegisterDay < @FromDate 
								THEN Amount ELSE 0 END) AS PaymentBefore,
								SUM(CASE WHEN RegisterDay BETWEEN @FromDate AND @ToDate
											AND Amount BETWEEN @FromAmount AND @ToAmount 
											THEN Amount ELSE 0 END) AS PaymentBetween,
								SUM(CASE WHEN RegisterDay > @ToDate THEN Amount ELSE 0 END) AS PaymentAfter,
								MAX(RegisterDay) AS LastPaymentDate
							FROM [CustomerWarehouse].dbo.Payments
							WHERE ZoneId=@ZoneId
							GROUP BY ZoneId, CustomerNumber
						),
						
						-- شمارش قبض‌های ثبت‌شده بعد از آخرین پرداخت (با LEFT JOIN)
						DebtAfterLastPayment AS (
							SELECT b.ZoneId, b.CustomerNumber, COUNT(*) AS DebtPeriodsAfter
							FROM [CustomerWarehouse].dbo.Bills b
							LEFT JOIN PaymentAgg p
								ON b.ZoneId = p.ZoneId AND b.CustomerNumber = p.CustomerNumber
							WHERE b.RegisterDay > ISNULL(p.LastPaymentDate, '0001/01/01')
							  AND b.RegisterDay < @ToDate
							GROUP BY b.ZoneId, b.CustomerNumber
							HAVING COUNT(*) BETWEEN @FromDebtPeriodCount AND @ToDebtPeriodCount
						)
						
						-- کوئری نهایی
						SELECT	
							C.*,
							ISNULL(P.PaymentBetween, 0) AS PayedAmount,
							ISNULL(B.BillBefore, 0) - ISNULL(P.PaymentBefore, 0) AS BeginDebt,
							ISNULL(B.BillBetween, 0) + ISNULL(B.BillBefore, 0) - ISNULL(P.PaymentBetween, 0) - ISNULL(P.PaymentBefore, 0) AS EndingDebt,
							ISNULL(D.DebtPeriodsAfter, 0) AS DebtPeriodCount
						
						FROM FilteredClients C
						LEFT JOIN BillAgg B
							ON C.ZoneId = B.ZoneId AND C.CustomerNumber = B.CustomerNumber
						LEFT JOIN PaymentAgg P
							ON C.ZoneId = P.ZoneId AND C.CustomerNumber = P.CustomerNumber
						LEFT JOIN DebtAfterLastPayment D
							ON C.ZoneId = D.ZoneId AND C.CustomerNumber = D.CustomerNumber;
						";
        }

    }
}
