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
    internal sealed class RequestIndividualCreateHandler : IRequestIndividualCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IRequestIndividualCommandService _requestIndividualCommandService;
        private readonly IValidator<IndividualRequestCreateDto> _validator;

        public RequestIndividualCreateHandler(
            IMapper mapper,
            IRequestIndividualCommandService requestIndividualCommandService,
            IValidator<IndividualRequestCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _requestIndividualCommandService = requestIndividualCommandService;
            _requestIndividualCommandService.NotNull(nameof(_requestIndividualCommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(IndividualRequestCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            var requestIndividual = _mapper.Map<RequestIndividual>(createDto);
            await _requestIndividualCommandService.Add(requestIndividual);
        }
    }
}
