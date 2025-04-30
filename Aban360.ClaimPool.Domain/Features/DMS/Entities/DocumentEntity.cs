using Aban360.ClaimPool.Domain.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.DMS.Entities
{
    [Table(nameof(DocumentEntity))]
    public class DocumentEntity
    {
        public long Id { get; set; }
        public Guid DocumentId { get; set; }
        public long TableId { get; set; }
        public RelationEntityEnum RelationEntityId { get; set; }
        public string?  BillId { get; set; }
    }

}
