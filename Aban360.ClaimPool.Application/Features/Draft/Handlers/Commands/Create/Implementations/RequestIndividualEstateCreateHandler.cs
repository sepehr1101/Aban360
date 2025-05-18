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
    internal sealed class RequestIndividualEstateCreateHandler : IRequestIndividualEstateCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IRequestIndividualEstateCommandService _requestIndividualEstateCommandService;
        private readonly IValidator<IndividualEstateRequestCreateDto> _validator;

        public RequestIndividualEstateCreateHandler(
            IMapper mapper,
            IRequestIndividualEstateCommandService requestIndividualEstateCommandService,
            IValidator<IndividualEstateRequestCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _requestIndividualEstateCommandService = requestIndividualEstateCommandService;
            _requestIndividualEstateCommandService.NotNull(nameof(_requestIndividualEstateCommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(IndividualEstateRequestCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            var requestIndividualEstate = _mapper.Map<RequestIndividualEstate>(createDto);

            await _requestIndividualEstateCommandService.Add(requestIndividualEstate);
        }
    }
}
