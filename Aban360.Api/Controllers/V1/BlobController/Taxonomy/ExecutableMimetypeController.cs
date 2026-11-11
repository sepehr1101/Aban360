using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Create.Contracts;
using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Delete.Contracts;
using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Update.Contracts;
using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Queries.Contracts;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Queries;
using Aban360.BlobPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.BlobController.Taxonomy
{
    [Route("v1/executable-mimetype")]
    public class ExecutableMimetypeController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IExecutableMimetypeCreateHandler _executableMimetypeCreateHandler;
        private readonly IExecutableMimetypeUpdateHandler _executableMimetypeUpdateHandler;
        private readonly IExecutableMimetypeDeleteHandler _executableMimetypeDeleteHandler;
        private readonly IExecutableMimetypeGetSingleHandler _executableMimetypeGetSingleHandler;
        private readonly IExecutableMimetypeGetAllHandler _executableMimetypeGetAllHandler;

        public ExecutableMimetypeController(
            IUnitOfWork uow,
            IExecutableMimetypeCreateHandler executableMimetypeCreateHandler,
            IExecutableMimetypeUpdateHandler executableMimetypeUpdateHandler,
            IExecutableMimetypeDeleteHandler executableMimetypeDeleteHandler,
            IExecutableMimetypeGetSingleHandler executableMimetypeGetSingleHandler,
            IExecutableMimetypeGetAllHandler executableMimetypeGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _executableMimetypeCreateHandler = executableMimetypeCreateHandler;
            _executableMimetypeCreateHandler.NotNull(nameof(executableMimetypeCreateHandler));

            _executableMimetypeUpdateHandler = executableMimetypeUpdateHandler;
            _executableMimetypeUpdateHandler.NotNull(nameof(executableMimetypeUpdateHandler));

            _executableMimetypeDeleteHandler = executableMimetypeDeleteHandler;
            _executableMimetypeDeleteHandler.NotNull(nameof(executableMimetypeDeleteHandler));

            _executableMimetypeGetSingleHandler = executableMimetypeGetSingleHandler;
            _executableMimetypeGetSingleHandler.NotNull(nameof(executableMimetypeGetSingleHandler));

            _executableMimetypeGetAllHandler = executableMimetypeGetAllHandler;
            _executableMimetypeGetAllHandler.NotNull(nameof(executableMimetypeGetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ExecutableMimetypeCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] ExecutableMimetypeCreateDto createDto, CancellationToken cancellationToken)
        {
            await _executableMimetypeCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ExecutableMimetypeUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] ExecutableMimetypeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _executableMimetypeUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ExecutableMimetypeDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] ExecutableMimetypeDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _executableMimetypeDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ExecutableMimetypeGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var executableMimetypes = await _executableMimetypeGetSingleHandler.Handle(id, cancellationToken);
            return Ok(executableMimetypes);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<ExecutableMimetypeGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var executableMimetypes = await _executableMimetypeGetAllHandler.Handle(cancellationToken);
            return Ok(executableMimetypes);
        }
    }
}
