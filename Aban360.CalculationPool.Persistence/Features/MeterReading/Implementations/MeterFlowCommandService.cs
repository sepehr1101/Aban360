using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Dapper;
using System.Data;

namespace Aban360.CalculationPool.Persistence.Features.MeterReading.Implementations
{
    public sealed class MeterFlowCommandService
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        public MeterFlowCommandService(
            IDbConnection connection,
            IDbTransaction transaction)
        {
            _connection = connection;
            _connection.NotNull(nameof(connection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }

        public async Task<int> Insert(MeterFlowCreateDto input)
        {
            string command = GetInsertCommand();
            int id = await _connection.ExecuteScalarAsync<int>(command, input, _transaction);
            if (id <= 0)
            {
                throw new ReadingException(ExceptionLiterals.InvalidMeterFlow);
            }

            return id;
        }
        public async Task Insert(ICollection<MeterFlowCreateDto> input)
        {
            string command = GetInsertCommand();
            await _connection.ExecuteAsync(command, input, _transaction);

        }
        public async Task Update(MeterFlowUpdateDto input)
        {
            string query = GetUpdateCommand();
            // int rowEffected = await _connection.ExecuteAsync(query, input, _transaction);
            int rowEffected = 0;
            if (rowEffected <= 0)
            {
                throw new ReadingException(ExceptionLiterals.InvalidMeterFlow);
            }
        }

        private string GetInsertCommand()
        {
            return @"INSERT [Atlas].[dbo].[MeterFlow] 
                        (
                            MeterFlowStepId,FileName,ZoneId,
                            InsertDateTime,InsertByUserId,Description
                        )
                    VALUES 
                        (
                            @MeterFlowStepId,@FileName,@ZoneId,
                            @InsertDateTime,@InsertByUserId,@Description
                        );
                    SELECT CAST(SCOPE_IDENTITY() AS int);";
        }
        private string GetUpdateCommand()
        {
            return @"Update Atlas.dbo.MeterFlow
                        Set RemovedDateTime=@RemovedDateTime , RemovedByUserId=@RemovedByUserId
                        Where Id=@id";
        }

    }
}
