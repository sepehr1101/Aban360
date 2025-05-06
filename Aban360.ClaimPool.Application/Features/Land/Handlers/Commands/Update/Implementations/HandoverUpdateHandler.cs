using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Implementations
{
    internal sealed class HandoverUpdateHandler : IHandoverUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IHandoverQueryService _handoverQueryService;
        private readonly IValidator<HandoverUpdateDto> _validator;

        public HandoverUpdateHandler(
            IMapper mapper,
            IHandoverQueryService handoverQueryService,
            IValidator<HandoverUpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _handoverQueryService = handoverQueryService;
            _handoverQueryService.NotNull(nameof(_handoverQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(HandoverUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            var handover = await _handoverQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, handover);
        }
    }
}
