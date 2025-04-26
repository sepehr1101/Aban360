using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;
using System.Threading;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Implementation
{
    internal sealed class LineItemTypeGroupCreateHandler : ILineItemTypeGroupCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly ILineItemTypeGroupCommandService _lineItemTypeGroupCommandService;
        private readonly IValidator<LineItemTypeGroupCreateDto> _validator;

        public LineItemTypeGroupCreateHandler(
            IMapper mapper,
            ILineItemTypeGroupCommandService lineItemTypeGroupCommandService,
            IValidator<LineItemTypeGroupCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _lineItemTypeGroupCommandService = lineItemTypeGroupCommandService;
            _lineItemTypeGroupCommandService.NotNull(nameof(lineItemTypeGroupCommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(LineItemTypeGroupCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            LineItemTypeGroup lineItemTypeGroup = _mapper.Map<LineItemTypeGroup>(createDto);
            await _lineItemTypeGroupCommandService.Add(lineItemTypeGroup);
        }
    }
}
