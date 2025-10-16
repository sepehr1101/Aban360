using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Implementations
{
    internal sealed class PendingPaymentsQueryService : AbstractBaseConnection, IPendingPaymentsQueryService
    {
        public PendingPaymentsQueryService(IConfiguration configuration)
            : base(configuration)
        { 
		}

        public async Task<ReportOutput<PendingPaymentsHeaderOutputDto, PendingPaymentsDataOutputDto>> GetInfo(PendingPaymentsInputDto input)
        {
            string usageTitleQuery = GetUsageTitleById();
            string pendingPaymentsQueryString = GetPendingPaymentsDataQuery(input.UsageSellIds.HasValue(),input.UsageConsumptionIds.HasValue());
            var @params = new
            {
                FromReadingNumber = string.IsNullOrWhiteSpace(input.FromReadingNumber)? input.FromReadingNumber :"0000000000",
                ToReadingNumber = string.IsNullOrWhiteSpace(input.ToReadingNumber)? input.ToReadingNumber :"9999999999",
                FromDate = input.FromDateJalali,
                ToDate = input.ToDateJalali,
                FromAmount = input.FromAmount!=null? input.FromAmount :0,
                ToAmount = input.ToAmount!=null? input.ToAmount :long.MaxValue ,
				FromDebtPeriodCount = input.FromDebtPeriodCount!=null? input.FromDebtPeriodCount :0,
                ToDebtPeriodCount = input.ToDebtPeriodCount!=null? input.ToDebtPeriodCount:int.MaxValue,
                UsageConsumptionIds =  input.UsageConsumptionIds,
                UsageSellIds = input.UsageSellIds,
                ZoneIds = input.ZoneIds
            };
			IEnumerable<PendingPaymentsDataOutputDto> pendingPaymentsData = await _sqlReportConnection.QueryAsync<PendingPaymentsDataOutputDto>(pendingPaymentsQueryString, @params, null, 120);
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
                ZoneCount = input.ZoneIds.Count(),
                RecordCount = (pendingPaymentsData is not null && pendingPaymentsData.Any()) ? pendingPaymentsData.Count() : 0,
                TotalBeginDebt = pendingPaymentsData.Sum(payment => payment.BeginDebt),
                TotalDebtPeriodCount = pendingPaymentsData.Sum(payment => payment.DebtPeriodCount),
                TotalEndingDebt = pendingPaymentsData.Sum(payment => payment.EndingDebt),
                TotalPayedAmount = pendingPaymentsData.Sum(payment => payment.PayedAmount),
                ReportDateJalali=DateTime.Now.ToShortPersianDateString(),
				Title= ReportLiterals.PendingPayments
            };

            ReportOutput<PendingPaymentsHeaderOutputDto, PendingPaymentsDataOutputDto> result = new (ReportLiterals.PendingPayments, pendingPaymentsHeader, pendingPaymentsData);
            return result;
        }

        private string GetUsageTitleById()
        {
            return @"Select u.Title
					From  [Aban360].ClaimPool.Usage u
					Where u.Id In @UsageId";
        }

        private string GetPendingPaymentsDataQuery(bool hasUsageSellId,bool hasUsageConsumptionId)
        {
			string usageSellQuery = hasUsageSellId == true ? "AND (UsageId IN @UsageSellIds)" : string.Empty;
			string usageConsumptionQuery = hasUsageConsumptionId == true ? "AND (UsageId2 IN @UsageConsumptionIds)" : string.Empty;
            return @$"-- مشتریان هدف
						WITH FilteredClients AS (
							SELECT 
								ZoneId,
								ZoneTitle,
								VillageName,
								CustomerNumber,
								BillId,
								ReadingNumber,
								UsageTitle AS UsageSellTitle ,
								IIF(UsageId2=0,UsageTitle,UsageTitle2) AS UsageConsumptionTitle,
								TRIM(FirstName) As FirstName,
								TRIM(SureName) Surname,
								MobileNo AS MobileNumber,
								PhoneNo AS PhoneNumber,
								DeletionStateTitle AS UseStateTitle,
								'--' AS HeadquarterTitle ,
								t46.C2 AS RegionTitle
							FROM [CustomerWarehouse].dbo.Clients 
						    Join [Db70].dbo.T51 t51
						    	On t51.C0=ZoneId
						    Join [Db70].dbo.T46 t46
						    	On t51.C1=t46.C0
							WHERE 
								ToDayJalali IS NULL
								AND ZoneId IN @ZoneIds
								AND (@FromReadingNumber IS NULL OR 
									@ToReadingNumber IS NULL OR 
									TRIM(ReadingNumber) BETWEEN @FromReadingNumber AND @ToReadingNumber) AND
									DeletionStateId NOT IN (1,2)
								{usageSellQuery}
								{usageConsumptionQuery}
								--AND (@UsageConsumptionIds IS NULL OR UsageId2 IN @UsageConsumptionIds)
						),
						
						-- تجمیعی قبض‌ها
						BillAgg AS (
							SELECT 
								ZoneId,
								CustomerNumber,
								SUM(CASE WHEN RegisterDay < @FromDate THEN SumItems ELSE 0 END) AS BillBefore,
								SUM(CASE WHEN RegisterDay BETWEEN @FromDate AND @ToDate THEN SumItems ELSE 0 END) AS BillBetween,
								SUM(CASE WHEN RegisterDay > @ToDate THEN SumItems ELSE 0 END) AS BillAfter
							FROM [CustomerWarehouse].dbo.Bills
							WHERE 
								TypeCode NOT IN(7,8) AND 
								ZoneId IN @ZoneIds AND
								SumItems<>0
							GROUP BY ZoneId, CustomerNumber
						),
						
						-- تجمیعی پرداخت‌ها + آخرین پرداخت
						PaymentAgg AS (
							SELECT 
								ZoneId,
								CustomerNumber,
								SUM(CASE WHEN RegisterDay < @FromDate THEN Amount ELSE 0 END) AS PaymentBefore,
								SUM(CASE WHEN RegisterDay BETWEEN @FromDate AND @ToDate	THEN Amount ELSE 0 END) AS PaymentBetween,
								SUM(CASE WHEN RegisterDay > @ToDate THEN Amount ELSE 0 END) AS PaymentAfter,
								MAX(RegisterDay) AS LastPaymentDate
							FROM [CustomerWarehouse].dbo.Payments
							WHERE 
								ZoneId IN @ZoneIds AND
								Amount<>0 
							GROUP BY ZoneId, CustomerNumber
						),
						
						-- شمارش قبض‌های ثبت‌شده بعد از آخرین پرداخت (با LEFT JOIN)
						DebtAfterLastPayment AS (
							SELECT b.ZoneId, b.CustomerNumber, COUNT(1) AS DebtPeriodsAfter
							FROM [CustomerWarehouse].dbo.Bills b
							LEFT JOIN PaymentAgg p
								ON b.ZoneId = p.ZoneId AND b.CustomerNumber = p.CustomerNumber
							WHERE 
							  b.RegisterDay > ISNULL(p.LastPaymentDate, '0001/01/01')
							  AND b.RegisterDay < @ToDate
							  AND b.TypeCode NOT IN(7,8) AND
							  b.SumItems<>0
							GROUP BY b.ZoneId, b.CustomerNumber
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
							ON C.ZoneId = D.ZoneId AND C.CustomerNumber = D.CustomerNumber
						Where
							(
							 @FromAmount IS NULL OR
							 @ToAmount IS NULL OR
							 (ISNULL(B.BillBetween, 0) + ISNULL(B.BillBefore, 0) - ISNULL(P.PaymentBetween, 0) - ISNULL(P.PaymentBefore, 0)) BETWEEN @FromAmount AND @ToAmount
							) AND
							(
							 @FromDebtPeriodCount IS NULL OR
							 @toDebtPeriodCount IS NULL OR
							 D.DebtPeriodsAfter BETWEEN @FromDebtPeriodCount AND @toDebtPeriodCount) AND
							(				
							 @FromDebtPeriodCount IS NULL OR
							 @ToDebtPeriodCount IS NULL OR 
							 D.DebtPeriodsAfter BETWEEN @FromDebtPeriodCount AND @ToDebtPeriodCount
							)";
        }
    }
}
