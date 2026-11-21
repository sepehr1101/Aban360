# بررسی ادغام کلاس‌های دسترسی به دیتابیس Aban360

تاریخ بررسی: 2026/11/20. بررسی در ویندوز بومی و مسیر D:/Projects/Aban360 انجام شد. هیچ ادغامی در کد اجرایی انجام نشده است.

## نتیجه

اسکن نحوی Roslyn در پروژه‌های *.Persistence و Aban360.Common.Db، تعداد 905 تعریف کلاس را بررسی کرد. از این تعداد، 679 تعریف کلاس غیر abstract در پوشه‌های Features قرار داشت و 153 جفت هم‌نام CommandService/QueryService در یک ماژول پیدا شد. شمارش کلی شامل migration، seeder و بخش‌های partial است؛ این موارد وارد پیشنهاد ادغام سرویس‌ها نشده‌اند. فایل‌های bin و obj و کلاس‌های کامنت‌شده کنار گذاشته شدند.

پیشنهاد اولیه: **104 جفت EF و 6 جفت Dapper؛ در مجموع 110 جفت، یعنی 220 implementation به 110 implementation**. هر جفت فقط در محدوده همان موجودیت و همان ماژول ادغام می‌شود. دو AbstractBaseConnection نیز یک فرصت مستقل برای حذف تکرار دارند. 43 جفت دیگر فعلاً پیشنهاد نمی‌شوند.

این بررسی ساختاری است، نه تأیید صحت رفتار روی دیتابیس واقعی. وابستگی سازنده، نوع DbSet، namespace مربوط به UnitOfWork، وراثت، امضاهای متدها، ثبت DI و شیوه اتصال و تراکنش بررسی شدند. در جفت‌های پیشنهادی، تداخل یکسان نام و نوع پارامتر متدها دیده نشد؛ پیش از اجرا همچنان کامپایل و مرور تک‌به‌تک لازم است.

## مبنای اندازه‌گیری

خط فعلی از اعلان کلاس تا آکولاد پایانی با Roslyn محاسبه شده است؛ شامل خط خالی، توضیح و SQL چندخطی داخل کلاس است و using و namespace و interface مستقل را شامل نمی‌شود. عدد نهایی برآورد است: جمع دو کلاس، منهای یک سازنده تکراری، فیلدهای مشترک و اعلان و آکولادهای کلاس دوم. تغییر قالب‌بندی یا توضیحات می‌تواند عدد نهایی را تغییر دهد. جمع فعلی معیار محافظه‌کارانه‌ای برای حجم ادغام ساده است.

سقف انتخاب اولیه برای این بررسی 250 خط مجموع دو کلاس بود؛ همه 110 جفت منتخب عملاً حداکثر 156 خط مجموع دارند. این عدد استاندارد عمومی Code Smell نیست. اشتراک مسئولیت، تعداد وابستگی‌ها، حفظ مرز تراکنش و پیچیدگی SQL بر عدد خطوط اولویت دارند. کلاس‌های EF منتخب با برآورد 31 تا 73 خط و Dapper منتخب با برآورد 47 تا 149 خط کوچک می‌مانند.

## خلاصه ماژول‌ها

| ماژول | تعداد جفت | کلاس فعلی به نهایی | بازه برآورد خطوط نهایی |
| --- | ---: | --- | ---: |
| BlobPool | 6 | 12 -> 6 | 39–59 |
| CalculationPool | 19 | 38 -> 19 | 33–149 |
| ClaimPool | 53 | 106 -> 53 | 32–48 |
| InstallationPool | 4 | 8 -> 4 | 33–33 |
| LocationPool | 8 | 16 -> 8 | 32–60 |
| MeterPool | 4 | 8 -> 4 | 33–38 |
| OldCalcPool | 1 | 2 -> 1 | 73–73 |
| PaymentPool | 5 | 10 -> 5 | 33–38 |
| ReportPool | 1 | 2 -> 1 | 50–50 |
| UserPool | 6 | 12 -> 6 | 31–56 |
| WorkflowPool | 3 | 6 -> 3 | 37–52 |

## اولویت اول: 104 جفت EF

این جفت‌ها UnitOfWork از یک namespace و DbSet از یک نوع موجودیت دارند. شرط ادغام حفظ SaveChanges در لایه فعلی، رفتار tracking، فیلتر حذف نرم، Include و IQueryable است. هیچ SaveChanges جدیدی نباید به عملیات اضافه شود. اشتراک دیتابیس به‌تنهایی مجوز ادغام DbContext ماژول‌های مختلف نیست.

### BlobPool — 6 جفت

| CommandService | QueryService | خط Command | خط Query | جمع فعلی | برآورد نهایی |
| --- | --- | ---: | ---: | ---: | ---: |
| [DocumentEntityCommandService](sandbox:/D:/Projects/Aban360/Aban360.BlobPool.Persistence/Features/DMS/Commands/Implementations/DocumentEntityCommandService.cs) | [DocumentEntityQueryService](sandbox:/D:/Projects/Aban360/Aban360.BlobPool.Persistence/Features/DMS/Queries/Implementations/DocumentEntityQueryService.cs) | 23 | 29 | 52 | ≈39 |
| [DocumentCategoryCommandService](sandbox:/D:/Projects/Aban360/Aban360.BlobPool.Persistence/Features/Taxonomy/Commands/Implementations/DocumentCategoryCommandService.cs) | [DocumentCategoryQueryService](sandbox:/D:/Projects/Aban360/Aban360.BlobPool.Persistence/Features/Taxonomy/Queries/Implementations/DocumentCategoryQueryService.cs) | 37 | 21 | 58 | ≈48 |
| [DocumentCommandService](sandbox:/D:/Projects/Aban360/Aban360.BlobPool.Persistence/Features/Taxonomy/Commands/Implementations/DocumentCommandService.cs) | [DocumentQueryService](sandbox:/D:/Projects/Aban360/Aban360.BlobPool.Persistence/Features/Taxonomy/Queries/Implementations/DocumentQueryService.cs) | 23 | 49 | 72 | ≈59 |
| [DocumentTypeCommandService](sandbox:/D:/Projects/Aban360/Aban360.BlobPool.Persistence/Features/Taxonomy/Commands/Implementations/DocumentTypeCommandService.cs) | [DocumentTypeQueryService](sandbox:/D:/Projects/Aban360/Aban360.BlobPool.Persistence/Features/Taxonomy/Queries/Implementations/DocumentTypeQueryService.cs) | 37 | 24 | 61 | ≈51 |
| [ExecutableMimetypeCommandService](sandbox:/D:/Projects/Aban360/Aban360.BlobPool.Persistence/Features/Taxonomy/Commands/Implementations/ExecutableMimetypeCommandService.cs) | [ExecutableMimetypeQueryService](sandbox:/D:/Projects/Aban360/Aban360.BlobPool.Persistence/Features/Taxonomy/Queries/Implementations/ExecutableMimetypeQueryService.cs) | 37 | 24 | 61 | ≈51 |
| [MimetypeCategoryCommandService](sandbox:/D:/Projects/Aban360/Aban360.BlobPool.Persistence/Features/Taxonomy/Commands/Implementations/MimetypeCategoryCommandService.cs) | [MimetypeCategoryQueryService](sandbox:/D:/Projects/Aban360/Aban360.BlobPool.Persistence/Features/Taxonomy/Queries/Implementations/MimetypeCategoryQueryService.cs) | 37 | 24 | 61 | ≈51 |

