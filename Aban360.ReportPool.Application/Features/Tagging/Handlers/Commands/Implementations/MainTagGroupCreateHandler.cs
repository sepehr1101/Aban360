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
    public sealed class MainTagGroupCreateHandler : AbstractBaseConnection, IMainTagGroupCreateHandler
    {
        private readonly IMainTagGroupService _service;
        public MainTagGroupCreateHandler(
            IMainTagGroupService service,
            IConfiguration configuration)
                : base(configuration)
        {
            _service = service;
            _service.NotNull(nameof(service));
        }

        public async Task Handle(MainTagGroupInsertInputDto input)
        {
            MainTagGroupInsertDto insertDto = new(input.Title);
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    MainTagGroupCommandService mainTagGroupCommandService = new(connection, transaction);
                    await mainTagGroupCommandService.Insert(insertDto);
                    transaction.Commit();
                }
            }
        }
    }
}
