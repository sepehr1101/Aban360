using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ClaimPool.Persistence.Features.Request.Queries.Implementations
{
    internal sealed class MotherQueryService : AbstractBaseConnection, IMotherQueryService
    {
        public MotherQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<MotherInfoOutputDto?> Get(string stringTrackNumber, int zoneId)
        {
			string dbName=GetDbName(zoneId);
            string query = GetQuery(dbName);
            MotherInfoOutputDto? result = await _sqlReportConnection.QueryFirstOrDefaultAsync<MotherInfoOutputDto>(query, new { stringTrackNumber });
            return result;
        }
        private string GetQuery(string dbName)
        {
            return $@"Select 
						id,
						town ZoneId,
						radif CustomerNumber,
						eshtrak ReadingNumber,
						par_no StringTrackNumber,
						mother_rad MotherCustomerNumber,
						name FirstName,
						family Surname,
						father_nam FatherName,
						address Address,
						sif_1 Siphon100,
						sif_2 Siphon125,
						sif_3 Siphon150,
						sif_4 Siphon200,
						sif_mosh_1 CommonSiphon,
						enshab MeterDiamaterId,
						t5.C2 MeterDiamterTitle,
						cod_enshab UsageId,
						t41.C1 UsageTitle,
						tedad_vahd OtherUnit,
						tedad_mas DomesticUnit,
						tedad_tej CommercialUnit,
						arse Premises,
						aian ImprovementOverall,
						aian_mas ImprovementDomestic,
						aian_tej ImprovementCommercial,
						fix_mas ContractualCapacity,
						edareh_k IsSpecial
					From [{dbName}].dbo.mother
					Join [Db70].dbo.T41 t41
						ON cod_enshab=t41.C0
					Join [Db70].dbo.T5 t5
						ON enshab=t5.C0
					Where par_no=@stringTrackNumber";
        }
    }
}
