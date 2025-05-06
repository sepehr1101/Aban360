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
    internal sealed class UsageLevel1CreateHandler : IUsageLevel1CreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IUsageLevel1CommandService _usageLevel1CommandService;
        private readonly IValidator<UsageLevel1CreateDto> _validator;

        public UsageLevel1CreateHandler(
            IMapper mapper,
            IUsageLevel1CommandService usageLevel1CommandService,
            IValidator<UsageLevel1CreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _usageLevel1CommandService = usageLevel1CommandService;
            _usageLevel1CommandService.NotNull(nameof(_usageLevel1CommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(UsageLevel1CreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            var usageLevel1 = _mapper.Map<UsageLevel1>(createDto);
            await _usageLevel1CommandService.Add(usageLevel1);
        }
    }
}
