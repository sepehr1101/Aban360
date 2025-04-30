namespace Aban360.ClaimPool.GatewayAdhoc.Features.DMS.Contracts
{
    public interface IDocumentEntityGetByBillIdAddhoc
    {
        Task<ICollection<Guid>> Handle(string billId, CancellationToken cancellationToken);
    }
}
