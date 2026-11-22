# ادغام 20 جفت Persistence — مرحله اول

تاریخ: 2026/11/22. محیط اجرا: ویندوز بومی، D:/Projects/Aban360.

همان 20 مورد تأییدشده، شامل 6 مورد BlobPool و 14 مورد CalculationPool انجام شد: 40 implementation به 20 Repository تبدیل شدند. هر 40 interface قبلی بدون تغییر حفظ شدند. در فهرست اولیه 110 جفت، اکنون 90 جفت باقی است؛ این گزارش مجوز اجرای موارد بعدی نیست.

## کلاس‌های نهایی

| ردیف | Repository | خطوط واقعی بدنه کلاس |
| ---: | --- | ---: |
| 1 | [DocumentEntityRepository](sandbox:/D:/Projects/Aban360/Aban360.BlobPool.Persistence/Features/DMS/Repositories/DocumentEntityRepository.cs) | 38 |
| 2 | [DocumentCategoryRepository](sandbox:/D:/Projects/Aban360/Aban360.BlobPool.Persistence/Features/Taxonomy/Repositories/DocumentCategoryRepository.cs) | 47 |
| 3 | [DocumentRepository](sandbox:/D:/Projects/Aban360/Aban360.BlobPool.Persistence/Features/Taxonomy/Repositories/DocumentRepository.cs) | 58 |
| 4 | [DocumentTypeRepository](sandbox:/D:/Projects/Aban360/Aban360.BlobPool.Persistence/Features/Taxonomy/Repositories/DocumentTypeRepository.cs) | 49 |
| 5 | [ExecutableMimetypeRepository](sandbox:/D:/Projects/Aban360/Aban360.BlobPool.Persistence/Features/Taxonomy/Repositories/ExecutableMimetypeRepository.cs) | 49 |
| 6 | [MimetypeCategoryRepository](sandbox:/D:/Projects/Aban360/Aban360.BlobPool.Persistence/Features/Taxonomy/Repositories/MimetypeCategoryRepository.cs) | 49 |
| 7 | [CompanyServiceRepository](sandbox:/D:/Projects/Aban360/Aban360.CalculationPool.Persistence/Features/Bill/Repositories/CompanyServiceRepository.cs) | 45 |
| 8 | [CompanyServiceOfferingRepository](sandbox:/D:/Projects/Aban360/Aban360.CalculationPool.Persistence/Features/Bill/Repositories/CompanyServiceOfferingRepository.cs) | 38 |
| 9 | [CompanyServiceTypeRepository](sandbox:/D:/Projects/Aban360/Aban360.CalculationPool.Persistence/Features/Bill/Repositories/CompanyServiceTypeRepository.cs) | 37 |
| 10 | [ImpactSignRepository](sandbox:/D:/Projects/Aban360/Aban360.CalculationPool.Persistence/Features/Bill/Repositories/ImpactSignRepository.cs) | 32 |
| 11 | [InvoiceInstallmentRepository](sandbox:/D:/Projects/Aban360/Aban360.CalculationPool.Persistence/Features/Bill/Repositories/InvoiceInstallmentRepository.cs) | 44 |
| 12 | [InvoiceLineItemInsertModeRepository](sandbox:/D:/Projects/Aban360/Aban360.CalculationPool.Persistence/Features/Bill/Repositories/InvoiceLineItemInsertModeRepository.cs) | 32 |
| 13 | [InvoiceStatusRepository](sandbox:/D:/Projects/Aban360/Aban360.CalculationPool.Persistence/Features/Bill/Repositories/InvoiceStatusRepository.cs) | 32 |
| 14 | [InvoiceTypeRepository](sandbox:/D:/Projects/Aban360/Aban360.CalculationPool.Persistence/Features/Bill/Repositories/InvoiceTypeRepository.cs) | 32 |
| 15 | [LineItemTypeRepository](sandbox:/D:/Projects/Aban360/Aban360.CalculationPool.Persistence/Features/Bill/Repositories/LineItemTypeRepository.cs) | 37 |
| 16 | [LineItemTypeGroupRepository](sandbox:/D:/Projects/Aban360/Aban360.CalculationPool.Persistence/Features/Bill/Repositories/LineItemTypeGroupRepository.cs) | 37 |
| 17 | [OfferingGroupRepository](sandbox:/D:/Projects/Aban360/Aban360.CalculationPool.Persistence/Features/Bill/Repositories/OfferingGroupRepository.cs) | 32 |
| 18 | [OfferingUnitRepository](sandbox:/D:/Projects/Aban360/Aban360.CalculationPool.Persistence/Features/Bill/Repositories/OfferingUnitRepository.cs) | 32 |
| 19 | [WaterMeterChangeNumberHistoryRepository](sandbox:/D:/Projects/Aban360/Aban360.CalculationPool.Persistence/Features/Bill/Repositories/WaterMeterChangeNumberHistoryRepository.cs) | 32 |
| 20 | [TariffCalculationModeRepository](sandbox:/D:/Projects/Aban360/Aban360.CalculationPool.Persistence/Features/Rule/Repositories/TariffCalculationModeRepository.cs) | 32 |

