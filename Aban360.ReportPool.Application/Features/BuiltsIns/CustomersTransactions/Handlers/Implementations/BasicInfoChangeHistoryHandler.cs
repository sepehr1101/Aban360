using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using FluentValidation;
using System.Runtime.InteropServices;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Implementations
{
    internal sealed class BasicInfoChangeHistoryHandler : IBasicInfoChangeHistoryHandler
    {
        private readonly IBasicInfoChangeHistoryQueryService _basicInfoChangeHistoryQueryService;
        private readonly IValidator<BasicInfoChangeHistoryInputDto> _validator;
        public BasicInfoChangeHistoryHandler(
            IBasicInfoChangeHistoryQueryService basicInfoChangeHistoryQueryService,
            IValidator<BasicInfoChangeHistoryInputDto> validator)
        {
            _basicInfoChangeHistoryQueryService = basicInfoChangeHistoryQueryService;
            _basicInfoChangeHistoryQueryService.NotNull(nameof(basicInfoChangeHistoryQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<BasicInfoChangeHistoryHeaderOutputDto, BasicInfoChangeHistoryDataOutputDto>> Handle(BasicInfoChangeHistoryInputDto input, [Optional] CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input/*, cancellationToken*/);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            ReportOutput<BasicInfoChangeHistoryHeaderOutputDto, BasicInfoChangeHistoryDataOutputDto> BasicInfoChangeHistory = await _basicInfoChangeHistoryQueryService.GetInfo(input);
            return BasicInfoChangeHistory;
        }
    }
}
