namespace Aban360.BlobPool.Domain.Features.DmsServices.Dto.Queries
{
    public record MetaDataOutput
    {
        public string SectionTitle { get; set; }
        public string ValueTitle { get; set; }
        public MetaDataOutput(string sectionTitle, string valueTitle)
        {
            SectionTitle = sectionTitle;
            ValueTitle = valueTitle;
        }
    }
}
