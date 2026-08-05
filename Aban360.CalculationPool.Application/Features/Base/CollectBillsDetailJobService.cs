using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Implementations;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.CalculationPool.Application.Features.Base
{
    public sealed class CollectBillsDetailJobService : AbstractBaseConnection
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ICollectBillsDetailQueryService _detailQueryService;
        public CollectBillsDetailJobService(
            IHttpContextAccessor contextAccessor,
            ICollectBillsDetailQueryService detailQueryService,
            IConfiguration configuration)
                : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _detailQueryService = detailQueryService;
            _detailQueryService.NotNull(nameof(detailQueryService));
        }
        ///inComplete
        public async Task Insert(IAppUser appUser, int stepId, string? description)
        {
            CollectBillsDetailInsertDto insertDto = new(Guid.NewGuid(), stepId, DateTime.Now, description);
            string opLogText = string.Format(OpLogLiterals.CollectBillsDetailInsertOpLog, insertDto.GroupingId, insertDto.StepId);

            await ExecSql(insertDto, appUser, opLogText);
            //
        }
        public async Task Update(IAppUser appUser, int id)
        {
            CollectBillsDetailGetDto collectBillDetailInfo = await _detailQueryService.Get(id);
            CollectBillsDetailUpdateDto updateDto = new(id, DateTime.Now);
            string opLogText = string.Format(OpLogLiterals.CollectBillsDetailUpdateOpLog, collectBillDetailInfo.GroupingId, collectBillDetailInfo.StepId);

            await ExecSql(updateDto, appUser, opLogText);
        }
        private async Task ExecSql(CollectBillsDetailInsertDto insertDto, IAppUser appUser, string opLogText)
        {
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    CollectBillsDetailCommandService collectBillsDetailCommandService = new(connection, transaction);
                    OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);

                    await collectBillsDetailCommandService.Insert(insertDto);
                    await opLogCommandService.Insert(opLogText, appUser);

                    transaction.Commit();
                }
            }
        }
        private async Task ExecSql(CollectBillsDetailUpdateDto updateDto, IAppUser appUser, string opLogText)
        {
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    CollectBillsDetailCommandService collectBillsDetailCommandService = new(connection, transaction);
                    OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);

                    await collectBillsDetailCommandService.Update(updateDto);
                    await opLogCommandService.Insert(opLogText, appUser);

                    transaction.Commit();
                }
            }
        }
    }
}
