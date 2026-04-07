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
        public KartCommandService(
            IDbConnection connection,
            IDbTransaction transaction)
        {
            _connection = connection;
            _connection.NotNull(nameof(connection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }

        public async Task Insert(KartInsertDto inputDto, string dbName)
        {
            string command = GetInsertCommand(dbName);
            int rowEffected = await _connection.ExecuteAsync(command, inputDto, _transaction);
            if (rowEffected <= 0)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidInsertKart);
            }
        }
        public async Task Insert(ICollection<KartInsertDto> inputDto, string dbName)
        {
            string command = GetInsertCommand(dbName);
            int rowEffected = await _connection.ExecuteAsync(command, inputDto, _transaction);
            if (rowEffected <= 0)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidInsertKart);
            }
        }
        public async Task Remove(KartRemoveDto input, string dbName)
        {
            string command = GetRemoveCommand(dbName);
            int rowEffected = await _connection.ExecuteAsync(command,input, _transaction);
            if (rowEffected <= 0)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidRemoveKart);
            }
        }

        private string GetInsertCommand(string dbName)
        {
            return $@"Insert Into  [{dbName}].dbo.kart 
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
                    	@PardN ,@PardG ,@Sum ,@Type ,@ServiceSelectedId ,
                    	@Ser ,@MeterDiameterId ,@SiphonId ,@UsageId ,@IsRegister  ,
                    	@TotalServicesAmount ,@TotalServicesAmount ,@FirstInstallment ,@JGEST_FA ,@PishFa ,
                    	@InstallmentPercent ,@InstallmentCount ,@Installment ,@BankDateJalali ,0 ,
                    	@Operator ,@DomesticUnit  ,@CommercialUnit ,@OtherUnit ,@KartTypeId ,
                    	@InsertedBy ,@Barge)";
        }
        private string GetRemoveCommand(string dbName)
        {
            return $@"Delete [{dbName}].dbo.kart 
                    Where 
                        id=@id AND
                        Serial=@serial";
        }
    }
}
