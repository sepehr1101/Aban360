﻿using Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.People.Queries
{
    [Route("individual-tag-definition")]
    public class IndividualTagDefinitionGetAllController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IIndividualTagDefinitionGetAllHandler _tagDefinitionHandler;
        public IndividualTagDefinitionGetAllController(
            IUnitOfWork uow,
            IIndividualTagDefinitionGetAllHandler tagDefinitionHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _tagDefinitionHandler = tagDefinitionHandler;
            _tagDefinitionHandler.NotNull(nameof(tagDefinitionHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var IndividualTagDefinition = await _tagDefinitionHandler.Handle(cancellationToken);
            return Ok(IndividualTagDefinition);
        }
    }
}