خطوط از اعلان کلاس تا آکولاد پایانی، با احتساب خطوط خالی و توضیحات محاسبه شد. using و namespace جدا هستند. محدوده نهایی 32 تا 58 خط است و قالب چندخطی متدهای قبلی حفظ شده است.

## رفتار و DI

- هر کلاس فقط عملیات همان موجودیت را پیاده‌سازی می‌کند؛ منطق هیچ‌یک از 103 متد تغییر نکرده است. تنها نام چند فیلد خصوصی یکسان‌سازی شده است.
- سازنده مشترک بر پایه سازنده Command قبلی است؛ بررسی‌های null آن حفظ شده‌اند. Queryهایی که قبلاً بررسی null نداشتند، اکنون از همان سازنده دارای بررسی استفاده می‌کنند.
- UnitOfWork و DbSet همان ماژول حفظ شده‌اند. محل SaveChanges، فیلترها، Include، AsNoTracking، ترتیب await، نوع خروجی و استثناهای عملیات حفظ شدند.
- Repositoryها از ثبت عمومی Scrutor کنار گذاشته و یک بار Scoped ثبت شدند. هر دو interface با factory به همان نمونه Scoped متصل می‌شوند؛ تست‌ها یکسان‌بودن نمونه در Scope و متفاوت‌بودن آن بین Scopeها را بررسی می‌کنند.
- کلاس‌های پایه اتصال، DbContextها، migrationها و سرویس‌های Dapper در این مرحله تغییر نکردند.

## اعتبارسنجی

خروجی نهایی تست اختصاصی: 737 بررسی موفق، شامل 311 سناریوی مقایسه قبل و بعد برای 103 متد و 20 Repository.

- 103 متد با مقایسه توکن‌های نحوی با نسخه پیش از ادغام بررسی شدند؛ interfaceها با SHA-256 کنترل شدند.
- 311 سناریوی قبل و بعد با پیاده‌سازی‌های ثبت‌شده قبلی و EF InMemory اجرا شدند: داده موجود، شناسه ناموجود، ورودی null، عملیات تکی و گروهی، tracking، عدم ذخیره ضمنی و رفتار استثناها.
- رفتار خطای سازنده با UnitOfWork برابر null، ثبت یکتای DI، هویت مشترک قراردادها و جدایی Scopeها نیز بررسی می‌شود.
- تست‌ها به SQL Server واقعی متصل نمی‌شوند؛ این نتیجه تأیید تست یکپارچه SQL Server یا رفتار provider SQL Server نیست. منطق پرس‌وجوها در سطح نحوی عیناً حفظ شده است.
- Build نهایی API و Bridge بدون خطای کامپایل انجام شد؛ warningهای پروژه همچنان وجود دارند.

## ایراد قدیمی کشف‌شده و حفظ‌شده

در [CompanyServiceOfferingRepository.Get(short id)](sandbox:/D:/Projects/Aban360/Aban360.CalculationPool.Persistence/Features/Bill/Repositories/CompanyServiceOfferingRepository.cs)، پارامتر id در فیلتر استفاده نمی‌شود و متد روی کل مجموعه SingleAsync اجرا می‌کند. نسخه قبلی نیز دقیقاً همین منطق را داشت. با چند رکورد خطای Sequence contains more than one element رخ می‌دهد؛ با یک رکورد، حتی برای شناسه ناموجود، همان رکورد برمی‌گردد.

این ایراد در این مرحله اصلاح نشد تا اصلاح منطق از ادغام مستقل بماند. تست اختصاصی هر دو رفتار قدیمی و حالت موفق با یک رکورد را ثبت و با Repository جدید مقایسه می‌کند.

## اجرای مجدد

پیش‌نیاز: SDK .NET 10 برای ابزار مقایسه Roslyn، وابستگی‌های فعلی پروژه .NET 8 و provider آزمایشی EF InMemory 8.0.0. دانلود provider فقط برای پروژه تست است و وابستگی production تغییر نکرده است.

از ریشه همین checkout ویندوز اجرا شود:

```powershell
powershell -File scripts/test-persistence-merge20.ps1
```

اگر Build نهایی API قبلاً انجام شده است، پارامتر -SkipBuild قابل استفاده است. فایل‌های Baseline و manifest در tests/Aban360.PersistenceMerge20.RegressionTests نسخه قبل از ادغام را نگه می‌دارند و نباید برای پوشاندن اختلاف رفتاری بازتولید شوند.
