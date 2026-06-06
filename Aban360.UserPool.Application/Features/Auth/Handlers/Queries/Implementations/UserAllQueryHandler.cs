using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Implementations
{
    public sealed class UserAllQueryHandler : IUserAllQueryHandler
    {
        private readonly IUserQueryService _userQueryService;
        private readonly IT51QueryService _t51QueryService;
        private readonly IMapper _mapper;
        public UserAllQueryHandler(
            IMapper mapper,
            IUserClaimQueryService userClaimQueryService,
            IT51QueryService t51QueryService,
            IUserQueryService userQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _userQueryService = userQueryService;
            _userQueryService.NotNull(nameof(userQueryService));

            _t51QueryService = t51QueryService;
            _t51QueryService.NotNull(nameof(t51QueryService));
        }
        public async Task<ICollection<UserQueryDto>> Handle(CancellationToken cancellationToken)
        {
            IEnumerable<NumericDictionary> zonesInfo = await _t51QueryService.Get();
            ICollection<UserQueryDto> userQueryDtoList = await _userQueryService.GetWithDefaultZone();
            userQueryDtoList.ForEach(u =>
                u.DefaultZoneTitle = zonesInfo
                    .Where(zone => zone.Id == (string.IsNullOrWhiteSpace(u.DefaultZoneId) ? 0 : (int.Parse)(u.DefaultZoneId)))
                    .FirstOrDefault()?
                    .Title ?? string.Empty
            );
            return userQueryDtoList;
        }
    }
}
