using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Domain.Features.Transactions;
using Aban360.ReportPool.Persistence.Features.Transactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.Transactions.Imlementations
{
    internal sealed class BranchEventSummaryWithLastDbQueryService : AbstractBaseConnection, IBranchEventSummaryWithLastDbQueryService
    {
        public BranchEventSummaryWithLastDbQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<BranchEventSummaryHeaderOutputDto, BranchEventSummaryDataOutputDto>> Get(CardexInputDto input)
        {
            string dbName = GetDbName(input.ZoneId);
            string brachSummeryHeaderQueryString = GetBrachEventSummaryHeaderQuery(dbName);
            string brachSummeryDataQueryString = GetBrachEventSummaryDataQuery(dbName);

            BranchEventSummaryHeaderOutputDto branchHeader = await _sqlReportConnection.QueryFirstAsync<BranchEventSummaryHeaderOutputDto>(brachSummeryHeaderQueryString, input);
            if (branchHeader is null)
            {
                throw new BaseException(ExceptionLiterals.BillIdNotFound);
            }
            branchHeader.ReportDateJalali = DateTime.Now.ToShortPersianDateString();
            branchHeader.Title = ReportLiterals.BranchEventSummary;

            IEnumerable<BranchEventSummaryDataOutputDto> branchData = await _sqlReportConnection.QueryAsync<BranchEventSummaryDataOutputDto>(brachSummeryDataQueryString, input);
            IEnumerable<BranchEventSummaryDataOutputDto> branchDateOrder = branchData.OrderBy(t => t.RegisterDateJalali);
            branchHeader.RowCount = branchData?.Count() ?? 0;

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
        private string GetBrachEventSummaryHeaderQuery(string dbName)
        {
            return $@"Select 
                    	TRIM(m.name) FirstName ,
                    	TRIM(m.family) AS Surname,
                    	TRIM(m.name) + ' ' + TRIM(m.family) AS FullName,
                    	t51.C2 ZoneTitle ,
                    	m.bill_id BillId,
                    	TRIM(m.eshtrak) ReadingNumber,
                    
                    	t41.C1 UsageTitle,
                    	'' AS JobTitle,
                    	'' GuildTitle,--todo
                    
                    	m.tedad_mas AS DomesticUnit,
                    	m.tedad_tej AS CommercialUnit,
                    	m.tedad_vahd AS OtherUnit,
                    	t5.C2 AS MeterDiameterTitle,
                    	Case When m.sif_1>0 Then N'قطر 100'
                    		When m.sif_2>0 Then N'قطر 125'
                    		When m.sif_3>0 Then N'قطر 150'
                    		When m.sif_4>0 Then N'قطر 200'
                    		When m.sif_5>0 Then N'قطر 5'
                    		When m.sif_6>0 Then N'قطر 6'
                    		When m.sif_7>0 Then N'قطر 7'
                    		When m.sif_8>0 Then N'قطر 8'
                    		Else N'ندارد'
                    	End as SiphonDiameterTitle,
						t7.c1 branchTypeTitle,
						t46.C2 RegionTitle
                    From [{dbName}].dbo.members m
                    Join [Db70].dbo.T51 t51
                    	On m.town=t51.C0
                    Join [Db70].dbo.T46 t46
                    	On t51.c1=t46.c0
                    Join [Db70].dbo.T41 t41
                    	On m.cod_enshab=t41.C0
                    Join [Db70].dbo.T5 t5
                    	ON m.enshab=t5.C0
                    Join [Db70].dbo.T7 t7
                    	ON m.noe_va=t7.C0
                    Where 
                    	m.town=@zoneId AND
                    	m.radif=@customerNumber";
        }
        private string GetBrachEventSummaryDataQuery(string dbName)
        {
            return $@"Select 
                    	t41.c1 UsageTitle,
                    	t41.c0 UsageId,
                    	k.par_no TrackNumber,
                    	k.date RegisterDateJalali,
                        IIF(k.pard>0 AND k.type NOT IN (3,4,6) , k.pard, 0) DebtAmount,
                        IIF(k.pard>0 AND k.type NOT IN (3,4,6) , k.pard, 0) AS AmountAfterDiscount,--todo  
                        IIF(k.type in (3,4,6) , k.pard, 0) AS CreditAmount,                  	                     
                        '' AS BankDateJalali,
                        '' AS BankName,
                        t100.c1 AS Description	,
                    	 0 AS BankCode, 
                        k.takhfif as DiscountAmount,
                        IIF(k.cod_takh<>0, t15.c1,'') as DiscountTitle
                    From [{dbName}].dbo.Karten75 k
                    Join [Db70].dbo.T41 t41
                    	ON k.cod_enshab=t41.c0
                    Join [Db70].dbo.T15 t15  
                    	ON k.cod_takh=t15.c0
                    Join [Db70].dbo.T100 t100
                    	ON k.noe_bed=t100.c0
                    Where	
                    	k.town=@zoneId AND
                    	k.radif=@customerNumber
                    Union All
                    Select 
                    	t41.c1 UsageTitle,
                    	t41.c0 UsageId,
                    	v.par_no AS TrackNumber,
                    	v.date_bes AS RegisterDateJalali,--todo
                    	0 AS DebtAmount,
                    	0 AmountAfterDiscount,
                    	IIF(v.pard>0,v.pard,0) AS CreditAmount,                    	
                    	v.date_bank AS BankDateJalali,
                    	v.cod_bank BankName,
                    	v.cod_bank collate Arabic_CI_AS +' - '+t150.C2 AS Description,
                    	v.cod_bank BankCode,
                    	0 as DiscountAmount,
                    	'' as DiscountTitle
                    From [{dbName}].dbo.vosolEN v
                    Join [Db70].dbo.T41 t41
                    	ON v.cod_enshab=t41.c0
                    Join [Db70].dbo.T150 t150
                    	ON v.type_pay collate Arabic_CI_AS=t150.c1
                    	Where 
                    	v.town=@zoneId AND
                    	v.radif=@customerNumber";
        }
    }
}
