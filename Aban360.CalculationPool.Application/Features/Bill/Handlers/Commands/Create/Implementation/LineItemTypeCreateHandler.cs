using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;
using System.ComponentModel.DataAnnotations;
using System.Threading;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Implementation
{
    internal sealed class LineItemTypeCreateHandler : ILineItemTypeCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly ILineItemTypeCommandService _lineItemTypeCommandService;
        private readonly IValidator<LineItemTypeCreateDto> _validator;
        public LineItemTypeCreateHandler(
            IMapper mapper,
            ILineItemTypeCommandService lineItemTypeCommandService,
            IValidator<LineItemTypeCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _lineItemTypeCommandService = lineItemTypeCommandService;
            _lineItemTypeCommandService.NotNull(nameof(lineItemTypeCommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task Handle(LineItemTypeCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            LineItemType lineItemType = _mapper.Map<LineItemType>(createDto);
            await _lineItemTypeCommandService.Add(lineItemType);
        }
    }
}
