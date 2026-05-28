using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Dapper;
using System.Data;

namespace Aban360.CalculationPool.Persistence.Features.ServiceLink.Commands.Implementations
{
    public sealed class VosolEnCommandService
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        public VosolEnCommandService(IDbConnection connection, IDbTransaction transaction)
        {
            _connection = connection;
            _connection.NotNull(nameof(connection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }
        public async Task<int> Insert(VosoEnInsertDto inputDto, string dbName)
        {
            string command = GetInsertCommand(dbName, true);
            int recordId = await _connection.QueryFirstOrDefaultAsync<int>(command, inputDto, _transaction);
            if (recordId <= 0)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidInsertVosolEn);
            }
            return recordId;
        }
        public async Task Insert(IEnumerable<VosoEnInsertDto> inputDto, string dbName)
        {
            string command = GetInsertCommand(dbName, false);
            int affectedRecords = await _connection.ExecuteAsync(command, inputDto, _transaction);
            if (affectedRecords != (inputDto?.Count() ?? 0))
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidInsertVosolEn);
            }
        }
        public async Task Remove(ServiceLinkPaymentRemoveInputDto inputDto, string dbName)
        {
            string command = GetRemoveCommand(dbName, true);
            int affectedRecords = await _connection.ExecuteAsync(command, inputDto, _transaction);
            if (affectedRecords <= 0)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidDeleteVosolEn);
            }
        }

        private string GetInsertCommand(string dbName, bool hasReturnId)
        {
            string recordIdQuery = hasReturnId ? "Select SCOPE_IDENTITY();" : string.Empty;
            return $@"INSERT INTO [{dbName}].dbo.vosolEN (
                        town, radif, par_no, pay_date, date_bes, date_bank, date_sabt, cod_bank,
                        serial, ser, cod1, cod2, cod3, pard, jam, elat, barge, enshab, cod_enshab,
                        operator, type_pay, sh_pard, sh_ghabs, type, noe_bed, mohlat, tedad_mas,
                        tedad_tej, tedad_vahd, CHECK_NO, cod_report, CHK_KARBARI, Pass_check,
                        C120, C220, tmp_date_bes, tmp_date_sabt, tmp_pay_date, tmp_date_bank
                    )
                    VALUES (
                        @Town, @Radif, @ParNo, @PayDate, @DateBes, @DateBank, @DateSabt, @CodBank,
                        @Serial, @Ser, @Cod1, @Cod2, @Cod3, @Pard, @Jam, @Elat, @Barge, @Enshab, @CodEnshab,
                        @Operator, @TypePay, @ShPard, @ShGhabs, @Type, @NoeBed, @Mohlat, @TedadMas,
                        @TedadTej, @TedadVahd, @CheckNo, @CodReport, @ChkKarbari, @PassCheck,
                        @C120, @C220, @TmpDateBes, @TmpDateSabt, @TmpPayDate, @TmpDateBank
                    );
                    {recordIdQuery}";
        }
        private string GetRemoveCommand(string dbName, bool hasReturnId)
        {
            return $@"Delete [{dbName}].dbo.vosolEN 
                    Where 
                    	town=@ZoneId AND 
                    	radif=@CustomerNumber AND
                    	ID=@Id";
        }

    }
}
