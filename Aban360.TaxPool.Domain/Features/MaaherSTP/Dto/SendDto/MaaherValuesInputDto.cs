namespace Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.SendDto
{
    public record MaaherValuesInputDto
    {
        public MaaherTSPHeaderInputDto HeaderInput { get; set; }
        public ICollection<MaaherTSPBodyInputDto>  BodyInput { get; set; }
    }
}
