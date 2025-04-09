using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.BlobController.Queries
{
    [Route("v1/document")]
    public class DocumentGetController:BaseController
    {
        private readonly IDocumentGetSingleHandler _documentGetSingle;
        public DocumentGetController(IDocumentGetSingleHandler documentGetSingle)
        {
            _documentGetSingle = documentGetSingle;
            _documentGetSingle.NotNull(nameof(_documentGetSingle));
        }

        [Route("single/{id}")]
        [HttpPost,HttpGet]
        public async Task<FileResult> Get(Guid id,CancellationToken cancellationToken)
        {
            var y = await _documentGetSingle.Handle(id,cancellationToken);
            return File(y.FileContent, y.ContentType);
        }
    }
}
