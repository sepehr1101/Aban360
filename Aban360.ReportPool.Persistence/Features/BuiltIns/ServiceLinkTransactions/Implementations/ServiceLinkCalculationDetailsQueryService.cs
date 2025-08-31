using Aban360.Common.BaseEntities;
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


            //todo: PreviousItems OrderBy
            IEnumerable<SiphonDetailItemTitleDto> siphonItems=await _sqlReportConnection.QueryAsync< SiphonDetailItemTitleDto >(siphonItemsQueryString,new {zoneId=zoneId, parNoId =input.Input});
            ServiceLinkCalculationDetailsHeaderOutputDto header = await _sqlReportConnection.QueryFirstOrDefaultAsync<ServiceLinkCalculationDetailsHeaderOutputDto>(calculationHeaderInMostarakQueryString, new { parNoId = input.Input,zoneId=zoneId });
            header.BillId = await _sqlReportConnection.QueryFirstOrDefaultAsync<string>(billIdQueryString, new { zoneId = zoneId, parNoId = input.Input });
            header.PreviousItems = await _sqlReportConnection.QueryFirstOrDefaultAsync<ItemsHeaderOutputDto>(calculationHeaderInArchMemQueryString, new { requestDateJalali=header.RequestDateJalali, zoneId = zoneId });
            header.InheritedItems= await _sqlReportConnection.QueryFirstOrDefaultAsync<ItemsHeaderOutputDto>(calculationHeaderInMotherQueryString, new { parNoId = input.Input, zoneId = zoneId });
            header.SiphonDetails = siphonItems;
            header.CurrentItems = GetCurrentItems(header);
            //todo: Create Second Dto in Application to remove duplicate Prop


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

        private ItemsHeaderOutputDto GetCurrentItems(ServiceLinkCalculationDetailsHeaderOutputDto header)
        {
            ItemsHeaderOutputDto currentItems = new ItemsHeaderOutputDto();
            currentItems.Primises = header.CurrentPrimises;
            currentItems.ImprovementOverall = header.CurrentImprovementOverall;
            currentItems.ImprovementCommericial = header.CurrentImprovementCommericial;
            currentItems.ImprovementDomestic = header.CurrentImprovementDomestic;
            currentItems.ImprovementOther = header.CurrentImprovementOther;
            currentItems.UnitCommericial = header.CurrentUnitCommericial;
            currentItems.UnitDomestic = header.CurrentUnitDomestic;
            currentItems.UnitOther = header.CurrentUnitOther;
            currentItems.ContractualCapacity = header.CurrentContractualCapacity;
            currentItems.SumPremisesImprovement = header.SumCurrentPremisesImprovement;

            return currentItems;
        }

        private string GetCalculationDetailsDataQuery(string dataBaseName)
        {
            return @$"select
                    	k.noe_bed AS ItemId,
						t.C1 AS ItemTitle,
                    	k.pard AS Amount,
                    	k.takhfif AS DiscountAmount,
                        k.pish_gest AS InstallmentAmount,
                        k.tedad_gest AS InstallmentCount ,
                        k.type AS TypeId
                    From [{dataBaseName}].dbo.karten75 k
                    Join [Db70].dbo.T100 t on k.noe_bed=t.C0
                    Where	
                    	k.par_no=@parNoId AND
                    	k.town=@zoneId";
        }

        private string GetCalculationHeaderInfoMotherQuery(string dataBaseName)
        {
            return @$"Select 
                    	m.arse AS Primises,
                    	m.aian AS ImprovementOverall,
                    	m.aian_tej AS ImprovementCommericial,
                    	m.aian_mas AS ImprovementDomestic,
                    	m.aian-(m.aian_tej+m.aian_mas) AS ImprovementOther,
                    	m.tedad_tej AS UnitCommericial,
                    	m.tedad_mas AS UnitDomestic,
                    	m.tedad_vahd AS UnitOther,
                    	m.fix_mas AS ContractualCapacity,
                    	m.aian + m.arse AS SumPremisesImprovement,
                        m.mother_rad AS InheritedFromCustomerNumber
                    From [{dataBaseName}].dbo.mother m
                    Where	
                    	m.par_no=@parNoId AND
                    	m.town=@zoneId";
        }
        private string GetCalculationHeaderInfoArchMemQuery(string dataBaseName)
        {
            return @$"Select Top 1
                    	m.arse AS Primises,
                    	m.aian AS ImprovementOverall,
                    	m.aian_tej AS ImprovementCommericial,
                    	m.aian_mas AS ImprovementDomestic,
                    	m.aian-(m.aian_tej+m.aian_mas) AS ImprovementOther,
                    	m.tedad_tej AS UnitCommericial,
                    	m.tedad_mas AS UnitDomestic,
                    	m.tedad_vahd AS UnitOther,
                    	m.fix_mas AS ContractualCapacity,
                    	m.aian + m.arse AS SumPremisesImprovement
                    From [{dataBaseName}].dbo.arch_mem m
                    Where
                    	m.town=@zoneId AND
                    	m.date_roz<@requestDateJalali
                    Order By m.date_roz Desc";
        }
        private string GetCalculatinoHeaderInMoshtarahQuery(string dataBaseName)
        {
            return @$"Select
                    	z.C2 AS ZoneTitle, 
                    	m.town AS ZoneId,
                    	TRIM(m.name)+' ' +TRIM(m.family) AS FullName,
                    	m.meli_cod AS NationalCode,
                    	m.phone_no AS PhoneNumber,
                    	m.post_cod AS PostalCode, 
                    	TRIM(m.address) AS Address,
                    	m.BLOCK_COD AS ReadingBlock,
                    	m.cod_enshab AS MeterDiameterTitle,
                    	
                    	m.radif AS CustomerNumber,
                    	m.eshtrak AS ReadingNumber,
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
                    	TRIM(g.sh_ghabs1) AS BillId
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
