using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Implementations
{
    internal sealed class UsageLevel4UpdateHandler : IUsageLevel4UpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IUsageLevel4QueryService _usageLevel4QueryService;
        private readonly IValidator<UsageLevel4UpdateDto> _validator;

        public UsageLevel4UpdateHandler(
            IMapper mapper,
            IUsageLevel4QueryService usageLevel4QueryService,
            IValidator<UsageLevel4UpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _usageLevel4QueryService = usageLevel4QueryService;
            _usageLevel4QueryService.NotNull(nameof(_usageLevel4QueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(UsageLevel4UpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            var usageLevel4 = await _usageLevel4QueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, usageLevel4);
        }
    }
}
