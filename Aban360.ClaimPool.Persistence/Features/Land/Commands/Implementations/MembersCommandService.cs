using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using CommonLiteral = Aban360.Common.Literals;
using ClaimLiteral = Aban360.ClaimPool.Persistence.Constants.Literals;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations
{
    public sealed class MembersCommandService
    {
        private readonly IDbConnection _sqlConnection;
        private readonly IDbTransaction _dbTransaction;
        public MembersCommandService(IDbConnection sqlConnection, IDbTransaction transaction)
        {
            _sqlConnection = sqlConnection;
            _sqlConnection.NotNull(nameof(sqlConnection));

            _dbTransaction = transaction;
            _dbTransaction.NotNull(nameof(_dbTransaction));
        }

        public async Task Insert(CustomerInsertDto updateDto, string dbName)
        {
            string command = GetInsertCommand(dbName);
            int recordCount = await _sqlConnection.ExecuteAsync(command, updateDto, _dbTransaction);
            if (recordCount <= 0)
            {
                throw new InvalidCustomerCommandException(ClaimLiteral.ExceptionLiterals.InvalidUpdateMoshtrakin);
            }
        }
        public async Task Update(CustomerUpdateDto updateDto, string dbName)
        {
            string command = GetUpdateCommand(dbName);
            int recordCount = await _sqlConnection.ExecuteAsync(command, updateDto, _dbTransaction);
            if (recordCount <= 0)
            {
                throw new InvalidCustomerCommandException(ClaimLiteral.ExceptionLiterals.InvalidUpdateMoshtrakin);
            }
        }
        public async Task Update(CustomerMobileUpdateDto updateDto, string dbName)
        {
            string command = GetUpdateMobileCommand(dbName);
            int recordCount = await _sqlConnection.ExecuteAsync(command, updateDto, _dbTransaction);
            if (recordCount <= 0)
            {
                throw new InvalidCustomerCommandException(ClaimLiteral.ExceptionLiterals.InvalidUpdateMoshtrakin);
            }
        }
        public async Task Update(CustomerBranchTypeUpdateDto updateDto, string dbName)
        {
            string command = GetUpdateBranchTypeCommand(dbName);
            int recordCount = await _sqlConnection.ExecuteAsync(command, updateDto, _dbTransaction);
            if (recordCount <= 0)
            {
                throw new InvalidCustomerCommandException(ClaimLiteral.ExceptionLiterals.InvalidUpdateMoshtrakin);
            }
        }
        public async Task Update(CustomerDeletionStateUpdateDto updateDto, string dbName)
        {
            string command = GetUpdateDeletionStateCommand(dbName);
            int recordCount = await _sqlConnection.ExecuteAsync(command, updateDto, _dbTransaction);
            if (recordCount <= 0)
            {
                throw new InvalidCustomerCommandException(ClaimLiteral.ExceptionLiterals.InvalidUpdateMoshtrakin);
            }
        }
        public async Task UpdateBedbes(ZoneIdAndCustomerNumber inputDto, long amount, string dbName)
        {
            string command = GetUpdateBedBesCommand(dbName);
            int recordCount = await _sqlConnection.ExecuteAsync(command, new { inputDto.CustomerNumber, inputDto.ZoneId, amount }, _dbTransaction);
            if (recordCount <= 0)
            {
                throw new InvalidCustomerCommandException(ClaimLiteral.ExceptionLiterals.InvalidUpdateBillAmount);
            }
        }
        public async Task UpdateBedbes(IEnumerable<MembersDebtAmountUpdateDto> input, string dbName)
        {
            DataTable table = UpdateDebtAmountDataTable(input);
            string tempTableCommand = GetUpdateDebtAmountCreateTmpTableCommand();
            await _sqlConnection.ExecuteAsync(tempTableCommand, null, _dbTransaction);

            using var bulk = new SqlBulkCopy((SqlConnection)_sqlConnection, SqlBulkCopyOptions.Default, (SqlTransaction)_dbTransaction)
            {
                DestinationTableName = $"#DebtAmountUpdateTemp",
                BatchSize = 5000,
                BulkCopyTimeout = 0
            };

            foreach (DataColumn col in table.Columns)
                bulk.ColumnMappings.Add(col.ColumnName, col.ColumnName);
            await bulk.WriteToServerAsync(table);

            string updateCommand = GetUpdateDebtAmountCommand(dbName);
            int recordEffected = await _sqlConnection.ExecuteAsync(updateCommand, null, _dbTransaction);
            if (recordEffected != (input?.Count() ?? 0))
            {
                throw new ReadingException(CommonLiteral.ExceptionLiterals.InvalidUpdateMembersDebtAmount);
            }
        }

        private DataTable UpdateDebtAmountDataTable(IEnumerable<MembersDebtAmountUpdateDto> input)
        {
            DataTable table = new DataTable();

            table.Columns.Add("ZoneId", typeof(int));
            table.Columns.Add("CustomerNumber", typeof(int));
            table.Columns.Add("BillId", typeof(string));
            table.Columns.Add("Amount", typeof(long));

            foreach (var item in input)
            {
                table.Rows.Add(item.ZoneId, item.CustomerNumber, item.BillId, item.Amount);
            }
            return table;
        }
        private string GetUpdateDebtAmountCreateTmpTableCommand()
        {
            return $@"Create Table #DebtAmountUpdateTemp
                    (
                    	ZoneId int Not Null,
                    	CustomerNumber  int Not Null,
                    	BillId  nvarchar(20) Not Null,
                    	Amount bigint Not Null
                    )";
        }
        private string GetUpdateDebtAmountCommand(string dbName)
        {
            return $@"Update m
                    Set m.bed_bes=m.bed_bes+t.Amount
                    From [{dbName}].dbo.members m
                    Join #DebtAmountUpdateTemp t
                    	On m.Town=t.ZoneId AND m.radif=t.CustomerNumber";
        }
        private string GetInsertCommand(string dbName)
        {
            return @$"INSERT INTO [{dbName}].dbo.members 
                        (
                            bill_id, town, radif, eshtrak, name,
                            family, father_nam, enshab, cod_enshab, tedad_vahd,
                            tedad_mas, ted_khane, tedad_tej, arse, aian,
                            aian_mas, aian_tej, ask_ab, inst_ab, ask_fas,
                            inst_fas, address, pelak, edareh_k, noe_va,
                            master_sif, sif_1, sif_2, sif_3, sif_4,
                            sif_5, sif_6, sif_7, sif_8, fix_mas,
                            group1, operator, POST_COD, PHONE_NO, MOBILE,
                            MELI_COD, Khali_s, date_KHANE, X, Y,
                            date_sabt, hasf, serial_co, sif_mosh_1, G_inst_ab,
                            G_inst_fas, Senf
                        )
                        VALUES (
                            @billId, @zoneId, @customerNumber, @ReadingNumber, @FirstName,
                            @SurName, @FatherName, @MeterDiamterId, @UsageSellId, @OtherUnit,
                            @DomesticUnit, @HouseholdNumber, @CommertialUnit, @Premises, @ImprovementOverall,
                            @ImprovementDomestic, @ImprovementCommertial, @MeterRequestDateJalali, @MeterInstallationDateJalali, @SewageRequestDateJalali,
                            @SewageInstallationDateJalali, @Address, @Plaque, @IsSpecial, @BranchTypeId,
                            @MainSiphon, @Siphon100, @Siphon125, @Siphon150, @Siphon200,
                            @Siphon5, @Siphon6, @Siphon7, @Siphon8, @ContractualCapacity,
                            @UsageConsumptionId, @Operator, @PostalCode, @PhoneNumber, @MobileNumber,
                            @NationalCode, @EmptyUnit, @HouseholdDateJalali, @x, @y,
                            @ToDayDateJalali, @DeletionStateId, @BodySerial, @CommonSiphon, @MeterRegisterDateJalali,
                            @SewageRegisterDateJalali, @GuildId
                        );";
        }
        private string GetUpdateCommand(string dbName)
        {
            return @$"UPDATE [{dbName}].dbo.members
                     SET 
                        eshtrak=@ReadingNumber,
	                    name=@FirstName,
	                    family=@SurName,
	                    father_nam=@FatherName,
	                    enshab=@MeterDiamterId,
	                    cod_enshab=@UsageSellId,
	                    tedad_vahd=@OtherUnit,
	                    tedad_mas=@DomesticUnit,
	                    ted_khane=@HouseholdNumber,
	                    tedad_tej=@CommertialUnit,
	                    arse=@Premises,
	                    aian=@ImprovementOverall,
	                    aian_mas=@ImprovementDomestic,
	                    aian_tej=@ImprovementCommertial,
	                    ask_ab=@MeterRequestDateJalali,
	                    inst_ab=@MeterInstallationDateJalali,
	                    ask_fas=@SewageRequestDateJalali,
	                    inst_fas=@SewageInstallationDateJalali,
	                    address=@Address,
	                    pelak=@Plaque,
	                    edareh_k=@IsSpecial,
	                    noe_va=@BranchTypeId,
	                    master_sif=@MainSiphon,
	                    sif_1=@Siphon100,
	                    sif_2=@Siphon125,
	                    sif_3=@Siphon150,
	                    sif_4=@Siphon200,
	                    sif_5=@Siphon5,
	                    sif_6=@Siphon6,
	                    sif_7=@Siphon7,
	                    sif_8=@Siphon8,	
	                    fix_mas=@ContractualCapacity,
	                    group1=@UsageConsumptionId,
	                    operator=@Operator,
	                    POST_COD=@PostalCode,
	                    PHONE_NO=@PhoneNumber,
	                    MOBILE=@MobileNumber,
	                    MELI_COD=@NationalCode,
	                    Khali_s=@EmptyUnit,
	                    date_KHANE=@HouseholdDateJalali,
	                    X=@x,
	                    Y=@y,
	                    date_sabt=@ToDayDateJalali,
						hasf=@DeletionStateId,
						serial_co=@BodySerial,
						sif_mosh_1=@CommonSiphon,
						G_inst_ab=@MeterRegisterDateJalali,
						G_inst_fas=@SewageRegisterDateJalali,
						Senf=@GuildId
                     WHERE 
                        id=@id AND
						TRIM(bill_id)=@billId AND
						town=@zoneId AND
						radif=@customerNumber ";
        }
        private string GetUpdateMobileCommand(string dbName)
        {
            return @$"UPDATE [{dbName}].dbo.members
                     SET 
	                    MOBILE=@MobileNumber,
	                    date_sabt=@ToDayDateJalali
                     WHERE 
                        id=@id AND
						TRIM(bill_id)=@billId AND
						town=@zoneId AND
						radif=@customerNumber ";
        }
        private string GetUpdateBranchTypeCommand(string dbName)
        {
            return @$"UPDATE [{dbName}].dbo.members
                     SET 
	                    noe_va=@BranchTypeId,
	                    date_sabt=@ToDayDateJalali
                     WHERE 
                        id=@id AND
						TRIM(bill_id)=@billId AND
						town=@zoneId AND
						radif=@customerNumber ";
        }
        private string GetUpdateDeletionStateCommand(string dbName)
        {
            return @$"UPDATE [{dbName}].dbo.members
                     SET 
	                    hasf=@deletionStateId,
	                    date_sabt=@ToDayDateJalali
                     WHERE 
                        id=@id AND
						TRIM(bill_id)=@billId AND
						town=@zoneId AND
						radif=@customerNumber ";
        }
        private string GetUpdateBedBesCommand(string dbName)
        {
            return $@" Update [{dbName}].dbo.members
					Set bed_bes=bed_bes + @amount
					Where 
						radif=@customerNumber AND
						town=@zoneId";
        }
    }

}
