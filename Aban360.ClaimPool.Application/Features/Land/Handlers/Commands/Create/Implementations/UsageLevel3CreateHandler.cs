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
    internal sealed class UsageLevel3CreateHandler : IUsageLevel3CreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IUsageLevel3CommandService _usageLevel3CommandService;
        private readonly IValidator<UsageLevel3CreateDto> _validator;

        public UsageLevel3CreateHandler(
            IMapper mapper,
            IUsageLevel3CommandService usageLevel3CommandService,
            IValidator<UsageLevel3CreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _usageLevel3CommandService = usageLevel3CommandService;
            _usageLevel3CommandService.NotNull(nameof(_usageLevel3CommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(UsageLevel3CreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            var usageLevel3 = _mapper.Map<UsageLevel3>(createDto);
            await _usageLevel3CommandService.Add(usageLevel3);
        }
    }
}
