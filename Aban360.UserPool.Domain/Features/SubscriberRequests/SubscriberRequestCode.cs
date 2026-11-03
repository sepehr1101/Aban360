namespace Aban360.UserPool.Domain.Features.SubscriberRequests;

public enum SubscriberRequestCode : long
{
    SubmitMeterReading = 15033585000, // اعلام کارکرد کنتور و صدور قبض آنی
    ChangeMobile = 10031452100, // تغییر و ثبت شماره همراه
    ReviewBill = 15033584000, // درخواست بررسی صورتحساب
    ReportReadingError = 10000000014, // اعلام اشتباه در قرائت
    ChangeOwnerDetails = 15033573000, // درخواست تغییر نام و مشخصات
    SewerConnection = 15033571000, // درخواست انشعاب فاضلاب
    ChangeWaterUsage = 15033572000, // درخواست تغییر کاربری انشعاب آب
    ChangeUnitCount = 15033574000, // درخواست تغییر تعداد واحد
    ChangeContractCapacity = 15033575000, // درخواست تغییر ظرفیت قراردادی
    HouseholdCensus = 10000000010, // درخواست خانوار شماری
    EndConstruction = 10000000011, // اعلام خروج از ساخت و ساز
    ChangeConnectionDiameter = 15033576000, // درخواست تغییر قطر انشعاب
    SeparateMeter = 15033579000, // درخواست تفکیک کنتور
    RelocateMeter = 15033580000, // درخواست تغییر مکان کنتور
    TestMeter = 15033581000, // درخواست آزمایش کنتور
    ReplaceMeter = 15033582000, // درخواست تعویض کنتور
    ReportBrokenMeter = 10000000015, // اعلام خرابی کنتور
    ReportWaterOutage = 15033594088, // اطلاع رسانی قطعی آب
    AdditionalSewerSiphon = 15033577000, // نصب سیفون اضافی فاضلاب
    ChangeSiphonDiameter = 10000000017, // درخواست تغییر قطر سیفون
    ReplaceSiphon = 10000000016, // درخواست تعویض سیفون
    RelocateSiphon = 10000000013, // درخواست جابجایی سیفون
    TemporaryDisconnect = 15033586000, // قطع موقت انشعاب آب
    Reconnect = 15033587000, // وصل مجدد انشعاب آب
    RemoveOrMergeConnection = 15033578000, // جمع آوری یا ادغام انشعاب آب
    NotaryOwnershipInquiry = 15033591099, // پاسخ به استعلام مالکیت محضر
    TankerWater = 15033588000, // فروش آب تانکری
    ReportIncident = 15033593000 // رسیدگی به حوادث
}
