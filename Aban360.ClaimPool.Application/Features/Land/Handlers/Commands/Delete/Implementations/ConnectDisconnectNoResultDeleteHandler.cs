using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Implementations
{
    internal sealed class ConnectDisconnectNoResultDeleteHandler : AbstractBaseConnection, IConnectDisconnectNoResultDeleteHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IConnectDisconnectQueryService _connectDisconnectQueryService;
        private const int _connectTypeId = 1;
        public ConnectDisconnectNoResultDeleteHandler(
            IHttpContextAccessor contextAccessor,
            IConnectDisconnectQueryService connectDisconnectQueryService,
            IConfiguration configuration)
            : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _connectDisconnectQueryService = connectDisconnectQueryService;
            _connectDisconnectQueryService.NotNull(nameof(connectDisconnectQueryService));
        }

        public async Task Handle(ConnectDisconnectRemoveInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            ConnectDisconnectGetDto connectDisconnectInfo = await _connectDisconnectQueryService.Get(inputDto.Id, null);
            if (connectDisconnectInfo.RemovedDateTime is not null)
            {
                throw new InvalidCustomerCommandException(ExceptionLiterals.InvalidConnectDisconnectDeleteDuplicate);
            }

            ConnectDisconnectRemoveDto connectDisconnectRemoveDto = new(inputDto.Id, DateTime.Now, appUser.UserId, string.Join("_", new string[] { connectDisconnectInfo.Description ?? string.Empty, inputDto.Description ?? string.Empty }));
            string connectOpLog = string.Format(OpLogLiterals.ServiceLinkConnectRemoveOpLog, connectDisconnectInfo.BillId, inputDto.Id);
            string disconnectOpLog = string.Format(OpLogLiterals.ServiceLinkDisconnectRemoveOpLog, connectDisconnectInfo.BillId, inputDto.Id);
            string opLogText = connectDisconnectInfo.TypeId == _connectTypeId ? connectOpLog : disconnectOpLog;

            await SqlCommands(connectDisconnectRemoveDto, appUser, opLogText);
        }
        private async Task SqlCommands(ConnectDisconnectRemoveDto connectDisconnectRemoveDto, IAppUser appUser, string opLogText)
        {
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    ConnectDisconnectCommandService _connectDisconnectCommandService = new(connection, transaction);
                    OpLogWithTransactionCommandService _opLogCommandService = new(_contextAccessor, connection, transaction);

                    await _connectDisconnectCommandService.Remove(connectDisconnectRemoveDto);
                    await _opLogCommandService.Insert(opLogText, appUser);

                    transaction.Commit();
                }
            }

        }
    }
}
