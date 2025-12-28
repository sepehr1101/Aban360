using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Domain.Features.Transactions;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.Transactions.Contracts
{
    public interface IBranchEventSummaryWithLastDbQueryService
    {
        Task<ReportOutput<BranchEventSummaryHeaderOutputDto, BranchEventSummaryDataOutputDto>> Get(CardexInputDto input);
    }
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
                    	m.name FirstName ,
                    	m.family AS Surname,
                    	TRIM(m.name) + ' ' + TRIM(m.family) AS FullName,
                    	t51.C2 ZoneTitle ,
                    	m.bill_id BillId,
                    	m.eshtrak ReadingNumber,
                    
                    	t41.C2 UsageTitle,
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
                    	End as SiphonDiameterTitle
                    From [{dbName}].dbo.members m
                    Join [Db70].dbo.T51 t51
                    	On m.town=t51.C0
                    Join [Db70].dbo.T41 t41
                    	On m.cod_enshab=t41.C0
                    Join [Db70].dbo.T5 t5
                    	ON m.enshab=t5.C0
                    Where 
                    	m.town=@zoneId AND
                    	m.radif=@customerNumber";
        }
        private string GetBrachEventSummaryDataQuery(string dbName)
        {
            return $@"";
        }
    }
}
