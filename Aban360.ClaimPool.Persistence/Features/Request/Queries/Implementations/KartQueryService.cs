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
                    	town ZoneId,
                    	radif CustomerNumber,
                    	eshtrak ReadingNumber,
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
                    	barge Barge 
                    From [{dbName}].dbo.kart
                    Where par_no=@stringTrackNumber
                    Order by date Asc";
        }
    }
}
