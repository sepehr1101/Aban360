using global::Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Update.Contracts;
using global::Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Update.Contracts;
using global::Aban360.ClaimPool.Domain.Constants;
using global::Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using global::Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using global::Aban360.ClaimPool.Persistence.Features.Request.Commands.Implementations;
using global::Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using global::Aban360.Common.Db.Dapper;
using global::Aban360.Common.Exceptions;
using global::Aban360.Common.Extensions;
using global::Aban360.Common.Literals;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace BeforeServiceWorkflow10.Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Update.Implementations{
    internal sealed class MotherUpdateHandler : AbstractBaseConnection, IMotherUpdateHandler
    {
        private readonly IMotherQueryService _motherQueryService;
        private readonly IMoshtrakQueryService _moshtrakQueryService;
        private readonly ITrackingQueryService _trackingQueryService;
        private readonly IValidator<MotherChildUpdateInputDto> _childValidator;
        private readonly IValidator<CommonSiphonUpdateInputDto> _commonSiphonValidator;
        public MotherUpdateHandler(
            IMotherQueryService motherQueryService,
            IMoshtrakQueryService moshtrakQueryService,
            ITrackingQueryService trackingQueryService,
            IValidator<MotherChildUpdateInputDto> childValidator,
            IValidator<CommonSiphonUpdateInputDto> commonSiphonValidator,
            IConfiguration configuration)
            : base(configuration)
        {
            _motherQueryService = motherQueryService;
            _motherQueryService.NotNull(nameof(motherQueryService));

            _moshtrakQueryService = moshtrakQueryService;
            _moshtrakQueryService.NotNull(nameof(moshtrakQueryService));

            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));

            _childValidator = childValidator;
            _childValidator.NotNull(nameof(childValidator));

            _commonSiphonValidator = commonSiphonValidator;
            _commonSiphonValidator.NotNull(nameof(commonSiphonValidator));
        }

        public async Task Handle(MotherChildUpdateInputDto inputDto, CancellationToken cancellationToken)
        {
            await InputValidation(inputDto, cancellationToken);
            TrackingOutputDto trackingInfo = await _trackingQueryService.GetLatest(inputDto.TrackNumber);
            await MoshtrakValdiation(trackingInfo.ZoneId, inputDto.TrackNumber);
            string dbName = GetDbName(trackingInfo.ZoneId);

            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    MotherCommandService motherCommandService = new(connection, transaction);
                    await motherCommandService.Update(inputDto, dbName);

                    transaction.Commit();
                }
            }
        }
        public async Task Handle(CommonSiphonUpdateInputDto inputDto, CancellationToken cancellationToken)
        {
            await InputValidation(inputDto, cancellationToken);
            TrackingOutputDto trackingInfo = await _trackingQueryService.GetLatest(inputDto.TrackNumber);
            await MoshtrakValdiation(trackingInfo.ZoneId, inputDto.TrackNumber);
            string dbName = GetDbName(trackingInfo.ZoneId);

            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    MotherCommandService motherCommandService = new(connection, transaction);
                    await motherCommandService.Update(inputDto, dbName);

                    transaction.Commit();
                }
            }
        }
        private async Task MoshtrakValdiation(int zoneId, int trackNumber)
        {
            MoshtrakOutputDto moshtrakInfo = (await _moshtrakQueryService.Get(new MoshtrakGetDto(zoneId, null, null, trackNumber), MoshtrakSearchTypeEnum.ByTrackNumber)).FirstOrDefault();
            if (moshtrakInfo.IsRegistered)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidMotherRequest);
            }
        }
        private async Task InputValidation(MotherChildUpdateInputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _childValidator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
        private async Task InputValidation(CommonSiphonUpdateInputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _commonSiphonValidator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
    }
}
