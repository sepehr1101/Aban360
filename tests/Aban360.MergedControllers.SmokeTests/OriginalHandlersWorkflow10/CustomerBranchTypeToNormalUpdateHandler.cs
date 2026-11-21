using global::Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using global::Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using global::Aban360.ClaimPool.Domain.Constants;
using global::Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using global::Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations;
using global::Aban360.Common.ApplicationUser;
using global::Aban360.Common.BaseEntities;
using global::Aban360.Common.Db.Constants.Literals;
using global::Aban360.Common.Db.Dapper;
using global::Aban360.Common.Db.Services;
using global::Aban360.Common.Exceptions;
using global::Aban360.Common.Extensions;
using global::Aban360.Common.Literals;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace BeforeServiceWorkflow10.Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Implementations{
    internal sealed class CustomerBranchTypeToNormalUpdateHandler : AbstractBaseConnection, ICustomerBranchTypeToNormalUpdateHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ICommonZoneService _zoneService;
        private readonly ICommonMemberQueryService _memberQueryService;
        public CustomerBranchTypeToNormalUpdateHandler(
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

        public async Task Handle(BranchTypeToNormalUpdateInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await _memberQueryService.Get(inputDto.BillId);
            await _zoneService.IsUserInZone(appUser, zoneIdAndCustomerNumber.ZoneId);
            MemberInfoGetDto memberInfo = await _memberQueryService.Get(zoneIdAndCustomerNumber);
            if (memberInfo.UseStateId == (int)BranchTypeEnum.Normal)
            {
                throw new InvalidCustomerCommandException(ExceptionLiterals.InvalidDuplicateBranchType);
            }
            CustomerBranchTypeUpdateDto branchTypeUpdateDto = new(memberInfo.Id, memberInfo.ZoneId, memberInfo.CustomerNumber, memberInfo.BillId, (int)BranchTypeEnum.Normal);
            string opLogtText = string.Format(OpLogLiterals.CustomerBranchTypeUpdateOpLog, memberInfo.BillId);

            await ExecSql(zoneIdAndCustomerNumber, branchTypeUpdateDto, appUser, opLogtText);
        }
        private async Task ExecSql(ZoneIdAndCustomerNumber zoneIdAndCustomerNumber, CustomerBranchTypeUpdateDto branchTypeUpdateDto, IAppUser appUser, string opLogText)
        {
            //string dbName = ReportLiterals.Atlas;
            string dbName = GetDbName(branchTypeUpdateDto.ZoneId);
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

                    await membersCommandService.Update(branchTypeUpdateDto, dbName);
                    int archMemId = await archMemCommandService.Insert(branchTypeUpdateDto, dbName, dbName);
                    await clientsCommandService.UpdateToDayJalali(zoneIdAndCustomerNumber, branchTypeUpdateDto.ToDayDateJalali);
                    await clientsCommandService.InsertByArchMemId(archMemId, dbName);
                    await opLogCommandService.Insert(opLogText, appUser);

                    transaction.Commit();
                }
            }
        }
    }
}
