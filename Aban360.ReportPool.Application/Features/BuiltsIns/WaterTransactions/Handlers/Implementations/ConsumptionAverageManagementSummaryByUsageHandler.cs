using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using FluentValidation;
using System.Linq;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Implementations
{
    internal sealed class ConsumptionAverageManagementSummaryByUsageHandler : IConsumptionAverageManagementSummaryByUsageHandler
    {
        private readonly IConsumptionAverageManagementSummaryByUsageQueryService _consumptionAverageManagerQueryService;
        private readonly IValidator<ConsumptionAverageManagementInputDto> _validator;
        public ConsumptionAverageManagementSummaryByUsageHandler(
            IConsumptionAverageManagementSummaryByUsageQueryService consumptionAverageManagerQueryService,
            IValidator<ConsumptionAverageManagementInputDto> validator)
        {
            _consumptionAverageManagerQueryService = consumptionAverageManagerQueryService;
            _consumptionAverageManagerQueryService.NotNull(nameof(consumptionAverageManagerQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<ConsumptionAverageManagementHeaderOutputDto, ConsumptionAverageManagementSummaryDataOutputDto>> Handle(ConsumptionAverageManagementInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            ReportOutput<ConsumptionAverageManagementHeaderOutputDto, ConsumptionAverageManagementSummaryDataOutputDto> result = await _consumptionAverageManagerQueryService.Get(input, ReportLiterals.UsageTitle);

            return result;
        }

        public async Task Handle(ConsumptionAverageManagementSummrayInputDto input, CancellationToken cancellationToken)
        {
            //validation
            IEnumerable<ConsumptionAverageManagementSummaryOutputDto> management = await _consumptionAverageManagerQueryService.Get(input);

            IEnumerable<KeyValueDto> usageGroupConsumptionAverage = management
                .GroupBy(m => m.UsageTitle)
                .Select(s => new KeyValueDto(s.Key, s.Average(a => a.ConsumptionAverage)))
                .ToList();

            int lessThanContOrOlgo = management.Count(m => m.ConsumptionAverage <= m.ContracutalOrOlgo);
            int between1_2ContOrOlgo = management.Count(m => m.ConsumptionAverage > m.ContracutalOrOlgo && m.ConsumptionAverage <= m.ContracutalOrOlgo * 2);
            int between2_3ContOrOlgo = management.Count(m => m.ConsumptionAverage > m.ContracutalOrOlgo * 2 && m.ConsumptionAverage <= m.ContracutalOrOlgo * 3);
            int moreThanContOrOlgo = management.Count(m => m.ConsumptionAverage > m.ContracutalOrOlgo * 3);
            QuarterDto consumptionQuarter=new(lessThanContOrOlgo,between1_2ContOrOlgo,between2_3ContOrOlgo,moreThanContOrOlgo);
        }
        public record KeyValueDto
        {
            public string Title { get; }
            public float Count { get; }
            public KeyValueDto(string title, float count)
            {
                Title = title;
                Count = count;
            }
        }
        public record QuarterDto
        {
            public int LessThanContractualOrOlgo { get; set; }
            public int Between1_2ContractualOrOlgo { get; set; }
            public int Between2_3ContractualOrOlgo { get; set; }
            public int MoreThanContractualOrOlgo { get; set; }
            public QuarterDto(int lessThanContractualOrOlgo, int between1_2ContractualOrOlgo, int between2_3ContractualOrOlgo, int moreThanContractualOrOlgo)
            {
                LessThanContractualOrOlgo = lessThanContractualOrOlgo;
                Between1_2ContractualOrOlgo = between1_2ContractualOrOlgo;
                Between2_3ContractualOrOlgo = between2_3ContractualOrOlgo;
                MoreThanContractualOrOlgo = moreThanContractualOrOlgo;
            }
        }
    }
}