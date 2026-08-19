using Aban360.ClaimPool.Domain.Constants;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Base
{
    internal abstract class FinancialStatementBase : AbstractBaseConnection
    {
        private string rawTypeCode = $"( {(int)BillTypeCodeEnum.Ghabz}, {(int)BillTypeCodeEnum.EslahatMosbat}, {(int)BillTypeCodeEnum.EslahatManfi}, {(int)BillTypeCodeEnum.Bargashti} )";
        private int NetTypeCode = (int)BillTypeCodeEnum.Ghabz;
        private int ReturnedTypeCode = (int)BillTypeCodeEnum.Bargashti;
        public FinancialStatementBase(IConfiguration configuration)
            : base(configuration)
        {
        }

        internal string GetWaterTotalQuery()
        {
            return $@";With PerBillId As(
						Select 
							b.billId billId,
							u2.Title UsageGroupTitle,
							COUNT(1) CustomerCount,
							SUM(ISNULL(b.CommercialCount, 0) + ISNULL(b.DomesticCount, 0) + ISNULL(b.OtherCount, 0)) AS ConsumptionTotalUnit,
							AVG(b.Consumption) DailyAverage,
							SUM(IIF(b.TypeCode = {NetTypeCode} , b.Consumption , 0)) NetConsumption,
							SUM(IIF(b.TypeCode = {NetTypeCode} , b.Item1 + b.Item11 , 0)) NetAmount,
							SUM(IIF(b.TypeCode = {ReturnedTypeCode}, b.Consumption , 0)) ReturnedConsumption,
							SUM(IIF(b.TypeCode = {ReturnedTypeCode}, b.Item1 + b.Item11 , 0)) ReturnedAmount,
							SUM( b.ItemOff1 + b.ItemOff11 ) DiscountAmount,
							SUM(IIF(b.TypeCode IN {rawTypeCode} , b.Consumption , 0)) RawConsumption,
							SUM(IIF(b.TypeCode IN {rawTypeCode} , b.Item1 + b.Item11 , 0)) RawAmount,
							SUM(IIF(b.TypeCode IN {rawTypeCode} , b.Item1 + b.Item11 , 0)) /IIF(SUM(IIF(b.TypeCode IN {rawTypeCode} , b.Consumption , 0))=0,1,SUM(IIF(b.TypeCode IN {rawTypeCode} , b.Consumption , 0))) RawAmountAverage,
							AVG(ConsumptionAverage) ConsumptionAverageInMonth 
						From [CustomerWarehouse].dbo.Bills b 
						Join [Db70].dbo.UsageGroup2 u2
							ON u2.Id IN @UsageGroupIds
						Join [Db70].dbo.UsageGroup3 u3
							ON u3.Group2Id=u2.Id
						Where
							b.ZoneId IN @ZoneIds AND
							b.RegisterDay BETWEEN @FromDateJalali AND @ToDateJalali 
						Group By b.BillId,u2.Title
					)
					Select 
						UsageGroupTitle,
						SUM( CustomerCount ) CustomerCount,
						SUM( ConsumptionTotalUnit ) ConsumptionTotalUnit,
						SUM( DailyAverage ) DailyAverage,
						SUM( NetConsumption ) NetConsumption,
						SUM( NetAmount ) NetAmount,
						SUM( ReturnedConsumption ) ReturnedConsumption,
						SUM( ReturnedAmount ) ReturnedAmount,
						SUM( DiscountAmount ) DiscountAmount,
						SUM( RawConsumption ) RawConsumption,
						SUM( RawAmount  ) RawAmount,
						AVG( RawAmountAverage ) RawAmountAverage,
						AVG( ConsumptionAverageInMonth ) ConsumptionAverageInMonth
					From PerBillId
					Group By UsageGroupTitle 
					Order by UsageGroupTitle";
        }
    }
}