### CalculationPool — 16 جفت

| CommandService | QueryService | خط Command | خط Query | جمع فعلی | برآورد نهایی |
| --- | --- | ---: | ---: | ---: | ---: |
| [CompanyServiceCommandService](sandbox:/D:/Projects/Aban360/Aban360.CalculationPool.Persistence/Features/Bill/Commands/Implementations/CompanyServiceCommandService.cs) | [CompanyServiceQueryService](sandbox:/D:/Projects/Aban360/Aban360.CalculationPool.Persistence/Features/Bill/Queries/Implementations/CompanyServiceQueryService.cs) | 23 | 36 | 59 | ≈46 |
| [CompanyServiceOfferingCommandService](sandbox:/D:/Projects/Aban360/Aban360.CalculationPool.Persistence/Features/Bill/Commands/Implementations/CompanyServiceOfferingCommandService.cs) | [CompanyServiceOfferingQueryService](sandbox:/D:/Projects/Aban360/Aban360.CalculationPool.Persistence/Features/Bill/Queries/Implementations/CompanyServiceOfferingQueryService.cs) | 23 | 29 | 52 | ≈39 |
| [CompanyServiceTypeCommandService](sandbox:/D:/Projects/Aban360/Aban360.CalculationPool.Persistence/Features/Bill/Commands/Implementations/CompanyServiceTypeCommandService.cs) | [CompanyServiceTypeQueryService](sandbox:/D:/Projects/Aban360/Aban360.CalculationPool.Persistence/Features/Bill/Queries/Implementations/CompanyServiceTypeQueryService.cs) | 23 | 28 | 51 | ≈38 |
| [ImpactSignCommandService](sandbox:/D:/Projects/Aban360/Aban360.CalculationPool.Persistence/Features/Bill/Commands/Implementations/ImpactSignCommandService.cs) | [ImpactSignQueryService](sandbox:/D:/Projects/Aban360/Aban360.CalculationPool.Persistence/Features/Bill/Queries/Implementations/ImpactSignQueryService.cs) | 23 | 23 | 46 | ≈33 |
| [InvoiceInstallmentCommandService](sandbox:/D:/Projects/Aban360/Aban360.CalculationPool.Persistence/Features/Bill/Commands/Implementations/InvoiceInstallmentCommandService.cs) | [InvoiceInstallmentQueryService](sandbox:/D:/Projects/Aban360/Aban360.CalculationPool.Persistence/Features/Bill/Queries/Implementations/InvoiceInstallmentQueryService.cs) | 27 | 31 | 58 | ≈45 |
| [InvoiceLineItemInsertModeCommandService](sandbox:/D:/Projects/Aban360/Aban360.CalculationPool.Persistence/Features/Bill/Commands/Implementations/InvoiceLineItemInsertModeCommandService.cs) | [InvoiceLineItemInsertModeQueryService](sandbox:/D:/Projects/Aban360/Aban360.CalculationPool.Persistence/Features/Bill/Queries/Implementations/InvoiceLineItemInsertModeQueryService.cs) | 23 | 23 | 46 | ≈33 |
| [InvoiceStatusCommandService](sandbox:/D:/Projects/Aban360/Aban360.CalculationPool.Persistence/Features/Bill/Commands/Implementations/InvoiceStatusCommandService.cs) | [InvoiceStatusQueryService](sandbox:/D:/Projects/Aban360/Aban360.CalculationPool.Persistence/Features/Bill/Queries/Implementations/InvoiceStatusQueryService.cs) | 23 | 23 | 46 | ≈33 |
| [InvoiceTypeCommandService](sandbox:/D:/Projects/Aban360/Aban360.CalculationPool.Persistence/Features/Bill/Commands/Implementations/InvoiceTypeCommandService.cs) | [InvoiceTypeQueryService](sandbox:/D:/Projects/Aban360/Aban360.CalculationPool.Persistence/Features/Bill/Queries/Implementations/InvoiceTypeQueryService.cs) | 23 | 23 | 46 | ≈33 |
| [LineItemTypeCommandService](sandbox:/D:/Projects/Aban360/Aban360.CalculationPool.Persistence/Features/Bill/Commands/Implementations/LineItemTypeCommandService.cs) | [LineItemTypeQueryService](sandbox:/D:/Projects/Aban360/Aban360.CalculationPool.Persistence/Features/Bill/Queries/Implementations/LineItemTypeQueryService.cs) | 23 | 28 | 51 | ≈38 |
| [LineItemTypeGroupCommandService](sandbox:/D:/Projects/Aban360/Aban360.CalculationPool.Persistence/Features/Bill/Commands/Implementations/LineItemTypeGroupCommandService.cs) | [LineItemTypeGroupQueryService](sandbox:/D:/Projects/Aban360/Aban360.CalculationPool.Persistence/Features/Bill/Queries/Implementations/LineItemTypeGroupQueryService.cs) | 23 | 28 | 51 | ≈38 |
| [OfferingGroupCommandService](sandbox:/D:/Projects/Aban360/Aban360.CalculationPool.Persistence/Features/Bill/Commands/Implementations/OfferingGroupCommandService.cs) | [OfferingGroupQueryService](sandbox:/D:/Projects/Aban360/Aban360.CalculationPool.Persistence/Features/Bill/Queries/Implementations/OfferingGroupQueryService.cs) | 23 | 23 | 46 | ≈33 |
| [OfferingUnitCommandService](sandbox:/D:/Projects/Aban360/Aban360.CalculationPool.Persistence/Features/Bill/Commands/Implementations/OfferingUnitCommandService.cs) | [OfferingUnitQueryService](sandbox:/D:/Projects/Aban360/Aban360.CalculationPool.Persistence/Features/Bill/Queries/Implementations/OfferingUnitQueryService.cs) | 23 | 23 | 46 | ≈33 |
| [WaterMeterChangeNumberHistoryCommandService](sandbox:/D:/Projects/Aban360/Aban360.CalculationPool.Persistence/Features/Bill/Commands/Implementations/WaterMeterChangeNumberHistoryCommandService.cs) | [WaterMeterChangeNumberHistoryQueryService](sandbox:/D:/Projects/Aban360/Aban360.CalculationPool.Persistence/Features/Bill/Queries/Implementations/WaterMeterChangeNumberHistoryQueryService.cs) | 23 | 23 | 46 | ≈33 |
| [TariffCalculationModeCommandService](sandbox:/D:/Projects/Aban360/Aban360.CalculationPool.Persistence/Features/Rule/Commands/Implementations/TariffCalculationModeCommandService.cs) | [TariffCalculationModeQueryService](sandbox:/D:/Projects/Aban360/Aban360.CalculationPool.Persistence/Features/Rule/Queries/Implementations/TariffCalculationModeQueryService.cs) | 23 | 23 | 46 | ≈33 |
| [TariffCommandService](sandbox:/D:/Projects/Aban360/Aban360.CalculationPool.Persistence/Features/Rule/Commands/Implementations/TariffCommandService.cs) | [TariffQueryService](sandbox:/D:/Projects/Aban360/Aban360.CalculationPool.Persistence/Features/Rule/Queries/Implementations/TariffQueryService.cs) | 23 | 63 | 86 | ≈73 |
| [TariffConstantCommandService](sandbox:/D:/Projects/Aban360/Aban360.CalculationPool.Persistence/Features/Rule/Commands/Implementations/TariffConstantCommandService.cs) | [TariffConstantQueryService](sandbox:/D:/Projects/Aban360/Aban360.CalculationPool.Persistence/Features/Rule/Queries/Implementations/TariffConstantQueryService.cs) | 23 | 41 | 64 | ≈51 |

