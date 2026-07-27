using Aban360.Common.Db.Dapper;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Tagging.Handlers.Commands.Contracts;
using Aban360.ReportPool.Persistence.Features.Tagging;
using Aban360.ReportPool.Persistence.Features.Tagging.Commands;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.ReportPool.Application.Features.Tagging.Handlers.Commands.Implementations
{
    internal sealed class DeleteBillIdTagHandler : AbstractBaseConnection, IDeleteBillIdTagHandler
    {
        private readonly IBillIdTagService _service;
        public DeleteBillIdTagHandler(
            IBillIdTagService service,
            IConfiguration configuration)
            : base(configuration)
        {
            _service = service;
            _service.NotNull(nameof(service));
        }

        public async Task<bool> Handle(long id)
        {
            bool result = false;
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    BillIdTagCommandService billIdTagCommandService = new(connection, transaction);

                    result = await billIdTagCommandService.Delete(id);

                    transaction.Commit();
                }
            }
            return result;
        }
    }
}
