using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Implementations
{
    internal sealed class RequestIndividualDiscountTypeCreateHandler : IRequestIndividualDiscountTypeCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IRequestIndividualDiscountTypeCommandService _requestIndividualDiscountTypeCommandService;
        private readonly IValidator<RequestIndividualDiscountTypeCreateDto> _validator;

        public RequestIndividualDiscountTypeCreateHandler(
            IMapper mapper,
            IRequestIndividualDiscountTypeCommandService requestIndividualDiscountTypeCommandService,
            IValidator<RequestIndividualDiscountTypeCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _requestIndividualDiscountTypeCommandService = requestIndividualDiscountTypeCommandService;
            _requestIndividualDiscountTypeCommandService.NotNull(nameof(_requestIndividualDiscountTypeCommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(RequestIndividualDiscountTypeCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            var individualDiscountType = _mapper.Map<RequestIndividualDiscountType>(createDto);
            individualDiscountType.Hash = "hash";
            individualDiscountType.ValidFrom = DateTime.Now;
            individualDiscountType.InsertLogInfo = "insertLogInfo";

            await _requestIndividualDiscountTypeCommandService.Add(individualDiscountType);
        }
    }
}