### ClaimPool — 51 جفت

| CommandService | QueryService | خط Command | خط Query | جمع فعلی | برآورد نهایی |
| --- | --- | ---: | ---: | ---: | ---: |
| [RequestEstateCommandService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Draft/Commands/Implementations/RequestEstateCommandService.cs) | [RequestEstateQueryService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Draft/Queries/Implementations/RequestEstateQueryService.cs) | 25 | 23 | 48 | ≈35 |
| [RequestFlatCommandService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Draft/Commands/Implementations/RequestFlatCommandService.cs) | [RequestFlatQueryService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Draft/Queries/Implementations/RequestFlatQueryService.cs) | 23 | 30 | 53 | ≈40 |
| [RequestIndividualCommandService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Draft/Commands/Implementations/RequestIndividualCommandService.cs) | [RequestIndividualQueryService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Draft/Queries/Implementations/RequestIndividualQueryService.cs) | 23 | 23 | 46 | ≈33 |
| [RequestIndividualDiscountTypeCommandService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Draft/Commands/Implementations/RequestIndividualDiscountTypeCommandService.cs) | [RequestIndividualDiscountTypeQueryService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Draft/Queries/Implementations/RequestIndividualDiscountTypeQueryService.cs) | 23 | 30 | 53 | ≈40 |
| [RequestIndividualEstateCommandService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Draft/Commands/Implementations/RequestIndividualEstateCommandService.cs) | [RequestIndividualEstateQueryService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Draft/Queries/Implementations/RequestIndividualEstateQueryService.cs) | 23 | 32 | 55 | ≈42 |
| [RequestIndividualTagCommandService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Draft/Commands/Implementations/RequestIndividualTagCommandService.cs) | [RequestIndividualTagQueryService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Draft/Queries/Implementations/RequestIndividualTagQueryService.cs) | 27 | 29 | 56 | ≈43 |
| [RequestSiphonCommandService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Draft/Commands/Implementations/RequestSiphonCommandService.cs) | [RequestSiphonQueryService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Draft/Queries/Implementations/RequestSiphonQueryService.cs) | 23 | 23 | 46 | ≈33 |
| [RequestTrackingCommandService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Draft/Commands/Implementations/RequestTrackingCommandService.cs) | [RequestTrackingQueryService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Draft/Queries/Implementations/RequestTrackingQueryService.cs) | 23 | 30 | 53 | ≈40 |
| [RequestUserCommandService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Draft/Commands/Implementations/RequestUserCommandService.cs) | [RequestUserQueryService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Draft/Queries/Implementations/RequestUserQueryService.cs) | 22 | 23 | 45 | ≈32 |
| [RequestWaterMeterCommandService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Draft/Commands/Implementations/RequestWaterMeterCommandService.cs) | [RequestWaterMeterQueryService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Draft/Queries/Implementations/RequestWaterMeterQueryService.cs) | 23 | 30 | 53 | ≈40 |
| [RequestWaterMeterSiphonCommandService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Draft/Commands/Implementations/RequestWaterMeterSiphonCommandService.cs) | [RequestWaterMeterSiphonQueryService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Draft/Queries/Implementations/RequestWaterMeterSiphonQueryService.cs) | 23 | 38 | 61 | ≈48 |
| [RequestWaterMeterTagCommandService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Draft/Commands/Implementations/RequestWaterMeterTagCommandService.cs) | [RequestWaterMeterTagQueryService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Draft/Queries/Implementations/RequestWaterMeterTagQueryService.cs) | 27 | 30 | 57 | ≈44 |
| [CapacityCalculationIndexCommandService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Land/Commands/Implementations/CapacityCalculationIndexCommandService.cs) | [CapacityCalculationIndexQueryService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Land/Queries/Implementations/CapacityCalculationIndexQueryService.cs) | 23 | 23 | 46 | ≈33 |
| [ConstructionTypeCommandService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Land/Commands/Implementations/ConstructionTypeCommandService.cs) | [ConstructionTypeQueryService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Land/Queries/Implementations/ConstructionTypeQueryService.cs) | 23 | 23 | 46 | ≈33 |
| [EstateBoundTypeCommandService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Land/Commands/Implementations/EstateBoundTypeCommandService.cs) | [EstateBoundTypeQueryService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Land/Queries/Implementations/EstateBoundTypeQueryService.cs) | 23 | 23 | 46 | ≈33 |
| [EstateCommandService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Land/Commands/Implementations/EstateCommandService.cs) | [EstateQueryService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Land/Queries/Implementations/EstateQueryService.cs) | 23 | 23 | 46 | ≈33 |
| [EstateWaterResourceCommandService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Land/Commands/Implementations/EstateWaterResourceCommandService.cs) | [EstateWaterResourceQueryService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Land/Queries/Implementations/EstateWaterResourceQueryService.cs) | 23 | 30 | 53 | ≈40 |
| [FlatCommandService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Land/Commands/Implementations/FlatCommandService.cs) | [FlatQueryService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Land/Queries/Implementations/FlatQueryService.cs) | 23 | 28 | 51 | ≈38 |
| [GuildCommandService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Land/Commands/Implementations/GuildCommandService.cs) | [GuildQueryService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Land/Queries/Implementations/GuildQueryService.cs) | 23 | 28 | 51 | ≈38 |
| [HandoverCommandService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Land/Commands/Implementations/HandoverCommandService.cs) | [HandoverQueryService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Land/Queries/Implementations/HandoverQueryService.cs) | 23 | 23 | 46 | ≈33 |
| [OfficialHolidayCommandService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Land/Commands/Implementations/OfficialHolidayCommandService.cs) | [OfficialHolidayQueryService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Land/Queries/Implementations/OfficialHolidayQueryService.cs) | 23 | 23 | 46 | ≈33 |
| [ProfessionCommandService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Land/Commands/Implementations/ProfessionCommandService.cs) | [ProfessionQueryService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Land/Queries/Implementations/ProfessionQueryService.cs) | 23 | 28 | 51 | ≈38 |
| [UsageLevel1CommandService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Land/Commands/Implementations/UsageLevel1CommandService.cs) | [UsageLevel1QueryService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Land/Queries/Implementations/UsageLevel1QueryService.cs) | 23 | 23 | 46 | ≈33 |
| [UsageLevel2CommandService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Land/Commands/Implementations/UsageLevel2CommandService.cs) | [UsageLevel2QueryService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Land/Queries/Implementations/UsageLevel2QueryService.cs) | 23 | 28 | 51 | ≈38 |
| [UsageLevel3CommandService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Land/Commands/Implementations/UsageLevel3CommandService.cs) | [UsageLevel3QueryService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Land/Queries/Implementations/UsageLevel3QueryService.cs) | 23 | 28 | 51 | ≈38 |
| [UsageLevel4CommandService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Land/Commands/Implementations/UsageLevel4CommandService.cs) | [UsageLevel4QueryService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Land/Queries/Implementations/UsageLevel4QueryService.cs) | 23 | 28 | 51 | ≈38 |
| [UserLeaveCommandService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Land/Commands/Implementations/UserLeaveCommandService.cs) | [UserLeaveQueryService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Land/Queries/Implementations/UserLeaveQueryService.cs) | 23 | 23 | 46 | ≈33 |
| [UserWorkdayCommandService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Land/Commands/Implementations/UserWorkdayCommandService.cs) | [UserWorkdayQueryService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Land/Queries/Implementations/UserWorkdayQueryService.cs) | 23 | 23 | 46 | ≈33 |
| [WaterResourceCommandService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Land/Commands/Implementations/WaterResourceCommandService.cs) | [WaterResourceQueryService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Land/Queries/Implementations/WaterResourceQueryService.cs) | 23 | 23 | 46 | ≈33 |
| [MeterDiameterCommandService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Metering/Commands/Implementations/MeterDiameterCommandService.cs) | [MeterDiameterQueryService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Metering/Queries/Implementations/MeterDiameterQueryService.cs) | 23 | 23 | 46 | ≈33 |
| [MeterMaterialCommandService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Metering/Commands/Implementations/MeterMaterialCommandService.cs) | [MeterMaterialQueryService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Metering/Queries/Implementations/MeterMaterialQueryService.cs) | 23 | 23 | 46 | ≈33 |
| [MeterProducerCommandService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Metering/Commands/Implementations/MeterProducerCommandService.cs) | [MeterProducerQueryService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Metering/Queries/Implementations/MeterProducerQueryService.cs) | 23 | 23 | 46 | ≈33 |
| [MeterTypeCommandService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Metering/Commands/Implementations/MeterTypeCommandService.cs) | [MeterTypeQueryService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Metering/Queries/Implementations/MeterTypeQueryService.cs) | 23 | 23 | 46 | ≈33 |
| [MeterUseTypeCommandService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Metering/Commands/Implementations/MeterUseTypeCommandService.cs) | [MeterUseTypeQueryService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Metering/Queries/Implementations/MeterUseTypeQueryService.cs) | 23 | 23 | 46 | ≈33 |
| [SubscriptionTypeCommandService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Metering/Commands/Implementations/SubscriptionTypeCommandService.cs) | [SubscriptionTypeQueryService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Metering/Queries/Implementations/SubscriptionTypeQueryService.cs) | 23 | 23 | 46 | ≈33 |
| [UseStateCommandService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Metering/Commands/Implementations/UseStateCommandService.cs) | [UseStateQueryService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Metering/Queries/Implementations/UseStateQueryService.cs) | 23 | 23 | 46 | ≈33 |
| [WaterMeterCommandService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Metering/Commands/Implementations/WaterMeterCommandService.cs) | [WaterMeterQueryService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Metering/Queries/Implementations/WaterMeterQueryService.cs) | 23 | 34 | 57 | ≈44 |
| [WaterMeterInstallationStructureCommandService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Metering/Commands/Implementations/WaterMeterInstallationStructureCommandService.cs) | [WaterMeterInstallationStructureQueryService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Metering/Queries/Implementations/WaterMeterInstallationStructureQueryService.cs) | 23 | 23 | 46 | ≈33 |
| [WaterMeterTagCommandService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Metering/Commands/Implementations/WaterMeterTagCommandService.cs) | [WaterMeterTagQueryService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Metering/Queries/Implementations/WaterMeterTagQueryService.cs) | 23 | 25 | 48 | ≈35 |
| [WaterMeterTagDefinitionCommandService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Metering/Commands/Implementations/WaterMeterTagDefinitionCommandService.cs) | [WaterMeterTagDefinitionQueryService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Metering/Queries/Implementations/WaterMeterTagDefinitionQueryService.cs) | 23 | 23 | 46 | ≈33 |
| [IndividualCommandService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/People/Commands/Implementation/IndividualCommandService.cs) | [IndividualQueryService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/People/Queries/Implementations/IndividualQueryService.cs) | 28 | 23 | 51 | ≈38 |
| [IndividualDiscountTypeCommandService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/People/Commands/Implementation/IndividualDiscountTypeCommandService.cs) | [IndividualDiscountTypeQueryService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/People/Queries/Implementations/IndividualDiscountTypeQueryService.cs) | 23 | 23 | 46 | ≈33 |
| [IndividualEstateCommandService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/People/Commands/Implementation/IndividualEstateCommandService.cs) | [IndividualEstateQueryService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/People/Queries/Implementations/IndividualEstateQueryService.cs) | 23 | 23 | 46 | ≈33 |
| [IndividualEstateRelationTypeCommandService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/People/Commands/Implementation/IndividualEstateRelationTypeCommandService.cs) | [IndividualEstateRelationTypeQueryService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/People/Queries/Implementations/IndividualEstateRelationTypeQueryService.cs) | 23 | 23 | 46 | ≈33 |
| [IndividualTagCommandService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/People/Commands/Implementation/IndividualTagCommandService.cs) | [IndividualTagQueryService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/People/Queries/Implementations/IndividualTagQueryService.cs) | 23 | 26 | 49 | ≈36 |
| [IndividualTagDefinitionCommandService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/People/Commands/Implementation/IndividualTagDefinitionCommandService.cs) | [IndividualTagDefinitionQueryService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/People/Queries/Implementations/IndividualTagDefinitionQueryService.cs) | 23 | 23 | 46 | ≈33 |
| [IndividualTypeCommandService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/People/Commands/Implementation/IndividualTypeCommandService.cs) | [IndividualTypeQueryService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/People/Queries/Implementations/IndividualTypeQueryService.cs) | 23 | 23 | 46 | ≈33 |
| [GatewayCommandService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Request/Commands/Implementations/GatewayCommandService.cs) | [GatewayQueryService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Request/Queries/Implementations/GatewayQueryService.cs) | 23 | 23 | 46 | ≈33 |
| [SiphonCommandService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/WasteWater/Commands/Implementation/SiphonCommandService.cs) | [SiphonQueryService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/WasteWater/Queries/Implementation/SiphonQueryService.cs) | 27 | 23 | 50 | ≈37 |
| [SiphonDiameterCommandService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/WasteWater/Commands/Implementation/SiphonDiameterCommandService.cs) | [SiphonDiameterQueryService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/WasteWater/Queries/Implementation/siphonDiameterQueryService.cs) | 23 | 26 | 49 | ≈36 |
| [SiphonMaterialCommandService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/WasteWater/Commands/Implementation/SiphonMaterialCommandService.cs) | [SiphonMaterialQueryService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/WasteWater/Queries/Implementation/siphonMaterialQueryService.cs) | 23 | 26 | 49 | ≈36 |

