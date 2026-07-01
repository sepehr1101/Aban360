using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ClaimPool.Persistence.Features.Request.Queries.Implementations
{
    internal sealed class KartQueryService : AbstractBaseConnection, IKartQueryService
    {
        public KartQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<IEnumerable<CalculationRequestDisplayDataOutputDto>> Get(string stringTrackNumber, int zoneId)
        {
            string dbName = GetDbName(zoneId);
            string query = GetByTrackNumberQuery(dbName);
            IEnumerable<CalculationRequestDisplayDataOutputDto> data = await _sqlReportConnection.QueryAsync<CalculationRequestDisplayDataOutputDto>(query, new { stringTrackNumber });
            return data;
        }
        public async Task<CalculationRequestDisplayDataOutputDto> Get(int id, int zoneId)
        {
            string dbName = GetDbName(zoneId);
            string query = GetByIdQuery(dbName);
            CalculationRequestDisplayDataOutputDto data = await _sqlReportConnection.QueryFirstOrDefaultAsync<CalculationRequestDisplayDataOutputDto>(query, new { id });
            return data;
        }
        public async Task<IEnumerable<KartGetDto>> GetAll(string stringTrackNumber, int zoneId)
        {
            string dbName = GetDbName(zoneId);
            string query = GetAllByTrackNumberQuery(dbName);
            IEnumerable<KartGetDto> data = await _sqlReportConnection.QueryAsync<KartGetDto>(query, new { stringTrackNumber });
            return data;
        }
        public async Task<IEnumerable<KartGetDto>> GetAll(int customerNumber, int zoneId)
        {
            string dbName = GetDbName(zoneId);
            string query = GetAllByCustomeNumberQuery(dbName);
            IEnumerable<KartGetDto> data = await _sqlReportConnection.QueryAsync<KartGetDto>(query, new { customerNumber, zoneId });
            return data;
        }
        public async Task<IEnumerable<ServiceLinkUnconfirmedDataOutputDto>> GetTodayInfoByCustomerNumber(int customerNumber, int zoneId)
        {
            string dbName = GetDbName(zoneId);
            string query = GetByCustomeNumberQuery(dbName);
            IEnumerable<ServiceLinkUnconfirmedDataOutputDto> data = await _sqlReportConnection.QueryAsync<ServiceLinkUnconfirmedDataOutputDto>(query, new { customerNumber, zoneId });
            return data;
        }

        private string GetByTrackNumberQuery(string dbName)
        {
            return $@"Select 
                        k.id,
                    	t100.C1 Title,
                    	k.pard Amount,
                    	k.takhfif Discount,
                        k.cod_takh  DiscountTypeId,
						IIF(k.Serial=0,0,1) Removable
                    From [{dbName}].dbo.kart k
                    Join [Db70].dbo.T100 t100	
                    	ON k.noe_bed=t100.C0
                    where k.par_no=@stringTrackNumber";
        }
        private string GetByIdQuery(string dbName)
        {
            return $@"Select 
                            k.id,
                    	    t100.C1 Title,
                    	    k.pard Amount,
                    	    k.takhfif Discount,
                            k.cod_takh  DiscountTypeId,
						    IIF(k.Serial=0,0,1) Removable
                    From [{dbName}].dbo.kart k
                    Join [Db70].dbo.T100 t100	
                    	ON k.noe_bed=t100.C0
                    where k.id=@id";
        }
        private string GetAllByTrackNumberQuery(string dbName)
        {
            return $@"Select
                        id,
                    	town ZoneId,
                    	radif CustomerNumber,
                    	TRIM(eshtrak) ReadingNumber,
                    	par_no StringTrackNumber,
                    	serial Serial,
                       	date CurrentDateJalali,
                    	cod_takh DiscountTypeId,
                    	pard FinalAmount,
                    	takhfif DiscountAmount,
                       	pard_n  PardN,
                    	pard_g PardG,
                    	jam_ha Sum,
                    	type Type,
                    	noe_bed AmountItemId,
                       	ser Ser,
                    	enshab MeterDiameterId,
                    	siphon SiphonId,
                    	cod_enshab UsageId,
                    	sabt IsRegister,
                       	kol_hasene TotalServicesAmount,
                    	total TotalServicesAmount,
                    	pish_gest FirstInstallment,
                    	JGEST_FA JGEST_FA,
                    	pish_fa PishFa,
                       	drsd_gest InstallmentPercent,
                    	tedad_gest InstallmentCount,
                    	ghest Installment,
                    	date_bank BankDateJalali,
                       	operator Operator,
                    	tedad_mas DomesticUnit,
                    	tedad_tej CommercialUnit,
                    	tedad_vahd OtherUnit ,
                    	cat_cod KartTypeId,
                       	ICT_CO InsertedBy,
                    	barge Barge ,
                        mohlat DueDateJalali
                    From [{dbName}].dbo.kart
                    Where par_no=@stringTrackNumber
                    Order by date Asc";
        }
        private string GetAllByCustomeNumberQuery(string dbName)
        {
            return $@"Select
                        k.id,
                    	k.town ZoneId,
                    	k.radif CustomerNumber,
                    	TRIM(k.eshtrak) ReadingNumber,
                    	k.par_no StringTrackNumber,
                    	k.serial Serial,
                       	k.date CurrentDateJalali,
                    	k.cod_takh DiscountTypeId,
                    	k.pard FinalAmount,
                    	k.takhfif DiscountAmount,
                       	k.pard_n  PardN,
                    	k.pard_g PardG,
                    	k.jam_ha Sum,
                    	k.type Type,
                    	k.noe_bed AmountItemId,
                       	k.ser Ser,
                    	k.enshab MeterDiameterId,
                    	k.siphon SiphonId,
                    	k.cod_enshab UsageId,
                    	k.sabt IsRegister,
                       	k.kol_hasene TotalServicesAmount,
                    	k.total TotalServicesAmount,
                    	k.pish_gest FirstInstallment,
                    	k.JGEST_FA JGEST_FA,
                    	k.pish_fa PishFa,
                       	k.drsd_gest InstallmentPercent,
                    	k.tedad_gest InstallmentCount,
                    	k.ghest Installment,
                    	k.date_bank BankDateJalali,
                       	k.operator Operator,
                    	k.tedad_mas DomesticUnit,
                    	k.tedad_tej CommercialUnit,
                    	k.tedad_vahd OtherUnit ,
                    	k.cat_cod KartTypeId,
                       	k.ICT_CO InsertedBy,
                    	k.barge Barge ,
                        k.mohlat DueDateJalali
                    From [{dbName}].dbo.kart k
                    Where 
                        k.radif=@CustomerNumber AND
                        k.town=@ZoneId AND
						CAST(CustomerWarehouse.dbo.PersianToMiladi(k.date) As date)=CAST(GETDATE() As date) AND
						k.type IN (3,4,5)
                    Order by k.id Asc";
        }
        private string GetByCustomeNumberQuery(string dbName)
        {
            return $@"Select
						k.id,
						k.town ZoneId,
						k.radif CustomerNumber,
						TRIM(k.eshtrak) ReadingNumber,
						k.date RegisterDateJalali,
						k.pard Amount,
						k.cod_takh DiscountTypeId,
						t15.C1 DiscountTypeTitle,
						k.takhfif DiscountAmount,
						type Type,
						t.Title TypeTitle,
						k.noe_bed OfferingId,
                    	t100.C1 OfferingTitle
                    From [{dbName}].dbo.kart k
                    Join [Db70].dbo.T100 t100	
                    	ON k.noe_bed=t100.C0
					Join [Db70].dbo.ModifyType t
						ON k.type=t.Karten75Id
					Left Join [Db70].dbo.T15 t15
						ON k.cod_takh=t15.C0
                    Where 
                        k.radif=@CustomerNumber AND
                        k.town=@ZoneId AND
						CAST(CustomerWarehouse.dbo.PersianToMiladi(k.date) As date)=CAST(GETDATE() As date) AND
						k.type IN (3,4,5)
                    Order by k.id Asc";
        }

    }
}
