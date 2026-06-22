using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Persistence.Constants.Literals;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Dapper;
using System.Data;

namespace Aban360.ClaimPool.Persistence.Features.Request.Commands.Implementations
{
    public sealed class KartCommandService
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        string _karten75TableName = "karten75";
        string _kartTableName = "kart";
        public KartCommandService(
            IDbConnection connection,
            IDbTransaction transaction)
        {
            _connection = connection;
            _connection.NotNull(nameof(connection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }

        public async Task Insert(KartInsertDto inputDto, bool isKarten75, string dbName)
        {
            string tableName = GetTableName(isKarten75);
            string command = GetInsertCommand(dbName, tableName);
            int rowEffected = await _connection.ExecuteAsync(command, inputDto, _transaction);
            if (rowEffected <= 0)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidInsertKart);
            }
        }
        public async Task Insert(IEnumerable<KartInsertDto> inputDto, bool isKarten75, string dbName)
        {
            string tableName = GetTableName(isKarten75);
            string command = GetInsertCommand(dbName, tableName);
            int rowEffected = await _connection.ExecuteAsync(command, inputDto, _transaction);
            if (rowEffected != (inputDto?.Count() ?? 0))
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidInsertKart);
            }
        }
        public async Task Remove(string stringTrackNumber, string dbName)
        {
            string command = GetRemoveCommand(dbName);
            int rowEffected = await _connection.ExecuteAsync(command, new { stringTrackNumber }, _transaction);
        }
        public async Task Remove(KartRemoveDto input, string dbName)
        {
            string command = GetRemoveByIdCommand(dbName);
            int rowEffected = await _connection.ExecuteAsync(command, input, _transaction);
            if (rowEffected <= 0)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidRemoveKart);
            }
        }
        public async Task Remove(KartRemoveByConditionDto input, string dbName)
        {
            string command = GetRemoveByConditionCommand(dbName);
            int rowEffected = await _connection.ExecuteAsync(command, input, _transaction);
            if (rowEffected <= 0)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidRemoveKart);
            }
        }
        public async Task Update(KartInstallmentUpdateDto inputDto, string dbName)
        {
            string command = GetUpdateInstallmentCommand(dbName);
            int rowEffected = await _connection.ExecuteAsync(command, inputDto, _transaction);
            if (rowEffected <= 0)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidUpdateInstallmentKart);
            }
        }

        private string GetTableName(bool isKarten75) => isKarten75 ? _karten75TableName : _kartTableName;
        private string GetInsertCommand(string dbName, string tableName)
        {
            return $@"Insert Into  [{dbName}].dbo.{tableName} 
                    	(town,radif,eshtrak,par_no,serial,
                    	date,mohlat,cod_takh,pard,takhfif,
                    	pard_n,pard_g,jam_ha,type,noe_bed,
                    	ser,enshab,siphon,cod_enshab,sabt,
                    	kol_hasene,total,pish_gest,JGEST_FA,pish_fa,
                    	drsd_gest,tedad_gest,ghest,date_bank,date_sabt,
                    	operator,tedad_mas,tedad_tej,tedad_vahd,cat_cod,
                    	ICT_CO,barge)
                    Values
                    	(@ZoneId ,@CustomerNumber ,@ReadingNumber  ,@StringTrackNumber ,@Serial ,
                    	@CurrentDateJalali ,@DueDateJalali ,@DiscountTypeId ,@FinalAmount  ,@DiscountAmount ,
                    	@PardN ,@PardG ,@Sum ,@Type ,@AmountItemId ,
                    	@Ser ,@MeterDiameterId ,@SiphonId ,@UsageId ,@IsRegister  ,
                    	@TotalServicesAmount ,@TotalServicesAmount ,@FirstInstallment ,@JGEST_FA ,@PishFa ,
                    	@InstallmentPercent ,@InstallmentCount ,@Installment ,@BankDateJalali ,0 ,
                    	@Operator ,@DomesticUnit  ,@CommercialUnit ,@OtherUnit ,@KartTypeId ,
                    	@InsertedBy ,@Barge)";
        }
        private string GetRemoveCommand(string dbName)
        {
            return $@"Delete [{dbName}].dbo.kart 
                    Where par_no=@stringTrackNumber";
        }
        private string GetRemoveByIdCommand(string dbName)
        {
            return $@"Delete [{dbName}].dbo.kart 
                    Where 
                        id=@id AND
                        Serial=@serial";
        }
        private string GetRemoveByConditionCommand(string dbName)
        {
            return $@";With Cte As(
                    	Select Top 1 *
                    	From [{dbName}].dbo.karten75 
                    	Where 
                    		radif=@CustomerNumber AND
                    		town=@ZoneId AND
                    	    date=@RegisterDateJalali AND
                    		pard=@Amount AND
                    		type=@TypeCode AND 
                    		noe_bed=@ItemId
                    )
                    Delete Cte ";
        }
        private string GetUpdateInstallmentCommand(string dbName)
        {
            return $@"Update[{dbName}].dbo.kart 
                    Set
                    	pard_n = pard * @CashPercent ,
                    	pard_g = pard * @UncashPercent ,
                    	pish_gest = @FirstInstallment ,
                    	drsd_gest = @InstallmentPercent ,
                    	tedad_gest = @InstallmentCount,
                    	ghest = @Installment  ,
                    	ICT_CO = @InsertedBy ,
                        operator = @Operator
                    Where       
                        par_no = @StringTrackNumber AND
                        town = @ZoneId ";
        }
    }
}
