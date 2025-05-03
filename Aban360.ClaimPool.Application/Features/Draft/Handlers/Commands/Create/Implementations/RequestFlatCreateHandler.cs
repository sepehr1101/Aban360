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
    internal sealed class RequestFlatCreateHandler : IRequestFlatCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IRequestFlatCommandService _requestFlatCommandService;
        private readonly IValidator<FlatRequestCreateDto> _validator;

        public RequestFlatCreateHandler(
            IMapper mapper,
            IRequestFlatCommandService requestFlatCommandService,
            IValidator<FlatRequestCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _requestFlatCommandService = requestFlatCommandService;
            _requestFlatCommandService.NotNull(nameof(_requestFlatCommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(FlatRequestCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            var requestFlat = _mapper.Map<RequestFlat>(createDto);
            await _requestFlatCommandService.Add(requestFlat);
        }
    }
}
