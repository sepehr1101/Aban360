using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Implementations
{
    internal sealed class ServiceLinkCalculationDetailsQueryService : AbstractBaseConnection, IServiceLinkCalculationDetailsQueryService
    {
        public ServiceLinkCalculationDetailsQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<ServiceLinkCalculationDetailsHeaderOutputDto, ServiceLinkCalculationDetailsDataOutputDto>> GetInfo(ServiceLinkCalculationDetailsInputDto input)
        {
            string zoneIdQueryString = GetZoneIdQuery();
            int? zoneId = await _sqlReportConnection.QueryFirstOrDefaultAsync<int?>(zoneIdQueryString, new { parNoId = input.Input });
            if (!zoneId.HasValue)
            {
                throw new BaseException(ExceptionLiterals.InvalidRequestData);
            }
            string dataBaseName=GetDbName(zoneId.Value);

            string siphonItemsQueryString = GetSiphonItemsQuery(dataBaseName);
            string billIdQueryString = GetBillIdQuery(dataBaseName);
            string calculationHeaderInMostarakQueryString = GetCalculatinoHeaderInMoshtarahQuery(dataBaseName);
            string calculationHeaderInArchMemQueryString = GetCalculationHeaderInfoArchMemQuery(dataBaseName);
            string calculationHeaderInMotherQueryString= GetCalculationHeaderInfoMotherQuery(dataBaseName);
            string calculationDetailsDataInfoQuery = GetCalculationDetailsDataQuery(dataBaseName);


            IEnumerable<SiphonDetailItemTitleDto> siphonItems=await _sqlReportConnection.QueryAsync< SiphonDetailItemTitleDto >(siphonItemsQueryString,new {zoneId=zoneId, parNoId =input.Input});
            ServiceLinkCalculationDetailsHeaderOutputDto header = await _sqlReportConnection.QueryFirstOrDefaultAsync<ServiceLinkCalculationDetailsHeaderOutputDto>(calculationHeaderInMostarakQueryString, new { parNoId = input.Input,zoneId=zoneId });
            header.BillId = await _sqlReportConnection.QueryFirstOrDefaultAsync<string>(billIdQueryString, new { zoneId = zoneId, parNoId = input.Input });
            header.PreviousItems = await _sqlReportConnection.QueryFirstOrDefaultAsync<PreviousItemsHeaderOutpuDto>(calculationHeaderInArchMemQueryString, new { parNoId = input.Input, zoneId = zoneId });
            header.InheritedItems= await _sqlReportConnection.QueryFirstOrDefaultAsync<InheritedItemsHeaderOutpuDto>(calculationHeaderInMotherQueryString, new { parNoId = input.Input, zoneId = zoneId });
            header.SiphonDetails = siphonItems;


            IEnumerable<ServiceLinkCalculationDetailsDataOutputDto> calculationDetailsData = await _sqlReportConnection.QueryAsync<ServiceLinkCalculationDetailsDataOutputDto>(calculationDetailsDataInfoQuery, new { parNoId = input.Input ,zoneId=zoneId});
            if (calculationDetailsData is not null && calculationDetailsData.Any())
            {

                header.InstallmentAmount = calculationDetailsData.FirstOrDefault().InstallmentAmount;
                header.InstallmentCount = calculationDetailsData.FirstOrDefault().InstallmentCount;
                header.ReportDateJalali = DateTime.Now.ToShortPersianDateString();
                header.SumItemAmount = calculationDetailsData.Sum(x => x.Amount);
            }
            var result = new ReportOutput<ServiceLinkCalculationDetailsHeaderOutputDto, ServiceLinkCalculationDetailsDataOutputDto>(ReportLiterals.ServiceLinkCalculationDetails, header, calculationDetailsData );
            return result;
        }

        private string GetCalculationDetailsDataQuery(string dataBaseName)
        {
            return @$"select
                    	k.noe_bed AS ItemId,
                    	k.pard AS Amount,
                    	k.takhfif AS DiscountAmount,
                        k.pish_gest AS InstallmentAmount,
                        k.tedad_gest AS InstallmentCount ,
                        k.type AS TypeId
                    From [{dataBaseName}].dbo.karten75 k
                    Where	
                    	k.par_no=@parNoId AND
                    	k.town=@zoneId";
        }

        private string GetCalculationHeaderInfoMotherQuery(string dataBaseName)
        {
            return @$"Select 
                    	m.arse AS InheritedPrimises,
                    	m.aian AS InheritedImprovementOverall,
                    	m.aian_tej AS InheritedImprovementCommericial,
                    	m.aian_mas AS InheritedImprovementDomestic,
                    	m.aian-(m.aian_tej+m.aian_mas) AS InheritedImprovementOther,
                    	m.tedad_tej AS InheritedUnitCommericial,
                    	m.tedad_mas AS InheritedUnitDomestic,
                    	m.tedad_vahd AS InheritedUnitOther,
                    	m.fix_mas AS InheritedContractualCapacity,
                    	m.aian + m.arse AS SumInheritedPremisesImprovement,
                        m.mother_rad AS InheritedFromCustomerNumber
                    From [{dataBaseName}].dbo.mother m
                    Where	
                    	m.par_no=@parNoId AND
                    	m.town=@zoneId";
        }
        private string GetCalculationHeaderInfoArchMemQuery(string dataBaseName)
        {
            return @$"Select 
                    	m.arse AS PreviousPrimises,
                    	m.aian AS PreviousImprovementOverall,
                    	m.aian_tej AS PreviousImprovementCommericial,
                    	m.aian_mas AS PreviousImprovementDomestic,
                    	m.aian-(m.aian_tej+m.aian_mas) AS PreviousImprovementOther,
                    	m.tedad_tej AS PreviousUnitCommericial,
                    	m.tedad_mas AS PreviousUnitDomestic,
                    	m.tedad_vahd AS PreviousUnitOther,
                    	m.fix_mas AS PreviousContractualCapacity,
                    	m.aian + m.arse AS SumPreviousPremisesImprovement
                    
                    From [{dataBaseName}].dbo.arch_mem m
                    Where	
                    	m.par_no=@parNoId AND
                    	m.town=@zoneId";
        }
        private string GetCalculatinoHeaderInMoshtarahQuery(string dataBaseName)
        {
            return @$"Select
                    	z.C2 AS ZoneTitle, 
                    	m.town AS ZoneId,
                    	m.name+' ' +m.family AS FullName,
                    	m.meli_cod AS NationalCode,
                    	m.phone_no AS PhoneNumber,
                    	m.post_cod AS PostalCode, 
                    	m.address AS Address,
                    	'' AS ServiceDescription, --
                    	m.BLOCK_COD AS ReadingBlock,
                    	m.cod_enshab AS MeterDiameterTitle,
                    	
                    	m.radif AS CustomerNumber,
                    	m.eshtrak AS ReadingNumber,
                    	'' AS BillId,--
                    	m.barge AS PageNumber,
                    	m.par_no AS RequestNumber,
                    	m.date_ask AS RequestDateJalali,
                    	m.date_sabt AS RegisterDatejalali,
                    	 
                    	m.arse AS CurrentPrimises,
                    	m.aian AS CurrentImprovementOverall,
                    	m.aian_tej AS CurrentImprovementCommericial,
                    	m.aian_mas AS CurrentImprovementDomestic,
                    	m.aian-(m.aian_tej+m.aian_mas) AS CurrentImprovementOther,
                    	m.tedad_mas AS CurrentUnitDomestic,
                    	m.tedad_tej AS CurrentUnitCommericial,
                    	m.tedad_vahd AS CurrentUnitOther,
                    	m.fix_mas AS CurrentContractualCapacity,
                    	m.aian + m.arse AS SumCurrentPremisesImprovement,
                    	0 AS InheritedFromCustomerNumber,

                        m.noe_va AS UseStateId,
                        u.C1 AS UseStateTitle,
                        m.cod_enshab AS UsageId,
                        t.C1 AS UsageTitle,
                       	m.sharh AS Description,
                       	IIF(m.s0>0 , N'انشعاب جدید' ,
                       		IIF(m.s1>0,N'فاضلاب جدید',
                       			IIF(ISNULL(m.s0, 0) = 0 AND ISNULL(m.s1, 0) = 0,N'پس از فروش',N'هیچی')))AS ServiceDescription
                    
                    From [{dataBaseName}].dbo.moshtrak m 
                    Join [Db70].dbo.T41 t On m.cod_enshab=t.C0
                    Join [Db70].dbo.T7 u On m.noe_va=u.C0
                    Join [Db70].dbo.T51 z On m.town=z.C0
                    Where
                    	m.par_no=@parNoId AND
                    	m.town=@zoneId";
        }

        private string GetSiphonItemsQuery(string dataBaseName)
        {
            return @$"select
                    	S.item AS SiphonType,
                    	S.value AS Count
                    From [{dataBaseName}].dbo.moshtrak m
                    Cross Apply
                    (
                    	values
                    	(m.sif_1 ,N'100'),
                    	(m.sif_2 ,N'125'),
                    	(m.sif_3 ,N'150'),
                    	(m.sif_4 ,N'200'),
                    	(m.sif_5 ,N'5'),
                    	(m.sif_6 ,N'6'),
                    	(m.sif_7 ,N'7'),
                    	(m.sif_8 ,N'8')
                    )S(value, item)
                    Where m.par_no=@parNoId";
        }

        private string GetBillIdQuery(string dataBaseName)
        {
            return @$"Select top 1
                    	g.sh_ghabs1 AS BillId
                    From [{dataBaseName}].dbo.ghest g
                    Where 
                    	g.par_no=@parNoId AND
                    	g.TOWN=@zoneId";
        }
        
        private string GetZoneIdQuery()
        {
            return @"Select r.ZoneId
					From [CustomerWarehouse].dbo.Requests r
					Where r.TrackNumber=@parNoId";
        }
    }
}
