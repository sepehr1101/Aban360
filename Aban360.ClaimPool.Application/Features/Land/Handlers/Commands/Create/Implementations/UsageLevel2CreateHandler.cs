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
    internal sealed class UsageLevel2CreateHandler : IUsageLevel2CreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IUsageLevel2CommandService _usageLevel2CommandService;
        private readonly IValidator<UsageLevel2CreateDto> _validator;

        public UsageLevel2CreateHandler(
            IMapper mapper,
            IUsageLevel2CommandService usageLevel2CommandService,
            IValidator<UsageLevel2CreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _usageLevel2CommandService = usageLevel2CommandService;
            _usageLevel2CommandService.NotNull(nameof(_usageLevel2CommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(UsageLevel2CreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            var usageLevel2 = _mapper.Map<UsageLevel2>(createDto);
            await _usageLevel2CommandService.Add(usageLevel2);
        }
    }
}
