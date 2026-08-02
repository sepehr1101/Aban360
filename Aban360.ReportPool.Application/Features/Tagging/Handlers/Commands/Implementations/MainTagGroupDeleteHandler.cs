using Aban360.Common.Db.Dapper;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Tagging.Handlers.Commands.Contracts;
using Aban360.ReportPool.Domain.Features.Tagging;
using Aban360.ReportPool.Domain.Features.Tagging.Commands;
using Aban360.ReportPool.Persistence.Features.Tagging.Commands;
using Aban360.ReportPool.Persistence.Features.Tagging.Queries.Contracts;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.ReportPool.Application.Features.Tagging.Handlers.Commands.Implementations
{
    public sealed class MainTagGroupDeleteHandler : AbstractBaseConnection, IMainTagGroupDeleteHandler
    {
        private readonly IMainTagGroupQueryService _service;
        private readonly ITagGroupQueryService _tagGroupService;
        public MainTagGroupDeleteHandler(
            IMainTagGroupQueryService service,
            ITagGroupQueryService tagGroupService,
            IConfiguration configuration)
                : base(configuration)
        {
            _service = service;
            _service.NotNull(nameof(service));

            _tagGroupService = tagGroupService;
            _tagGroupService.NotNull(nameof(tagGroupService));
        }

        public async Task Handle(int id)
        {
            MainTagGroupRemoveDto removeDto = new(id);
            IEnumerable<TagGroupDto> tagGroupsInfo = await _tagGroupService.GetByMainTagGroupId(id);

            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    MainTagGroupCommandService mainTagGroupCommandService = new(connection, transaction);
                    TagGroupCommandService tagGroupCommandService = new(connection, transaction);
                    TagCommandService tagCommandService = new(connection, transaction);

                    await mainTagGroupCommandService.Remove(removeDto);
                    await tagGroupCommandService.DeleteByMainGroupId(id);
                    await tagCommandService.DeleteByTagGroupId(tagGroupsInfo.Select(t => t.Id));

                    transaction.Commit();
                }
            }
        }
    }
}
