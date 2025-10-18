using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.Transactions;
using Aban360.ReportPool.Persistence.Features.Transactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.Transactions.Imlementations
{
    internal sealed class BranchEventSummaryQueryService : AbstractBaseConnection, IBranchEventSummaryQueryService
    {
        public BranchEventSummaryQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<BranchEventSummaryHeaderOutputDto, BranchEventSummaryDataOutputDto>> Get(string billId)
        {
            string zoneIdAndCustomerNumberQueryString = GetZoneIdAndCustomerNumberQuery();
            string brachSummeryHeaderQueryString = GetBrachEventSummaryHeaderQuery();
            string brachSummeryDataQueryString = GetBrachEventSummaryDataQuery();

            ZoneIdAndCustomerNumberOutputDto zoneIdCustomerNumber = await _sqlReportConnection.QueryFirstOrDefaultAsync<ZoneIdAndCustomerNumberOutputDto>(zoneIdAndCustomerNumberQueryString, new { billId });
            if (zoneIdCustomerNumber is null)
            {
                throw new BaseException(ExceptionLiterals.BillIdNotFound);
            }

            BranchEventSummaryHeaderOutputDto branchHeader = await _sqlReportConnection.QueryFirstAsync<BranchEventSummaryHeaderOutputDto>(brachSummeryHeaderQueryString, new { billId });
            if (branchHeader is null)
            {
                throw new BaseException(ExceptionLiterals.BillIdNotFound);
            }
            branchHeader.ReportDateJalali = DateTime.Now.ToShortPersianDateString();
            branchHeader.Title = ReportLiterals.BranchEventSummary;

            IEnumerable<BranchEventSummaryDataOutputDto> branchData = await _sqlReportConnection.QueryAsync<BranchEventSummaryDataOutputDto>(brachSummeryDataQueryString, new { zoneId = zoneIdCustomerNumber.ZoneId, customerNumber = zoneIdCustomerNumber.CustomerNumber });
            IEnumerable < BranchEventSummaryDataOutputDto > branchDateOrder = branchData.OrderBy(t => t.RegisterDateJalali);
         
            long lastRemained = 0;
            for (int i = 0; i < branchDateOrder.Count(); i++)
            {
                BranchEventSummaryDataOutputDto row = branchDateOrder.ElementAt(i);
                lastRemained = lastRemained + (row.DebtAmount - row.CreditAmount - row.DiscountAmount);
                row.Remained = lastRemained;
            }
            ReportOutput<BranchEventSummaryHeaderOutputDto, BranchEventSummaryDataOutputDto> result = new(ReportLiterals.BranchEventSummary, branchHeader, branchDateOrder);
            return result;
        }
        private string GetZoneIdAndCustomerNumberQuery()
        {
            return @"Select 
                    	c.ZoneId,
                    	c.CustomerNumber
                    From [CustomerWarehouse].dbo.Clients c
                    Where 
                    	c.ToDayJalali IS NULL AND
                    	c.BillId=@billId";
        }
        private string GetBrachEventSummaryHeaderQuery()
        {
            return @"Select 
                    	c.FirstName ,
                    	c.SureName AS Surname,
                    	TRIM(c.FirstName) + ' ' + TRIM(c.SureName) AS FullName,
                    	c.ZoneTitle ,
                    	c.BillId,
                    	c.ReadingNumber,
                    
                    	c.UsageTitle,
                    	'' AS JobTitle,
                    	c.GuildTitle,
                    
                    	c.DomesticCount AS DomesticUnit,
                    	c.CommercialCount AS CommercialUnit,
                    	c.OtherCount AS OtherUnit,
                    	c.WaterDiameterTitle AS MeterDiameterTitle,
                    	MainSiphonTitle AS SiphonDiameterTitle 
                    From [CustomerWarehouse].dbo.Clients c
                    Where 
                    	c.ToDayJalali IS NULL AND
                    	c.BillId=@billId";
        }
        private string GetBrachEventSummaryDataQuery()
        {
            return @"Select 
                    	r.UsageTitle AS UsageTitle,
                    	r.UsageId AS UsageId,
                    	r.TrackNumber,
                    	r.RegisterDate collate SQL_Latin1_General_CP1_CI_AS AS RegisterDateJalali,
                        IIF(r.Amount>0 AND TypeCode NOT IN (3,4,6) , r.Amount, 0) DebtAmount,
                        IIF(r.FinalAmount>0, r.FinalAmount,0) AS AmountAfterDiscount,  
                    	IIF(TypeCode in (3,4,6) AND r.FinalAmount<0, -1*r.FinalAmount, IIF(r.finalAmount<0,r.FinalAmount,0)) AS CreditAmount,                    	                     
                    	'' AS BankDateJalali,
                    	'' AS BankName,
                    	r.ItemTitle+'('+r.TypeId+')' AS Description	,
						0 AS BankCode, 
                        r.OffAmount as DiscountAmount,
                        IIF(r.OffAmount<>0, r.OffTitle,'') as DiscountTitle
                    From [CustomerWarehouse].dbo.RequestBillDetails r
                    Where
                    	r.CustomerNumber=@customerNumber AND 
                    	r.ZoneId=@zoneId AND
                    	r.FinalAmount!=0
                    Union All
                    Select 
                    	'' AS UsageTitle,
                    	0 AS UsageId,
                    	'' AS TrackNumber,
                    	p.RegisterDay AS RegisterDateJalali,
                        0 AS DebtAmount,
                        0 AmountAfterDiscount,
                    	IIF(p.Amount>0,p.Amount,0) AS CreditAmount,                    	
                    	p.RegisterDay AS BankDateJalali,
                    	p.BankName,
                    	p.BankName +' - '+p.PaymentGateway AS Description,
						p.BankCode,
                        0 as DiscountAmount,
                        '' as DiscountTitle
                    From [CustomerWarehouse].dbo.PaymentsEn p
                    Where 
                    	p.CustomerNumber=@customerNumber AND 
                    	p.ZoneId=@zoneId AND
                    	p.Amount!=0";
        }
    }
}
