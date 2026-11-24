# ادغام ردیف‌های 51 تا 80 و بررسی مجموع 80 مورد

تاریخ: 2026/11/22. مسیر اجرا: D:/Projects/Aban360 در ویندوز بومی.

این مرحله همان 30 جفت تأییدشده ردیف‌های 51 تا 80 است: 23 مورد ClaimPool، چهار مورد InstallationPool و سه مورد LocationPool. در این مرحله 60 implementation به 30 Repository تبدیل شدند. مجموع سه مرحله: 160 implementation به 80 Repository با حفظ هر 160 interface. از فهرست اولیه 110 جفت، 30 جفت باقی است.

## نتیجه اعتبارسنجی

- 2737 بررسی موفق برای مجموع هر 80 Repository.
- 1091 سناریوی قبل و بعد با EF InMemory؛ منطق هر 363 متد با نسخه ثبت‌شده پیش از ادغام تطبیق نحوی داده شد.
- SHA-256 هر 160 interface با نسخه پیش از ادغام یکسان است. 160 فایل implementation قبلی در Baseline برای مقایسه حفظ شده‌اند.
- فایل کد هر 50 Repository قبلی نسبت به ابتدای این مرحله بدون تغییر بایت باقی ماند. helper ثبت DI ماژول ClaimPool برای Repositoryهای جدید تکمیل و برای تمام قراردادهای قبلی و جدید تست شد.
- ثبت یکتای interfaceها، نمونه مشترک دو قرارداد در یک Scope، جدایی Scopeها، UnitOfWork صحیح، داده موجود و ناموجود، ورودی null، افزودن و حذف تکی و گروهی، tracking و عدم ذخیره ضمنی کنترل شدند.
- Build API: صفر خطای کامپایل؛ 917 warning. Build Bridge: صفر خطای کامپایل؛ 25 warning. موفقیت Build به معنی نبود warning در پروژه نیست.
- تست‌های رفتاری با EF InMemory انجام شدند؛ به SQL Server واقعی متصل نشده‌ایم و ترجمه و اجرای queryها روی provider واقعی SQL Server در این نتیجه تأیید نشده است. متن متدهای query در ادغام حفظ شده است.

## کلاس‌های نهایی این مرحله

| ردیف | Repository | خطوط واقعی بدنه کلاس |
| ---: | --- | ---: |
| 51 | [WaterResourceRepository](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Land/Repositories/WaterResourceRepository.cs) | 34 |
| 52 | [MeterDiameterRepository](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Metering/Repositories/MeterDiameterRepository.cs) | 34 |
| 53 | [MeterMaterialRepository](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Metering/Repositories/MeterMaterialRepository.cs) | 34 |
| 54 | [MeterProducerRepository](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Metering/Repositories/MeterProducerRepository.cs) | 34 |
| 55 | [MeterTypeRepository](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Metering/Repositories/MeterTypeRepository.cs) | 34 |
| 56 | [MeterUseTypeRepository](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Metering/Repositories/MeterUseTypeRepository.cs) | 34 |
| 57 | [SubscriptionTypeRepository](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Metering/Repositories/SubscriptionTypeRepository.cs) | 34 |
| 58 | [UseStateRepository](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Metering/Repositories/UseStateRepository.cs) | 34 |
| 59 | [WaterMeterRepository](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Metering/Repositories/WaterMeterRepository.cs) | 45 |
| 60 | [WaterMeterInstallationStructureRepository](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Metering/Repositories/WaterMeterInstallationStructureRepository.cs) | 34 |
| 61 | [WaterMeterTagRepository](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Metering/Repositories/WaterMeterTagRepository.cs) | 36 |
| 62 | [WaterMeterTagDefinitionRepository](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Metering/Repositories/WaterMeterTagDefinitionRepository.cs) | 34 |
| 63 | [IndividualRepository](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/People/Repositories/IndividualRepository.cs) | 39 |
| 64 | [IndividualDiscountTypeRepository](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/People/Repositories/IndividualDiscountTypeRepository.cs) | 34 |
| 65 | [IndividualEstateRepository](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/People/Repositories/IndividualEstateRepository.cs) | 34 |
| 66 | [IndividualEstateRelationTypeRepository](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/People/Repositories/IndividualEstateRelationTypeRepository.cs) | 34 |
| 67 | [IndividualTagRepository](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/People/Repositories/IndividualTagRepository.cs) | 37 |
| 68 | [IndividualTagDefinitionRepository](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/People/Repositories/IndividualTagDefinitionRepository.cs) | 34 |
| 69 | [IndividualTypeRepository](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/People/Repositories/IndividualTypeRepository.cs) | 34 |
| 70 | [GatewayRepository](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Request/Repositories/GatewayRepository.cs) | 34 |
| 71 | [SiphonRepository](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/WasteWater/Repositories/SiphonRepository.cs) | 38 |
| 72 | [SiphonDiameterRepository](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/WasteWater/Repositories/SiphonDiameterRepository.cs) | 37 |
| 73 | [SiphonMaterialRepository](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/WasteWater/Repositories/SiphonMaterialRepository.cs) | 37 |
| 74 | [EquipmentBrokerRepository](sandbox:/D:/Projects/Aban360/Aban360.InstallationPool.Persistence/Features/Repositories/EquipmentBrokerRepository.cs) | 34 |
| 75 | [EquipmentBrokerZoneRepository](sandbox:/D:/Projects/Aban360/Aban360.InstallationPool.Persistence/Features/Repositories/EquipmentBrokerZoneRepository.cs) | 34 |
| 76 | [SewageEquipmentBrokerRepository](sandbox:/D:/Projects/Aban360/Aban360.InstallationPool.Persistence/Features/Repositories/SewageEquipmentBrokerRepository.cs) | 34 |
| 77 | [SewageEquipmentBrokerZoneRepository](sandbox:/D:/Projects/Aban360/Aban360.InstallationPool.Persistence/Features/Repositories/SewageEquipmentBrokerZoneRepository.cs) | 34 |
| 78 | [CordinalDirectionRepository](sandbox:/D:/Projects/Aban360/Aban360.LocationPool.Persistence/Features/MainHierarchy/Repositories/CordinalDirectionRepository.cs) | 34 |
| 79 | [CountryRepository](sandbox:/D:/Projects/Aban360/Aban360.LocationPool.Persistence/Features/MainHierarchy/Repositories/CountryRepository.cs) | 33 |
| 80 | [HeadquarterRepository](sandbox:/D:/Projects/Aban360/Aban360.LocationPool.Persistence/Features/MainHierarchy/Repositories/HeadquarterRepository.cs) | 38 |

