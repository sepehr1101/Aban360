using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;
using System.Threading;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Implementations
{
    internal sealed class UsageCreateHandler : IUsageCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IUsageCommandSevice _usageCommandService;
        private readonly IValidator<UsageCreateDto> _validator;

        public UsageCreateHandler(
            IMapper mapper,
            IUsageCommandSevice usageCommandService,
            IValidator<UsageCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _usageCommandService = usageCommandService;
            _usageCommandService.NotNull(nameof(usageCommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(UsageCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            Usage usage = _mapper.Map<Usage>(createDto);
            await _usageCommandService.Add(usage);
        }
    }
}
