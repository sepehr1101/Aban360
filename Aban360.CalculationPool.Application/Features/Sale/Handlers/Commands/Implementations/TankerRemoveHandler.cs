using Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Persistence.Features.Sale.Commands.Implementations;
using Aban360.Common.Db.Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Implementations
{
    internal sealed class TankerRemoveHandler : AbstractBaseConnection, ITankerRemoveHandler
    {
        public TankerRemoveHandler(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task Handle(TankerRemoveInputDto inputDto, int userCode, CancellationToken cancellationToken)
        {
            string dbName = GetDbName(inputDto.ZoneId);

            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    TankerCommandService tankerCommandService = new(connection, transaction);
                    await tankerCommandService.Remove(GetTankerRemoveDto(inputDto, userCode), dbName);

                    transaction.Commit();
                }
            }
        }
        private TankerRemoveDto GetTankerRemoveDto(TankerRemoveInputDto inputDto, int userCode)
        {
            return new TankerRemoveDto()
            {
                CustomerNumber = inputDto.CustomerNumber,
                UserCode = userCode
            };
        }
    }
}
