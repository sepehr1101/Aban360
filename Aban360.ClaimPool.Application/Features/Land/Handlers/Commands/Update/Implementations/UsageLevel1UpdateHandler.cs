using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Implementations
{
    internal sealed class UsageLevel1UpdateHandler : IUsageLevel1UpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IUsageLevel1QueryService _usageLevel1QueryService;
        private readonly IValidator<UsageLevel1UpdateDto> _validator;

        public UsageLevel1UpdateHandler(
            IMapper mapper,
            IUsageLevel1QueryService usageLevel1QueryService,
            IValidator<UsageLevel1UpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _usageLevel1QueryService = usageLevel1QueryService;
            _usageLevel1QueryService.NotNull(nameof(_usageLevel1QueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(UsageLevel1UpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            var usageLevel1 = await _usageLevel1QueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, usageLevel1);
        }
    }
}
