using Aban360.ClaimPool.Application.Features.DMS.Handlers.Queries.Contracts;
using Aban360.ClaimPool.GatewayAdhoc.Features.DMS.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.GatewayAdhoc.Features.DMS.Implementations
{
    internal sealed class DocumentEntityGetByBillIdAddhoc : IDocumentEntityGetByBillIdAddhoc
    {
        private readonly IDocumentEntityGetByBillIdHandler _documentEntitybyBillIdHandle;
        public DocumentEntityGetByBillIdAddhoc(IDocumentEntityGetByBillIdHandler documentEntitybyBillIdHandle)
        {
            _documentEntitybyBillIdHandle = documentEntitybyBillIdHandle;
            _documentEntitybyBillIdHandle.NotNull(nameof(documentEntitybyBillIdHandle));
        }

        public async Task<ICollection<Guid>> Handle(string billId, CancellationToken cancellationToken)
        {
            var documentId = await _documentEntitybyBillIdHandle.Handle(billId, cancellationToken);
            return documentId.Select(d => d.DocumentId).ToList();
        }
    }
}
