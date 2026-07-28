using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Application.Features.Tagging.Handlers.Commands.Contracts;
using Aban360.ReportPool.Domain.Features.Tagging;
using Aban360.ReportPool.Domain.Features.Tagging.Commands;
using Aban360.ReportPool.Persistence.Features.Tagging.Commands;
using Aban360.ReportPool.Persistence.Features.Tagging.Queries.Contracts;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.ReportPool.Application.Features.Tagging.Handlers.Commands.Implementations
{
    public sealed class CreateTagGroupHandler : AbstractBaseConnection, ICreateTagGroupHandler
    {
        private readonly ITagGroupService _service;
        public CreateTagGroupHandler(
            ITagGroupService service,
            IConfiguration configuration)
                : base(configuration)
        {
            _service = service;
            _service.NotNull(nameof(service));
        }

        public async Task<int> Handle(CreateTagGroupDto dto)
        {
            TagGroupDto? tagGroupData = await _service.GetByStringCode(dto.StringCode);
            if (tagGroupData is not null)
            {
                throw new CustomValidationException(ExceptionLiterals.InvalidDuplicateStringCode);
            }

            int result = 0;
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    TagGroupCommandService tagGroupCommandService = new(connection, transaction);
                    result = await tagGroupCommandService.Create(dto);
                    transaction.Commit();
                }
            }
            return result;
        }
    }
}