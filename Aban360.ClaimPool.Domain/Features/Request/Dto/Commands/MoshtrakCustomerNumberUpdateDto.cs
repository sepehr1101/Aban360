namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Commands
{
    public record MoshtrakCustomerNumberUpdateDto
    {
        public int TrackNumber { get; set; }
        public int CustomerNumber { get; set; }
        public MoshtrakCustomerNumberUpdateDto(int trackNumber, int customerNumber)
        {
            TrackNumber = trackNumber;
            CustomerNumber = customerNumber;
        }
        public MoshtrakCustomerNumberUpdateDto()
        {
        }
    }
}