### InstallationPool — 4 جفت

| CommandService | QueryService | خط Command | خط Query | جمع فعلی | برآورد نهایی |
| --- | --- | ---: | ---: | ---: | ---: |
| [EquipmentBrokerCommandService](sandbox:/D:/Projects/Aban360/Aban360.InstallationPool.Persistence/Features/Commands/Implementations/EquipmentBrokerCommandService.cs) | [EquipmentBrokerQueryService](sandbox:/D:/Projects/Aban360/Aban360.InstallationPool.Persistence/Features/Queries/Implementations/EquipmentBrokerQueryService.cs) | 23 | 23 | 46 | ≈33 |
| [EquipmentBrokerZoneCommandService](sandbox:/D:/Projects/Aban360/Aban360.InstallationPool.Persistence/Features/Commands/Implementations/EquipmentBrokerZoneCommandService.cs) | [EquipmentBrokerZoneQueryService](sandbox:/D:/Projects/Aban360/Aban360.InstallationPool.Persistence/Features/Queries/Implementations/EquipmentBrokerZoneQueryService.cs) | 23 | 23 | 46 | ≈33 |
| [SewageEquipmentBrokerCommandService](sandbox:/D:/Projects/Aban360/Aban360.InstallationPool.Persistence/Features/Commands/Implementations/SewageEquipmentBrokerCommandService.cs) | [SewageEquipmentBrokerQueryService](sandbox:/D:/Projects/Aban360/Aban360.InstallationPool.Persistence/Features/Queries/Implementations/SewageEquipmentBrokerQueryService.cs) | 23 | 23 | 46 | ≈33 |
| [SewageEquipmentBrokerZoneCommandService](sandbox:/D:/Projects/Aban360/Aban360.InstallationPool.Persistence/Features/Commands/Implementations/SewageEquipmentBrokerZoneCommandService.cs) | [SewageEquipmentBrokerZoneQueryService](sandbox:/D:/Projects/Aban360/Aban360.InstallationPool.Persistence/Features/Queries/Implementations/SewageEquipmentBrokerZoneQueryService.cs) | 23 | 23 | 46 | ≈33 |

