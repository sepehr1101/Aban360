using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Extensions;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Implementations
{
    internal sealed class UsageGroup1DuplicateInsertHandler : AbstractBaseConnection, IUsageGroup1DuplicateInsertHandler
    {
        private readonly IUsageGroup1QueryService _UsageGroup1QueryService;
        private readonly IUsageGroup2QueryService _UsageGroup2QueryService;
        private readonly IUsageGroup3QueryService _UsageGroup3QueryService;
        public UsageGroup1DuplicateInsertHandler(
            IUsageGroup1QueryService UsageGroup1QueryService,
            IUsageGroup2QueryService UsageGroup2QueryService,
            IUsageGroup3QueryService UsageGroup3QueryService,
            IConfiguration conifguration)
                : base(conifguration)
        {
            _UsageGroup1QueryService = UsageGroup1QueryService;
            _UsageGroup1QueryService.NotNull(nameof(UsageGroup1QueryService));

            _UsageGroup2QueryService = UsageGroup2QueryService;
            _UsageGroup2QueryService.NotNull(nameof(UsageGroup2QueryService));

            _UsageGroup3QueryService = UsageGroup3QueryService;
            _UsageGroup3QueryService.NotNull(nameof(UsageGroup3QueryService));
        }

        public async Task<UsageGroup1DuplicateInsertOutputDto> Handle(UsageGroup1DuplicateInsetInputDto inputDto, CancellationToken cancellationToken)
        {
            UsageGroup1GetDto usageGroup1 = await _UsageGroup1QueryService.Get(inputDto.UsageGroup1Id);
            IEnumerable<UsageGroup2GetDto> usageGroup2s = await _UsageGroup2QueryService.GetByParentId(inputDto.UsageGroup1Id);
            IEnumerable<UsageGroup3GetDto> usageGroup3s = await _UsageGroup3QueryService.GetByParrentIds(usageGroup2s.Select(tg => tg.Id));

            if (!inputDto.IsConfirm)
            {
                return new UsageGroup1DuplicateInsertOutputDto(1, usageGroup2s?.Count() ?? 0, usageGroup3s?.Count() ?? 0, inputDto.UsageGroup1Id, inputDto.IsConfirm);
            }
            UsageGroup1DuplicateInsertDto UsageGroup1InsertDto = new(inputDto.UsageGroup1Id, inputDto.UsageGroup1Title);

            await ExecSql(inputDto.UsageGroup1Id,UsageGroup1InsertDto);
            return new UsageGroup1DuplicateInsertOutputDto(1, usageGroup2s?.Count() ?? 0, usageGroup3s?.Count() ?? 0, inputDto.UsageGroup1Id, inputDto.IsConfirm);
        }
        private async Task ExecSql(short previousGroup1Id, UsageGroup1DuplicateInsertDto UsageGroup1InsertDto)
        {
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    UsageGroup1CommandService usageGroup1CommandService = new(connection, transaction);
                    UsageGroup2CommandService usageGroup2CommandService = new(connection, transaction);
                    UsageGroup3CommandService usageGroup3CommandService = new(connection, transaction);

                    short newUsageGroup1Id = await usageGroup1CommandService.InsertDuplicate(UsageGroup1InsertDto);
                    IEnumerable<UsageGroup2GetDto> prevoiusGroup2 = await _UsageGroup2QueryService.GetByParentId(previousGroup1Id);
                    foreach (var pre2 in prevoiusGroup2)
                    {
                        short newGroup2Id = await usageGroup2CommandService.InsertDuplicateById(pre2.Id, newUsageGroup1Id);
                        await usageGroup3CommandService.InsertByParentId(pre2.Id, newGroup2Id);
                    }

                    transaction.Commit();
                }
            }
        }
    }
}
