using Aban360.Common.Db.Dapper;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.WaterReturn.Queries.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.OldCalcPool.Persistence.Features.WaterReturn.Queries.Implementations
{
    internal sealed class AutoBackQueryService : AbstractBaseConnection, IAutoBackQueryService
    {
        public AutoBackQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<AutoBackGetDto> Get(ReturnBillConfirmeByZoneAndCustomerNumberInputDto input)
        {
            //string dbName = GetDbName((int)input.Town);
            string dbName = "Atlas";
            string query = GetQuery(dbName);
            IEnumerable<AutoBackGetDto> datas = await _sqlReportConnection.QueryAsync<AutoBackGetDto>(query, input);
			if (!datas.Any() || datas.Count() != 3)
			{
				throw new InvalidDataException(ExceptionLiterals.InvalidId);//todo: change exception
			}

			return datas.ElementAt(2);
        }
        private string GetQuery(string dbName)
        {
            return @$"Select 
						town,
						radif,
						eshtrak,
						barge,
						pri_no PreviousNumber,
						today_no CurrentNumber,
						pri_date PreviousDate,
						today_date CurrentDate,
						abon_fas AbonFas,
						fas_baha FasBaha,
						ab_baha AbBaha,
						ztadil,
						masraf,
						shahrdari,
						modat,
						date_bed DateBed,
						jalase_no  JalaseNo, 
						mohlat , 
						baha,
						abon_ab AbonAb,
						pard ,
						jam,
						cod_vas CodVas,
						ghabs ,
						del,
						type,
						cod_enshab CodEnshab,
						enshab,
						elat,
						serial,
						ser,
						zaribfasl, 
						ab_10 Ab10, 
						ab_20 Ab20,
						tedad_vahd TedadVahd,
						ted_khane TedKhane ,
						tedad_mas TedadMas,
						tedad_tej TedadTej, 
						noe_va NoeVa,
						jarime, 
						masjar,
						sabt,
						rate, 
						operator,
						mamor, 
						taviz_date	TavizDate , 
						zarib_cntr ZaribCntr, 
						zabresani , 
						zarib_d ZaribD, 
						tafavot , 
						mas_hadar MasHadar,
						ab_hadar AbHadar,
						range_mas RangeMas,
						taf_back TafBack,
						ted_ghabs TedGhabs,
						TAB_ABN_A TabAbnA, 
						TAB_ABN_F TabAbnF,
						TABS_FA TabsFa,
						bodjeh, 
						FAZ,
						tmp_pri_date TmpPriDate,
						tmp_today_date TmpTodayDate,
						 tmp_mohlat TmpMohlat, 
						 tmp_taviz_date TmpTavizDate, 
						 tmp_date_bed TmpDateBed
					From [{dbName}].dbo.autoback
					Where
						town=@ZoneId AND
						radif=@CustomerNumber AND
						jalase_no=@JalaseNumber";
        }
    }
}