خطوط از اعلان کلاس تا آکولاد پایانی، شامل خطوط خالی و توضیحات اندازه‌گیری شده‌اند. using و namespace جدا هستند. کلاس‌های این مرحله 33 تا 45 خط و کل 80 Repository در محدوده 32 تا 74 خط هستند. قالب چندخطی متدهای قبلی حفظ شده است.

## قراردادها و ثبت DI

نام قراردادها تغییر نکرده است؛ برای نمونه WaterMeterInstallationStructureRepository همان IWaterMeterInstallationMethodCommandService و IWaterMeterInstallationMethodQueryService قبلی را پیاده می‌کند. نام واقعی قراردادها از تعریف آن‌ها خوانده شد؛ از روی نام implementation فرض نشده‌اند.

Repositoryها از ثبت عمومی Scrutor کنار گذاشته می‌شوند و هر دو قرارداد از یک instance مشترک Scoped resolve می‌شوند. DbContext، UnitOfWork، محل SaveChanges، مدل‌های داده، migrationها و کلاس‌های پایه اتصال تغییر نکرده‌اند.

## نکات منطق قدیمی

نقص‌های قبلی CompanyServiceOffering.Get(short id)، RequestUser و RequestEstate.Remove در این مرحله اصلاح نشده‌اند. [شرح کامل و تست‌های رفتار قبلی](sandbox:/D:/Projects/Aban360/docs/persistence-consolidation-next30.fa.md) در گزارش مرحله دوم آمده است. موفقیت مقایسه نشان‌دهنده حفظ رفتار است و ادعای بی‌نقص‌بودن آن منطق نیست.

در overload فیلتر [WaterMeter.Get](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Metering/Repositories/WaterMeterRepository.cs)، پارامترهای individualTypeId و ZoneId از قبل در شرط query استفاده نمی‌شدند؛ این وضعیت حفظ شده است. تبدیل تاریخ و شناسه قبض با DateTime.Parse و long.Parse نیز همان منطق قبلی است. در تست موفق این overload از تاریخ و شناسه قبض معتبر استفاده شد؛ ورودی null و بازه فاقد نتیجه نیز قبل و بعد مقایسه شدند.

## اجرای مجدد

از ریشه همین checkout ویندوز:

```powershell
powershell -File scripts/test-persistence-merge20.ps1 -All80
```

گزینه -SkipBuild در صورت انجام قبلی Build API قابل استفاده است. نام پروژه تست برای سازگاری با مراحل قبل حفظ شده؛ -All80 همه 80 مورد، -All50 مرحله اول و دوم، و اجرای بدون این گزینه‌ها فقط 20 مورد اول را بررسی می‌کند.

فهرست این مرحله در rows51to80.json، مرحله دوم در next30.json و مرحله اول در manifest.json قرار دارند. نسخه‌های ثبت‌شده Baseline نباید برای پوشاندن اختلاف رفتاری بازتولید شوند.