### LocationPool — 8 جفت

| CommandService | QueryService | خط Command | خط Query | جمع فعلی | برآورد نهایی |
| --- | --- | ---: | ---: | ---: | ---: |
| [CordinalDirectionCommandService](sandbox:/D:/Projects/Aban360/Aban360.LocationPool.Persistence/Features/MainHierarchy/Commands/Implementations/CordinalDirectionCommandService.cs) | [CordinalDirectionQueryService](sandbox:/D:/Projects/Aban360/Aban360.LocationPool.Persistence/Features/MainHierarchy/Queries/Implementations/CordinalDirectionQueryService.cs) | 23 | 23 | 46 | ≈33 |
| [CountryCommandService](sandbox:/D:/Projects/Aban360/Aban360.LocationPool.Persistence/Features/MainHierarchy/Commands/Implementations/CountryCommandService.cs) | [CountryQueryService](sandbox:/D:/Projects/Aban360/Aban360.LocationPool.Persistence/Features/MainHierarchy/Queries/Implementations/CountryQueryService.cs) | 22 | 23 | 45 | ≈32 |
| [HeadquarterCommandService](sandbox:/D:/Projects/Aban360/Aban360.LocationPool.Persistence/Features/MainHierarchy/Commands/Implementations/HeadquarterCommandService.cs) | [HeadquarterQueryService](sandbox:/D:/Projects/Aban360/Aban360.LocationPool.Persistence/Features/MainHierarchy/Queries/Implementations/HeadquarterQueryService.cs) | 24 | 27 | 51 | ≈37 |
| [MunicipalityCommandService](sandbox:/D:/Projects/Aban360/Aban360.LocationPool.Persistence/Features/MainHierarchy/Commands/Implementations/MunicipalityCommandService.cs) | [MunicipalityQueryService](sandbox:/D:/Projects/Aban360/Aban360.LocationPool.Persistence/Features/MainHierarchy/Queries/Implementations/MunicipalityQueryService.cs) | 23 | 36 | 59 | ≈46 |
| [ProvinceCommandService](sandbox:/D:/Projects/Aban360/Aban360.LocationPool.Persistence/Features/MainHierarchy/Commands/Implementations/ProvinceCommandService.cs) | [ProvinceQueryService](sandbox:/D:/Projects/Aban360/Aban360.LocationPool.Persistence/Features/MainHierarchy/Queries/Implementations/ProvinceQueryService.cs) | 23 | 30 | 53 | ≈40 |
| [ReadingBoundCommandService](sandbox:/D:/Projects/Aban360/Aban360.LocationPool.Persistence/Features/MainHierarchy/Commands/Implementations/ReadingBoundCommandService.cs) | [ReadingBoundQueryService](sandbox:/D:/Projects/Aban360/Aban360.LocationPool.Persistence/Features/MainHierarchy/Queries/Implementations/ReadingBoundQueryService.cs) | 23 | 40 | 63 | ≈50 |
| [RegionCommandService](sandbox:/D:/Projects/Aban360/Aban360.LocationPool.Persistence/Features/MainHierarchy/Commands/Implementations/RegionCommandService.cs) | [RegionQueryService](sandbox:/D:/Projects/Aban360/Aban360.LocationPool.Persistence/Features/MainHierarchy/Queries/Implementations/RegionQueryService.cs) | 23 | 35 | 58 | ≈45 |
| [ZoneCommandService](sandbox:/D:/Projects/Aban360/Aban360.LocationPool.Persistence/Features/MainHierarchy/Commands/Implementations/ZoneCommandService.cs) | [ZoneQueryService](sandbox:/D:/Projects/Aban360/Aban360.LocationPool.Persistence/Features/MainHierarchy/Queries/Implementations/ZoneQueryService.cs) | 23 | 50 | 73 | ≈60 |

### MeterPool — 4 جفت

| CommandService | QueryService | خط Command | خط Query | جمع فعلی | برآورد نهایی |
| --- | --- | ---: | ---: | ---: | ---: |
| [CounterStateCommandService](sandbox:/D:/Projects/Aban360/Aban360.MeterPool.Persistence/Features/Management/Commands/Implementations/CounterStateCommandService.cs) | [CounterStateQueryService](sandbox:/D:/Projects/Aban360/Aban360.MeterPool.Persistence/Features/Management/Queries/Implementations/CounterStateQueryService.cs) | 23 | 23 | 46 | ≈33 |
| [ReadingConfigDefaultCommandService](sandbox:/D:/Projects/Aban360/Aban360.MeterPool.Persistence/Features/Management/Commands/Implementations/ReadingConfigDefaultCommandService.cs) | [ReadingConfigDefaultQueryService](sandbox:/D:/Projects/Aban360/Aban360.MeterPool.Persistence/Features/Management/Queries/Implementations/ReadingConfigDefaultQueryService.cs) | 23 | 23 | 46 | ≈33 |
| [ReadingPeriodCommandService](sandbox:/D:/Projects/Aban360/Aban360.MeterPool.Persistence/Features/Management/Commands/Implementations/ReadingPeriodCommandService.cs) | [ReadingPeriodQueryService](sandbox:/D:/Projects/Aban360/Aban360.MeterPool.Persistence/Features/Management/Queries/Implementations/ReadingPeriodQueryService.cs) | 23 | 28 | 51 | ≈38 |
| [ReadingPeriodTypeCommandService](sandbox:/D:/Projects/Aban360/Aban360.MeterPool.Persistence/Features/Management/Commands/Implementations/ReadingPeriodTypeCommandService.cs) | [ReadingPeriodTypeQueryService](sandbox:/D:/Projects/Aban360/Aban360.MeterPool.Persistence/Features/Management/Queries/Implementations/ReadingPeriodTypeQueryService.cs) | 23 | 23 | 46 | ≈33 |

