using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Implementations
{
    internal sealed class UsageLevel2UpdateHandler : IUsageLevel2UpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IUsageLevel2QueryService _usageLevel2QueryService;
        private readonly IValidator<UsageLevel2UpdateDto> _validator;

        public UsageLevel2UpdateHandler(
            IMapper mapper,
            IUsageLevel2QueryService usageLevel2QueryService,
            IValidator<UsageLevel2UpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _usageLevel2QueryService = usageLevel2QueryService;
            _usageLevel2QueryService.NotNull(nameof(_usageLevel2QueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(UsageLevel2UpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            var usageLevel2 = await _usageLevel2QueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, usageLevel2);
        }
    }
}
