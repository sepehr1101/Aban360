using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts;
using AutoMapper;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Implementations
{
    internal sealed class UserWorkdayUpdateHandler : IUserWorkdayUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IUserWorkdayQueryService _userWorkdayQueryService;
        private readonly IZoneTitleAddhoc _zoneTitleAddhoc;
        private readonly IValidator<UserWorkdayUpdateDto> _validator;

        public UserWorkdayUpdateHandler(
            IMapper mapper,
            IUserWorkdayQueryService userWorkdayQueryService,
            IZoneTitleAddhoc zoneTitleAddhoc,
            IValidator<UserWorkdayUpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _userWorkdayQueryService = userWorkdayQueryService;
            _userWorkdayQueryService.NotNull(nameof(_userWorkdayQueryService));

            _zoneTitleAddhoc = zoneTitleAddhoc;
            _zoneTitleAddhoc.NotNull(nameof(_zoneTitleAddhoc));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(UserWorkdayUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            var userWorkday = await _userWorkdayQueryService.Get(updateDto.Id);
            var zoneTitle = await _zoneTitleAddhoc.Handle(updateDto.ZoneId, cancellationToken);

            userWorkday.ZoneTitle = zoneTitle;
            _mapper.Map(updateDto, userWorkday);
        }
    }
}