### PaymentPool — 5 جفت

| CommandService | QueryService | خط Command | خط Query | جمع فعلی | برآورد نهایی |
| --- | --- | ---: | ---: | ---: | ---: |
| [AccountTypeCommandService](sandbox:/D:/Projects/Aban360/Aban360.PaymentPool.Persistence/Features/NegotiableInstrument/Commands/Implementations/AccountTypeCommandService.cs) | [AccountTypeQueryService](sandbox:/D:/Projects/Aban360/Aban360.PaymentPool.Persistence/Features/NegotiableInstrument/Queries/Impelmentations/AccountTypeQueryService.cs) | 23 | 23 | 46 | ≈33 |
| [BankAccountCommandService](sandbox:/D:/Projects/Aban360/Aban360.PaymentPool.Persistence/Features/NegotiableInstrument/Commands/Implementations/BankAccountCommandService.cs) | [BankAccountQueryService](sandbox:/D:/Projects/Aban360/Aban360.PaymentPool.Persistence/Features/NegotiableInstrument/Queries/Impelmentations/BankAccountQueryService.cs) | 23 | 28 | 51 | ≈38 |
| [BankFileStructureCommandService](sandbox:/D:/Projects/Aban360/Aban360.PaymentPool.Persistence/Features/NegotiableInstrument/Commands/Implementations/BankFileStructureCommandService.cs) | [BankFileStructureQueryService](sandbox:/D:/Projects/Aban360/Aban360.PaymentPool.Persistence/Features/NegotiableInstrument/Queries/Impelmentations/BankFileStructureQueryService.cs) | 23 | 23 | 46 | ≈33 |
| [CreditCommandService](sandbox:/D:/Projects/Aban360/Aban360.PaymentPool.Persistence/Features/NegotiableInstrument/Commands/Implementations/CreditCommandService.cs) | [CreditQueryService](sandbox:/D:/Projects/Aban360/Aban360.PaymentPool.Persistence/Features/NegotiableInstrument/Queries/Impelmentations/CreditQueryService.cs) | 27 | 23 | 50 | ≈37 |
| [UploaderCommandService](sandbox:/D:/Projects/Aban360/Aban360.PaymentPool.Persistence/Features/NegotiableInstrument/Commands/Implementations/UploaderCommandService.cs) | [UploaderQueryService](sandbox:/D:/Projects/Aban360/Aban360.PaymentPool.Persistence/Features/NegotiableInstrument/Queries/Impelmentations/UploaderQueryService.cs) | 23 | 23 | 46 | ≈33 |

### ReportPool — 1 جفت

| CommandService | QueryService | خط Command | خط Query | جمع فعلی | برآورد نهایی |
| --- | --- | ---: | ---: | ---: | ---: |
| [DynamicReportCommandService](sandbox:/D:/Projects/Aban360/Aban360.ReportPool.Persistence/Features/DynamicGenerator/Commands/Implementations/DynamicReportCommandService.cs) | [DynamicReportQueryService](sandbox:/D:/Projects/Aban360/Aban360.ReportPool.Persistence/Features/DynamicGenerator/Queries/Implementations/DynamicReportQueryService.cs) | 23 | 40 | 63 | ≈50 |

### UserPool — 6 جفت

| CommandService | QueryService | خط Command | خط Query | جمع فعلی | برآورد نهایی |
| --- | --- | ---: | ---: | ---: | ---: |
| [RoleCommandService](sandbox:/D:/Projects/Aban360/Aban360.UserPool.Persistence/Features/Auth/Commands/Implementations/RoleCommandService.cs) | [RoleQueryService](sandbox:/D:/Projects/Aban360/Aban360.UserPool.Persistence/Features/Auth/Queries/Implementations/RoleQueryService.cs) | 22 | 30 | 52 | ≈39 |
| [UserLoginCommandService](sandbox:/D:/Projects/Aban360/Aban360.UserPool.Persistence/Features/Auth/Commands/Implementations/UserLoginCommandService.cs) | [UserLoginQueryService](sandbox:/D:/Projects/Aban360/Aban360.UserPool.Persistence/Features/Auth/Queries/Implementations/UserLoginQueryService.cs) | 17 | 27 | 44 | ≈31 |
| [TicketAttachmentCommandService](sandbox:/D:/Projects/Aban360/Aban360.UserPool.Persistence/Features/Ticketing/Commands/Implementations/TicketAttachmentCommandService.cs) | [TicketAttachmentQueryService](sandbox:/D:/Projects/Aban360/Aban360.UserPool.Persistence/Features/Ticketing/Queries/Implementations/TicketAttachmentQueryService.cs) | 25 | 42 | 67 | ≈56 |
| [TicketCommandService](sandbox:/D:/Projects/Aban360/Aban360.UserPool.Persistence/Features/Ticketing/Commands/Implementations/TicketCommandService.cs) | [TicketQueryService](sandbox:/D:/Projects/Aban360/Aban360.UserPool.Persistence/Features/Ticketing/Queries/Implementations/TicketQueryService.cs) | 25 | 35 | 60 | ≈51 |
| [TicketMessageCommandService](sandbox:/D:/Projects/Aban360/Aban360.UserPool.Persistence/Features/Ticketing/Commands/Implementations/TicketMessageCommandService.cs) | [TicketMessageQueryService](sandbox:/D:/Projects/Aban360/Aban360.UserPool.Persistence/Features/Ticketing/Queries/Implementations/TicketMessageQueryService.cs) | 25 | 30 | 55 | ≈46 |
| [TicketRatingCommandService](sandbox:/D:/Projects/Aban360/Aban360.UserPool.Persistence/Features/Ticketing/Commands/Implementations/TicketRatingCommandService.cs) | [TicketRatingQueryService](sandbox:/D:/Projects/Aban360/Aban360.UserPool.Persistence/Features/Ticketing/Queries/Implementations/TicketRatingQueryService.cs) | 25 | 16 | 41 | ≈32 |

### WorkflowPool — 3 جفت

| CommandService | QueryService | خط Command | خط Query | جمع فعلی | برآورد نهایی |
| --- | --- | ---: | ---: | ---: | ---: |
| [StateCommandService](sandbox:/D:/Projects/Aban360/Aban360.WorkflowPool.Persistence/Features/Design/Commands/Implementations/StateCommandService.cs) | [StateQueryService](sandbox:/D:/Projects/Aban360/Aban360.WorkflowPool.Persistence/Features/Design/Queries/Implementations/StateQueryService.cs) | 27 | 23 | 50 | ≈37 |
| [WorkflowCommandService](sandbox:/D:/Projects/Aban360/Aban360.WorkflowPool.Persistence/Features/Design/Commands/Implementations/WorkflowCommandService.cs) | [WorkflowQueryService](sandbox:/D:/Projects/Aban360/Aban360.WorkflowPool.Persistence/Features/Design/Queries/Implementations/WorkflowQueryService.cs) | 23 | 42 | 65 | ≈52 |
| [WorkflowStatusCommandService](sandbox:/D:/Projects/Aban360/Aban360.WorkflowPool.Persistence/Features/Design/Commands/Implementations/WorkflowStatusCommandService.cs) | [WorkflowStatusQueryService](sandbox:/D:/Projects/Aban360/Aban360.WorkflowPool.Persistence/Features/Design/Queries/Implementations/WorkflowStatusQueryService.cs) | 23 | 27 | 50 | ≈37 |

