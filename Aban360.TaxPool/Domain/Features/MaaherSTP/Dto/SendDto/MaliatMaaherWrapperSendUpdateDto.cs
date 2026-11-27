namespace Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.SendDto
{
    public record MaliatMaaherWrapperSendUpdateDto
    {
        public int Id { get; set; }
        public DateTime SendDateTime { get; }
        public Guid SendByUserId { get; }
        public MaliatMaaherWrapperSendUpdateDto(int id, DateTime sendDateTime, Guid sendByUserId)
        {
            Id = id;
            SendDateTime = sendDateTime;
            SendByUserId = sendByUserId;    
        }
    }
}
