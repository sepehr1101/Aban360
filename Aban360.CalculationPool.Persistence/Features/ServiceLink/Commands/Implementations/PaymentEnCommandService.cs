using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Dapper;
using System.Data;

namespace Aban360.CalculationPool.Persistence.Features.ServiceLink.Commands.Implementations
{
    public sealed class PaymentEnCommandService
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        public PaymentEnCommandService(IDbConnection connection, IDbTransaction transaction)
        {
            _connection = connection;
            _connection.NotNull(nameof(connection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }
        public async Task Insert(PaymentEnInsertDto inputDto)
        {
            string command = GetInsertCommand();
            int affectedRecords = await _connection.ExecuteAsync(command, inputDto, _transaction);
            if (affectedRecords <= 0)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidInsertVosolEn);
            }
        }
        public async Task Insert(IEnumerable<PaymentEnInsertDto> inputDto)
        {
            string command = GetInsertCommand();
            int affectedRecords = await _connection.ExecuteAsync(command, inputDto, _transaction);
            if (affectedRecords != (inputDto?.Count() ?? 0))
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidInsertVosolEn);
            }
        }
        public async Task Remove(ServiceLinkPaymentRemoveInputDto inputDto)
        {
            string command = GetRemoveCommand();
            int affectedRecords = await _connection.ExecuteAsync(command, inputDto, _transaction);
            if (affectedRecords <= 0)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidDeleteVosolEn);
            }
        }

        private string GetInsertCommand()
        {
            return $@"Insert Into CustomerWarehouse.dbo.PaymentsEn (
                        ZoneId,ZoneTitle,CustomerNumber,BillId,Amount,
                        RegisterDay,RegisterDayGregorian,BankName,BankBranchCode,PaymentGateway,
                        BillTableId,VillageId,VillageName,IsVillage,PayId,
                        BankCode,PayDateJalali,TempId,VosoolTableId
                    ) Values (
                        @ZoneId,@ZoneTitle,@CustomerNumber,@BillId,@Amount,
                        @RegisterDay,@RegisterDayGregorian,@BankName,@BankBranchCode,@PaymentGateway,
                        @BillTableId,@VillageId,@VillageName,@IsVillage,@PayId,
                        @BankCode,@PayDateJalali,@TempId,@VosoolTableId
                    )";
        }
        private string GetRemoveCommand()
        {
            return $@"Delete CustomerWarehouse.dbo.PaymentsEn 
                    Where 
                    	ZoneId=@ZoneId AND
                    	CustomerNumber=@CustomerNumber AND
                    	VosoolTableId=@Id";
        }

    }
}
