using Aban360.Common.Db.Exceptions;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.ConsumersInfo.Implementations
{
    internal sealed class ChangeMainInfoService : AbstractBaseConnection, IChangeMainInfoService
    {
        public ChangeMainInfoService(IConfiguration configuration)
            : base(configuration) { }

        public async Task<ICollection<ChangeMainOutputDto>> GetInfo(string billId)
        {
            string clientData = GetClientsDataQuery();
            IEnumerable<ClientDto> clients = await _sqlReportConnection.QueryAsync<ClientDto>(clientData, new { billId = billId });
            if (!clients.Any())
            {
                throw new InvalidIdException();
            }
            var result = GetChangeData(clients);
            return result;
        }

        private ICollection<ChangeMainOutputDto> GetChangeData(IEnumerable<ClientDto> clients)
        {
            var fieldMap = new Dictionary<string, string>
            {
                ["Id"] = "شناسه",
                ["ZoneId"] = "شناسه منطقه",
                ["ZoneTitle"] = "نام منطقه",
                ["CustomerNumber"] = "شماره ردیف",
                ["BillId"] = "شناسه قبض",
                ["ReadingNumber"] = "اشتراک",
                ["FirstName"] = "نام",
                ["SureName"] = "نام خانوادگی",
                ["FatherName"] = "نام پدر",
                ["PhoneNo"] = "شماره تلفن",
                ["MobileNo"] = "شماره همراه",
                ["PostalCode"] = "کد پستی",
                ["NationalId"] = "کد ملی",
                ["UsageId"] = "نوع مصرف",
                ["UsageId2"] = "نوع مصرف ۲",
                ["UsageTitle"] = "کاربری فروش",
                ["UsageTitle2"] = "کابری مصرف",
                ["BranchType"] = "نوع انشعاب",
                ["WaterDiameterId"] = "کد قطر آب",
                ["WaterDiameterTitle"] = "عنوان قطر آب",
                ["Siphon100"] = "سیفون ۱۰۰",
                ["Siphon125"] = "سیفون ۱۲۵",
                ["Siphon150"] = "سیفون ۱۵۰",
                ["Siphon200"] = "سیفون ۲۰۰",
                ["Siphon5"] = "سیفون ۵",
                ["Siphon6"] = "سیفون ۶",
                ["Siphon7"] = "سیفون ۷",
                ["Siphon8"] = "سیفون ۸",
                ["DomesticCount"] = "تعداد مسکونی",
                ["CommercialCount"] = "تعداد تجاری",
                ["OtherCount"] = "تعداد سایر",
                ["EmptyCount"] = "تعداد خالی",
                ["FamilyCount"] = "تعداد خانوار",
                ["RegisterationJalaliDate"] = "تاریخ ثبت",
                ["FieldArea"] = "مساحت زمین",
                ["ConstructedArea"] = "مساحت ساخت",
                ["DomesticArea"] = "مساحت مسکونی",
                ["CommercialArea"] = "مساحت تجاری",
                ["WaterRequestDate"] = "تاریخ درخواست آب",
                ["SewageRequestDate"] = "تاریخ درخواست فاضلاب",
                ["WaterInstallDate"] = "تاریخ نصب آب",
                ["SewageInstallDate"] = "تاریخ نصب فاضلاب",
                ["HasWater"] = "دارای آب",
                ["HasSewage"] = "دارای فاضلاب",
                ["Address"] = "آدرس",
                ["IsGovermental"] = "دولتی؟",
                ["DeletionStateId"] = "شناسه حذف",
                ["DeletionStateTitle"] = "وضعیت حذف",
                ["UsageStateId"] = "وضعیت مصرف",
                ["ContractCapacity"] = "ظرفیت قراردادی",
                ["VillageId"] = "شناسه روستا",
                ["VillageName"] = "نام روستا",
                ["X"] = "X",
                ["Y"] = "Y",
                ["IsSpecial"] = "انشعاب خاص",
                ["DiscountTypeId"] = "نوع تخفیف",
                ["DiscountTypeTitle"] = "عنوان تخفیف",
                ["MeterSerialBody"] = "سریال کنتور",
                ["HasCommonSiphon"] = "سیفون مشترک",
                ["IsVillage"] = "روستا؟",
                ["TempRowNumber"] = "شماره ردیف موقت",
                ["RegisterDayJalali"] = "تاریخ ثبت روز",
                ["FromDayJalali"] = "از تاریخ",
                ["ToDayJalali"] = "تا تاریخ",
                ["HouseholdDateJalali"] = "تاریخ خانوار",
                ["GuildId"] = "شناسه صنف",
                ["GuildTitle"] = "عنوان صنف",
                ["IsNonPermanent"] = "غیر دائم",
                ["MainSiphonTitle"] = "عنوان سیفون اصلی"
            };
            //var all=new Dictionary<string, List<string>>();
            ICollection<ChangeMainOutputDto> all = new List<ChangeMainOutputDto>();

            for (int i = 1; i < clients.Count(); i++)
            {
                ClientDto current = clients.ElementAt(i);
                ClientDto last = clients.ElementAt(i - 1);



                var differences = new List<string>();
                var props = typeof(ClientDto).GetProperties();


                foreach (var prop in props)
                {
                    if (!fieldMap.ContainsKey(prop.Name) || prop.Name == "ToDayJalali")
                        continue;


                    var oldValue = prop.GetValue(last)?.ToString() ?? "خالی";
                    var newValue = prop.GetValue(current)?.ToString() ?? "خالی";

                    if (oldValue != newValue)
                    {
                        var persianName = fieldMap[prop.Name];
                        differences.Add($"{persianName}: {oldValue} → {newValue}");
                    }
                }
                if (differences.Count > 0)
                {
                    all.Add(new ChangeMainOutputDto()
                    {
                        ChangeDate = clients.ElementAt(i-1).ToDayJalali ?? DateTime.Now.ToShortPersianDateString(),
                        ChangeDetail = differences
                    });
                }
            }

            return all;

        }
        private string GetChangeMainSummayDtoQuery()
        {
            return @"";
        }
        private IEnumerable<ChangeMainInfoDto> GetFakeChangeMainInfo()
        {
            IEnumerable<ChangeMainInfoDto> changeMainInfo = new List<ChangeMainInfoDto>()
            {
                new ChangeMainInfoDto(){ChangeTypeTitle="تغیر نام",LastState="احمدد",CurrentState="احمد",ChangeDate="1401/01/01",SystemUserCode="15224"},
                new ChangeMainInfoDto(){ChangeTypeTitle="تغیر کاربری",LastState="مسکونی",CurrentState="تجاری",ChangeDate="1402/03/24",SystemUserCode="15224"},
                new ChangeMainInfoDto(){ChangeTypeTitle="تغیر آدرس",LastState="خیابان جی - خیابان شهید رجایی",CurrentState="خیابان احمد اباد - خیابان نشاط",ChangeDate="1404/01/29",SystemUserCode="15224"},
                new ChangeMainInfoDto(){ChangeTypeTitle="تغیر شماره تماس",LastState="09131002320",CurrentState="091322353536",ChangeDate="1403/08/10",SystemUserCode="15223"},
            };

            return changeMainInfo;
        }
        private string GetClientsDataQuery()
        {
            return @"select *
                     From [CustomerWarehouse].dbo.Clients c
                     where c.BillId=@billId 
                     Order By c.FromDayJalali";
        }

        public record ClientDto
        {
            public int ZoneId { get; init; }
            public string ZoneTitle { get; init; }
            public long CustomerNumber { get; init; }
            public string BillId { get; init; }
            public string ReadingNumber { get; init; }
            public string FirstName { get; init; }
            public string SureName { get; init; }
            public string FatherName { get; init; }
            public string PhoneNo { get; init; }
            public string MobileNo { get; init; }
            public string PostalCode { get; init; }
            public string NationalId { get; init; }
            public byte UsageId { get; init; }
            public byte? UsageId2 { get; init; }
            public string UsageTitle { get; init; }
            public string? UsageTitle2 { get; init; }
            public string BranchType { get; init; }
            public short WaterDiameterId { get; init; }
            public string WaterDiameterTitle { get; init; }
            public byte Siphon100 { get; init; }
            public byte Siphon125 { get; init; }
            public byte Siphon150 { get; init; }
            public byte Siphon200 { get; init; }
            public byte Siphon5 { get; init; }
            public byte Siphon6 { get; init; }
            public byte Siphon7 { get; init; }
            public byte Siphon8 { get; init; }
            public int DomesticCount { get; init; }
            public int CommercialCount { get; init; }
            public int OtherCount { get; init; }
            public int EmptyCount { get; init; }
            public short FamilyCount { get; init; }
            // public string RegisterationJalaliDate { get; init; }
            public int FieldArea { get; init; }
            public int ConstructedArea { get; init; }
            public int DomesticArea { get; init; }
            public int CommercialArea { get; init; }
            public string WaterRequestDate { get; init; }
            public string SewageRequestDate { get; init; }
            public string WaterInstallDate { get; init; }
            public string SewageInstallDate { get; init; }
            public int HasWater { get; init; }
            public int HasSewage { get; init; }
            public string Address { get; init; }
            public bool IsGovermental { get; init; }
            public byte DeletionStateId { get; init; }
            public string DeletionStateTitle { get; init; }
            public byte UsageStateId { get; init; }
            public int ContractCapacity { get; init; }
            public string VillageId { get; init; }
            public string VillageName { get; init; }
            public string X { get; init; }
            public string Y { get; init; }
            public bool IsSpecial { get; init; }
            public int DiscountTypeId { get; init; }
            public string DiscountTypeTitle { get; init; }
            public string MeterSerialBody { get; init; }
            public bool HasCommonSiphon { get; init; }
            public bool IsVillage { get; init; }
            public int? TempRowNumber { get; init; }
            //   public string RegisterDayJalali { get; init; }
           // public string FromDayJalali { get; init; }
            public string ToDayJalali { get; init; }
            public string HouseholdDateJalali { get; init; }
            public int? GuildId { get; init; }
            public string? GuildTitle { get; init; }
            public int IsNonPermanent { get; init; }
            public string? MainSiphonTitle { get; init; }
        }
    }
}
