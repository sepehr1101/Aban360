using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.BlobPool.Domain.Features.DmsServices.Entities
{
    [Table(nameof(OpenKmMetaData))]
    public class OpenKmMetaData
    {
        public int Id { get; set; }
        public string Section { get; set; }
        public string KeyLabel { get; set; }
        public string Label { get; set; }
        public int Value { get; set; }
    }
}