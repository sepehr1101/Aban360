using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.OldCalcPools.Domain.Features.WaterReturn.Dto.Commands;
using Dapper;
using System.Data;

namespace Aban360.OldCalcPool.Persistence.Features.WaterReturn.Command.Implementations
{
    public sealed class HRepairCommandService
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        public HRepairCommandService(
            IDbConnection connection,
            IDbTransaction transaction)
        {
            _connection = connection;
            _connection.NotNull(nameof(connection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }
        public async Task<int> Insert(HRepairByRepairIdInsertDto inputDto, string dbName)
        {
            string query = InsertCommand(dbName);
            int recordId = await _connection.QueryFirstOrDefaultAsync<int>(query, inputDto, _transaction);
            if (recordId <= 0)
            {
                throw new ReturnedBillException(ExceptionLiterals.InvalidConfirmedReturn);
            }
            return recordId;
        }
        private string InsertCommand(string dbName)
        {
            return @$"Insert INTO [{dbName}].dbo.H_REPAIR
                    (
                        id, town, radif, eshtrak, barge, pri_no, today_no, pri_date, today_date,
                        abon_fas, fas_baha, ab_baha, ztadil, masraf, shahrdari, modat, date_bed,
                        jalase_no, mohlat, baha, abon_ab, pard, jam, cod_vas, ghabs, del, [type],
                        cod_enshab, enshab, elat, serial, ser, zaribfasl, ab_10, ab_20, tedad_vahd,
                        ted_khane, tedad_mas, tedad_tej, noe_va, jarime, masjar, sabt, rate,
                        operator, mamor, taviz_date, zarib_cntr, zabresani, zarib_d, tafavot,
                        mas_hadar, ab_hadar, range_mas, taf_back, ted_ghabs, TAB_ABN_A, TAB_ABN_F,
                        TABS_FA, bodjeh, group1, FAZ, CHK_KARBARI, C200,
                        edareh_k, date_sbt, Avarez 
                    )
                    Select 
                    
                    	id, town, radif, eshtrak, barge, pri_no, today_no, pri_date, today_date,
                    	abon_fas, fas_baha, ab_baha, ztadil, masraf, shahrdari, modat, date_bed,
                    	jalase_no, mohlat, baha, abon_ab, pard, jam, cod_vas, ghabs, del, [type],
                    	cod_enshab, enshab, elat, serial, ser, zaribfasl, ab_10, ab_20, tedad_vahd,
                    	ted_khane, tedad_mas, tedad_tej, noe_va, jarime, masjar, sabt, rate,
                    	operator, mamor, taviz_date, zarib_cntr, zabresani, zarib_d, tafavot,
                    	mas_hadar, ab_hadar, range_mas, taf_back, ted_ghabs, TAB_ABN_A, TAB_ABN_F,
                    	TABS_FA, bodjeh, group1, FAZ, CHK_KARBARI, C200,
                    	edareh_k, date_sbt, Avarez           
                    From [{dbName}].dbo.REPAIR
                    Where 
                        id = Id AND
                    	town = @ZoneId AND
                    	radif = @CustomerNumber AND
                    	jalase_no = @MinutesNumber;

                    Select SCOPE_IDENTITY();";
        }

    }
}
