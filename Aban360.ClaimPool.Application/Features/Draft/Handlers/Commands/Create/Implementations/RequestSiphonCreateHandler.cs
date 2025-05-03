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
    internal sealed class RequestSiphonCreateHandler : IRequestSiphonCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IRequestSiphonCommandService _requestSiphonCommandService;
        private readonly IValidator<SiphonRequestCreateDto> _validator;

        public RequestSiphonCreateHandler(
            IMapper mapper,
            IRequestSiphonCommandService requestSiphonCommandService,
            IValidator<SiphonRequestCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _requestSiphonCommandService = requestSiphonCommandService;
            _requestSiphonCommandService.NotNull(nameof(_requestSiphonCommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(SiphonRequestCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            var requestSiphon = _mapper.Map<RequestSiphon>(createDto);
            await _requestSiphonCommandService.Add(requestSiphon);
        }
    }
}
