﻿using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.AccessTree.Quereis
{
    [Route("v1/module")]
    public class ModuleGetAllController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IModuleGetAllHandler _moduleGetAllHandler;
        public ModuleGetAllController(
            IUnitOfWork uow,
            IModuleGetAllHandler moduleGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _moduleGetAllHandler = moduleGetAllHandler;
            _moduleGetAllHandler.NotNull(nameof(moduleGetAllHandler));
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<ModuleGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<ModuleGetDto> module = await _moduleGetAllHandler.Handle(cancellationToken);
            return Ok(module);
        }
    }
}
