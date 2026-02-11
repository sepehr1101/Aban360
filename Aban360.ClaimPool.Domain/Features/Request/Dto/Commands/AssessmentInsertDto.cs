using DNTPersianUtils.Core;

namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Commands
{
    public record AssessmentInsertDto
    {
        public int TrackNumber { get; set; }
        public string BillId { get; set; }
        public int AssessmentCode { get; set; }
        public string AssessmentName { get; set; }
        public string AssessmentMobile { get; set; }
        public string AssessmentDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString(); //todo : check
        public DateTime AssessmentGregorianDateTime { get; set; } = DateTime.Now; //todo: check
        public int ZoneId { get; set; }
        public int ResultId { get; set; }
        public DateTime SetResultDateTime { get; set; } = DateTime.Now;//todo : check
        public string? Description { get; set; }
        public Guid TrackId { get; set; }
        public Guid? TrackIdResult { get; set; }//todo : check


        public string X1 { get; set; }
        public string Y1 { get; set; }
        public string X2 { get; set; }
        public string Y2 { get; set; }
        public string Accuracy { get; set; }
        //
        public int? TrenchLenW { get; set; }
        public int? TrenchLenS { get; set; }
        public int? AsphaltLenW { get; set; }
        public int? AsphaltLenS { get; set; }
        public int? RockyLenW { get; set; }
        public int? RockyLenS { get; set; }
        public int? OtherLenW { get; set; }
        public int? OtherLenS { get; set; }
        public int? BasementDepth { get; set; }
        public bool? HasMap { get; set; }
        public string ReadingNumber { get; set; }
        public int Premises { get; set; }
        public int HouseValue { get; set; }
        public int UsageId { get; set; }
    }
}
