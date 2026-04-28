using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Request.Commands.Implementations;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.QueryServices;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Implementations
{
    internal sealed class SetBillIdHandler : AbstractBaseConnection, ISetBillIdHandler
    {
        private readonly ICommonMemberQueryService _memberQueryService;
        private readonly IVariabService _variabService;
        private readonly ITrackingQueryService _trackingQueryService;
        private readonly IT52QueryService _t52QueryService;
        static int _setAssessmentResult = 110;
        static int _reCalculateRequired = 65;
        static int[] _allowedStatus = { _reCalculateRequired, _setAssessmentResult };
        public SetBillIdHandler(
            ICommonMemberQueryService memberQueryService,
            IVariabService variabService,
            ITrackingQueryService trackingQueryService,
            IT52QueryService t52QueryService,
            IConfiguration configuration)
            : base(configuration)
        {
            _memberQueryService = memberQueryService;
            _memberQueryService.NotNull(nameof(memberQueryService));

            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));

            _variabService = variabService;
            _variabService.NotNull(nameof(variabService));

            _t52QueryService = t52QueryService;
            _t52QueryService.NotNull(nameof(t52QueryService));
        }

        public async Task Handle(int trackNumber, CancellationToken cancellationToken)
        {
            TrackingOutputDto latestTrackingInfo = await _trackingQueryService.GetLatest(trackNumber);
            if (!_allowedStatus.Contains(latestTrackingInfo.StatusId))
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidStatusId);
            }

            int customerNumber = await GetCustomerNumber(latestTrackingInfo.ZoneId);
            string _3digitZoneId = await _t52QueryService.Get(new ZoneIdAndCustomerNumber(latestTrackingInfo.ZoneId, customerNumber));
            string billId = TransactionIdGenerator.GenerateBillId(customerNumber.ToString(), _3digitZoneId);

            TrackingBillIdUpdateDto trackingBillIdUpdateDto = new(trackNumber, billId);
            MoshtrakCustomerNumberUpdateDto moshtrackCustomerNumberUpdateDto = new(trackNumber, customerNumber);
            string dbName = GetDbName(latestTrackingInfo.ZoneId);

            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    TrackingCommandService trackingCommandService = new(connection, transaction);
                    MoshtrakCommandService moshtrakCommandService = new(connection, transaction);

                    await trackingCommandService.UpdateBillId(trackingBillIdUpdateDto);
                    await moshtrakCommandService.Update(moshtrackCustomerNumberUpdateDto, dbName);

                    transaction.Commit();
                }
            }
        }
        private async Task<int> GetCustomerNumber(int zoneId)
        {
            int customerNumber = await _variabService.GetAndRenewRadif(zoneId);

            int[] differentZoneIds = { 131301, 131302, 131303, 131304, 131305 };
            if (differentZoneIds.Contains(zoneId))
            {
                int leftUnit = int.Parse(zoneId.ToString().Substring(zoneId.ToString().Length - 1, 1));
                int rightUnit = new Random().Next(0, 10);
                return int.Parse($"{leftUnit}{customerNumber}{rightUnit}");
            }
            return customerNumber;
        }
    }
}
