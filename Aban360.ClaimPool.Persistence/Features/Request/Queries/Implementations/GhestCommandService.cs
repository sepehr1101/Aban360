using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Persistence.Constants.Literals;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Dapper;
using System.Data;

namespace Aban360.ClaimPool.Persistence.Features.Request.Queries.Implementations
{
    public sealed class GhestCommandService
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        public GhestCommandService(
            IDbConnection connection,
            IDbTransaction transaction)
        {
            _connection = connection;
            _connection.NotNull(nameof(connection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }


        public async Task Insert(IEnumerable<GhestInsertDto> inputDto, string dbName)
        {
            string command = GetInsertCommand(dbName);
            int recordCount = await _connection.ExecuteAsync(command, inputDto, _transaction);
            if (recordCount <= 0)
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidInsertGhest);
            }
        }
        private string GetInsertCommand(string dbName)
        {
            return $@"Insert Into [{dbName}].dbo.ghest(
                    	town,radif_g,par_no,idntify,
                    	cod1,cod2,cod3,barge_g,
                    	pard,type,noe_bed,date,no_gest,
                    	mohlat,ICT_CO,sh_ghabs1,sh_pard1)
                    Values(
                        @ZoneId ,@CustomerNumber ,@StringTrackNumber ,@Identify ,
                        @Cod1 ,@Cod2 ,@Cod3 ,@Barge ,
                        @Payable ,@Type ,0 ,@CurrentDateJalali ,@InstallmentNumber ,
                        @DueDateJalali ,@InsertBy ,@BillId ,@PaymentId )";
        }
    }
}
