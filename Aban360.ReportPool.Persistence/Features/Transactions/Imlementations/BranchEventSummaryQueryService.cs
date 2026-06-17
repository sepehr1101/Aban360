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
            IEnumerable<BranchEventSummaryDataOutputDto> branchDateOrder = branchData.OrderBy(t => t.RegisterDateJalali);
            string dateCheck = await GetDateCheck(zoneIdCustomerNumber.ZoneId);
            long lastRemained = 0;
            for (int i = 0; i < branchDateOrder.Count(); i++)
            {
                BranchEventSummaryDataOutputDto row = branchDateOrder.ElementAt(i);
                lastRemained = lastRemained + (row.DebtAmount - row.CreditAmount - row.DiscountAmount);
                row.Remained = lastRemained;
                row.IsRemovable = GetIsRemovable(dateCheck, row.RegisterDateJalali, row.TypeCode);
            }

            ReportOutput<BranchEventSummaryHeaderOutputDto, BranchEventSummaryDataOutputDto> result = new(ReportLiterals.BranchEventSummary, branchHeader, branchDateOrder);
            return result;
        }
        private async Task<string> GetDateCheck(int zoneId)
        {
            string dbName = GetDbName(zoneId);
            string query = GetDateCheckQuery(dbName);
            string? dateCheck = await _sqlReportConnection.QueryFirstOrDefaultAsync<string>(query, new { zoneId });
            if (string.IsNullOrWhiteSpace(dateCheck))
            {
                throw new InvalidDataException(ExceptionLiterals.InvalidDateCheckFormat);
            }
            return dateCheck;
        }
        private bool GetIsRemovable(string dateCheck, string registerDateJalali, int typeCode)
        {
            int[] allowedRemovableTypeCode = { 4, 5, 6 };

            if (registerDateJalali.CompareTo(dateCheck) < 0 ||
                DateTime.Now.AddDays(-7).ToShortPersianDateString().CompareTo(registerDateJalali) > 0 ||
               !allowedRemovableTypeCode.Contains(typeCode))
            {
                return false;
            }
            return true;
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
                    	TRIM(c.FirstName) AS FirstName ,
                    	TRIM(c.SureName) AS Surname,
                    	TRIM(c.FirstName) + ' ' + TRIM(c.SureName) AS FullName,
                    	c.ZoneTitle ,
                    	c.BillId,
                    	TRIM(c.ReadingNumber) ReadingNumber,
                    
                    	c.UsageTitle,
                    	'' AS JobTitle,
                    	c.GuildTitle,
                    
                    	c.DomesticCount AS DomesticUnit,
                    	c.CommercialCount AS CommercialUnit,
                    	c.OtherCount AS OtherUnit,
                    	c.WaterDiameterTitle AS MeterDiameterTitle,
                    	MainSiphonTitle AS SiphonDiameterTitle ,
						c.DeletionStateTitle DeletionStateTitle,
						c.BranchType BranchTypeTitle
                    From [CustomerWarehouse].dbo.Clients c
                    Where 
                    	c.ToDayJalali IS NULL AND
                    	c.BillId=@billId";
        }
        private string GetBrachEventSummaryDataQuery()
        {
            return @"Select 
                        r.Id,
                    	r.UsageTitle AS UsageTitle,
                    	r.UsageId AS UsageId,
                    	TRIM(r.TrackNumber) TrackNumber,
                    	r.RegisterDate collate SQL_Latin1_General_CP1_CI_AS AS RegisterDateJalali,
                        IIF(r.Amount>0 AND TypeCode NOT IN (3,4,6) , r.Amount, 0) DebtAmount,
                        IIF(r.FinalAmount>0, r.FinalAmount,0) AS AmountAfterDiscount,  
                    	IIF(TypeCode in (3,4,6) AND r.FinalAmount<0, -1*r.FinalAmount, IIF(r.finalAmount<0,r.FinalAmount,0)) AS CreditAmount,                    	                     
                    	'' AS BankDateJalali,
                    	'' AS BankName,
                    	ISNULL(r.ItemTitle,N'')+N' '+ISNULL(r.TypeId,'') AS Description	,
						0 AS BankCode, 
                        r.OffAmount as DiscountAmount,
                        r.OffTitle as DiscountTitle,
                        r.TypeCode
                    From [CustomerWarehouse].dbo.RequestBillDetails r
                    Where
                    	r.CustomerNumber=@customerNumber AND 
                    	r.ZoneId=@zoneId AND
                    	r.FinalAmount!=0
                    Union All
                    Select 
                        p.Id,
                    	'' AS UsageTitle,
                    	0 AS UsageId,
                    	'' AS TrackNumber,
                    	p.RegisterDay AS RegisterDateJalali,
                        IIF(p.Amount>0, 0, -1*p.Amount) AS DebtAmount,
                        IIF(p.Amount>0, 0, -1*p.Amount) AmountAfterDiscount,
                    	IIF(p.Amount>0,p.Amount,0) AS CreditAmount,                    	
                    	p.RegisterDay AS BankDateJalali,
                    	p.BankName,
                    	p.BankName +' - '+p.PaymentGateway AS Description,
						p.BankCode,
                        0 as DiscountAmount,
                        '' as DiscountTitle,
                        -1 TypeCode
                    From [CustomerWarehouse].dbo.PaymentsEn p
                    Where 
                    	p.CustomerNumber=@customerNumber AND 
                    	p.ZoneId=@zoneId AND
                    	p.Amount!=0";
        }
        private string GetDateCheckQuery(string dbName)
        {
            return $@"Select date_check
                    From [{dbName}].dbo.variab 
                    Where town=@zoneId";
        }
    }
}
