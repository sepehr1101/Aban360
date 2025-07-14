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
				//todo: 1.use Literal 2. Use Custom Exception
				throw new InvalidDataException("کد درخواست ناصحیح است");
			}

            string prepaymentCustomerHeaderQueryString = GetPrepaymentCustomerHeaderQuery(zoneId.Value);
            PrepaymentAndCalculationCustomerHeaderOutputDto? requestHeader = await _sqlReportConnection.QueryFirstOrDefaultAsync<PrepaymentAndCalculationCustomerHeaderOutputDto>(prepaymentCustomerHeaderQueryString, new { zoneId = zoneId.Value, parNoId = input.Input });
			if(requestHeader is null)
			{
                //todo: 1.use Literal 2. Use Custom Exception
                throw new InvalidDataException("کد درخواست ناصحیح است");
            }

            string installmentHeaderQuery = GetPrepaymentInstallmentHeaderQuery(zoneId.Value);
            PrepaymentAndCalculationInstallmentHeaderOutputDto? installmentHeader = await _sqlReportConnection.QueryFirstOrDefaultAsync<PrepaymentAndCalculationInstallmentHeaderOutputDto>(installmentHeaderQuery, new { zoneId = zoneId.Value, parNoId = input.Input });
            if (installmentHeader is null)
            {
                //todo: 1.use Literal 2. Use Custom Exception
                throw new InvalidDataException("درخواست ثبت قطعی نشده است");
            }

            string prepaymentDataQuery = GetPrepaymentDataQuery(zoneId.Value);
            IEnumerable<PrepaymentAndCalculationDataOutputDto> data = await _sqlReportConnection.QueryAsync<PrepaymentAndCalculationDataOutputDto>(prepaymentDataQuery, new { zoneId = zoneId.Value, parNoId = input.Input });
            if (prepaymentDataQuery is null)
            {
                //todo: 1.use Literal 2. Use Custom Exception
                throw new InvalidDataException("محاسبه انجام نشده است");
            }

			//todo: move it to another private method or object constructor
            PrepaymentAndCalculationHeaderOutputDto header = new()
			{
				CustomerHeader = requestHeader,
				InstallmentHeader = installmentHeader,
				SumItemsAmount = data.Sum(x => x.Amount),
				SumItemsDiscount = data.Sum(x => x.Discount),
				DebtorOrCreditorAmount = 0,
				Description = string.Empty,// todo: get from desc/sharh of moshtrak table
				PaymentDateJalali=string.Empty, //todo: fill it 
				GetwayToPaied=string.Empty, //todo: rename field and get data
				InstallmentCount = data.First().InstallmentCount,
				InstallmentNumber = data.First().InstallmentNumber,
			};

			var result = new ReportOutput<PrepaymentAndCalculationHeaderOutputDto, PrepaymentAndCalculationDataOutputDto>(ReportLiterals.PrepaymentAndCalculation, header, data);
			return result;
        }

        private string GetZoneIdWithParNoQuery()
        {
            return @"Select r.ZoneId
					From [CustomerWarehouse].dbo.Requests r
					Where TRIM(r.TrackNumber)=@parNoId";
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
					     			IIF(ISNULL(m.s0, 0) = 0 AND ISNULL(m.s1, 0) = 0,N'پس از فروش',N'هیچی'))) AS ServiceDescription
					
						From [{GetDbName(zoneId)}].dbo.moshtrak m
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
					From [{GetDbName(zoneId)}].dbo.ghest g
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
					From [{GetDbName(zoneId)}].dbo.karten75 k
					Where
						k.par_no=@parNoId AND
						k.town =@zoneId";
        }
    }
}
