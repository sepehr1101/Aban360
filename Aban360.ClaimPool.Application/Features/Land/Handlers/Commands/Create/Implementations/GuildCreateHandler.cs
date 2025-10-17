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
    internal sealed class GuildCreateHandler : IGuildCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IGuildCommandService _commandService;
        private readonly IValidator<GuildCreateDto> _validator;

        public GuildCreateHandler(
            IMapper mapper,
            IGuildCommandService commandService,
            IValidator<GuildCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _commandService = commandService;
            _commandService.NotNull(nameof(commandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(GuildCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            Guild guild = _mapper.Map<Guild>(createDto);
            await _commandService.Add(guild);
        }
    }
}
