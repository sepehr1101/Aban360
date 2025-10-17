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
    internal sealed class UserLeaveCreateHandler : IUserLeaveCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IUserLeaveCommandService _userLeaveCommandService;
        private readonly IValidator<UserLeaveCreateDto> _validator;

        public UserLeaveCreateHandler(
            IMapper mapper,
            IUserLeaveCommandService userLeaveCommandService,
            IValidator<UserLeaveCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _userLeaveCommandService = userLeaveCommandService;
            _userLeaveCommandService.NotNull(nameof(_userLeaveCommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(UserLeaveCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            var userLeave = _mapper.Map<UserLeave>(createDto);
            await _userLeaveCommandService.Add(userLeave);
        }
    }
}
