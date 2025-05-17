using Aban360.ClaimPool.Domain.Constants;

namespace Aban360.ClaimPool.Domain.Features.People.Dto.Commands
{
    public record IndividualCreateDto
    {
        public int WaterMeterId { get; set; }//doto: remove , add estateId
        public int EstateId { get; set; }
        public string FullName { get; set; } = null!;
        public string? NationalId { get; set; }
        public string? FatherName { get; set; }
        public string? PhoneNumbers { get; set; }
        public string? MobileNumbers { get; set; }
        public Guid UserId { get; set; }
        public int? PreviousId { get; set; }
        public short IndividualTypeId { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public string InsertLogInfo { get; set; } = null!;
        //new
        public IndividualEstateRelationTypeEnum IndividualEstateRelationTypeId { get; set; }
    }
}
