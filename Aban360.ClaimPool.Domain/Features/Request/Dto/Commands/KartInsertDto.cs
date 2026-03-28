namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Commands
{
    public record KartInsertDto
    {
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public string? ReadingNumber { get; set; }
        public string StringTrackNumber { get; set; }
        public int Serial { get; set; }
        public int Barge { get; set; }

        public string CurrentDateJalali { get; set; }
        public string DueDateJalali { get; set; }
        public int DiscountTypeId { get; set; }
        public long FinalAmount { get; set; }// & Jam_ha
        public long DiscountAmount { get; set; }
        public long PardN{ get; set; }
        public long PardG { get; set; }
        public long Sum { get; set; }
        public int Type { get; set; }
        public int Ser { get; set; }
        public int ServiceSelectedId { get; set; }
        public int MeterDiameterId { get; set; }
        public int SiphonId { get; set; }
        public int UsageId { get; set; }
        public bool IsRegister { get; set; }
        public long TotalServicesAmount { get; set; }//& total
        public long FirstInstallment { get; set; }
        public long JGEST_FA { get; set; } = 0;//todo: rename
        public long PishFa { get; set; }//todo: rename
        public long InstallmentPercent { get; set; }
        public int InstallmentCount { get; set; }
        public long Installment { get; set; } = 0;
        public int Operator { get; set; }
        public string BankDateJalali { get; set; }
        public int DomesticUnit { get; set; }
        public int CommercialUnit { get; set; }
        public int OtherUnit { get; set; }
        public int KartTypeId { get; set; }
        public string InsertedBy { get; set; }

    }
}
