using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Persistence.Constants.Literals;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Dapper;
using System.Data;

namespace Aban360.ClaimPool.Persistence.Features.Request.Commands.Implementations
{
    public sealed class RequestBillDetailsCommandService
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        public RequestBillDetailsCommandService(
            IDbConnection connection,
            IDbTransaction transaction)
        {
            _connection = connection;
            _connection.NotNull(nameof(connection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }
        public async Task Insert(RequestBillDetailsInsertDto inputDto)
        {
            string command = GetInsertCommand();
            int rowEffected = await _connection.ExecuteAsync(command, inputDto, _transaction);
            if (rowEffected <= 0)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidInsertRequestBillDetails);
            }
        }
        public async Task Insert(IEnumerable<RequestBillDetailsInsertDto> inputDto)
        {
            string command = GetInsertCommand();
            int rowEffected = await _connection.ExecuteAsync(command, inputDto, _transaction);
            if (rowEffected != (inputDto?.Count() ?? 0))
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidInsertKart);
            }
        }
        private string GetInsertCommand()
        {
            return $@"Insert Into [CustomerWarehouse].dbo.RequestBillDetails 
                    (
                        TrackNumber,ZoneId,CustomerNumber,TypeId,
                        ItemId,ItemTitle,Amount,BillId ,
                        DomesticCount,CommercialCount,OtherCount,ContractualCapacity, 
                        OffAmount,OffTitle,FinalAmount,RegisterDate,
                        ZoneTitle,TypeCode,UsageId,UsageTitle,PayId
                    )
                    Values (
                        @TrackNumber,@ZoneId,@CustomerNumber,@TypeId,
                        @ItemId,@ItemTitle,@Amount,@BillId,
                        @DomesticCount,@CommercialCount,@OtherCount,@ContractualCapacity,
                        @OffAmount,@OffTitle,@FinalAmount,@RegisterDate,
                        @ZoneTitle,@TypeCode,@UsageId,@UsageTitle,@PayId
                    )";
        }
    }
}
