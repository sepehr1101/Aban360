using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.Features.Auth.Commands.Contracts;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Update.Implementations
{
    public class UserUpdateHandler
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IUserCommandService _userCommandService;
        private readonly IUserQueryService _userQueryService;
        private readonly IUserClaimQueryService _userClaimsQueryService;
        private readonly IUserRoleQueryService _userRoleQueryService;

        private readonly IHttpContextAccessor _contextAccessor;
        public UserUpdateHandler(
            IUnitOfWork uow,
            IMapper mapper,
            IUserCommandService userCommandService,
            IUserQueryService userQueryService,
            IUserClaimQueryService userClaimsQueryService,
            IUserRoleQueryService userRoleQueryService,
            IHttpContextAccessor contextAccessor)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _userCommandService = userCommandService;
            _userCommandService.NotNull(nameof(userCommandService));

            _userQueryService = userQueryService;
            _userQueryService.NotNull(nameof(userQueryService));

            _userClaimsQueryService = userClaimsQueryService;
            _userClaimsQueryService.NotNull(nameof(userClaimsQueryService));

            _userRoleQueryService = userRoleQueryService;
            _userRoleQueryService.NotNull(nameof(userRoleQueryService));

            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));
        }
        public async Task Handle(UserUpdateDto userUpdateDto, CancellationToken cancellationToken)
        {
            var logInfo = DeviceDetection.GetLogInfo(_contextAccessor.HttpContext.Request);
            var logInfoString= JsonOperation.Marshal(logInfo);
            var userInDb= await _userQueryService.Get(userUpdateDto.Id);
            var user= _mapper.Map<User>(userInDb);
            user.PreviousId = userInDb.Id;
            _userCommandService.Remove(userInDb, logInfoString);
        }
    }
}
