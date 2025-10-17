using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Implementations
{
    internal sealed class HandoverCreateHandler : IHandoverCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IHandoverCommandService _handoverCommandService;
        private readonly IValidator<HandoverCreateDto> _validator;

        public HandoverCreateHandler(
            IMapper mapper,
            IHandoverCommandService handoverCommandService,
            IValidator<HandoverCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _handoverCommandService = handoverCommandService;
            _handoverCommandService.NotNull(nameof(_handoverCommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(HandoverCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            var handover = _mapper.Map<Handover>(createDto);
            await _handoverCommandService.Add(handover);
        }
    }
}