## اولویت دوم: 6 جفت Dapper کوچک

هر جفت از IConfiguration و AbstractBaseConnection مشترک استفاده می‌کند و تراکنش به سازنده آن تزریق نمی‌شود. تراکنش‌های محلی داخل Update در Article11 و InstallationAndEquipment باید عیناً باقی بمانند. ادغام به‌خودی‌خود عملیات مستقل خواندن و نوشتن را اتمیک نمی‌کند.

| ماژول | CommandService | QueryService | خط Command | خط Query | جمع فعلی | برآورد نهایی |
| --- | --- | --- | ---: | ---: | ---: | ---: |
| CalculationPool | [Article11CommandService](sandbox:/D:/Projects/Aban360/Aban360.CalculationPool.Persistence/Features/Sale/Commands/Implementations/Article11CommandService.cs) | [Article11QueryService](sandbox:/D:/Projects/Aban360/Aban360.CalculationPool.Persistence/Features/Sale/Queries/Implementations/Article11QueryService.cs) | 60 | 96 | 156 | ≈149 |
| CalculationPool | [InstallationAndEquipmentCommandService](sandbox:/D:/Projects/Aban360/Aban360.CalculationPool.Persistence/Features/Sale/Commands/Implementations/InstallationAndEquipmentCommandService.cs) | [InstallationAndEquipmentQueryService](sandbox:/D:/Projects/Aban360/Aban360.CalculationPool.Persistence/Features/Sale/Queries/Implementations/InstallationAndEquipmentQueryService.cs) | 62 | 67 | 129 | ≈122 |
| CalculationPool | [TankerWaterDistanceTariffCommandService](sandbox:/D:/Projects/Aban360/Aban360.CalculationPool.Persistence/Features/Sale/Commands/Implementations/TankerWaterDistanceTariffCommandService.cs) | [TankerWaterDistanceTariffQueryService](sandbox:/D:/Projects/Aban360/Aban360.CalculationPool.Persistence/Features/Sale/Queries/Implementations/TankerWaterDistanceTariffQueryService.cs) | 53 | 60 | 113 | ≈106 |
| ClaimPool | [InstallationDateAssignmentCommandService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Land/Commands/Implementations/InstallationDateAssignmentCommandService.cs) | [InstallationDateAssignmentQueryService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/Land/Queries/Implementations/InstallationDateAssignmentQueryService.cs) | 28 | 25 | 53 | ≈47 |
| ClaimPool | [ClientCommentCommandService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/People/Commands/Implementation/ClientCommentCommandService.cs) | [ClientCommentQueryService](sandbox:/D:/Projects/Aban360/Aban360.ClaimPool.Persistence/Features/People/Queries/Implementations/ClientCommentQueryService.cs) | 28 | 27 | 55 | ≈48 |
| OldCalcPool | [VirtualCategoryCommandService](sandbox:/D:/Projects/Aban360/Aban360.OldCalcPool.Persistence/Features/Db70/Commands/Implementations/VirtualCategoryCommandService.cs) | [VirtualCategoryQueryService](sandbox:/D:/Projects/Aban360/Aban360.OldCalcPool.Persistence/Features/Db70/Queries/Implementations/VirtualCategoryQueryService.cs) | 45 | 35 | 80 | ≈73 |

## زیرساخت اتصال: یک فرصت مستقل

دو کلاس [Common.Db.AbstractBaseConnection](sandbox:/D:/Projects/Aban360/Aban360.Common.Db/Dapper/AbstractBaseConnection.cs) با 77 خط و [ReportPool.AbstractBaseConnection](sandbox:/D:/Projects/Aban360/Aban360.ReportPool.Persistence/Base/AbstractBaseConnection.cs) با 35 خط، در ساخت اتصال DefaultConnection و ReportConnection و منطق GetDbName تکرار دارند. نسخه Common علاوه بر آن GetMergedZoneId و ExecuteBatchAsync دارد.

با استفاده ReportPool از نسخه Common می‌توان تکرار را حذف کرد و کلاس پایه مشترک نزدیک به 77 خط باقی می‌ماند. این تغییر مستقل و با دامنه اثر گسترده‌تر است: namespace، internal/public بودن، ساخت اتصال تازه در هر دسترسی و مسیر اتصال گزارش‌ها باید بررسی شوند. جفت‌های کوچک EF برای شروع اولویت بالاتری دارند.

## 43 جفت که فعلاً پیشنهاد نمی‌شوند

40 جفت دارای Command با اتصال و تراکنش دریافتی از فراخواننده و Query با IConfiguration و اتصال مستقل هستند. ادغام ساده می‌تواند Query را خارج از تراکنش فعلی اجرا کند یا مالکیت آزادسازی اتصال را تغییر دهد؛ حتی موارد کوچک این دسته کنار گذاشته شده‌اند. سه استثنای دیگر User، UserRole و CustomerInfo هستند.

