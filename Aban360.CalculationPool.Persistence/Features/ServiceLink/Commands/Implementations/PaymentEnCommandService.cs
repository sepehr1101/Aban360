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
        public async Task Insert(IEnumerable<PaymentEnInsertDto> inputDto)
        {
            string command = GetInsertCommand();
            int affectedRecords = await _connection.ExecuteAsync(command, inputDto, _transaction);
            if (affectedRecords != (inputDto?.Count() ?? 0))
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidInsertVosolEn);
            }
        }

        private string GetInsertCommand()
        {
            return $@"Insert Into CustomerWarehouse.dbo.PaymentsEn (
                        ZoneId,ZoneTitle,CustomerNumber,BillId,Amount,
                        RegisterDay,RegisterDayGregorian,BankName,BankBranchCode,PaymentGateway,
                        BillTableId,VillageId,VillageName,IsVillage,PayId,
                        BankCode,PayDateJalali,TempId
                    ) Values (
                        @ZoneId,@ZoneTitle,@CustomerNumber,@BillId,@Amount,
                        @RegisterDay,@RegisterDayGregorian,@BankName,@BankBranchCode,@PaymentGateway,
                        @BillTableId,@VillageId,@VillageName,@IsVillage,@PayId,
                        @BankCode,@PayDateJalali,@TempId
                    )";
        }
    }
}
