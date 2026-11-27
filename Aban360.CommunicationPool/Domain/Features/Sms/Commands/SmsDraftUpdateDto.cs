namespace Aban360.CommunicationPool.Domain.Features.Sms.Commands
{
    public record SmsDraftUpdateDto
    {
        public IEnumerable<Guid> Ids { get; set; }
        public DateTime EffectDateTime { get; set; } = DateTime.Now;
        public string GroupId { get; set; }
        public int SmsFlowId { get; set; }
        public SmsDraftUpdateDto(IEnumerable<Guid> ids, string groupId,int smsFlowId)
        {
            Ids = ids;
            GroupId = groupId;
            SmsFlowId = smsFlowId;
        }
    }
}
