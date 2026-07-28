using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Tagging.Handlers.Commands.Contracts;
using Aban360.ReportPool.Domain.Features.Tagging;
using Aban360.ReportPool.Domain.Features.Tagging.CustomerWarehouse.Application.DTOs;
using Aban360.ReportPool.Persistence.Features.Tagging.Commands;
using Aban360.ReportPool.Persistence.Features.Tagging.Queries.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.ReportPool.Application.Features.Tagging.Handlers.Commands.Implementations
{
    internal sealed class BillIdTagRemoveByTagIdsHandler : AbstractBaseConnection, IBillIdTagRemoveByTagIdsHandler
    {
        private readonly IBillIdTagQueryService _billIdTagQueryService;
        private readonly IHttpContextAccessor _contextAccessor;
        public BillIdTagRemoveByTagIdsHandler(
            IBillIdTagQueryService billIdTagQueryService,
             IHttpContextAccessor contextAccessor,
             IConfiguration configuration)
                : base(configuration)
        {
            _billIdTagQueryService = billIdTagQueryService;
            _billIdTagQueryService.NotNull(nameof(billIdTagQueryService));

            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));
        }

        public async Task<BillIdTagRemoveByTagIdsOutputDto> Handle(BillIdTagRemoveByTagIdsInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            IEnumerable<BillIdTagDto> billIdTaglist = await _billIdTagQueryService.GetByTagIds(inputDto.TagIds);
            int billCount = billIdTaglist?.DistinctBy(b => b.BillId)?.Count() ?? 0;
            int recordCount = billIdTaglist?.Count() ?? 0;

            if (!inputDto.IsConfirm)
            {
                return new BillIdTagRemoveByTagIdsOutputDto(billCount, recordCount, false);
            }
            string opLogText = string.Format(OpLogLiterals.BillIdTagListDelete, inputDto?.TagIds?.Count() ?? 0, recordCount);
            await ExecSql(inputDto.TagIds, appUser, opLogText);

            return new BillIdTagRemoveByTagIdsOutputDto(billCount, recordCount, true);
        }
        private async Task ExecSql(IEnumerable<int> tagIds, IAppUser appUser, string opLogText)
        {
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    BillIdTagCommandService billIdTagCommandService = new(connection, transaction);
                    OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);

                    await billIdTagCommandService.Delete(tagIds);
                    await opLogCommandService.Insert(opLogText, appUser);

                    transaction.Commit();
                }
            }
        }
    }
}
