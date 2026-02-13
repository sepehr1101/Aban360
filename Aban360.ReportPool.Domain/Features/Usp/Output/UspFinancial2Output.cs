namespace Aban360.ReportPool.Domain.Features.Usp.Output
{
    public record UspFinancial2Output
    {
        public double Avrage { get; set; }
        public int Tedad_ghabs { get; set; }
        public int Ahad_ghabs { get; set; }
        public double Tedad_Moshtrak { get; set; }
        public double Tedad_vahd_Moshtrak { get; set; }
        public int Tedad_ghabs_faz { get; set; }
        public int Ahad_ghabs_Faz { get; set; }
        public double Tedad_ensh_Faz { get; set; }
        public double Tedad_vahed_Faz { get; set; }
        public int masraf { get; set; }
        public long modat { get; set; }
        public double Sahm { get; set; }
        public double Modat_Faz { get; set; }
        public double Sahm_Faz { get; set; }
        public long Ab_baha { get; set; }
        public long Abon_ab { get; set; }
        public long Tabsare3_ab { get; set; }
        public long Tabsare_abon { get; set; }
        public long Tabsare2 { get; set; }
        public long Zaribfasl { get; set; }
        public long Jam_Ab_baha { get; set; }
        public long waste_Faz { get; set; }
        public long baha_fas { get; set; }
        public long Tabsare_Fa { get; set; }
        public long Abon_fas { get; set; }
        public long Jam_Fas { get; set; }
        public long Avarez { get; set; }
        public long Javani { get; set; }
        public long Bodjeh { get; set; }
        public long Maliat { get; set; }
        public long Jam_kol { get; set; }
        public string ZoneTitle { get; set; }
        public int roz_gheraat { get; set; }
        public int roz_gheraat_faz { get; set; }
        public string? date { get; set; }
        public string? Karbari { get; set; }
    }
    public record UspFinancialHeader
    {
        public int Tedad_ghabs { get; set; }
        public int Ahad_ghabs { get; set; }
        public double Tedad_Moshtrak { get; set; }
        public double Tedad_vahd_Moshtrak { get; set; }
        public int Tedad_ghabs_faz { get; set; }
        public int Ahad_ghabs_Faz { get; set; }
        public double Tedad_ensh_Faz { get; set; }
        public double Tedad_vahed_Faz { get; set; }
        public long Ab_baha { get; set; }
        public long Abon_ab { get; set; }
        public long Tabsare3_ab { get; set; }
        public long Tabsare_abon { get; set; }
        public long Tabsare2 { get; set; }
        public long Zaribfasl { get; set; }
        public long Jam_Ab_baha { get; set; }
        public long waste_Faz { get; set; }
        public long baha_fas { get; set; }
        public long Tabsare_Fa { get; set; }
        public long Abon_fas { get; set; }
        public long Jam_Fas { get; set; }
        public long Avarez { get; set; }
        public long Javani { get; set; }
        public long Bodjeh { get; set; }
        public long Maliat { get; set; }
        public long Jam_kol { get; set; }
        public string Sp { get; set; }
    }
}
