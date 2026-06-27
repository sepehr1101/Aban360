using Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Request.Commands.Implementations;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Implementations
{
    internal sealed class ServiceLinkRemoveUnconfirmedHandler : AbstractBaseConnection, IServiceLinkRemoveUnconfirmedHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly ICommonZoneService _commonZoneService;
        private readonly IKartQueryService _kartQueryService;
        public ServiceLinkRemoveUnconfirmedHandler(
            IHttpContextAccessor contextAccessor,
            ICommonMemberQueryService commonMemberQueryService,
            ICommonZoneService commonZoneService,
            IKartQueryService kartQueryService,
            IConfiguration configuration)
                : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));

            _commonZoneService = commonZoneService;
            _commonMemberQueryService.NotNull(nameof(commonZoneService));

            _kartQueryService = kartQueryService;
            _kartQueryService.NotNull(nameof(kartQueryService));
        }

        public async Task Handle(ServiceLinkRemoveUnconfirmedInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await _commonZoneService.IsUserInZone(appUser, inputDto.ZoneId);
            KartRemoveByIdDto kartRemoveDto = new(inputDto.Id, inputDto.ZoneId, inputDto.CustomerNumber);
            await ExecSql(kartRemoveDto);
        }
        private async Task ExecSql(KartRemoveByIdDto kartRemoveDto)
        {
            //string dbName = "Atlas";
            string dbName = GetDbName(kartRemoveDto.ZoneId);

            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    KartCommandService kartCommandService = new(connection, transaction);
                    await kartCommandService.Remove(kartRemoveDto, dbName);

                    transaction.Commit();
                }
            }
        }
    }
}
