# ادغام 30 جفت بعدی و مرور مجدد هر 50 مورد

تاریخ: 2026/11/22. محیط اجرا: ویندوز بومی، D:/Projects/Aban360.

ردیف‌های 21 تا 50 فهرست تأییدشده انجام شدند: 60 implementation به 30 Repository تبدیل شد. به همراه مرحله قبل، 100 implementation به 50 Repository رسیده‌اند. همه 100 interface حفظ شده‌اند. از فهرست اولیه 110 جفت، اکنون 60 جفت باقی است؛ موارد بعدی در این مرحله اجرا نشده‌اند.

## نتیجه بررسی مجموع 50 مورد

- 240 متد با مقایسه توکن‌های نحوی با نسخه‌های ثبت‌شده قبل از ادغام کنترل شدند؛ فقط نام فیلدهای خصوصی مشترک یکسان‌سازی شد.
- 1765 بررسی موفق، شامل 722 سناریوی قبل و بعد با EF InMemory اجرا شد؛ هر 50 Repository و هر دو interface آن‌ها بررسی شدند.
- هویت مشترک دو قرارداد در یک Scope، جدایی Scopeها، ثبت یکتای قراردادها و استفاده از UnitOfWork همان ماژول بررسی شد.
- سناریوها داده موجود، داده ناموجود، ورودی null، افزودن و حذف تکی و گروهی، tracking، استثناها و عدم ذخیره ضمنی را پوشش می‌دهند. رفتار async void در تست با SynchronizationContext ویژه ثبت می‌شود؛ این کار اصلاح رفتار production نیست.
- فایل کد هر 20 Repository مرحله قبل، با SHA-256 قبل از این مرحله مقایسه شد و بدون تغییر باقی ماند. ثبت DI ماژول CalculationPool برای اضافه‌کردن دو Repository جدید به‌روزرسانی شد و مجدداً تست شد.
- SHA-256 هر 100 interface با نسخه قبل از ادغام یکسان است. 100 نسخه ثبت‌شده از implementationهای قبلی برای مقایسه در Baseline نگهداری می‌شوند.
- Build نهایی API و Bridge بدون خطای کامپایل انجام شد؛ warningهای پروژه همچنان وجود دارند.
- دامنه این تست‌ها EF InMemory است؛ اتصال یا تست روی SQL Server واقعی انجام نشده است. این نتیجه، ادعای بی‌نقص‌بودن منطق قدیمی برنامه نیست.

## کلاس‌های جدید مرحله دوم

| ردیف | Repository | خطوط واقعی بدنه کلاس |
| ---: | --- | ---: |
| 21 | [TariffRepository](sandbox:/D:/Projects/Aban360/Aban360.CalculationPool.Persistence/Features/Rule/Repositories/TariffRepository.cs) | 74 |
| 22 | [TariffConstantRepository](sandbox:/D:/Projects/Aban360/Aban360.CalculationPool.Persistence/Features/Rule/Repositories/TariffConstantRepository.cs) | 52 |
| 23 | [RequestEstateRepository](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Draft/Repositories/RequestEstateRepository.cs) | 36 |
| 24 | [RequestFlatRepository](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Draft/Repositories/RequestFlatRepository.cs) | 41 |
| 25 | [RequestIndividualRepository](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Draft/Repositories/RequestIndividualRepository.cs) | 34 |
| 26 | [RequestIndividualDiscountTypeRepository](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Draft/Repositories/RequestIndividualDiscountTypeRepository.cs) | 41 |
| 27 | [RequestIndividualEstateRepository](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Draft/Repositories/RequestIndividualEstateRepository.cs) | 43 |
| 28 | [RequestIndividualTagRepository](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Draft/Repositories/RequestIndividualTagRepository.cs) | 44 |
| 29 | [RequestSiphonRepository](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Draft/Repositories/RequestSiphonRepository.cs) | 34 |
| 30 | [RequestTrackingRepository](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Draft/Repositories/RequestTrackingRepository.cs) | 41 |
| 31 | [RequestUserRepository](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Draft/Repositories/RequestUserRepository.cs) | 38 |
| 32 | [RequestWaterMeterRepository](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Draft/Repositories/RequestWaterMeterRepository.cs) | 41 |
| 33 | [RequestWaterMeterSiphonRepository](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Draft/Repositories/RequestWaterMeterSiphonRepository.cs) | 49 |
| 34 | [RequestWaterMeterTagRepository](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Draft/Repositories/RequestWaterMeterTagRepository.cs) | 45 |
| 35 | [CapacityCalculationIndexRepository](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Land/Repositories/CapacityCalculationIndexRepository.cs) | 34 |
| 36 | [ConstructionTypeRepository](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Land/Repositories/ConstructionTypeRepository.cs) | 34 |
| 37 | [EstateBoundTypeRepository](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Land/Repositories/EstateBoundTypeRepository.cs) | 34 |
| 38 | [EstateRepository](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Land/Repositories/EstateRepository.cs) | 34 |
| 39 | [EstateWaterResourceRepository](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Land/Repositories/EstateWaterResourceRepository.cs) | 41 |
| 40 | [FlatRepository](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Land/Repositories/FlatRepository.cs) | 39 |
| 41 | [GuildRepository](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Land/Repositories/GuildRepository.cs) | 39 |
| 42 | [HandoverRepository](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Land/Repositories/HandoverRepository.cs) | 34 |
| 43 | [OfficialHolidayRepository](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Land/Repositories/OfficialHolidayRepository.cs) | 34 |
| 44 | [ProfessionRepository](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Land/Repositories/ProfessionRepository.cs) | 39 |
| 45 | [UsageLevel1Repository](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Land/Repositories/UsageLevel1Repository.cs) | 34 |
| 46 | [UsageLevel2Repository](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Land/Repositories/UsageLevel2Repository.cs) | 39 |
| 47 | [UsageLevel3Repository](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Land/Repositories/UsageLevel3Repository.cs) | 39 |
| 48 | [UsageLevel4Repository](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Land/Repositories/UsageLevel4Repository.cs) | 39 |
| 49 | [UserLeaveRepository](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Land/Repositories/UserLeaveRepository.cs) | 34 |
| 50 | [UserWorkdayRepository](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Land/Repositories/UserWorkdayRepository.cs) | 34 |

