﻿using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Delete.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Commands.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Delete.Implementations
{
    internal sealed class CordinalDirectionDeleteHandler : ICordinalDirectionDeleteHandler
    {
        private readonly ICordinalDirectionCommandService _cordinalDirectionCommandService;
        private readonly ICordinalDirectionQueryService _cordinalDirectionQueryService;
        public CordinalDirectionDeleteHandler(
            ICordinalDirectionQueryService cordinalDirectionQueryService,
            ICordinalDirectionCommandService cordinalDirectionCommandService)
        {
            _cordinalDirectionCommandService = cordinalDirectionCommandService;
            _cordinalDirectionCommandService.NotNull(nameof(cordinalDirectionCommandService));

            _cordinalDirectionQueryService = cordinalDirectionQueryService;
            _cordinalDirectionQueryService.NotNull(nameof(cordinalDirectionQueryService));
        }

        public async Task Handle(CordinalDirectionDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            CordinalDirection cordinalDirection =await _cordinalDirectionQueryService.Get(deleteDto.Id);
            await _cordinalDirectionCommandService.Remove(cordinalDirection);
        }
    }
}
