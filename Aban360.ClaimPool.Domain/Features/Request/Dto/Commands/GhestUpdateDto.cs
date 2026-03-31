namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Commands
{
    public record GhestUpdateDto
    {
        public string StringTrackNumber { get; set; }
        public long Amount { get; set; }
        public GhestUpdateDto(string stringTrackNumber,long amount)
        {
            StringTrackNumber = stringTrackNumber;  
            Amount = amount;    
        }
        public GhestUpdateDto()
        {
        }
    }
}
