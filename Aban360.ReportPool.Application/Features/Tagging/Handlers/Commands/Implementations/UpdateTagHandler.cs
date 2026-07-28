using Aban360.Common.Db.Dapper;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Tagging.Handlers.Commands.Contracts;
using Aban360.ReportPool.Domain.Features.Tagging.Commands;
using Aban360.ReportPool.Persistence.Features.Tagging.Commands;
using Aban360.ReportPool.Persistence.Features.Tagging.Queries.Contracts;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.ReportPool.Application.Features.Tagging.Handlers.Commands.Implementations
{
    public sealed class UpdateTagHandler : AbstractBaseConnection, IUpdateTagHandler
    {
        private readonly ITagService _service;
        public UpdateTagHandler(
            ITagService service,
            IConfiguration configuration)
                : base(configuration)
        {
            _service = service;
            _service.NotNull(nameof(service));
        }

        public async Task<bool> Handle(UpdateTagDto dto)
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
                    TagCommandService tagCommandService = new(connection, transaction);
                    result = await tagCommandService.Update(dto);
                    transaction.Commit();
                }
            }
            return result;
        }
    }
}