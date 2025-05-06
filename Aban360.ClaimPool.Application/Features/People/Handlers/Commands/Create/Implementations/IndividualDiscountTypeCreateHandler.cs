using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.People.Entities;
using Aban360.ClaimPool.Persistence.Features.People.Commands.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;
using System.Threading;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Create.Implementations
{
    internal sealed class IndividualDiscountTypeCreateHandler : IIndividualDiscountTypeCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IIndividualDiscountTypeCommandService _individualDiscountTypeCommandService;
        private readonly IValidator<IndividualDiscountTypeCreateDto> _validator;

        public IndividualDiscountTypeCreateHandler(
            IMapper mapper,
            IIndividualDiscountTypeCommandService individualDiscountTypeCommandService,
            IValidator<IndividualDiscountTypeCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _individualDiscountTypeCommandService = individualDiscountTypeCommandService;
            _individualDiscountTypeCommandService.NotNull(nameof(_individualDiscountTypeCommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(IndividualDiscountTypeCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            var individualDiscountType = _mapper.Map<IndividualDiscountType>(createDto);
            individualDiscountType.Hash = "hash";
            individualDiscountType.ValidFrom = DateTime.Now;
            individualDiscountType.InsertLogInfo = "insertLogInfo";

            await _individualDiscountTypeCommandService.Add(individualDiscountType);
        }
    }
}
