namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands
{
    public record BedBesCreateDto
    {
        public int Id { get; set; }
        public decimal Town { get; set; }
        public decimal Radif { get; set; }
        public string Eshtrak { get; set; }
        public decimal Barge { get; set; }//
        public decimal PriNo { get; set; }
        public decimal TodayNo { get; set; }
        public string PriDate { get; set; }
        public string TodayDate { get; set; }
        public decimal AbonFas { get; set; }
        public decimal FasBaha { get; set; }
        public decimal AbBaha { get; set; }
        public decimal Ztadil { get; set; }
        public decimal Masraf { get; set; }
        public decimal Shahrdari { get; set; }//0
        public decimal Modat { get; set; }
        public string DateBed { get; set; }//emroz
        public decimal JalaseNo { get; set; }//
        public string Mohlat { get; set; }//10 rooz bad emroz ( from appsettirn)
        public decimal Baha { get; set; }//sum all (dis ...)
        public decimal AbonAb { get; set; }
        public decimal Pard { get; set; }//ger baha -> 3 ragham akhar gerd
        public decimal Jam { get; set; }//baha+ bedehi ghabli
        public decimal CodVas { get; set; }//1--8 ->from inputDto
        public string Ghabs { get; set; }//?
        public bool Del { get; set; }//0
        public string Type { get; set; }//1
        public decimal CodEnshab { get; set; }//usageid
        public decimal Enshab { get; set; }//meterDiameterId
        public decimal Elat { get; set; }//0
        public decimal Serial { get; set; }//
        public decimal Ser { get; set; }//
        public decimal ZaribFasl { get; set; }
        public decimal Ab10 { get; set; }//0
        public decimal Ab20 { get; set; }//0
        public decimal TedadVahd { get; set; }//other
        public decimal TedKhane { get; set; }
        public decimal TedadMas { get; set; }
        public decimal TedadTej { get; set; }
        public decimal NoeVa { get; set; }//branch
        public decimal Jarime { get; set; }//0
        public decimal Masjar { get; set; }//0
        public decimal Sabt { get; set; }//1
        public decimal Rate { get; set; }//monthly avergecondmption 
        public decimal Operator { get; set; }//0 
        public decimal Mamor { get; set; }//input
        public string TavizDate { get; set; }//from input
        public decimal ZaribCntr { get; set; }//0
        public decimal Zabresani { get; set; }//0
        public decimal ZaribD { get; set; }//0
        public decimal Tafavot { get; set; }//0
        public decimal KasrHa { get; set; }//sum discount ha
        public decimal FixMas { get; set; }//contr..
        public string ShGhabs1 { get; set; }//billid
        public string ShPard1 { get; set; }//must generte in c#
        public decimal TabAbnA { get; set; }//0
        public decimal TabAbnF { get; set; }//0
        public decimal TabsFa { get; set; }//0
        public decimal NewAb { get; set; }//0
        public decimal NewFa { get; set; }//0
        public decimal Bodjeh { get; set; }//bodje amount
        public decimal Group1 { get; set; }//0
        public decimal MasFas { get; set; }//0
        public bool Faz { get; set; }//fazelab amount>0?1:0
        public decimal ChkKarbari { get; set; }//0
        public decimal C200 { get; set; }//0
        public string DateIns { get; set; }//8char -> ''   10char->emroz
        public decimal AbSevom { get; set; }//0
        public decimal AbSevom1 { get; set; }//0
        public decimal C70 { get; set; }//0
        public decimal C80 { get; set; }//0
        public string TmpDateBed { get; set; }//''
        public string TmpPriDate { get; set; }//''
        public string TmpTodayDate { get; set; }//''
        public string TmpMohlat { get; set; }//''
        public string TmpTavizDate { get; set; }//''
        public decimal C90 { get; set; }//0
        public decimal C101 { get; set; }//0
        public decimal KhaliS { get; set; }//empty unit
        public bool? EdarehK { get; set; }//isSpecial
        public decimal Tafa402 { get; set; }//0
        public decimal Avarez { get; set; }//avarez amoutn
        public long TrackNumber { get; set; }//frominput   felan 0
    }
}
