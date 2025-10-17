using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;
using System.Threading;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Update.Implementation
{
    internal sealed class LineItemTypeUpdateHandler : ILineItemTypeUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly ILineItemTypeQueryService _lineItemTypeQueryService;
        private readonly IValidator<LineItemTypeUpdateDto> _validator;

        public LineItemTypeUpdateHandler(
            IMapper mapper,
            ILineItemTypeQueryService lineItemTypeQueryService,
            IValidator<LineItemTypeUpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _lineItemTypeQueryService = lineItemTypeQueryService;
            _lineItemTypeQueryService.NotNull(nameof(lineItemTypeQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(LineItemTypeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            LineItemType lineItemType = await _lineItemTypeQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, lineItemType);
        }
    }
}
