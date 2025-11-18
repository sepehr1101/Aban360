using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using FluentValidation;
using static Aban360.Common.Timing.CalculationDistanceDate;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Implementations
{
    internal sealed class WaterDistanceDeliverToInstallDetailHandler : IWaterDistanceDeliverToInstallDetailHandler
    {
        private readonly IWaterDistanceDeliverToInstallDetailQueryService _waterDistanceDeliverToInstallQueryService;
        private readonly IValidator<WaterDistanceDeliverToInstallInputDto> _validator;
        public WaterDistanceDeliverToInstallDetailHandler(
            IWaterDistanceDeliverToInstallDetailQueryService waterDistanceDeliverToInstallQueryService,
            IValidator<WaterDistanceDeliverToInstallInputDto> validator)
        {
            _waterDistanceDeliverToInstallQueryService = waterDistanceDeliverToInstallQueryService;
            _waterDistanceDeliverToInstallQueryService.NotNull(nameof(waterDistanceDeliverToInstallQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<SewageWaterDistanceHeaderOutputDto, SewageWaterDistanceDetailDataOutputDto>> Handle(WaterDistanceDeliverToInstallInputDto input, CancellationToken cancellationToken)
        {
            var validatioResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validatioResult.IsValid)
            {
                var message = string.Join(", ", validatioResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            ReportOutput<SewageWaterDistanceHeaderOutputDto, SewageWaterDistanceDetailDataOutputDto> result = await _waterDistanceDeliverToInstallQueryService.Get(input);

            ICollection<int> requestToInstallDistances = new List<int>();
            result.ReportData.ForEach(data =>
            {
                CalcDistanceResultDto calcInstallationDistance = CalcDistance(data.RequestDate, data.InstallationDate);
                data.DistanceOfRequestAndInstallation = calcInstallationDistance.HasError ? ExceptionLiterals.Incalculable : calcInstallationDistance.DistanceText;

                CalcDistanceResultDto calcRegisterDistance = CalcDistance(data.RequestDate, data.RegisterDate);
                data.DistanceOfRequestAndRegister= calcRegisterDistance.HasError ? ExceptionLiterals.Incalculable : calcRegisterDistance.DistanceText;
              
                requestToInstallDistances.Add(calcInstallationDistance.HasError ? 0 : calcInstallationDistance.Distance);
            });

            int averageDistance = requestToInstallDistances.Sum() / (result.ReportHeader.RecordCount <= 0 ? 1 : result.ReportHeader.RecordCount);
            result.ReportHeader.AverageDistance = ConvertDayToDate(averageDistance);
            result.ReportHeader.MaxDistance = ConvertDayToDate(requestToInstallDistances.MaxValue());
            result.ReportHeader.MinDistance = ConvertDayToDate(requestToInstallDistances.MinValue());

            return result;
        }
    }
}