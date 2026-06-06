using Aban360.OldCalcPools.Domain.Features.WaterReturn.Dto.Queries;
using Aban360.OldCalcPools.Persistence.Features.WaterReturn.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Dapper;
using Microsoft.Extensions.Configuration;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;

namespace Aban360.OldCalcPools.Persistence.Features.WaterReturn.Queries.Implementations
{
    internal sealed class RepairQueryService : AbstractBaseConnection, IRepairQueryService
    {
        public RepairQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<RepairGetDto> Get(int id)
        {
            string query = GetQuery();
            RepairGetDto repair = await _sqlReportConnection.QueryFirstOrDefaultAsync<RepairGetDto>(query, new { id });
            return repair;
        }
        public async Task<RepairGetDto> GetByConfirmNumber(int confirmNumber)
        {
            string query = GetByConfirmNumberQuery();
            RepairGetDto? repair = await _sqlReportConnection.QueryFirstOrDefaultAsync<RepairGetDto>(query, new { confirmNumber });
            if (repair == null)
            {
                throw new InvalidBillIdException(ExceptionLiterals.InvalidConfirmedNumber);
            }

            return repair;
        }
        public async Task<IEnumerable<RepairGetDto>> Get(string billId)
        {
            int customerNumber = await GetCustomerNumber(billId);
            string query = GetByBillIdQuery();

            IEnumerable<RepairGetDto> repair = await _sqlReportConnection.QueryAsync<RepairGetDto>(query, new { customerNumber });

            return repair;
        }
        public async Task<int> GetRepairCount(ZoneIdAndCustomerNumberOutputDto input, int jalaseNumber)
        {
            //string dbName = GetDbName(input.ZoneId);
            string dbName = "Atlas";
            string query = GetRepairCountWithJalaseNumber(dbName);
            var @params = new
            {
                zoneId = input.ZoneId,
                customerNumber = input.CustomerNumber,
                jalaseNumber = jalaseNumber
            };
            int count = await _sqlReportConnection.QueryFirstOrDefaultAsync<int>(query, @params);
            return count;
        }
        public async Task<RepairedOutputDto?> GetRepairDateValidate(RepairDateValidateDto input)
        {
            string query = GetDateValidateQuery(GetDbName(input.ZoneId));
            RepairedOutputDto? result = await _sqlReportConnection.QueryFirstOrDefaultAsync<RepairedOutputDto>(query, input);
            return result;
        }

        private async Task<int> GetCustomerNumber(string billId)
        {
            string query = GetCustomerNumberQuery();
            int customerNumber = await _sqlConnection.QueryFirstOrDefaultAsync<int>(query, new { billId });

            return customerNumber;
        }
        private string GetQuery()
        {
            return @"Select * 
                    From [Atlas].dbo.REPAIR
                    Where Id=@Id";
        }
        private string GetByConfirmNumberQuery()
        {
            return @"SELECT
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
                        [type] AS Type,
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
                        group1 AS Group1,
                        FAZ AS Faz,
                        CHK_KARBARI AS ChkKarbari,
                        C200 AS C200,
                        tmp_pri_date AS TmpPriDate,
                        tmp_today_date AS TmpTodayDate,
                        tmp_mohlat AS TmpMohlat,
                        tmp_taviz_date AS TmpTavizDate,
                        tmp_date_bed AS TmpDateBed,
                        edareh_k AS EdarehK,
                        date_sbt AS DateSbt,
                        Avarez AS Avarez
                    FROM [Atlas].dbo.REPAIR
                    Where jalase_no=@confirmNumber";
        }
        private string GetByBillIdQuery()
        {
            return @$"Select * 
                    From [Atlas].dbo.REPAIR
                    Where radif=@customerNumber";
        }
        private string GetCustomerNumberQuery()
        {
            return @"Select CustomerNumber
                    From [CustomerWarehouse].dbo.Clients
                    Where 
                    	BillId=@billId AND
                    	ToDayJalali IS NULL";
        }
        private string GetRepairCountWithJalaseNumber(string dbName)
        {
            return $@"Select COUNT(1)
                    From Atlas.dbo.REPAIR
                    Where
                    	town=@zoneId AND
                    	radif=@customerNumber AND
                    	jalase_no=@jalaseNumber";
        }
        private string GetDateValidateQuery(string dbName)
        {
            return $@"Select Top 1
                    	radif CustomerNumber,
                    	date_bed RegisterDateJalali,
                    	pard Amount
                    From [{dbName}].dbo.Repair
                    Where 
                    	town=@zoneId AND
                    	radif=@customerNumber AND
                    	elat=@returnCauseId AND
                        serial<>7 AND
                    	date_bed BETWEEN @fromDateJalali AND @toDateJalali
                    Order by date_bed Desc";
        }

        //public async Task<RepairGetDto> Get(string billId)
        //{
        //    int zoneId=await GetZoneId(billId);
        //    string dbName = GetDbName(zoneId);
        //    string query = GetQuery(dbName);

        //    RepairGetDto repair = await _sqlReportConnection.QueryFirstOrDefaultAsync<RepairGetDto>(query, new { billId });

        //    return repair;
        //}
        //private async Task<int> GetZoneId(string billId)
        //{
        //    string query = GetZoneIdQuery();
        //    int zoneId = await _sqlConnection.QueryFirstOrDefaultAsync<int>(query, new { billId = billId });

        //    return zoneId;
        //}
        //private string GetQuery(string dbName)
        //{
        //    return @$"Select * 
        //            From [{dbName}].dbo.REPAIR
        //            Where radif=@customerNumber";
        //}
        //private string GetZoneIdQuery()
        //{
        //    return @"Select ZoneId
        //            From [CustomerWarehouse].dbo.Clients
        //            Where 
        //            	BillId=@billId AND
        //            	ToDayJalali IS NULL";
        //}
    }
}