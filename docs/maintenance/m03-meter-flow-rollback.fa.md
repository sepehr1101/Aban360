# بررسی دقیق M03: حذف flow پس از rollback

تاریخ بررسی: ۲۰۲۶/۱۱/۰۵  
دامنهٔ مجاز: بررسی و پیشنهاد؛ کد این مسیر تغییر نکرده است. بررسی با خواندن کد، مصرف‌کننده‌ها و تاریخچهٔ Git انجام شد؛ هیچ دستور SQL یا import واقعی اجرا نشد.

تصمیم کاربر: M03 در برنامهٔ اصلاح و فهرست موارد باز باقی بماند، اما فعلاً به‌دلیل ریسک تغییر نکند. پیشنهادهای زیر صرفاً نتیجهٔ بررسی‌اند؛ اجرای اصلاح نیازمند تأیید تازهٔ کاربر است.

## نتیجه

`await meterFlowCommand.Delete(meterFlowDeleteDto)` برای حذف جزئیات درج‌شده در همان تراکنش لازم نیست؛ rollback آن جزئیات را برمی‌گرداند. اما flow اولیه در تراکنش قبلی commit شده و rollback مرحلهٔ دوم آن را برنمی‌گرداند. این خط ظاهراً برای غیرفعال‌کردن همان flow اولیهٔ باقی‌مانده نوشته شده است. هدف نویسنده به‌طور صریح در تاریخچه مستند نشده و بخش «هدف» استنباط از رفتار کد است.

بااین‌حال، اجرای فعلی درست نیست: سرویس `meterFlowCommand` همان تراکنش rollback‌شده را در فیلد نگه داشته و به دستور SQL می‌دهد. افزون بر این، `Delete` حذف فیزیکی نیست و کنترل تکراری‌بودن فایل هم flow حذف‌شده را نادیده نمی‌گیرد؛ پس این خط به‌تنهایی اجازهٔ بارگذاری مجدد فایل را نمی‌دهد.

## ترتیب واقعی عملیات

| مرحله | عملیات | وضعیت پس از موفقیت | اثر rollback مرحلهٔ دوم |
|---|---|---|---|
| پیش از T1 | بررسی نام تکراری و در مسیر DBF/Excel ذخیرهٔ فایل روی دیسک | فایل ممکن است روی دیسک باشد | هیچ؛ فایل خارج از تراکنش SQL است |
| T1 | درج flow با مرحلهٔ `Imported`، تعیین `FirstFlowId` و خواندن دادهٔ مشترک | flow اولیه commit شده است | هیچ؛ T1 قبلاً پایان یافته است |
| میان T1 و T2 | تکمیل داده، نگاشت قرائت و محاسبهٔ مبالغ | هنوز جزئیات این مرحله ثبت نشده‌اند | این خطاها اصولاً وارد catch مورد بحث نمی‌شوند |
| T2 | درج جزئیات با SqlBulkCopy، غیرفعال‌کردن flow اولیه، درج flow مرحلهٔ `Calculated` و ثبت لاگ | همه با یک تراکنش commit می‌شوند | جزئیات، تغییر وضعیت اولیه، flow محاسبه‌شده و لاگ همین تراکنش برمی‌گردند |
| catch در T2 | rollback، سپس فراخوانی `Delete` برای flow اولیه | قرار است flow باقی‌مانده غیرفعال شود | فراخوانی حذف هنوز تراکنش پایان‌یافتهٔ T2 را استفاده می‌کند |

شواهد:

