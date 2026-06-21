using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Implemntations;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Commands
{
    [Route("v1/con-company")]
    public class ConCompanyController : BaseController
    {
        private readonly IConCompanyInsertHandler _conCompanyInsertHandler;
        private readonly IConCompanyUpdateHandler _conCompanyUpdateHandler;
        private readonly IConCompanyRemoveHandler _conCompanyRemoveHandler;
        private readonly IConCompanyGetHandler _conCompanyGetHandler;
        private readonly IConCompanyGetByIdHandler _conCompanyGetByIdHandler;
        private readonly IConCompanyPersonnelInsertHandler _conCompanyPersonnelInsertHandler;
        private readonly IConCompanyPersonnelUpdateHandler _conCompanyPersonnelUpdateHandler;
        private readonly IConCompanyPersonnelRemoveHandler _conCompanyPersonnelRemoveHandler;
        private readonly IConCompanyPersonalGetHandler _conCompanyPersonalGetHandler;
        private readonly IConCompanyGetDictionaryHandler _conCompanyGetDictionaryHandler;
        private readonly IConCompanyPersonnelGetDictionaryHandler _conCompanyPersonnelGetDictionaryHandler;
        public ConCompanyController(
            IConCompanyInsertHandler conCompanyInsertHandler,
            IConCompanyUpdateHandler conCompanyUpdateHandler,
            IConCompanyRemoveHandler conCompanyRemoveHandler,
            IConCompanyGetHandler conCompanyGetHandler,
            IConCompanyGetByIdHandler conCompanyGetByIdHandler,
            IConCompanyPersonnelInsertHandler conCompanyPersonnelInsertHandler,
            IConCompanyPersonnelUpdateHandler conCompanyPersonnelUpdateHandler,
            IConCompanyPersonnelRemoveHandler conCompanyPersonnelRemoveHandler,
            IConCompanyPersonalGetHandler conCompanyPersonalGetHandler,
            IConCompanyGetDictionaryHandler conCompanyGetDictionaryHandler,
            IConCompanyPersonnelGetDictionaryHandler conCompanyPersonnelGetDictionaryHandler)
        {
            _conCompanyInsertHandler = conCompanyInsertHandler;
            _conCompanyInsertHandler.NotNull(nameof(conCompanyInsertHandler));

            _conCompanyUpdateHandler = conCompanyUpdateHandler;
            _conCompanyUpdateHandler.NotNull(nameof(conCompanyUpdateHandler));

            _conCompanyRemoveHandler = conCompanyRemoveHandler;
            _conCompanyRemoveHandler.NotNull(nameof(conCompanyRemoveHandler));

            _conCompanyGetHandler = conCompanyGetHandler;
            _conCompanyGetHandler.NotNull(nameof(conCompanyGetHandler));

            _conCompanyGetByIdHandler = conCompanyGetByIdHandler;
            _conCompanyGetByIdHandler.NotNull(nameof(conCompanyGetByIdHandler));

            _conCompanyPersonnelInsertHandler = conCompanyPersonnelInsertHandler;
            _conCompanyPersonnelInsertHandler.NotNull(nameof(conCompanyPersonnelInsertHandler));

            _conCompanyPersonnelUpdateHandler = conCompanyPersonnelUpdateHandler;
            _conCompanyPersonnelUpdateHandler.NotNull(nameof(conCompanyPersonnelUpdateHandler));

            _conCompanyPersonnelRemoveHandler = conCompanyPersonnelRemoveHandler;
            _conCompanyPersonnelRemoveHandler.NotNull(nameof(conCompanyPersonnelRemoveHandler));

            _conCompanyPersonalGetHandler = conCompanyPersonalGetHandler;
            _conCompanyPersonalGetHandler.NotNull(nameof(conCompanyPersonalGetHandler));

            _conCompanyGetDictionaryHandler = conCompanyGetDictionaryHandler;
            _conCompanyGetDictionaryHandler.NotNull(nameof(conCompanyGetDictionaryHandler));

            _conCompanyPersonnelGetDictionaryHandler = conCompanyPersonnelGetDictionaryHandler;
            _conCompanyPersonnelGetDictionaryHandler.NotNull(nameof(conCompanyPersonnelGetDictionaryHandler));
        }


        [HttpPost, HttpGet]
        [Route("insert")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ConCompanyInsertInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Insert([FromBody] ConCompanyInsertInputDto inputDto, CancellationToken cancellationToken)
        {
            await _conCompanyInsertHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost, HttpGet]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ConCompanyUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] ConCompanyUpdateDto inputDto, CancellationToken cancellationToken)
        {
            await _conCompanyUpdateHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost, HttpGet]
        [Route("remove/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Remove(int id, CancellationToken cancellationToken)
        {
            await _conCompanyRemoveHandler.Handle(id, CurrentUser, cancellationToken);
            return Ok(id);
        }

        [HttpPost, HttpGet]
        [Route("get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<ConCompanyGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<ConCompanyGetDto> result = await _conCompanyGetHandler.Handle(cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("get/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ConCompanyGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            ConCompanyGetDto result = await _conCompanyGetByIdHandler.Handle(id, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("dictionary")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<NumericDictionary>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDictionary(CancellationToken cancellationToken)
        {
            IEnumerable<NumericDictionary> result = await _conCompanyGetDictionaryHandler.Handle(cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("personnel-insert")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ConCompanyPersonnelInsertInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> InsertPersonnel([FromBody] ConCompanyPersonnelInsertInputDto inputDto, CancellationToken cancellationToken)
        {
            await _conCompanyPersonnelInsertHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost, HttpGet]
        [Route("personnel-update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ConCompanyPersonnelUpdateInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdatePersonnel([FromBody] ConCompanyPersonnelUpdateInputDto inputDto, CancellationToken cancellationToken)
        {
            await _conCompanyPersonnelUpdateHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost, HttpGet]
        [Route("personnel-remove")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ConCompanyPersonnelRemoveInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RemovePersonnel([FromBody] ConCompanyPersonnelRemoveInputDto inputDto, CancellationToken cancellationToken)
        {
            await _conCompanyPersonnelRemoveHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost, HttpGet]
        [Route("personnel-get/{companyId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<ConCompanyPersonnelDetailOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPersonnels(int companyId, CancellationToken cancellationToken)
        {
            IEnumerable<ConCompanyPersonnelDetailOutputDto> result = await _conCompanyPersonalGetHandler.Handle(companyId, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("personnel-dictionary")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<NumericDictionary>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPersonnelsDictionary(CancellationToken cancellationToken)
        {
            IEnumerable<NumericDictionary> result = await _conCompanyPersonnelGetDictionaryHandler.Handle(cancellationToken);
            return Ok(result);
        }
    }
}
