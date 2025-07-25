﻿using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Extensions;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations
{
    internal sealed class SubscriptionAssignmentCommandService : AbstractBaseConnection, ISubscriptionAssignmentCommandService
    {
        public SubscriptionAssignmentCommandService(IConfiguration configuration)
            : base(configuration)
        { 
        }

        public async Task Update(SubscriptionAssignmentUpdateDto updateDto, string date)
        {

            string zoneIdQuery = GetZoneIdQuery();
            int zoneId = await _sqlReportConnection.QueryFirstAsync<int>(zoneIdQuery, new { billId = updateDto.BillId });

            string insertQuery = GetInsertQuery(zoneId);

            var @params = new
            {
                billId = updateDto.BillId,
                x = updateDto.X,
                y = updateDto.Y,
                readingNumber = updateDto.ReadingNumber,
                firstName = updateDto.FirstName,
                surName = updateDto.SurName,
                address = updateDto.Address,
                date = date
            };
            string updateQuery = GetUpdateQuery(zoneId);

            //if (_sqlReportConnection.State != System.Data.ConnectionState.Open)
            //    await _sqlReportConnection.OpenAsync();

            using (var transaction= TransactionBuilder.Create(0,10))
            {
                var insertResult = await _sqlReportConnection.ExecuteAsync(insertQuery, new { billId = updateDto. BillId, date=date });
                var updateResult = await _sqlReportConnection.ExecuteAsync(updateQuery, @params);
                transaction.Complete();
                //await transaction.CommitAsync();
            }
        }

        private string GetZoneIdQuery()
        {
            return @"Select c.ZoneId,c.CustomerNumber
                     From  [CustomerWarehouse].dbo.Clients c
                     Where 
						c.BillId=@billId AND
						c.ToDayJalali IS NULL";
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
                         X = @x,
                         Y = @y,
                         eshtrak = @readingNumber,
                         name = @firstName,
                         family = @surName,
                         address = @address,
                         date_KHANE = @date,
                         DATEINS=@date
                     WHERE bill_id=@billId";
        }
    }
}
