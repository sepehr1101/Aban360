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
    internal sealed class SetMotherChildRequestHandler : AbstractBaseConnection, ISetMotherChildRequestHandler
    {
        private readonly IMoshtrakQueryService _moshtrakQueryService;
        private readonly ITrackingQueryService _trackingQueryService;
        private readonly IMotherQueryService _motherQueryService;
        private readonly IValidator<MotherChildRequestInputDto> _validator;
        public SetMotherChildRequestHandler(
            IMoshtrakQueryService moshtrakQueryService,
            ITrackingQueryService trackingQueryService,
            IMotherQueryService motherQueryService,
            IValidator<MotherChildRequestInputDto> validator,
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

        public async Task Handle(MotherChildRequestInputDto inputDto, int userName, CancellationToken cancellationToken)
        {
            await InputValidation(inputDto, cancellationToken);
            TrackingOutputDto trackingInfo = await _trackingQueryService.GetLatest(inputDto.TrackNumber);
            MoshtrakOutputDto moshtrackInfo = (await _moshtrakQueryService.Get(new MoshtrakGetDto(trackingInfo.ZoneId, null, null, inputDto.TrackNumber), MoshtrakSearchTypeEnum.ByTrackNumber)).FirstOrDefault();
            MotherInsertDto motherInsertDto = GetMotherInsertDto(inputDto, moshtrackInfo);

            string dbName = GetDbName(trackingInfo.ZoneId);
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
        private async Task InputValidation(MotherChildRequestInputDto inputDto, CancellationToken cancellationToken)
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
        private MotherInsertDto GetMotherInsertDto(MotherChildRequestInputDto inputDto, MoshtrakOutputDto moshtrackInfo)
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
                Siphon100 = 0,
                Siphon125 = 0,
                Siphon150 = 0,
                Siphon200 = 0,
                CommonSiphon = 0,
                MeterDiameterId = moshtrackInfo.MeterDiameterId,
                UsageId = moshtrackInfo.UsageId,
                CommercialUnit = inputDto.CommercialUnit,
                DomesticUnit = inputDto.DomesticUnit,
                OtherUnit = inputDto.OtherUnit,
                Premises = inputDto.Premises,
                ImprovementOverall = inputDto.ImprovementOverall,
                ImprovementCommercial = inputDto.ImprovementCommercial,
                ImprovementDomestic = inputDto.ImprovementDomestic,
                IsSpecial = moshtrackInfo.IsSpecial,
                ContractualCapacity = inputDto.ContractualCapacity,
            };
        }
    }
}
