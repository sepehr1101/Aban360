using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.Transactions;
using Aban360.ReportPool.Persistence.Features.Transactions.Contracts;
using Dapper;
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
            //todo: چک کردن مشابه پیش پرداخت و ریز محاسبه
            string zoneIdAndCustomerNumberQueryString = GetZoneIdAndCustomerNumberQuery();
            string brachSummeryHeaderQueryString = GetBrachEventSummaryHeaderQuery();
            string brachSummeryDataQueryString = GetBrachEventSummaryDataQuery();

            ZoneIdAndCustomerNumberOutputDto zoneIdCustomerNumber = await _sqlReportConnection.QueryFirstOrDefaultAsync<ZoneIdAndCustomerNumberOutputDto>(zoneIdAndCustomerNumberQueryString, new { billId });
            BranchEventSummaryHeaderOutputDto branchHeader = await _sqlReportConnection.QueryFirstAsync<BranchEventSummaryHeaderOutputDto>(brachSummeryHeaderQueryString, new { billId });
            IEnumerable<BranchEventSummaryDataOutputDto> branchData = await _sqlReportConnection.QueryAsync<BranchEventSummaryDataOutputDto>(brachSummeryDataQueryString, new { zoneId = zoneIdCustomerNumber.ZoneId, customerNumber = zoneIdCustomerNumber.CustomerNumber });

            ReportOutput<BranchEventSummaryHeaderOutputDto, BranchEventSummaryDataOutputDto> result = new(ReportLiterals.BranchEventSummary, branchHeader, branchData);
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
            //todo: SiphonDiameterTitle from mainSiphon 
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
                    	'' AS SiphonDiameterTitle 
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
                    	IIF(r.FinalAmount<0,r.FinalAmount,0) AS CreditAmount,
                    	IIF(r.FinalAmount>0,r.FinalAmount,0) AS DebtAmount,
                    	'' AS BankDateJalali,
                    	'' AS BankName,
                    	r.ItemTitle+'('+r.TypeId+')' AS Description	
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
                    	IIF(p.Amount<0,p.Amount,0)  AS CreditAmount,
                    	0 AS DebtAmount,
                    	p.RegisterDay AS BankDateJalali,
                    	p.BankName,
                    	p.BankName +' - '+p.PaymentGateway AS Description
                    From [CustomerWarehouse].dbo.PaymentsEn p
                    Where 
                    	p.CustomerNumber=@customerNumber AND 
                    	p.ZoneId=@zoneId AND
                    	p.Amount!=0";
        }

    }
}
