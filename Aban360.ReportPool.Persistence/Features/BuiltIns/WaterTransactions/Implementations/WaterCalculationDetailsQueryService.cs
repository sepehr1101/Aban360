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
    internal sealed class WaterCalculationDetailsQueryService : AbstractBaseConnection, IWaterCalculationDetailsQueryService
    {
        public WaterCalculationDetailsQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<WaterCalculationDetailsHeaderOutputDto, WaterCalculationDetailsDataOutputDto>> GetInfo(WaterCalculationDetailsInputDto input)
        {
            string calculationDetailsDataInfoQuery = GetCalculationDetailsDataQuery();
			string calculationHeaderDataQuery = GetCalculationHeaderDataQuery();

            IEnumerable<WaterCalculationDetailsDataFromSqlOutputDto> calculationDetailsDataSql = await _sqlReportConnection.QueryAsync<WaterCalculationDetailsDataFromSqlOutputDto>(calculationDetailsDataInfoQuery, new { Id = input.Input });
            List<WaterCalculationDetailItemTitleDto> items = calculationDetailsDataSql.Select(x => new WaterCalculationDetailItemTitleDto
            {
                ItemTitle = x.ItemTitle,
                ItemValue = x.ItemValue,
            }).ToList();
            WaterCalculationDetailsDataOutputDto calculationDetailsData = new WaterCalculationDetailsDataOutputDto()
            {
                Payble = calculationDetailsDataSql.FirstOrDefault().Payble,
                SumItems = calculationDetailsDataSql.FirstOrDefault().SumItems,
                SumOffItems = calculationDetailsDataSql.FirstOrDefault().SumOffItems,
                ItemTitles = items
            };

			WaterCalculationDetailsHeaderOutputDto calculationDetailsHeader = await _sqlReportConnection.QueryFirstOrDefaultAsync<WaterCalculationDetailsHeaderOutputDto>(calculationHeaderDataQuery, new { Id = input.Input });
			calculationDetailsHeader.ReportDateJalali = DateTime.Now.ToShortPersianDateString();
			calculationDetailsHeader.Title = ReportLiterals.WaterCalculationDetails;

            var result = new ReportOutput<WaterCalculationDetailsHeaderOutputDto, WaterCalculationDetailsDataOutputDto>(ReportLiterals.WaterCalculationDetails, calculationDetailsHeader, new List<WaterCalculationDetailsDataOutputDto> { calculationDetailsData });
            return result;
        }

        private string GetCalculationDetailsDataQuery()
        {
            return @"Select
						v.Value AS ItemValue,
						v.Item AS ItemTitle,
						b.Payable ,
						b.SumItems,
						(b.ItemOff1 +b.ItemOff2 +b.ItemOff3 +b.ItemOff4 +
					     b.ItemOff5 +b.ItemOff6 +b.ItemOff7 +b.ItemOff8 +
					     b.ItemOff9 +b.ItemOff10 +b.ItemOff11 +b.ItemOff12 +
					     b.ItemOff13 +b.ItemOff14 +b.ItemOff15 +b.ItemOff16 +b.ItemOff17) AS SumOffItems
					From [CustomerWarehouse].dbo.Bills b
					Cross Apply(
						values
						(b.Item1, N'آب بها'),
						(b.Item2, N'کارمزد دفع فاضلاب'),
						(b.Item3, N'آبونمان آب'),
						(b.Item4, N'آبونمان فاضلاب'),
						(b.Item5, N'عوارض شهرداری'),
						(b.Item6, N'تبصره'),
						(b.Item7, N'تبصره 2 و 3 قدیم (قابل جمع با آب بها)'),
						(b.Item8, N'جریمه'),
						(b.Item9, N'آبرسانی'),
						(b.Item10, N'ضریب D/ جوانی جمعیت'),
						(b.Item11, N'فصل گرم سال'),
						(b.Item12, N'ضریب تعدیل'),
						(b.Item13, N'تبصره3  آب'),
						(b.Item14, N'تبصره3  فاضلاب'),
						(b.Item15, N'تبصره آبونمان فاضلاب'),
						(b.Item16, N'مبلغ قانون بودجه'),
						(b.Item17, N'قسط لوازم کاهنده مصرف'),
						(b.ItemOff1, N'تخفیف آب بها'),
						(b.ItemOff2, N'تخفیف کارمزد دفع فاضلاب'),
						(b.ItemOff3, N'تخفیف آبونمان آب'),
						(b.ItemOff4, N'تخفیف آبونمان فاضلاب'),
						(b.ItemOff5, N'تخفیف عوارض شهرداری'),
						(b.ItemOff6, N'تخفیف تبصره 2'),
						(b.ItemOff7, N'تخفیف تبصره 2 و 3 قدیم (قابل جمع با آب بها)'),
						(b.ItemOff8, N'تخفیف جریمه'),
						(b.ItemOff9, N'تخفیف آبرسانی'),
						(b.ItemOff10, N'تخفیف ضریب D/ جوانی جمعیت'),
						(b.ItemOff11, N'تخفیف فصل گرم سال'),
						(b.ItemOff12, N'تخفیف ضریب تعدیل'),
						(b.ItemOff13, N'تخفیف تبصره3 آب'),
						(b.ItemOff14, N'تخفیف تبصره3 فاضلاب'),
						(b.ItemOff15, N'تخفیف تبصره آبونمان فاضلاب'),
						(b.ItemOff16, N'تخفیف مبلغ قانون بودجه'),
						(b.ItemOff17, N'تخفیف قسط لوازم کاهنده مصرف')
					) v(Value,Item)
					Where b.Id=@Id	";
        }
		private string GetCalculationHeaderDataQuery()
		{
			return @"Select
						b.CustomerNumber, 
						b.ReadingNumber,
						b.BillId,
						TRIM(c.FirstName) AS FirstName,
						TRIM(c.SureName) AS Surname,
						TRIM(c.FirstName) +' '+TRIM(c.SureName) AS FullName,
						TRIM(c.NationalId) AS NationalCode,
						TRIM(c.MobileNo) AS MobileNumber,
						TRIM(c.Address) AS Address,
						c.PostalCode,
						TRIM(b.PayId) AS PayId,
						b.ZoneTitle,
						b.UsageTitle,
						b.Consumption,
						b.ConsumptionAverage,
						b.Duration,
						b.OtherCount AS OtherUnit,
						b.CommercialCount,
						b.DomesticCount,
						b.EmptyCount AS EmptyUnit,
						b.RegisterDay AS IssueDateJalali,
						b.BranchType,
						b.ReadingStateTitle,
						b.CounterStateTitle,
						b.PreviousDay,
						b.NextDay AS CurrentDay,
						b.PreviousNumber,
						b.NextNumber AS CurrentNumber,
						b.Deadline,
						b.WaterDiameterTitle AS MeterDiameterTitle	
					From [CustomerWarehouse].dbo.Bills b
					Join [CustomerWarehouse].dbo.Clients c 
						ON b.CustomerNumber=c.CustomerNumber AND b.ZoneId=c.ZoneId
					Where
						b.Id=@Id AND 
						c.ToDayJalali IS NULL"; 

        }
    }
}
