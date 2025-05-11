using Aban360.CalculationPool.Application.Features.Rule.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Queries;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Rule.Queries
{
    [Route("v1/tariff-constant")]
    public class TariffConstantDictionaryController : BaseController
    {
        private readonly ITariffConstantDictionaryHandler _tariffConstantDictionaryHandler;
        public TariffConstantDictionaryController(ITariffConstantDictionaryHandler tariffConstantDictionaryHandler)
        {
            _tariffConstantDictionaryHandler = tariffConstantDictionaryHandler;
            _tariffConstantDictionaryHandler.NotNull(nameof(tariffConstantDictionaryHandler));
        }

        [HttpPost, HttpGet]
        [Route("dictionary")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<StringDictionary>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Dictionary(CancellationToken cancellationToken)
        {
            ICollection<StringDictionary> tariffConstants = await _tariffConstantDictionaryHandler.Handle(cancellationToken);
            return Ok(tariffConstants);
        }
    }
}
