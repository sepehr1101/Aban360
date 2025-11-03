using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.UiElement.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Aban360.UserPool.Persistence.Constants.Enums;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using Aban360.UserPool.Persistence.Features.UiElement.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Implementations
{
    internal sealed class TopbarQueryHandler : ITopbarQueryHandler
    {
        private readonly IAppQueryService _appQueryService;
        private readonly IModuleQueryService _moduleQueryService;
        private readonly ISubModuleQueryService _subModuleQueryService;
        private readonly IEndpointQueryService _endpointQueryService;
        private readonly IUserClaimQueryService _userClaimQueryService;
        public TopbarQueryHandler(
            IAppQueryService appQueryService,
            IModuleQueryService moduleQueryService,
            ISubModuleQueryService subModuleQueryService,
            IEndpointQueryService endpointQueryService,
            IUserClaimQueryService userClaimQueryService)
        {
            _appQueryService = appQueryService;
            _appQueryService.NotNull(nameof(appQueryService));

            _moduleQueryService = moduleQueryService;
            _moduleQueryService.NotNull(nameof(_moduleQueryService));

            _subModuleQueryService = subModuleQueryService;
            _subModuleQueryService.NotNull(nameof(_subModuleQueryService));

            _endpointQueryService = endpointQueryService;
            _endpointQueryService.NotNull(nameof(_endpointQueryService));

            _userClaimQueryService = userClaimQueryService;
            _userClaimQueryService.NotNull(nameof(_userClaimQueryService));
        }
        public async Task<Topbar> Handle(Guid userId, CancellationToken cancellationToken)
        {
            var query = from app in _appQueryService.GetQuery().Where(a => a.IsActive && a.InMenu)
                        join module in _moduleQueryService.GetQuery().Where(a => a.IsActive && a.InMenu)
                            on app.Id equals module.AppId
                        join subModule in _subModuleQueryService.GetQuery().Where(a => a.IsActive && a.InMenu)
                            on module.Id equals subModule.ModuleId
                        join endpoint in _endpointQueryService.GetQuery().Where(a => a.IsActive && a.InMenu)
                            on subModule.Id equals endpoint.SubModuleId
                        join userClaim in _userClaimQueryService.GetQuery().Where(u => u.UserId == userId && u.ValidTo == null && u.RemoveGroupId == null && u.ClaimTypeId == ClaimType.Endpoint)
                            on endpoint.AuthValue equals userClaim.ClaimValue
                        select new
                        {
                            _App = new
                            {
                                app.Id,
                                app.Title,
                                app.LogicalOrder,
                                app.Style
                            },
                            _Module = new
                            {
                                module.Id,
                                module.Title,
                                module.ClientRoute,
                                module.LogicalOrder,
                                module.Style
                            }
                        };
            var list = await query.ToListAsync();
            var items = list
                .GroupBy(l => l._App.Id)
                .Select(appGroup => new TopbarLevel1()
                {
                    Id = appGroup.Key,
                    Style = appGroup.First()._App.Style,
                    Title = appGroup.First()._App.Title,
                    LogicalOrder = appGroup.First()._App.LogicalOrder,
                    Level2s = appGroup
                        .GroupBy(l => l._Module.Id)
                        .Select(moduleGroup => new TopbarLevel2()
                        {
                            Id = moduleGroup.Key,
                            Style = moduleGroup.First()._Module.Style,
                            LogicalOrder = moduleGroup.First()._Module.LogicalOrder,
                            ClientRoute = moduleGroup.First()._Module.ClientRoute,
                            Title = moduleGroup.First()._Module.Title
                        })
                        .OrderBy(subModule => subModule.LogicalOrder)
                        .ToList()
                })
                .OrderBy(module => module.LogicalOrder)
                .ToList();
            return new Topbar() { Level1s = items };
        }     
    }
}
