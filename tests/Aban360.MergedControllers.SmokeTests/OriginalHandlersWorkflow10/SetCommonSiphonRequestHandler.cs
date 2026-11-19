using global::Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
using global::Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
using global::Aban360.ClaimPool.Domain.Constants;
using global::Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using global::Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using global::Aban360.ClaimPool.Persistence.Features.Request.Commands.Implementations;
using global::Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using global::Aban360.Common.BaseEntities;
using global::Aban360.Common.Db.Dapper;
using global::Aban360.Common.Db.Services;
using global::Aban360.Common.Exceptions;
using global::Aban360.Common.Extensions;
using global::Aban360.Common.Literals;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace BeforeServiceWorkflow10.Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Implementations{
    internal sealed class SetCommonSiphonRequestHandler : AbstractBaseConnection, ISetCommonSiphonRequestHandler
    {
        private readonly IMoshtrakQueryService _moshtrakQueryService;
        private readonly ITrackingQueryService _trackingQueryService;
        private readonly IMotherQueryService _motherQueryService;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly IValidator<CommonSiphonRequestInputDto> _validator;
        public SetCommonSiphonRequestHandler(
            IMoshtrakQueryService moshtrakQueryService,
            ITrackingQueryService trackingQueryService,
            IMotherQueryService motherQueryService,
            ICommonMemberQueryService commonMemberQueryService,
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

            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task Handle(CommonSiphonRequestInputDto inputDto, int userName, CancellationToken cancellationToken)
        {
            await InputValidation(inputDto, cancellationToken);
            ZoneIdAndCustomerNumber motherCustomerNumberInfo = await _commonMemberQueryService.Get(inputDto.MotherBillId, true);
            MemberInfoGetDto motherInfo = await _commonMemberQueryService.Get(motherCustomerNumberInfo);
            TrackingOutputDto trackingInfo = await _trackingQueryService.GetLatest(inputDto.TrackNumber);
            CommonSiphonValidate(motherInfo, trackingInfo, inputDto);

              MoshtrakOutputDto moshtrackInfo = (await _moshtrakQueryService.Get(new MoshtrakGetDto(trackingInfo.ZoneId, null, null, inputDto.TrackNumber), MoshtrakSearchTypeEnum.ByTrackNumber)).FirstOrDefault();

            string dbName = GetDbName(trackingInfo.ZoneId);
            MotherInsertDto motherInsertDto = GetMotherInsertDto(inputDto, moshtrackInfo, motherCustomerNumberInfo.CustomerNumber);
            MoshtrakCommonSiphonUpdateDto moshtrakUpdateDto = new(inputDto.TrackNumber, true);
            await DuplicateValidation(moshtrackInfo.IsRegistered, motherInsertDto.StringTrackNumber, moshtrackInfo.ZoneId);

            await ExecSql(motherInsertDto, moshtrakUpdateDto, dbName);
        }
        private async Task ExecSql(MotherInsertDto motherInsertDto, MoshtrakCommonSiphonUpdateDto moshtrakUpdateDto, string dbName)
        {
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    MotherCommandService motherCommandService = new(connection, transaction);
                    MoshtrakCommandService moshtrakCommand = new(connection, transaction);

                    await motherCommandService.Insert(motherInsertDto, dbName);
                    await moshtrakCommand.Update(moshtrakUpdateDto, dbName);

                    transaction.Commit();
                }
            }
        }
        private void CommonSiphonValidate(MemberInfoGetDto motherInfo, TrackingOutputDto trackingInfo, CommonSiphonRequestInputDto inputDto)
        {
            if (trackingInfo.ZoneId != motherInfo.ZoneId)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidMotherZoneId);
            }

            if (inputDto.Siphon100 > motherInfo.Siphon100)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidSiphon100);
            }
            if (inputDto.Siphon125> motherInfo.Siphon125)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidSiphon125);
            }
            if (inputDto.Siphon150 > motherInfo.Siphon150)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidSiphon150);
            }
            if (inputDto.Siphon200 > motherInfo.Siphon200)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidSiphon200);
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
        private MotherInsertDto GetMotherInsertDto(CommonSiphonRequestInputDto inputDto, MoshtrakOutputDto moshtrackInfo, int motherCustomerNumber)
        {
            return new MotherInsertDto()
            {
                ZoneId = moshtrackInfo.ZoneId,
                CustomerNumber = moshtrackInfo.CustomerNumber,
                ReadingNumber = moshtrackInfo.ReadingNumber ?? string.Empty,
                StringTrackNumber = inputDto.TrackNumber.ToString().PadLeft(11, '0'),
                MotherCustomerNumber = motherCustomerNumber,
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
