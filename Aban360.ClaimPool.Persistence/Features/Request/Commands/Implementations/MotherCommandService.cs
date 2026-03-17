using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Persistence.Constants.Literals;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Dapper;
using System.Data;

namespace Aban360.ClaimPool.Persistence.Features.Request.Commands.Implementations
{
    public sealed class MotherCommandService
    {
        private readonly IDbConnection _sqlRonnection;
        private readonly IDbTransaction _transaction;
        public MotherCommandService(
            IDbConnection sqlRonnection,
            IDbTransaction transaction)
        {
            _sqlRonnection = sqlRonnection;
            _sqlRonnection.NotNull(nameof(sqlRonnection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }

        public async Task Insert(MotherInsertDto inputDto, string dbName)
        {
            string command = GetInsertCommand(dbName);
            int recordCount = await _sqlRonnection.ExecuteAsync(command, inputDto, _transaction);
            if (recordCount <= 0)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidInsertMother);
            }
        }
        public async Task Update(MotherChildUpdateInputDto inputDto, string dbName)
        {
            string command = GetMotherChildUpdateCommand(dbName);
            int recordCount = await _sqlRonnection.ExecuteAsync(command, inputDto, _transaction);
            if (recordCount <= 0)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidUpdateMother);
            }
        }
        public async Task Update(CommonSiphonUpdateInputDto inputDto, string dbName)
        {
            string command = GetCommonSiphonUpdateCommand(dbName);
            int recordCount = await _sqlRonnection.ExecuteAsync(command, inputDto, _transaction);
            if (recordCount <= 0)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidUpdateMother);
            }
        }
        public async Task Delete(string stringTrackNumber, string dbName)
        {
            string command = GetDeleteCommand(dbName);
            int recordCount = await _sqlRonnection.ExecuteAsync(command, new { stringTrackNumber }, _transaction);
            if (recordCount <= 0)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidDeleteMother);
            }
        }

        private string GetInsertCommand(string dbName)//todo: I send @CommonSiphon But DataBase Write *
        {
            return $@"Insert Into [{dbName}].dbo.mother(
                    	town,radif,eshtrak,par_no,mother_rad,
                    	name,family,father_nam,enshab,cod_enshab,
                    	tedad_vahd,tedad_mas,tedad_tej,
                    	arse,aian,aian_mas,aian_tej,
                    	address,sif_1,sif_2,sif_3,sif_4,sif_mosh_1,
                    	fix_mas,edareh_k)
                    Values (
                        @ZoneId ,@CustomerNumber ,@ReadingNumber ,@StringTrackNumber ,@MotherCustomerNumber ,
                        @FirstName ,@Surname ,@FatherName ,@MeterDiameterId ,@UsageId ,
                        @OtherUnit ,@DomesticUnit ,@CommercialUnit ,
                        @Premises ,@ImprovementOverall ,@ImprovementDomestic ,@ImprovementCommercial ,
                        @Address ,@Siphon100 ,@Siphon125 ,@Siphon150 ,@Siphon200 ,@CommonSiphon ,
                        @ContractualCapacity ,@IsSpecial )";
        }
        private string GetMotherChildUpdateCommand(string dbName)
        {
            return $@"Update [{dbName}].dbo.mother
                    Set 
                    	tedad_vahd=@OtherUnit ,
                    	tedad_mas=@DomesticUnit ,
                    	tedad_tej=@CommercialUnit ,
                    	arse=@Premises ,
                    	aian=@ImprovementOverall ,
                    	aian_mas=@ImprovementDomestic ,
                    	aian_tej=@ImprovementCommercial ,
                    	fix_mas=@ContractualCapacity 
                    Where TRIM(par_no)=@StringTrackNumber";
        }
        private string GetCommonSiphonUpdateCommand(string dbName)
        {
            return $@"Update [{dbName}].dbo.mother
                    Set 
                    	sif_1=@Siphon100 , 
	                    sif_2=@Siphon125 ,
	                    sif_3=@Siphon150 ,
	                    sif_4=@Siphon200  
                    Where TRIM(par_no)=@StringTrackNumber";
        }
        private string GetDeleteCommand(string dbName)
        {
            return $@"Delete [{dbName}].dbo.mother
                    Where TRIM(par_no)=@StringTrackNumber";
        }
    }
}