| ماژول | موجودیت؛ CommandService + QueryService | خط Command | خط Query | جمع فعلی | دلیل کنار گذاشتن |
| --- | --- | ---: | ---: | ---: | --- |
| CalculationPool | `CollectBillsDetail` | 49 | 97 | 146 | اتصال و تراکنش Command با اتصال مستقل Query متفاوت است |
| CalculationPool | `MeterFlow` | 113 | 229 | 342 | اتصال و تراکنش Command با اتصال مستقل Query متفاوت است؛ حجم زیاد |
| CalculationPool | `MeterReadingDetail` | 757 | 948 | 1705 | اتصال و تراکنش Command با اتصال مستقل Query متفاوت است؛ حجم زیاد |
| CalculationPool | `SmsFlow` | 79 | 77 | 156 | اتصال و تراکنش Command با اتصال مستقل Query متفاوت است |
| CalculationPool | `SmsStateTemplate` | 61 | 182 | 243 | اتصال و تراکنش Command با اتصال مستقل Query متفاوت است |
| CalculationPool | `Tanker` | 52 | 106 | 158 | اتصال و تراکنش Command با اتصال مستقل Query متفاوت است |
| CalculationPool | `OtherExpensesItems` | 64 | 95 | 159 | اتصال و تراکنش Command با اتصال مستقل Query متفاوت است |
| CalculationPool | `VosolEn` | 70 | 73 | 143 | اتصال و تراکنش Command با اتصال مستقل Query متفاوت است |
| ClaimPool | `ConCompany` | 158 | 198 | 356 | اتصال و تراکنش Command با اتصال مستقل Query متفاوت است؛ حجم زیاد |
| ClaimPool | `ConnectDisconnect` | 84 | 292 | 376 | اتصال و تراکنش Command با اتصال مستقل Query متفاوت است؛ حجم زیاد |
| ClaimPool | `UsageGroup1` | 80 | 55 | 135 | اتصال و تراکنش Command با اتصال مستقل Query متفاوت است |
| ClaimPool | `UsageGroup2` | 89 | 86 | 175 | اتصال و تراکنش Command با اتصال مستقل Query متفاوت است |
| ClaimPool | `UsageGroup3` | 110 | 129 | 239 | اتصال و تراکنش Command با اتصال مستقل Query متفاوت است |
| ClaimPool | `CustomerInfo` | 125 | 75 | 200 | نوشتن روی members دیتابیس منطقه و خواندن از CustomerWarehouse.Clients؛ دو مدل داده با همگام‌سازی مستقل |
| ClaimPool | `Examination` | 99 | 292 | 391 | اتصال و تراکنش Command با اتصال مستقل Query متفاوت است؛ حجم زیاد |
| ClaimPool | `Ghest` | 79 | 44 | 123 | اتصال و تراکنش Command با اتصال مستقل Query متفاوت است |
| ClaimPool | `Kart` | 162 | 273 | 435 | اتصال و تراکنش Command با اتصال مستقل Query متفاوت است؛ حجم زیاد |
| ClaimPool | `Moshtrak` | 451 | 669 | 1120 | اتصال و تراکنش Command با اتصال مستقل Query متفاوت است؛ حجم زیاد |
| ClaimPool | `Mother` | 99 | 53 | 152 | اتصال و تراکنش Command با اتصال مستقل Query متفاوت است |
| ClaimPool | `Tracking` | 135 | 523 | 658 | اتصال و تراکنش Command با اتصال مستقل Query متفاوت است؛ حجم زیاد |
| ClaimPool | `Queue` | 33 | 57 | 90 | اتصال و تراکنش Command با اتصال مستقل Query متفاوت است |
| CommunicationPool | `SmsDraft` | 144 | 60 | 204 | اتصال و تراکنش Command با اتصال مستقل Query متفاوت است |
| MeterPool | `Block` | 64 | 53 | 117 | اتصال و تراکنش Command با اتصال مستقل Query متفاوت است |
| MeterPool | `Bound` | 100 | 65 | 165 | اتصال و تراکنش Command با اتصال مستقل Query متفاوت است |
| MeterPool | `Detail` | 168 | 515 | 683 | اتصال و تراکنش Command با اتصال مستقل Query متفاوت است؛ حجم زیاد |
| MeterPool | `Tracking` | 194 | 292 | 486 | اتصال و تراکنش Command با اتصال مستقل Query متفاوت است؛ حجم زیاد |
| MeterPool | `MeterApkInfo` | 82 | 133 | 215 | اتصال و تراکنش Command با اتصال مستقل Query متفاوت است |
| OldCalcPool | `BillReturnCause` | 55 | 107 | 162 | اتصال و تراکنش Command با اتصال مستقل Query متفاوت است |
| OldCalcPool | `BedBes` | 342 | 1095 | 1437 | اتصال و تراکنش Command با اتصال مستقل Query متفاوت است؛ حجم زیاد |
| OldCalcPool | `GhestAb` | 72 | 166 | 238 | اتصال و تراکنش Command با اتصال مستقل Query متفاوت است |
| OldCalcPool | `Taviz` | 36 | 41 | 77 | اتصال و تراکنش Command با اتصال مستقل Query متفاوت است |
| OldCalcPool | `Abonman` | 53 | 53 | 106 | اتصال و تراکنش Command با اتصال مستقل Query متفاوت است |
| OldCalcPool | `S` | 92 | 131 | 223 | اتصال و تراکنش Command با اتصال مستقل Query متفاوت است |
| OldCalcPool | `ZaribC` | 76 | 104 | 180 | اتصال و تراکنش Command با اتصال مستقل Query متفاوت است |
| OldCalcPool | `AutoBack` | 94 | 330 | 424 | اتصال و تراکنش Command با اتصال مستقل Query متفاوت است؛ حجم زیاد |
| OldCalcPool | `Repair` | 183 | 373 | 556 | اتصال و تراکنش Command با اتصال مستقل Query متفاوت است؛ حجم زیاد |
| ReportPool | `BillIdTag` | 104 | 84 | 188 | اتصال و تراکنش Command با اتصال مستقل Query متفاوت است |
| ReportPool | `MainTagGroup` | 59 | 39 | 98 | اتصال و تراکنش Command با اتصال مستقل Query متفاوت است |
| ReportPool | `Tag` | 73 | 131 | 204 | اتصال و تراکنش Command با اتصال مستقل Query متفاوت است |
| ReportPool | `TagGroup` | 56 | 41 | 97 | اتصال و تراکنش Command با اتصال مستقل Query متفاوت است |
| UserPool | `ExaminerOff` | 56 | 72 | 128 | اتصال و تراکنش Command با اتصال مستقل Query متفاوت است |
| UserPool | `User` | 23 | 279 | 302 | Query به‌تنهایی 279 خط است؛ مجموع 302 خط و مسئولیت‌های متعدد احراز هویت |
| UserPool | `UserRole` | 30 | 127 | 157 | Query ترکیبی EF/Dapper با IConfiguration اضافی؛ سود ادغام محدود و نیازمند مرور جداگانه |

سرویس‌های گزارش‌گیری صرفاً به‌دلیل اشتراک پوشه، SQL یا Connection در یک کلاس جمع نشده‌اند. ادغام MeterReadingDetail به 1705 خط، BedBes به 1437 خط و Moshtrak به 1120 خط می‌رسد؛ این‌ها از فهرست ادغام خارج‌اند. شباهت سرویس‌های بدون جفت هم‌نام نیز دلیل کافی برای توصیه ادغام شناخته نشد.

## شرط اجرای هر ادغام در آینده

- حفظ دو interface فعلی و ادغام implementation کافی است؛ حذف interfaceها برای این هدف لازم نیست.
- ثبت Persistence فعلی با AsImplementedInterfaces و Scoped است. پیاده‌سازی دو interface قابل کشف است، اما اگر هر دو قرارداد باید دقیقاً یک instance داشته باشند، نگاشت صریح به instance مشترک Scoped و تست هویت لازم است؛ Scoped بودن به‌تنهایی این را تضمین نمی‌کند.
- UnitOfWork و DbContext ماژول، SaveChanges، tracking، ترتیب await، متن SQL، پارامترها، استثناها و مالکیت تراکنش و اتصال باید حفظ شوند.
- هیچ connection زنده‌ای در فیلد مشترک نگهداری نشود؛ رفتار ساخت و آزادسازی اتصال مطابق مسیر قبلی باقی بماند.
- پس از هر دسته کوچک: کامپایل، بررسی DI، مقایسه رفتار و تست متناسب با مسیرهای خواندن، نوشتن و rollback؛ تغییر حساس مالی باید روی دیتابیس آزمایشی نیز اعتبارسنجی شود.

در این تسک فقط گزارش ایجاد شد؛ Build یا تست رفتار برنامه و اتصال به دیتابیس اجرا نشد و هیچ کد production تغییر نکرد.
