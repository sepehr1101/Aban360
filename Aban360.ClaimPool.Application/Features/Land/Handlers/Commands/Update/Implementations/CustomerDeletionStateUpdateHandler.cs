using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Implementations
{
    internal sealed class CustomerDeletionStateUpdateHandler : AbstractBaseConnection, ICustomerDeletionStateUpdateHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ICommonZoneService _zoneService;
        private readonly ICommonMemberQueryService _memberQueryService;
        public CustomerDeletionStateUpdateHandler(
            IHttpContextAccessor contextAccessor,
            ICommonZoneService zoneService,
            ICommonMemberQueryService memberQueryService,
            IConfiguration configuration)
                : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _zoneService = zoneService;
            _zoneService.NotNull(nameof(zoneService));

            _memberQueryService = memberQueryService;
            _memberQueryService.NotNull(nameof(memberQueryService));
        }

        public async Task Handle(CustomerDeletionStateUpdateInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await _memberQueryService.Get(inputDto.BillId);
            await _zoneService.IsUserInZone(appUser, zoneIdAndCustomerNumber.ZoneId);
            MemberInfoGetDto memberInfo = await _memberQueryService.Get(zoneIdAndCustomerNumber);
            if (memberInfo.DeletionStateId == (int)inputDto.DeletionStateType)
            {
                throw new InvalidCustomerCommandException(ExceptionLiterals.InvalidDuplicateDeletionState);
            }
            CustomerDeletionStateUpdateDto deletionStateUpdateDto = new(memberInfo.Id, memberInfo.ZoneId, memberInfo.CustomerNumber, memberInfo.BillId, (int)inputDto.DeletionStateType);
            string opLogtText = string.Format(OpLogLiterals.CustomerBranchTypeUpdateOpLog, memberInfo.BillId);

            await ExecSql(zoneIdAndCustomerNumber, deletionStateUpdateDto, appUser, opLogtText);
        }
        private async Task ExecSql(ZoneIdAndCustomerNumber zoneIdAndCustomerNumber, CustomerDeletionStateUpdateDto deletionStateUpdateDto, IAppUser appUser, string opLogText)
        {
            //string dbName = ReportLiterals.Atlas;
            string dbName = GetDbName(deletionStateUpdateDto.ZoneId);
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    MembersCommandService membersCommandService = new(connection, transaction);
                    ArchMemCommandService archMemCommandService = new(connection, transaction);
                    ClientsCommandService clientsCommandService = new(connection, transaction);
                    OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);

                    await membersCommandService.Update(deletionStateUpdateDto, dbName);
                    int archMemId = await archMemCommandService.Insert(deletionStateUpdateDto, dbName, dbName);
                    await clientsCommandService.UpdateToDayJalali(zoneIdAndCustomerNumber, deletionStateUpdateDto.ToDayDateJalali);
                    await clientsCommandService.InsertByArchMemId(archMemId, dbName);
                    await opLogCommandService.Insert(opLogText, appUser);

                    transaction.Commit();
                }
            }
        }
    }
}
