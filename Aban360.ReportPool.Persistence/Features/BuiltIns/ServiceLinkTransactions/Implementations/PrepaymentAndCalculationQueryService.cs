using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Implementations
{
    internal sealed class PrepaymentAndCalculationQueryService : AbstractBaseConnection, IPrepaymentAndCalculationQueryService
    {
        public PrepaymentAndCalculationQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<PrepaymentAndCalculationHeaderOutputDto, PrepaymentAndCalculationDataOutputDto>> GetInfo(PrepaymentAndCalculationInputDto input)
        {
            string zoneIdQueryString = GetZoneIdWithParNoQuery();
            int zoneId = await _sqlReportConnection.QueryFirstOrDefaultAsync<int>(zoneIdQueryString, new { parNoId = input.Input });

            string prepaymentCustomerHeaderQueryString = GetPrepaymentCustomerHeaderQuery(zoneId);
            string prepaymentInstallmentHeaderQueryString = GetPrepaymentInstallmentHeaderQuery(zoneId);
            string prepaymentDataueryString = GetPrepaymentDataQuery(zoneId);

            IEnumerable<PrepaymentAndCalculationDataOutputDto> data = await _sqlReportConnection.QueryAsync<PrepaymentAndCalculationDataOutputDto>(prepaymentDataueryString, new { zoneId = zoneId, parNoId = input.Input });

            PrepaymentAndCalculationCustomerHeaderOutputDto moshtrakHeader = await _sqlReportConnection.QueryFirstOrDefaultAsync<PrepaymentAndCalculationCustomerHeaderOutputDto>(prepaymentCustomerHeaderQueryString, new { zoneId = zoneId, parNoId = input.Input });
            PrepaymentAndCalculationInstallmentHeaderOutputDto ghestHeader = await _sqlReportConnection.QueryFirstOrDefaultAsync<PrepaymentAndCalculationInstallmentHeaderOutputDto>(prepaymentInstallmentHeaderQueryString, new { zoneId = zoneId, parNoId = input.Input });

			PrepaymentAndCalculationHeaderOutputDto header = new()
			{
				CustomerHeader = moshtrakHeader,
				InstallmentHeader = ghestHeader,


				SumItemsAmount = data.Sum(x => x.Amount),
				SumItemsDiscount = data.Sum(x => x.Discount),
				DebtorOrCreditorAmount = 0,
				Description = "",
				PaymentDateJalali="",
				GetwayToPaied="",
				InstallmentCount = data.FirstOrDefault().InstallmentCount,
				InstallmentNumber = data.FirstOrDefault().InstallmentNumber,
			};

			var result = new ReportOutput<PrepaymentAndCalculationHeaderOutputDto, PrepaymentAndCalculationDataOutputDto>(ReportLiterals.PrepaymentAndCalculation, header, data);
			return result;
        }

        private string GetZoneIdWithParNoQuery()
        {
            return @"Select r.ZoneId
					From [CustomerWarehouse].dbo.Requests r
					Where r.TrackNumber=@parNoId";
        }
        private string GetPrepaymentCustomerHeaderQuery(int zoneId)
        {
            return @$"Select
						'' AS ZoneTitle,
						m.town AS ZoneId,
						TRIM(m.name) AS FirstName,
						TRIM(m.family) AS Surname,
						TRIM(m.name) + ' ' + TRIM(m.family) AS FullName,
						m.post_cod AS PostalCode,
						TRIM(m.address) AS Address,
					
						m.cod_enshab AS UsageId,--todo:Title
						m.noe_va AS UseStateId,--todo:Title
						m.fix_mas AS ContractualCapacity,
						
						m.radif AS CustomerNumber,
						m.eshtrak AS ReadingNumber,
						m.barge AS PageNumber,
						m.par_no AS RequestNumber,
					
						m.tedad_tej AS UnitCommercial,
						m.tedad_mas AS UnitDomestic,
						m.tedad_vahd AS UnitOther,
						m.arse AS Primises,
						m.aian_tej AS  ImprovementsCommercial,
						m.aian_mas AS ImprovementsDomestic,
						m.aian_mas+m.aian_tej AS ImprovementsOverall,
					
						m.BLOCK_COD AS ReadingBlock,
						IIF(m.s0>0 , N'انشعاب جدید' ,
					  		IIF(m.s1>0,N'فاضلاب جدید',
					     			IIF(ISNULL(m.s0, 0) = 0 AND ISNULL(m.s1, 0) = 0,N'پس از فروش',N'هیچی')))AS ServiceDescription
					
					From [{zoneId}].dbo.moshtrak m
					Where 
						m.par_no=@parNoId AND
						m.town=@zoneId";
        }

        private string GetPrepaymentInstallmentHeaderQuery(int zoneId)
        {
            return $@"Select Top 1
						TRIM(g.sh_ghabs1) AS BillId,
						TRIM(g.sh_pard1) AS PaymentId,
						g.pard AS Payable,
						g.mohlat AS DueDataJalali
					From [{zoneId}].dbo.ghest g
					Where	
						g.par_no=@parNoId AND
						g.TOWN =@zoneId
					Order By g.mohlat ASC";
        }

        private string GetPrepaymentDataQuery(int zoneId)
        {
            return $@"Select 
						k.noe_bed AS ItemId,
						'' AS ItemTitle,
						k.pard AS Amount,
						k.takhfif AS Discount,
						1 AS InstallmentNumber,
						k.tedad_gest AS InstallmentCount
					From [{zoneId}].dbo.karten75 k
					Where
						k.par_no=@parNoId AND
						k.town =@zoneId";
        }


    }
}