- ترتیب ورود DBF: [MeterReadingFileCreateHandler](../../Aban360.CalculationPool.Application/Features/MeterReading/Handlers/Commands/Creata/Implementations/MeterReadingFileCreateHandler.cs)، متد `Handle` و `GetMeterReadingDetails`.
- ورود Excel: [MeterReadingExcelFileCreateHandler](../../Aban360.CalculationPool.Application/Features/MeterReading/Handlers/Commands/Creata/Implementations/MeterReadingExcelFileCreateHandler.cs)، همان ترتیب آماده‌سازی، محاسبه و `ExecSql` را دارد.
- T1: [MeterReadingCreateBaseHandler](../../Aban360.CalculationPool.Application/Features/MeterReading/Handlers/Commands/Creata/Implementations/MeterReadingCreateBaseHandler.cs)، متد `GetCustomerInfoAndFirstFlowId`، خطوط ۲۳۷ تا ۲۹۷؛ commit در خط ۲۶۷.
- T2 و catch: همان فایل، متد `ExecSql`، خطوط ۳۶۷ تا ۴۰۵؛ تغییر مرحله در `MeterFlowCommands`، خطوط ۴۲۵ تا ۴۳۴.
- جزئیات در همان تراکنش: [MeterReadingDetailCommandService](../../Aban360.CalculationPool.Persistence/Features/MeterReading/Commands/Implementations/MeterReadingDetailCommandService.cs)، متد `Insert(IEnumerable<...>)`، خط ۴۱؛ SqlBulkCopy تراکنش دریافتی را می‌گیرد.
- مسیر بدون قرائت نیز flow اولیه را در تراکنش قبلی ثبت می‌کند: [MeterReadingNonReadCreateHandler](../../Aban360.CalculationPool.Application/Features/MeterReading/Handlers/Commands/Creata/Implementations/MeterReadingNonReadCreateHandler.cs)، متد `GetMeterReadingDetails`.
- مسیر جدید Excel چندمنطقه‌ای از `ExecSql` مورد بحث استفاده نمی‌کند و ذخیرهٔ جداگانهٔ `SaveToAtlas` دارد؛ نتیجهٔ این بررسی نباید خودکار به آن تعمیم داده شود.

## Delete دقیقاً چه می‌کند؟

در [MeterFlowCommandService](../../Aban360.CalculationPool.Persistence/Features/MeterReading/Commands/Implementations/MeterFlowCommandService.cs)، متد `Delete`، خط ۷۰، در نهایت این تغییر را انجام می‌دهد:

```sql
Update Atlas.dbo.MeterFlow
Set RemovedByUserId = @RemovedByUserId,
    RemovedDateTime = @RemovedDateTime
Where Id = @Id
```

بنابراین نام `Delete` به معنی «حذف منطقی/غیرفعال‌سازی» است. همان دو ستون در انتقال موفق از Imported به Calculated نیز برای بستن مرحلهٔ قبل استفاده می‌شوند؛ به‌تنهایی وضعیت «خطای import» را از «پایان موفق مرحله» متمایز نمی‌کنند.

در کارتابل عمومی، فقط مرحله‌های Calculated و ConsumptionChecked با `RemovedDateTime` و `RemovedByUserId` خالی نمایش داده می‌شوند. بنابراین باقی‌ماندن Imported بعد از rollback لزوماً آن را به‌عنوان کار محاسبه‌شدهٔ آماده در کارتابل عمومی نشان نمی‌دهد. سایر queryها و اعتبارسنجی‌های flow از همین فیلدهای حذف استفاده می‌کنند، پس غیرفعال‌سازی می‌تواند اثر داشته باشد، ولی اثرش باید براساس مصرف‌کنندهٔ موردنظر تعیین شود.

## آیا مشکل فایل تکراری را حل می‌کند؟

خیر. `CheckDuplicateFile` در کلاس پایه، از `GetInsertDateTime(fileName)` استفاده می‌کند. query مربوط در [MeterFlowQueryService](../../Aban360.CalculationPool.Persistence/Features/MeterReading/Queries/Implementations/MeterFlowQueryService.cs)، متد `GetValidationByFileNameQuery`، خط ۱۱۴، صرفاً `FileName=@fileName` را بررسی می‌کند؛ شرطی برای `RemovedDateTime`، مرحله یا موفقیت import ندارد.

