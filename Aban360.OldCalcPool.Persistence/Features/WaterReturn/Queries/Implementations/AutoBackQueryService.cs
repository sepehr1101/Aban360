using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
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
        public async Task<IEnumerable<AutoBackGetByBargeDto>> GetByConfirmNumber(int confirmedNumber)
        {
            string dbName = "Atlas";
            string query = GetByJalaseNumberQuery(dbName);
            IEnumerable<AutoBackGetByBargeDto> datas = await _sqlReportConnection.QueryAsync<AutoBackGetByBargeDto>(query, new { confirmedNumber });
            if (!datas.Any() || datas.Count() != 3)
            {
                throw new InvalidBillIdException(ExceptionLiterals.InvalidConfirmedNumber);
            }

            return datas; ;
        }
        public async Task<IEnumerable<UnconfirmedBillReturnDataOutputDto>> GetUnconfirmed(int zoneId)
        {
            string query = GetUnconfirmedQuery();
            IEnumerable<UnconfirmedBillReturnDataOutputDto> data = await _sqlReportConnection.QueryAsync<UnconfirmedBillReturnDataOutputDto>(query, new { zoneId });
            return data;
        }
        public async Task<int> GetCountByDateInterval(ReturnBillDateIntervalDto input)
        {
            string dbName = "Atlas";
            string query = GetCountByDateIntervalQuery(dbName);
            int count = await _sqlReportConnection.QueryFirstOrDefaultAsync<int>(query, input);
            if (count > 0)
            {
                throw new ReturnedBillException(ExceptionLiterals.InvalidTempReturnBill);
            }

            return count; 
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
						tmp_date_bed TmpDateBed,
                        IsConfirmed
					From [{dbName}].dbo.autoback
					Where
						town=@ZoneId AND
						radif=@CustomerNumber AND
						jalase_no=@JalaseNumber";
        }
        private string GetByJalaseNumberQuery(string dbName)
        {
            return $@"SELECT
                        id AS Id,    
                        town AS Town,
                        radif AS Radif,
                        eshtrak AS Eshtrak,
                        barge AS Barge,
                        pri_no AS PriNo,
                        today_no AS TodayNo,
                        pri_date AS PriDate,
                        today_date AS TodayDate,
                        abon_fas AS AbonFas,
                        fas_baha AS FasBaha,
                        ab_baha AS AbBaha,
                        ztadil AS Ztadil,
                        masraf AS Masraf,
                        shahrdari AS Shahrdari,
                        modat AS Modat,
                        date_bed AS DateBed,
                        jalase_no AS JalaseNo,
                        mohlat AS Mohlat,
                        baha AS Baha,
                        abon_ab AS AbonAb,
                        pard AS Pard,
                        jam AS Jam,
                        cod_vas AS CodVas,
                        ghabs AS Ghabs,
                        del AS Del,
                        type AS Type,
                        cod_enshab AS CodEnshab,
                        enshab AS Enshab,
                        elat AS Elat,
                        serial AS Serial,
                        ser AS Ser,
                        zaribfasl AS ZaribFasl,
                        ab_10 AS Ab10,
                        ab_20 AS Ab20,
                        tedad_vahd AS TedadVahd,
                        ted_khane AS TedKhane,
                        tedad_mas AS TedadMas,
                        tedad_tej AS TedadTej,
                        noe_va AS NoeVa,
                        jarime AS Jarime,
                        masjar AS Masjar,
                        sabt AS Sabt,
                        rate AS Rate,
                        operator AS Operator,
                        mamor AS Mamor,
                        taviz_date AS TavizDate,
                        zarib_cntr AS ZaribCntr,
                        zabresani AS Zabresani,
                        zarib_d AS ZaribD,
                        tafavot AS Tafavot,
                        mas_hadar AS MasHadar,
                        ab_hadar AS AbHadar,
                        range_mas AS RangeMas,
                        taf_back AS TafBack,
                        ted_ghabs AS TedGhabs,
                        TAB_ABN_A AS TabAbnA,
                        TAB_ABN_F AS TabAbnF,
                        TABS_FA AS TabsFa,
                        bodjeh AS Bodjeh,
                        FAZ AS Faz,
                        tmp_pri_date AS TmpPriDate,
                        tmp_today_date AS TmpTodayDate,
                        tmp_date_bed AS TmpDateBed,
                        tmp_mohlat AS TmpMohlat,
                        tmp_taviz_date AS TmpTavizDate,
                        IsConfirmed
                    FROM [{dbName}].dbo.[autoback]
                    Where jalase_no=@confirmedNumber
                    ORDER BY id ASC;";
        }
        private string GetUnconfirmedQuery()
        {
            return $@";With Cte As(
                    	Select 
                    		town ZoneId,
                    		t51.C2 ZoneTitle,
                    		a.cod_enshab UsageId,
                    		t41.C1 UsageTitle,
                    		a.enshab MeterDiameterId,
                    		t5.C1 MeterDiameterTitle,
                    		a.radif CustomerNumber,
                    		a.eshtrak ReadingNumber,
                    		a.pri_no PreviousNumber,
                    		a.today_no CurrentNumber,
                    		a.pri_date PreviousDateJalali,
                    		a.today_date CurrentDateJalali,
                    		a.date_bed RegisterDateJalali,
                    		a.jalase_no MinutesNumber,
                    		a.ted_ghabs BillCount,
                    		a.tedad_mas DomesticUnit,
                    		a.tedad_tej CommercialUnit,
                    		a.tedad_vahd OtherUnit,
                    		a.ted_khane HouseholdCount,
                    		a.rate ConsumptionAverage,
                    		a.masraf Consumption,
                    		a.modat Duration,
                    		a.baha Amount,
                    		a.ab_baha WaterAmount,
                    		a.fas_baha SewageAmount,
                    		a.operator Operator,
                    		a.elat ReturnCauseId,
                    		r.Title ReturnCauseTitle,
                    		Rn=Row_Number() Over(Partition By a.jalase_no Order by a.date_bed Desc, a.Id Desc)
                    	From [Atlas].dbo.autoback a
                    	Join [Db70].dbo.T51 t51
                    		ON a.town=t51.C0
                    	Join [Db70].dbo.T41 t41
                    		ON a.cod_enshab=t41.C0
                    	Join [Db70].dbo.T5 t5
                    		ON a.enshab=t5.C0
                    	Join [Db70].dbo.ReturnCause r
                    		ON a.elat=r.Id
                    	Where a.IsConfirmed=0 AND a.Town=@zoneId
                    )
                    Select c.*
                    From Cte c
                    Where c.Rn=1 
					Order By c.RegisterDateJalali Desc , c.jalase_no desc";
        }
        private string GetCountByDateIntervalQuery(string dbName)
        {
            return $@"Select COUNT(1)
                    From [{dbName}].dbo.autoback 
                    Where 
                    	town=@ZoneId AND
                    	radif=@CustomerNumber AND
                    	(pri_date>=@FromDateJalali AND today_date<=@ToDateJalali) ";
        }
    }
}