using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts;
using AutoMapper;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Implementations
{
    internal sealed class UserWorkdayCreateHandler : IUserWorkdayCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IUserWorkdayCommandService _userWorkdayCommandService;
        private readonly IZoneTitleAddhoc _zoneTitleAddhoc;
        private readonly IValidator<UserWorkdayCreateDto> _validator;

        public UserWorkdayCreateHandler(
            IMapper mapper,
            IUserWorkdayCommandService userWorkdayCommandService,
            IZoneTitleAddhoc zoneTitleAddhoc,
            IValidator<UserWorkdayCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _userWorkdayCommandService = userWorkdayCommandService;
            _userWorkdayCommandService.NotNull(nameof(_userWorkdayCommandService));

            _zoneTitleAddhoc=zoneTitleAddhoc;
            _zoneTitleAddhoc.NotNull(nameof(_zoneTitleAddhoc));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(UserWorkdayCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            var userWorkday = _mapper.Map<UserWorkday>(createDto);
            var zoneTitle=await _zoneTitleAddhoc.Handle(createDto.ZoneId,cancellationToken);

            userWorkday.ZoneTitle = zoneTitle;
            await _userWorkdayCommandService.Add(userWorkday);
        }
    }
}
