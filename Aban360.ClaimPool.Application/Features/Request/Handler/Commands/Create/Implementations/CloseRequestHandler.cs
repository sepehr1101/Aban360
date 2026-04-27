using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Request.Commands.Implementations;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Implementations
{
    internal sealed class CloseRequestHandler : AbstractBaseConnection, ICloseRequestHandler
    {
        private readonly IMoshtrakQueryService _moshtrakQueryService;
        private readonly ITrackingQueryService _trackingQueryService;
        static int _removeRequestStatusId = 90000;
        static int _requestOrigin = 12;
        public CloseRequestHandler(
            IMoshtrakQueryService moshtrakQueryService,
            ITrackingQueryService trackingQueryService,
            IConfiguration configuration)
            : base(configuration)
        {
            _moshtrakQueryService = moshtrakQueryService;
            _moshtrakQueryService.NotNull(nameof(moshtrakQueryService));

            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));
        }

        public async Task<RequestCloseOuputDto> Handle(CloseRequestInputDto inputDto, int userName, CancellationToken cancellationToken)
        {
            TrackingOutputDto? trackingInfo = await _trackingQueryService.GetFirstStep(inputDto.TrackNumber, false);
            string deleteDescription = $"حذف توسط {userName}";

            var (moshtrakSabtUpdate, moshtrakInfo) = await GetMoshtrakUpdateDto(trackingInfo, inputDto, userName, deleteDescription);
            TrackingInsertDuplicateDto trackingInsertDto = new(trackingInfo?.TrackNumber ?? 0, _removeRequestStatusId, deleteDescription, userName, _requestOrigin, true, false);

            string dbName = GetDbName(moshtrakInfo.ZoneId);
            await SqlCommands(trackingInfo, moshtrakSabtUpdate, trackingInsertDto, dbName);

            return new RequestCloseOuputDto(moshtrakInfo.ZoneId, moshtrakInfo.ZoneTitle, inputDto.TrackNumber);
        }
        private async Task<(MoshtrakSabtUpdateDto, MoshtrakOutputDto)> GetMoshtrakUpdateDto(TrackingOutputDto? trackingInfo, CloseRequestInputDto inputDto, int userName, string deleteDescription)
        {
            MoshtrakOutputDto moshtrakInfo;

            if (trackingInfo == null)
            {
                moshtrakInfo = await _moshtrakQueryService.Get(inputDto.Id, inputDto.ZoneId);
                if (moshtrakInfo.IsRegistered == false)
                {
                    throw new InvalidTrackingException(ExceptionLiterals.NotFountOpenRequest);
                }
            }
            else
            {
                IEnumerable<MoshtrakOutputDto> moshtrakListInfo = await _moshtrakQueryService.Get(new MoshtrakGetDto(trackingInfo.ZoneId, null, null, trackingInfo.TrackNumber), MoshtrakSearchTypeEnum.ByTrackNumber);
                if (!moshtrakListInfo.Where(m => m.IsRegistered == false).Any())
                {
                    throw new InvalidTrackingException(ExceptionLiterals.NotFountOpenRequest);
                }
                moshtrakInfo = (await _moshtrakQueryService.Get(new MoshtrakGetDto(trackingInfo.ZoneId, null, null, inputDto.TrackNumber), MoshtrakSearchTypeEnum.ByTrackNumber)).FirstOrDefault();
            }
            MoshtrakSabtUpdateDto moshtrakSabtUpdate = new(moshtrakInfo.Id, true, $"{moshtrakInfo.Description} - {deleteDescription}");

            return (moshtrakSabtUpdate, moshtrakInfo);
        }
        private async Task SqlCommands(TrackingOutputDto? trackingInfo, MoshtrakSabtUpdateDto moshtrakSabtUpdate, TrackingInsertDuplicateDto trackingInsertDto, string dbName)
        {
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    if (trackingInfo != null)
                    {
                        TrackingCommandService _trackingCommandService = new(connection, transaction);
                        await _trackingCommandService.InsertDuplicate(trackingInsertDto);
                        await _trackingCommandService.UpdateIsConsiderdLatest(trackingInfo.TrackNumber, true);
                    }
                    MoshtrakCommandService _moshtrackCommandService = new(connection, transaction);
                    await _moshtrackCommandService.UpdateSabt(moshtrakSabtUpdate, dbName);

                    transaction.Commit();
                }
            }
        }
    }
}
