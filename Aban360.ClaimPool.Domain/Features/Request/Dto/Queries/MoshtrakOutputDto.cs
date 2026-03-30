namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Queries
{
    public record MoshtrakOutputDto
    {
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public int CustomerNumber { get; set; }
        public string? ReadingNumber { get; set; }
        public string? FirstName { get; set; }
        public string? Surname { get; set; }
        public string FatherName { get; set; }
        public string NationalCode { get; set; }
        public string? PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string RequestDateJalali { get; set; }
        public string Address { get; set; }
        public string? PostalCode { get; set; }
        public string? NeighbourBillId { get; set; }
        public int TrackNumber { get; set; }
        public int UsageId { get; set; }
        public string UsageTitle { get; set; }
        public bool IsRegistered { get; set; }

        public int BranchTypeId { get; set; }
        public string BranchTypeTitle { get; set; }
        public int Premises { get; set; }
        public int ImprovementOverall { get; set; }
        public int ImprovementDomestic { get; set; }
        public int ImprovementCommercial { get; set; }
        public int OtherUnit { get; set; }
        public int DomesticUnit { get; set; }
        public int CommercialUnit { get; set; }
        public int ContractualCapacity { get; set; }
        public int Siphon100 { get; set; }
        public int Siphon125 { get; set; }
        public int Siphon150 { get; set; }
        public int Siphon200 { get; set; }
        public int MainSiphon { get; set; }
        public int CommonSiphon { get; set; }
        public string MeterDiameterTitle { get; set; }
        public int MeterDiameterId { get; set; }
        public int DiscountTypeId { get; set; }
        public string DiscountTypeTitle { get; set; }
        public int DiscountCount { get; set; }
        public bool IsSpecial { get; set; }
        public bool CounterType { get; set; }
        public string? NotificationMobile { get; set; }
        public string? Description { get; set; }
        public int HouseValue { get; set; }
        public bool IsNonPermanent { get; set; }
        public string? BlockId { get; set; }
        public int BrokerId { get; set; }//has=1,2 hasnt=0
        public bool HasBroker { get { return BrokerId >= 1; } }

        public int s0 { get; set; }
        public bool HasEnsheabAb { get { return s0 > 0; } }

        public int s1 { get; set; }
        public bool HasEnsheabFazelab { get { return s1 > 0; } }

        public int s2 { get; set; }
        public bool HasTaqirVahed { get { return s2 > 0; } }

        public int s3 { get; set; }
        public int s4 { get; set; }
        public bool HasTaqirName { get { return s4 > 0; } }

        public int s5 { get; set; }
        public bool HasTaqirQotrEnsheab { get { return s5 > 0; } }

        public int s8 { get; set; }
        public int s9 { get; set; }
        public int s10 { get; set; }
        public bool HasEstelamMahzar { get { return s10 > 0; } }

        public int s11 { get; set; }
        public int s12 { get; set; }
        public int s13 { get; set; }
        public bool HasTaqirSathCounter { get { return s13 > 0; } }

        public int s14 { get; set; }
        public int s15 { get; set; }
        public int s16 { get; set; }
        public bool HasTaqirKarbari { get { return s16 > 0; } }

        public int s17 { get; set; }
        public int s18 { get; set; }
        public int s19 { get; set; }
        public int s20 { get; set; }
        public bool HasJabejaiiKontor { get { return s20 > 0; } }

        public int s21 { get; set; }
        public int s22 { get; set; }
        public int s23 { get; set; }
        public int s24 { get; set; }
        public bool HasTaqirQotrSifoon { get { return s24 > 0; } }

        public int s25 { get; set; }
        public int s26 { get; set; }
        public bool HasAmadeSaziAb { get { return s26 > 0; } }

        public int s27 { get; set; }
        public bool HazAmadeSaziFazelab { get { return s27 > 0; } }

        public int s28 { get; set; }
        public int s29 { get; set; }
        public int s30 { get; set; }
        public int s31 { get; set; }
        public int s32 { get; set; }
        public bool HasQatVaslEnsheab { get { return s32 > 0; } }

        public int s33 { get; set; }
        public bool HasSifoonEzafe { get { return s33 > 0; } }

        public int s34 { get; set; }
        public bool HasWaterDiscount { get { return s34 > 0; } }

        public int s35 { get; set; }
        public bool HasSewageDiscount { get { return s35 > 0; } }

        public int s36 { get; set; }
        public bool HasJabejaiiSifoon { get { return s36 > 0; } }

        public int s37 { get; set; }
        public bool HasNezamMohandesi { get { return s37 > 0; } }

        public int s38 { get; set; }
        public int s39 { get; set; }
        public bool HasKhanevarShomari { get { return s39 > 0; } }

        public int s40 { get; set; }
        public bool HasTafkikEdqam { get { return s40 > 0; } }

        public int s41 { get; set; }
        public bool HasTavizKontor { get { return s41 > 0; } }

        public int s42 { get; set; }
        public int s43 { get; set; }
        public bool HasLooleGozareAbFazelab { get { return s43 > 0; } }

        public int s44 { get; set; }
        public bool HasZarfiatQarardadi { get { return s44 > 0; } }

        public int s45 { get; set; }
        public bool HasKontorMojaza { get { return s45 > 0; } }

        public int s46 { get; set; }
        public bool HasTaqirTarefe { get { return s46 > 0; } }

        public int s47 { get; set; }
        public bool HasPeymayesh { get { return s47 > 0; } }

        public int s48 { get; set; }
        public bool HasSaier { get { return s48 > 0; } }
    }
}
