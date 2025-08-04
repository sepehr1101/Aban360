namespace Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands
{
    public record Table1UpdateDto
    {
        public int Id { get; set; }
        public string? Flag { get; set; }
        public int Town { get; set; }
        public string? ModiDate { get; set; }
        public int ViewTown { get; set; }
        public int State { get; set; }
        public string? ZoneState { get; set; }
        public decimal Z1 { get; set; }
        public decimal Z2 { get; set; }
        public int Olgo { get; set; }
        public string? Provinse { get; set; }
        public string? Zone1 { get; set; }
        public string? Zone2 { get; set; }
        public byte DarsaGh { get; set; }
        public int? EnshMas { get; set; }
        public string? EnshNmas { get; set; }
        public int FixTejari { get; set; }
        public bool Tabs2 { get; set; }
        public short? ArseAb { get; set; }
        public short? ArseFa { get; set; }
        public string Aian2 { get; set; } = string.Empty;
        public byte GroupShahr { get; set; }
        public string? ServerNam { get; set; }
        public bool ReadyAb { get; set; }
        public bool ReadyFa { get; set; }
        public bool EntegalAb { get; set; }
        public short B_Entg_Ab { get; set; }
        public bool EntegalFa { get; set; }
        public short B_Entg_Fa { get; set; }
        public bool ManbaAb { get; set; }
        public int B_Manba_M { get; set; }
        public short B_Manba_T { get; set; }
        public short? CodBank { get; set; }
        public double ZaribBaha { get; set; }
        public string TabAb { get; set; } = string.Empty;
        public string TabFa { get; set; } = string.Empty;
        public string Tab20 { get; set; } = string.Empty;
        public byte? CodMant { get; set; }
        public double ZStudent { get; set; }
        public double ZSchool { get; set; }
        public byte AbfarTag { get; set; }
    }
}
