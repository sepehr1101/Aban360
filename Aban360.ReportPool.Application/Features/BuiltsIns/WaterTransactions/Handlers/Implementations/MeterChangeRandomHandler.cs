using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using FluentValidation;
using System.Runtime.InteropServices;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Implementations
{
    internal sealed class MeterChangeRandomHandler : IMeterChangeRandomHandler
    {
        private readonly IMeterChangeRandomQueryService _basicInfoChangeHistoryRandomQueryService;
        private readonly IValidator<MeterChangeRandomInputDto> _validator;
        public MeterChangeRandomHandler(
            IMeterChangeRandomQueryService basicInfoChangeHistoryRandomQueryService,
            IValidator<MeterChangeRandomInputDto> validator)
        {
            _basicInfoChangeHistoryRandomQueryService = basicInfoChangeHistoryRandomQueryService;
            _basicInfoChangeHistoryRandomQueryService.NotNull(nameof(basicInfoChangeHistoryRandomQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<MeterChangeRandomHeaderOutputDto, MeterChangeRandomDataOutputDto>> Handle(MeterChangeRandomInputDto input, [Optional] CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            ReportOutput<MeterChangeRandomHeaderOutputDto, MeterChangeRandomDataOutputDto> basicInfoChangeHistoryRandom = await _basicInfoChangeHistoryRandomQueryService.GetInfo(input);
            return basicInfoChangeHistoryRandom;
        }
    }
}
