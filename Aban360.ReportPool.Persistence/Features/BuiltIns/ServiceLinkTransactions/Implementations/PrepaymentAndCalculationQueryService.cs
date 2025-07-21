using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
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
        { 
		}

        public async Task<ReportOutput<PrepaymentAndCalculationHeaderOutputDto, PrepaymentAndCalculationDataOutputDto>> GetInfo(PrepaymentAndCalculationInputDto input)
        {
            string zoneIdQueryString = GetZoneIdWithParNoQuery();
            int? zoneId = await _sqlReportConnection.QueryFirstOrDefaultAsync<int?>(zoneIdQueryString, new { parNoId = input.Input });
			if(!zoneId.HasValue)
			{
				throw new BaseException(ExceptionLiterals.InvalidRequestData);
			}
            string DataBaseName = GetDbName(zoneId.Value);

            string prepaymentCustomerHeaderQueryString = GetPrepaymentCustomerHeaderQuery(DataBaseName);
            PrepaymentAndCalculationCustomerHeaderOutputDto? requestHeader = await _sqlReportConnection.QueryFirstOrDefaultAsync<PrepaymentAndCalculationCustomerHeaderOutputDto>(prepaymentCustomerHeaderQueryString, new { zoneId = zoneId.Value, parNoId = input.Input });
			if(requestHeader is null)
			{
                throw new BaseException(ExceptionLiterals.InvalidRequestData);
            }

            string installmentHeaderQuery = GetPrepaymentInstallmentHeaderQuery(DataBaseName);
            PrepaymentAndCalculationInstallmentHeaderOutputDto? installmentHeader = await _sqlReportConnection.QueryFirstOrDefaultAsync<PrepaymentAndCalculationInstallmentHeaderOutputDto>(installmentHeaderQuery, new { zoneId = zoneId.Value, parNoId = input.Input });
            if (installmentHeader is null)
            {
                throw new BaseException(ExceptionLiterals.UnconfirmedRequest);
            }

            string prepaymentDataQuery = GetPrepaymentDataQuery(DataBaseName);
            IEnumerable<PrepaymentAndCalculationDataOutputDto> data = await _sqlReportConnection.QueryAsync<PrepaymentAndCalculationDataOutputDto>(prepaymentDataQuery, new { zoneId = zoneId.Value, parNoId = input.Input });
            if (prepaymentDataQuery is null)
            {
                throw new BaseException(ExceptionLiterals.NotCalculation);
            }

            PrepaymentAndCalculationHeaderOutputDto header = GetHeader(data,installmentHeader,requestHeader);


            var result = new ReportOutput<PrepaymentAndCalculationHeaderOutputDto, PrepaymentAndCalculationDataOutputDto>(ReportLiterals.PrepaymentAndCalculation, header, data);
			return result;
        }
		private PrepaymentAndCalculationHeaderOutputDto GetHeader(IEnumerable<PrepaymentAndCalculationDataOutputDto> data,
                                                                  PrepaymentAndCalculationInstallmentHeaderOutputDto installmentHeader,
                                                                  PrepaymentAndCalculationCustomerHeaderOutputDto requestHeader)
		{
            PrepaymentAndCalculationHeaderOutputDto header = new()
            {
                CustomerHeader = requestHeader,
                InstallmentHeader = installmentHeader,
                SumItemsAmount = data.Sum(x => x.Amount),
                SumItemsDiscount = data.Sum(x => x.Discount),
                DebtorOrCreditorAmount = 0,
                PaymentDateJalali = string.Empty, //todo: fill it 
                PaymentGetway = string.Empty, //todo: rename field and get data
                InstallmentCount = data.First().InstallmentCount,
                InstallmentNumber = data.First().InstallmentNumber,
            };
			return header;
        }

        private string GetZoneIdWithParNoQuery()
        {
            return @"Select r.ZoneId
					From [CustomerWarehouse].dbo.Requests r
					Where TRIM(r.TrackNumber)=@parNoId";
        }
        private string GetPrepaymentCustomerHeaderQuery(string dataBaseName)
        {
            return @$"Select
						z.C2 AS ZoneTitle,
						m.town AS ZoneId,
						TRIM(m.name) AS FirstName,
						TRIM(m.family) AS Surname,
						TRIM(m.name) + ' ' + TRIM(m.family) AS FullName,
						m.post_cod AS PostalCode,
						TRIM(m.address) AS Address,
					
						m.cod_enshab AS UsageId,
						t.C1 AS UsageTitle,
						m.noe_va AS BranchTypeId,
						u.C1 AS BranchTypeTitle,
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
					
						m.sharh AS Description,
						m.BLOCK_COD AS ReadingBlock,
						IIF(m.s0>0 , N'انشعاب جدید' ,
					  		IIF(m.s1>0,N'فاضلاب جدید',
					     			IIF(ISNULL(m.s0, 0) = 0 AND ISNULL(m.s1, 0) = 0,N'پس از فروش',N'هیچی'))) AS ServiceDescription
					
						From [{dataBaseName}].dbo.moshtrak m
						Join [Db70].dbo.T41 t On m.cod_enshab=t.C0
						Join [Db70].dbo.T7 u On m.noe_va=u.C0
						Join [Db70].dbo.T51 z On m.town=z.C0
						Where 
							m.par_no=@parNoId AND
							m.town=@zoneId";
        }

        private string GetPrepaymentInstallmentHeaderQuery(string dataBaseName)
        {
            return $@"Select Top 1
						TRIM(g.sh_ghabs1) AS BillId,
						TRIM(g.sh_pard1) AS PaymentId,
						g.pard AS Payable,
						g.mohlat AS DueDataJalali
					From [{dataBaseName}].dbo.ghest g
					Where	
						g.par_no=@parNoId AND
						g.TOWN =@zoneId
					Order By g.mohlat ASC";
        }

        private string GetPrepaymentDataQuery(string dataBaseName)
        {
            return $@"Select 
						k.noe_bed AS ItemId,
						t.C1 AS ItemTitle,
						k.pard AS Amount,
						k.takhfif AS Discount,
						1 AS InstallmentNumber,
						k.tedad_gest AS InstallmentCount
					From [{dataBaseName}].dbo.karten75 k
					Join [Db70].dbo.T100 t On k.noe_bed=t.C0
					Where
						k.par_no=@parNoId AND
						k.town =@zoneId
					Order By t.C1 ASC";
        }
    }
}
