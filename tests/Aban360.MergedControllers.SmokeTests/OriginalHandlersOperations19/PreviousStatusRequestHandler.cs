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
using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Implementations;

namespace BeforeServiceOperations19.Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Implementations
{
    internal sealed class PreviousStatusRequestHandler : AbstractBaseConnection, IPreviousStatusRequestHandler
    {
        private readonly ITrackingQueryService _trackingQueryService;
        private readonly IMoshtrakQueryService _moshtrakQueryService;
        static int _requestOrigin = 12;
        public PreviousStatusRequestHandler(ITrackingQueryService trackingQueryService, IMoshtrakQueryService moshtrakQueryService, IConfiguration configuration) : base(configuration)
        {
            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));
            _moshtrakQueryService = moshtrakQueryService;
            _moshtrakQueryService.NotNull(nameof(moshtrakQueryService));
        }

        public async Task Handle(TrackNumberWithDescriptionInputDto inputDto, int userCode, CancellationToken cancellationToken)
        {
            TrackingOutputDto trackingInfo = await _trackingQueryService.GetLatest(inputDto.TrackNumber);
            MoshtrakOutputDto moshtrakInfo = (await _moshtrakQueryService.Get(new MoshtrakGetDto(trackingInfo.ZoneId, null, null, inputDto.TrackNumber), MoshtrakSearchTypeEnum.ByTrackNumber)).FirstOrDefault();
            Validate(inputDto.TrackNumber, moshtrakInfo.IsRegistered);
            TrackingOutputDto previousTrackingInfo = await _trackingQueryService.GetSecondToLatest(inputDto.TrackNumber); //todo : check
            TrackingInsertDuplicateDto trackingInsertDto = new TrackingInsertDuplicateDto(inputDto.TrackNumber, previousTrackingInfo.StatusId, inputDto.Description, userCode, _requestOrigin, true, false);
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    TrackingCommandService trackingCommandService = new(connection, transaction);
                    await trackingCommandService.UpdateIsConsiderdLatest(inputDto.TrackNumber, true);
                    await trackingCommandService.InsertDuplicate(trackingInsertDto);
                    transaction.Commit();
                }
            }
        }

        private void Validate(int trackNumber, bool isRegistered) //todo: need Or not?
        {
            if (isRegistered == true) //and not Deleted
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidStatusId);
            }
        }
    }
}
