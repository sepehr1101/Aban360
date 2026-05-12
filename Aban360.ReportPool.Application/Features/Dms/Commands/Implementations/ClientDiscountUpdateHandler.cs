using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Application.Features.Dms.Commands.Contracts;
using Aban360.ReportPool.Domain.Features.Dms;
using Aban360.ReportPool.Persistence.Features.Dms.Commands;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.ReportPool.Application.Features.Dms.Commands.Implementations
{
    internal sealed class ClientDiscountUpdateHandler : AbstractBaseConnection, IClientDiscountUpdateHandler
    {
        public ClientDiscountUpdateHandler(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task Handle(ClientDiscountUpdateDto input, CancellationToken cancellationToken)
        {
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    RequestDiscountCommandService clientDiscountCommandService = new(connection, transaction);
                    await clientDiscountCommandService.Update(input);
                    transaction.Commit();
                }
            }
        }
    }
}