پس حتی اگر حذف منطقی در یک تراکنش مستقل موفق شود، رکورد همچنان وجود دارد و بارگذاری دوبارهٔ همان نام رد می‌شود. حذف فایل دیسک هم در این catch اجرا نمی‌شود؛ فقط یک خط کامنت‌شده برای آن وجود دارد.

## تاریخچه چه می‌گوید؟

- این توالی در commit `c01466a0bdf1ef7a5f085493d5f87931f9511fbc` با عنوان `MeterReadingByExcelfile` وارد همین فایل شده است؛ همان نسخه نیز flow اولیه را پیش‌تر commit می‌کند و سپس در catch مرحلهٔ ثبت جزئیات، rollback و Delete دارد.
- `git blame` فعلی خطوط catch را به commit `e6a7a490f9` با عنوان `MeterReadingExcel,DBF,NonRead` نسبت می‌دهد؛ عنوان یا توضیح مستقلی که هدف دقیق Delete را تصریح کند پیدا نشد.
- بنابراین می‌توان گفت این خط از طراحی دومرحله‌ای import آمده است؛ نسبت‌دادن آن به حل یک خطای عملیاتی مشخص یا رفع تکراری‌بودن فایل، پشتوانهٔ تاریخی پیدا نکرد.

## آیا rollback به‌تنهایی کافی است؟

- **برای جلوگیری از ثبت ناقص جزئیات T2، در صورتی که rollback موفق باشد: بله.** همان تراکنش برای درج جزئیات، تغییر flow و ثبت لاگ استفاده می‌شود.
- **برای برگرداندن کل import به حالت قبل از شروع: خیر.** flow اولیه و فایل احتمالی دیسک بیرون از T2 باقی می‌مانند.
- **برای آزادشدن نام فایل جهت تلاش دوباره: خیر.** هم با rollback تنها و هم با حذف منطقی، کنترل فعلی نام فایل آن را تکراری می‌بیند.
- **برای پوشش همهٔ شکست‌ها: خیر.** خطاهای میان commit شدن T1 و ورود به T2، این catch را اجرا نمی‌کنند. خطای rollback نیز پیش از رسیدن به Delete می‌تواند خطای اصلی را بپوشاند.

## پیشنهاد، بدون اجرای اصلاح

دو رفتار محصول باید از هم تفکیک شوند: حفظ سابقهٔ import ناموفق و مجازبودن تلاش دوباره با همان نام.

اگر ثبت سابقهٔ ناموفق لازم است، بهتر است flow اولیه به‌طور صریح «ناموفق» شود و جبران در session مستقل، با حفظ خطای اصلی و قابلیت اجرای دوباره انجام شود. دامنهٔ رسیدگی باید شکست محاسبه و نگاشت میان T1 و T2 را هم پوشش دهد. صرف تغییر `throw ex` یا جابه‌جایی Delete به قبل از rollback مشکل را حل نمی‌کند؛ تغییر داخل همان تراکنش نیز rollback خواهد شد.

اگر نیاز محصول این است که import ناموفق هیچ flow اولیه‌ای باقی نگذارد، ایجاد flow و جزئیات باید به یک تراکنش نهایی منتقل شود و داده‌های آماده‌سازی پیش از آن بدون نوشتن flow جمع‌آوری شوند. این تغییر چند مسیر ورود و نحوهٔ تعیین `FlowImportedId` را درگیر می‌کند و از حذف یک خط بزرگ‌تر است.

با طراحی فعلی، حذف بی‌قید خط Delete به‌عنوان «کل اصلاح» توصیه نمی‌شود. ابتدا سیاست flow ناموفق و تلاش دوباره مشخص شود؛ سپس پیاده‌سازی جبران یا مرز تراکنش مطابق آن تغییر کند. در این نوبت هیچ‌کدام اعمال نشده‌اند.
