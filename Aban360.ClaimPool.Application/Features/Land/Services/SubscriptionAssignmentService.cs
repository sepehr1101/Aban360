
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Features.Request.Commands.Implementations;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Infrastructure.Features.Geo;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.ClaimPool.Application.Features.Land.Services;
public interface ISubscriptionAssignmentService
{
    Task<SubscriptionAssignmentGetDto> GetAsync(string input, CancellationToken cancellationToken);
    Task UpdateByTrackNumberAsync(SubscriptionAssignmentByTrackNumberUpdateDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
}

internal sealed class SubscriptionAssignmentService : AbstractBaseConnection, ISubscriptionAssignmentService
{
    private readonly ISubscriptionQueryService _subscriptionAssignmentQueryService;
    private readonly IGisService _gisService;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly ITrackingQueryService _trackingQueryService;
    private readonly IMoshtrakQueryService _moshtrakQueryService;
    private readonly ICommonZoneService _zoneService;
    private static int[] _invalidStatus =
    {
        (int)TrackingStatusEnum.Deleted,
        (int)TrackingStatusEnum.Archived,
        (int)TrackingStatusEnum.Registered
    };
    public SubscriptionAssignmentService(IConfiguration configuration, ISubscriptionQueryService subscriptionAssignmentQueryService, IGisService gisService, IHttpContextAccessor contextAccessor, ITrackingQueryService trackingQueryService, IMoshtrakQueryService moshtrakQueryService, ICommonZoneService zoneService) : base(configuration)
    {
        _subscriptionAssignmentQueryService = subscriptionAssignmentQueryService;
        _subscriptionAssignmentQueryService.NotNull(nameof(subscriptionAssignmentQueryService));
        _gisService = gisService;
        _gisService.NotNull(nameof(gisService));
        _contextAccessor = contextAccessor;
        _contextAccessor.NotNull(nameof(contextAccessor));
        _trackingQueryService = trackingQueryService;
        _trackingQueryService.NotNull(nameof(trackingQueryService));
        _moshtrakQueryService = moshtrakQueryService;
        _moshtrakQueryService.NotNull(nameof(moshtrakQueryService));
        _zoneService = zoneService;
        _zoneService.NotNull(nameof(zoneService));
    }

    public async Task<SubscriptionAssignmentGetDto> GetAsync(string input, CancellationToken cancellationToken)
    {
        SubscriptionAssignmentGetDto subscriptionAssignment = await _subscriptionAssignmentQueryService.Get(input);
        CustomerLocationDto customerLocation = await _gisService.GetCustomerLocation(new CustomerLocationInputDto(input));
        SubscriptionAssignmentGetDto result = GetGetLocationInfo(subscriptionAssignment, customerLocation);
        return result;
    }

    private SubscriptionAssignmentGetDto GetGetLocationInfo(SubscriptionAssignmentGetDto input, CustomerLocationDto customerLocation)
    {
        input.X = customerLocation.X;
        input.Y = customerLocation.Y;
        input.Easting = 0;
        input.Northing = 0;
        input.UtmZone = 0;
        input.Letter = string.Empty;
        if (!string.IsNullOrWhiteSpace(input.X) && !string.IsNullOrWhiteSpace(input.Y))
        {
            var utm = UtmConverter.LatLonToUtm(double.Parse(input.X), double.Parse(input.Y));
            input.Easting = utm.Easting;
            input.Northing = utm.Northing;
            input.UtmZone = utm.Zone;
            input.Letter = utm.Letter;
        }

        return input;
    }

    public async Task UpdateByTrackNumberAsync(SubscriptionAssignmentByTrackNumberUpdateDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        TrackingOutputDto trackingInfo = await _trackingQueryService.GetLatest(inputDto.TrackNumber);
        MoshtrakOutputDto? mosthrakInfo = (await _moshtrakQueryService.Get(new MoshtrakGetDto(trackingInfo.ZoneId, null, null, trackingInfo.TrackNumber), MoshtrakSearchTypeEnum.ByTrackNumber)).FirstOrDefault();
        UpdateByTrackNumberValidate(trackingInfo, mosthrakInfo);
        await _zoneService.IsUserInZone(appUser, trackingInfo.ZoneId);
        string opLogText = string.Format(OpLogLiterals.SubscriptionAssignmentByTrackNumberUpdateOpLog, inputDto.TrackNumber);
        await UpdateByTrackNumberExecSql(inputDto, appUser, trackingInfo.ZoneId, opLogText);
    }

    private async Task UpdateByTrackNumberExecSql(SubscriptionAssignmentByTrackNumberUpdateDto inputDto, IAppUser appUser, int zoneId, string opLogText)
    {
        string dbName = GetDbName(zoneId);
        using (IDbConnection connection = _sqlReportConnection)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                MoshtrakCommandService moshtrakCommandService = new(connection, transaction);
                OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);
                await moshtrakCommandService.Update(inputDto, dbName);
                await opLogCommandService.Insert(opLogText, appUser);
                transaction.Commit();
            }
        }
    }

    private void UpdateByTrackNumberValidate(TrackingOutputDto trackingInfo, MoshtrakOutputDto? mosthrakInfo)
    {
        if (_invalidStatus.Contains(trackingInfo.StatusId))
        {
            throw new InvalidTrackingException(ExceptionLiterals.InvalidStatusId);
        }

        if (mosthrakInfo is null)
        {
            throw new InvalidTrackingException(ExceptionLiterals.NotFoundMoshtrak);
        }

        if (mosthrakInfo.IsRegistered)
        {
            throw new InvalidTrackingException(ExceptionLiterals.TrackingRegistered);
        }
    }
}
