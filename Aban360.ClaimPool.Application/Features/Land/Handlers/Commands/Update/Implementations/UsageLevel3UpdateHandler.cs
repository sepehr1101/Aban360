using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Implementations
{
    internal sealed class UsageLevel3UpdateHandler : IUsageLevel3UpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IUsageLevel3QueryService _usageLevel3QueryService;
        private readonly IValidator<UsageLevel3UpdateDto> _validator;

        public UsageLevel3UpdateHandler(
            IMapper mapper,
            IUsageLevel3QueryService usageLevel3QueryService,
            IValidator<UsageLevel3UpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _usageLevel3QueryService = usageLevel3QueryService;
            _usageLevel3QueryService.NotNull(nameof(_usageLevel3QueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(UsageLevel3UpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            var usageLevel3 = await _usageLevel3QueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, usageLevel3);
        }
    }
}
