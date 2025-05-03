using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Implementations
{
    internal sealed class RequestFlatUpdateHandler : IRequestFlatUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IRequestFlatQueryService _requestFlatQueryService;
        private readonly IValidator<FlatRequestUpdateDto> _validator;

        public RequestFlatUpdateHandler(
            IMapper mapper,
            IRequestFlatQueryService requestFlatQueryService,
            IValidator<FlatRequestUpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _requestFlatQueryService = requestFlatQueryService;
            _requestFlatQueryService.NotNull(nameof(_requestFlatQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(FlatRequestUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            var requestFlat = await _requestFlatQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, requestFlat);
        }
    }
}
