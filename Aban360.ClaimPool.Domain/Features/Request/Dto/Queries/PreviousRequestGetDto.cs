namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Queries
{
    public record PreviousRequestGetDto
    {
        public string BillId { get; set; }
        public int CustomerNumber { get; set; }
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public int RegionId { get; set; }
        public string RegionTitle { get; set; }
        public int UsageId { get; set; }
        public string UsageTitle { get; set; }
        public int TrackNumber { get; set; }
        public string StringTrackNumber { get; set; }
        public string RequestDateJalali { get; set; }
        public string RegisterDateJalali { get; set; }
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
