using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations
{
    internal sealed class EmptyUnitRegisterCommandService : AbstractBaseConnection, IEmptyUnitRegisterCommandService
    {
        public EmptyUnitRegisterCommandService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task Update(EmptyUnitRegisterUpdateDto updateDto, string date)
        {
            string zoneIdQuery = GetZoneIdQuery();
            int zoneId = await _sqlReportConnection.QueryFirstOrDefaultAsync<int>(zoneIdQuery, new { billId = updateDto.BillId });
            if (zoneId == null)
            {
                throw new BaseException(ExceptionLiterals.BillIdNotFound);
            }
            var @params = new
            {
                billId = updateDto.BillId,
                emptyUnit=updateDto.EmptyUnit,
                date = date
            };

            string insertToArchmembers = GetInsertQuery(zoneId);
            string updateMembers = GetUpdateQuery(zoneId);

            using (var transaction = TransactionBuilder.Create(0, 10))
            {
                var insertResult = await _sqlReportConnection.ExecuteAsync(insertToArchmembers, new { date = date, billId = updateDto.BillId });
                var updateResult = await _sqlReportConnection.ExecuteAsync(updateMembers, @params);
                transaction.Complete();
            }
        }
        private string GetZoneIdQuery()
        {
            return @"Select Top 1
                    	c.ZoneId
                    From [CustomerWarehouse].dbo.Clients c
                    Where c.BillId=@billId";
        }
        private string GetInsertQuery(int zoneId)
        {
            return @$"INSERT INTO [{zoneId}].dbo.arch_mem(
                            town, radif, par_no, eshtrak, name, family, father_nam, enshab, cod_enshab,
                           tedad_vahd, tedad_mas, ted_khane, tedad_tej, date_sabt, arse, aian, aian_mas,
                           aian_tej, ask_ab, inst_ab, ask_fas, inst_fas, address, pelak, bed_bes, edareh_k,
                           hasf, n_ab, n_faz, noe_va, master_sif, sif_1, sif_2, sif_3, sif_4, sif_mosh_1,
                           fix_mas, group1, serial_co, G_inst_ab, G_inst_fas, operator, date_roz, POST_COD,
                           PHONE_NO, MOBILE, MELI_COD, oRadif, sif_5, sif_6, sif_7, sif_8, bill_id, MOJAVZ,
                           DATEINS, c20, balansing, tmp_date_sabt, tmp_ask_ab, tmp_ask_fas, tmp_inst_ab,
                           tmp_inst_fas, tmp_g_inst_ab, tmp_g_inst_fas, tmp_date_roz, Khali_s, Senf, date_KHANE
                       )
                       SELECT 
                            town, radif, ' ' AS par_no, eshtrak, name, family, father_nam, enshab, cod_enshab,
                           tedad_vahd, tedad_mas, ted_khane, tedad_tej, date_sabt, arse, aian, aian_mas,
                           aian_tej, ask_ab, inst_ab, ask_fas, inst_fas, address, pelak, bed_bes, edareh_k,
                           hasf, n_ab, n_faz, noe_va, master_sif, sif_1, sif_2, sif_3, sif_4, sif_mosh_1,
                           fix_mas, group1, serial_co, G_inst_ab, G_inst_fas, operator, @date AS date_roz, POST_COD,
                           PHONE_NO, MOBILE, MELI_COD, oRadif, sif_5, sif_6, sif_7, sif_8, bill_id, MOJAVZ,
                           DATEINS, c20, balansing, tmp_date_sabt, tmp_ask_ab, tmp_ask_fas, tmp_inst_ab,
                           tmp_inst_fas, tmp_g_inst_ab, tmp_g_inst_fas, tmp_date_sabt, Khali_s, Senf, date_KHANE
                       FROM [{zoneId}].dbo.members m
                       WHERE m.bill_id=@billId";
        }
        private string GetUpdateQuery(int zoneId)
        {
            return @$"UPDATE [{zoneId}].dbo.members
                     SET 
                         Khali_s = @emptyUnit,
                         date_sabt=@date
                     WHERE bill_id=@billId";
        }

    }
}
