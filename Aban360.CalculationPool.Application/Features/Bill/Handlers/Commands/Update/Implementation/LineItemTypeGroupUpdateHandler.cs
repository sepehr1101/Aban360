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
    internal sealed class LineItemTypeGroupUpdateHandler : ILineItemTypeGroupUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly ILineItemTypeGroupQueryService _lineItemTypeGroupQueryService;
        private readonly IValidator<LineItemTypeGroupUpdateDto> _validator;
        public LineItemTypeGroupUpdateHandler(
            IMapper mapper,
            ILineItemTypeGroupQueryService lineItemTypeGroupQueryService,
            IValidator<LineItemTypeGroupUpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _lineItemTypeGroupQueryService = lineItemTypeGroupQueryService;
            _lineItemTypeGroupQueryService.NotNull(nameof(lineItemTypeGroupQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(LineItemTypeGroupUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            LineItemTypeGroup lineItemTypeGroup = await _lineItemTypeGroupQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, lineItemTypeGroup);
        }
    }
}
