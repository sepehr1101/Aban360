using Aban360.Common.Db.Dapper;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Tagging.Handlers.Commands.Contracts;
using Aban360.ReportPool.Persistence.Features.Tagging.Commands;
using Aban360.ReportPool.Persistence.Features.Tagging.Queries.Contracts;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.ReportPool.Application.Features.Tagging.Handlers.Commands.Implementations
{
    public sealed class DeleteTagGroupHandler : AbstractBaseConnection, IDeleteTagGroupHandler
    {
        private readonly ITagGroupQueryService _service;
        public DeleteTagGroupHandler(
            ITagGroupQueryService service,
            IConfiguration configuration)
                : base(configuration)
        {
            _service = service;
            _service.NotNull(nameof(service));
        }

        // Soft delete
        public async Task<bool> Handle(int id)
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
                    TagGroupCommandService tagGroupCommandService = new(connection, transaction);
                    TagCommandService tagCommandService=new(connection, transaction);

                    result = await tagGroupCommandService.Delete(id);
                    await tagCommandService.DeleteByTagGroupId(id);

                    transaction.Commit();
                }
            }
            return result;
        }
    }
}