خطوط از اعلان کلاس تا آکولاد پایانی، شامل خطوط خالی و توضیحات محاسبه شده‌اند. using و namespace جدا هستند. کلاس‌های مرحله دوم 34 تا 74 خط و مجموع هر 50 کلاس 32 تا 74 خط هستند. قالب چندخطی متدها حفظ شده است.

## نکات اجرایی

Repositoryها از اسکن عمومی Scrutor کنار گذاشته می‌شوند و یک بار Scoped ثبت می‌شوند. هر دو interface با factory به همان نمونه متصل‌اند. برای کوتاه‌ماندن کد ثبت، helper خصوصی generic با قید پیاده‌سازی هر دو قرارداد استفاده شده است؛ پیکربندی سایر سرویس‌ها تغییر نکرده است.

در RequestUser، بر خلاف تخمین اولیه که بر هم‌نامی کلاس‌ها متکی بود، دو DbSet متفاوت وجود دارد. هر دو برای حفظ رفتار قراردادهای موجود نگهداری شدند. سازنده Command قبلی حفظ و مقداردهی DbSet موردنیاز Query به آن اضافه شده است. این مورد همچنان 38 خط است.

namespace قبلی بعضی Queryهای ClaimPool به‌اشتباه UserPool.TimeTable بود. Repositoryهای جدید در namespace متناظر پوشه ClaimPool قرار دارند؛ namespace و نام interfaceها عوض نشده‌اند.

## ایرادهای قدیمی که در این مرحله اصلاح نشدند

### CompanyServiceOffering.Get(short id) — از مرحله قبل

[متد](sandbox:/D:/Projects/Aban360/Aban360.CalculationPool.Persistence/Features/Bill/Repositories/CompanyServiceOfferingRepository.cs) از id استفاده نمی‌کند و روی کل مجموعه SingleAsync اجرا می‌کند. با چند رکورد خطا می‌دهد و با یک رکورد حتی برای شناسه ناموجود همان رکورد را برمی‌گرداند. نسخه قبل و بعد این رفتار را یکسان نشان دادند؛ تست ویژه آن حفظ شده است.

### RequestUser — پیاده‌سازی ناقص قدیمی

در [RequestUserRepository](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Draft/Repositories/RequestUserRepository.cs)، Add(RequestUser) از قبل بدنه خالی داشت و Add(RequestWaterMeter) یک کنتور درخواست را اضافه می‌کرد. Queryها از RequestUser به‌عنوان entity استفاده می‌کنند، ولی این نوع یک شیء تجمیعی فاقد کلید و ثبت EF است؛ در اجرای EF فعلی خطای InvalidOperationException می‌دهند. همه این رفتارها قبل از ادغام هم وجود داشته و در تست‌های قبل و بعد برابر بودند.

### RequestEstate.Remove — async void قدیمی

امضای [Remove](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Draft/Repositories/RequestEstateRepository.cs) از قبل async void بود. متد await ندارد؛ ولی خطاهای آن از مسیر async void منتشر می‌شوند و فراخواننده امکان await کردن ندارد. برای حفظ قرارداد در این مرحله تغییر نکرد؛ تست خطا با Context مخصوص، رفتار قبل و بعد را ثبت می‌کند.

این موارد نیازمند اصلاح مستقل منطق و قرارداد در صورت تصمیم کارفرما هستند. موفقیت تست مقایسه صرفاً نشان می‌دهد ادغام آن‌ها را تغییر نداده است.

## اجرای مجدد هر 50 مورد

از ریشه همین checkout ویندوز:

```powershell
powershell -File scripts/test-persistence-merge20.ps1 -All50
```

اگر Build API از قبل انجام شده، -SkipBuild قابل استفاده است. نام پروژه تست برای حفظ مسیر قبلی تغییر نکرده؛ گزینه --all50 در executable و -All50 در اسکریپت هر دو مرحله را بررسی می‌کند. اجرا بدون این گزینه همچنان فقط 20 مورد اول را تست می‌کند.

[گزارش مرحله اول](sandbox:/D:/Projects/Aban360/docs/persistence-consolidation-first20.fa.md)
