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
using FluentValidation;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Implementations
{
    internal sealed class SetCommonSiphonRequestHandler : AbstractBaseConnection, ISetCommonSiphonRequestHandler
    {
        private readonly IMoshtrakQueryService _moshtrakQueryService;
        private readonly ITrackingQueryService _trackingQueryService;
        private readonly IMotherQueryService _motherQueryService;
        private readonly IValidator<CommonSiphonRequestInputDto> _validator;
        public SetCommonSiphonRequestHandler(
            IMoshtrakQueryService moshtrakQueryService,
            ITrackingQueryService trackingQueryService,
            IMotherQueryService motherQueryService,
            IValidator<CommonSiphonRequestInputDto> validator,
            IConfiguration configuration)
            : base(configuration)
        {
            _moshtrakQueryService = moshtrakQueryService;
            _moshtrakQueryService.NotNull(nameof(moshtrakQueryService));

            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));

            _motherQueryService = motherQueryService;
            _motherQueryService.NotNull(nameof(motherQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task Handle(CommonSiphonRequestInputDto inputDto, int userName, CancellationToken cancellationToken)
        {
            await InputValidation(inputDto, cancellationToken);
            TrackingOutputDto trackingInfo = await _trackingQueryService.GetLatest(inputDto.TrackNumber);
            MoshtrakOutputDto moshtrackInfo = (await _moshtrakQueryService.Get(new MoshtrakGetDto(trackingInfo.ZoneId, null, null, inputDto.TrackNumber), MoshtrakSearchTypeEnum.ByTrackNumber)).FirstOrDefault();

            string dbName = GetDbName(trackingInfo.ZoneId);
            MotherInsertDto motherInsertDto = GetMotherInsertDto(inputDto, moshtrackInfo);
            await DuplicateValidation(moshtrackInfo.IsRegistered, motherInsertDto.StringTrackNumber, moshtrackInfo.ZoneId);

            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    MotherCommandService motherCommandService = new(connection, transaction);
                    await motherCommandService.Insert(motherInsertDto, dbName);
                    transaction.Commit();
                }
            }
        }
        private async Task InputValidation(CommonSiphonRequestInputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
        private async Task DuplicateValidation(bool isRegistered, string stringTrackNumber, int zoneId)
        {
            MotherInfoOutputDto? motherInfo = await _motherQueryService.Get(stringTrackNumber, zoneId);
            if (isRegistered)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidMotherRequest);
            }
            if (!isRegistered && motherInfo is not null)
            {
                throw new InvalidTrackingException(ExceptionLiterals.UseUpdateApi);
            }
        }
        private MotherInsertDto GetMotherInsertDto(CommonSiphonRequestInputDto inputDto, MoshtrakOutputDto moshtrackInfo)
        {
            return new MotherInsertDto()
            {
                ZoneId = moshtrackInfo.ZoneId,
                CustomerNumber = moshtrackInfo.CustomerNumber,
                ReadingNumber = moshtrackInfo.ReadingNumber ?? string.Empty,
                StringTrackNumber = inputDto.TrackNumber.ToString().PadLeft(11, '0'),
                MotherCustomerNumber = inputDto.MotherCustomerNumber,
                FirstName = moshtrackInfo.FirstName ?? string.Empty,
                Surname = moshtrackInfo.Surname ?? string.Empty,
                FatherName = moshtrackInfo.FatherName,
                Address = moshtrackInfo.Address,
                Siphon100 = inputDto.Siphon100,
                Siphon125 = inputDto.Siphon150,
                Siphon150 = inputDto.Siphon150,
                Siphon200 = inputDto.Siphon200,
                CommonSiphon = moshtrackInfo.CommonSiphon,
                MeterDiameterId = moshtrackInfo.MeterDiameterId,
                UsageId = moshtrackInfo.UsageId,
                CommercialUnit = 0,
                DomesticUnit = 0,
                OtherUnit = 0,
                Premises = 0,
                ImprovementOverall = 0,
                ImprovementCommercial = 0,
                ImprovementDomestic = 0,
                IsSpecial = moshtrackInfo.IsSpecial,
                ContractualCapacity = 0,
            };
        }
    }
}
