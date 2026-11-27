namespace Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.SendDto
{
    public record MaliatMaaherWrapperConfirmedUpdateDto
    {
        public int Id { get; }
        public DateTime ConfirmedDateTime { get; }
        public MaliatMaaherWrapperConfirmedUpdateDto(int id, DateTime confirmedDateTime)
        {
            Id = id;
            ConfirmedDateTime = confirmedDateTime;
        }
    }
}
