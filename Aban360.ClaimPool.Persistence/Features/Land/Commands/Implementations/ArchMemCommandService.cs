using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Constants.Literals;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations
{
    public sealed class ArchMemCommandService
    {
        private readonly SqlConnection _sqlConnection;
        private readonly IDbTransaction _dbTransaction;

        public ArchMemCommandService(SqlConnection sqlConnection, IDbTransaction transaction)
        {
            _sqlConnection = sqlConnection;
            _sqlConnection.NotNull(nameof(sqlConnection));

            _dbTransaction = transaction;
            _dbTransaction.NotNull(nameof(_dbTransaction));
        }

        public async Task<int> Insert(CustomerUpdateDto updateDto, string dbName)
        {
            string command = GetInsertQuery(dbName);
            int? insertResultId = await _sqlConnection.QueryFirstOrDefaultAsync<int>(command, updateDto, _dbTransaction);
            if (insertResultId is null || insertResultId <= 0)
            {
                throw new InvalidCustomerCommandException(ExceptionLiterals.InvalidInsertArchmem);
            }

            return insertResultId.Value;
        }

        private string GetInsertQuery(string dbName)
        {
            return @$";With cte as
                    (
                    	Select Top 1 *
                    	From [{dbName}].dbo.arch_mem 
                    	Where 
                    		town=@zoneId AND
                    		radif=@customerNumber 
                    	Order By date_roz desc,id desc
                    )
                    INSERT INTO [{dbName}].dbo.arch_mem(
                    	town, radif, par_no, eshtrak, name, family, father_nam, enshab, cod_enshab,
                    	tedad_vahd, tedad_mas, ted_khane, tedad_tej, date_sabt, arse, aian, aian_mas,
                    	aian_tej, ask_ab, inst_ab, ask_fas, inst_fas, address, pelak, bed_bes, edareh_k,
                    	hasf, n_ab, n_faz, noe_va, master_sif, sif_1, sif_2, sif_3, sif_4, sif_mosh_1,
                    	fix_mas, group1, serial_co, G_inst_ab, G_inst_fas, operator, date_roz, POST_COD,
                    	PHONE_NO, MOBILE, MELI_COD, oRadif, sif_5, sif_6, sif_7, sif_8, bill_id, MOJAVZ,
                    	c20, balansing, tmp_date_sabt, tmp_ask_ab, tmp_ask_fas, tmp_inst_ab,
                    	tmp_inst_fas, tmp_g_inst_ab, tmp_g_inst_fas, tmp_date_roz, Khali_s, Senf, date_KHANE--,x,y,DATEINS, 
                    )
                    SELECT 
                         town, radif, ' ' AS par_no, @ReadingNumber, @FirstName, @SurName, @FatherName, @MeterDiamterId, @UsageSellId,
                        @OtherUnit, @DomesticUnit, @HouseholdNumber, @CommertialUnit, @ToDayDateJalali, @Premises, @ImprovementOverall, @ImprovementDomestic,
                        @ImprovementCommertial, @MeterRequestDateJalali, @MeterInstallationDateJalali, @SewageRequestDateJalali, @SewageInstallationDateJalali, @Address, @Plaque, bed_bes, @IsSpecial,
                        @DeletionStateId, n_ab, n_faz, @BranchTypeId, @MainSiphon, @Siphon100, @Siphon125, @Siphon150, @Siphon200, @CommonSiphon,
                        @ContractualCapacity, @UsageConsumptionId, @BodySerial, @MeterRegisterDateJalali, @SewageRegisterDateJalali, @Operator, @ToDayDateJalali AS date_roz, @PostalCode,
                        @PhoneNumber, @MobileNumber, @NationalCode, oRadif, @Siphon5, @Siphon6, @Siphon7, @Siphon8, bill_id, MOJAVZ,
                         c20, balansing, tmp_date_sabt, tmp_ask_ab, tmp_ask_fas, tmp_inst_ab,
                        tmp_inst_fas, tmp_g_inst_ab, tmp_g_inst_fas, tmp_date_sabt, @EmptyUnit, @GuildId, @HouseholdDateJalali--,@x,@y,@ToDayDateJalaliWithFragmentYear
                    FROM cte m
                    WHERE 
                        m.town=@zoneId AND
                    	m.radif=@customerNumber 

                    SELECT CAST(SCOPE_IDENTITY() AS INT)";
        }
    }
}
