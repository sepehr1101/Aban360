using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts;
using Aban360.Common.Db.Dapper;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.CalculationPool.Persistence.Features.MeterReading.Implementations
{
    internal sealed class MeterReadingFileService : AbstractBaseConnection, IMeterReadingFileService
    {
        public MeterReadingFileService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task Create(MeterReadingFileCreateDto input)
        {
            string command = GetInsertCommand();
            await _sqlReportConnection.ExecuteAsync(command, input);
        }
        private string GetInsertCommand()
        {
            return @"INSERT INTO [Aban360].[CalculationPool].[MeterReadingFile] (
                         Title,FileName,RecordCount,AgentCode,ZoneId,InsertByUserId,InsertDateTime)
                    VALUES (
                         @Title,@FileName,@RecordCount,@AgentCode,@ZoneId,@InsertByUserId,@InsertDateTime);";
        }
    }
}
