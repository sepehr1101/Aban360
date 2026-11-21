# ادغام ۲۰ دستهٔ غیرگزارشی

در این مرحله ۷۵ Handler و قراردادهای آن‌ها به ۲۰ سرویس کاربردی تبدیل شدند. مجموع امضاهای عمومی با احتساب overload و متد dictionary برابر ۷۷ است. هیچ مسیر excel یا sti در فهرست انتخاب نشده است.

| ردیف | دسته | Handler قبلی | سرویس |
| --- | --- | ---: | --- |
| 21 | `Estate` | 5 | `IEstateService` |
| 22 | `UsageGroup1` | 6 | `IUsageGroup1Service` |
| 23 | `UsageGroup2` | 6 | `IUsageGroup2Service` |
| 24 | `UsageGroup3` | 5 | `IUsageGroup3Service` |
| 25 | `WaterMeter` | 5 | `IWaterMeterService` |
| 26 | `ConCompany` | 7 | `IConCompanyService` |
| 27 | `ConCompanyPersonnel` | 5 | `IConCompanyPersonnelService` |
| 28 | `ClientComment` | 2 | `IClientCommentService` |
| 29 | `CustomerInfo` | 2 | `ICustomerInfoService` |
| 30 | `SubscriptionAssignment` | 3 | `ISubscriptionAssignmentService` |
| 31 | `InstallationDateAssignment` | 2 | `IInstallationDateAssignmentService` |
| 32 | `RequestSubscription` | 2 | `IRequestSubscriptionService` |
| 33 | `BillReturnCause` | 5 | `IBillReturnCauseService` |
| 34 | `CounterStateLookup` | 2 | `ICounterStateLookupService` |
| 35 | `Offering` | 2 | `IOfferingService` |
| 36 | `MeterChange` | 2 | `IMeterChangeService` |
| 37 | `UserManagement` | 7 | `IUserManagementService` |
| 38 | `UserPassword` | 2 | `IUserPasswordService` |
| 39 | `UserLock` | 2 | `IUserLockService` |
| 40 | `UserToken` | 3 | `IUserTokenService` |

## حفظ رفتار

- متن SQL، پارامترها، شرط‌ها، اعتبارسنجی، ترتیب await و مرز تراکنش‌ها با نسخهٔ قبل مقایسه شده‌اند.
- Route، متد HTTP، مجوزها و metadata اکشن‌ها حفظ شده‌اند. چهار کنترلر RequestSubscription، CustomerInfo، CounterState و ServiceLink به Route بالای کلاس و مسیر نسبی اکشن تبدیل شدند؛ URL نهایی ثابت است.
- در CounterState، Handler هم‌نام ReportPool از Handler انتخاب‌شدهٔ OldCalcPool تفکیک شد؛ اتصال ReportPool حفظ شده است.
- نوع‌های داخلی ایجاد و ویرایش کاربر همراه سرویس منتقل شده‌اند؛ بدنهٔ کلاس پایهٔ مشترک تغییر نکرده و فقط import نوع‌ها به سرویس جدید اشاره می‌کند.
- TrackingFlowGetHandler و منطق ارسال پیامک تغییر نکرده‌اند.

## اعتبارسنجی

- API و Bridge: build بدون خطا؛ هشدارهای کامپایل پروژه همچنان وجود دارند.
- رگرسیون مسیرها: ۳۸۸۰ بررسی؛ ۹۰ کنترلر، ۴۵۰ اکشن، ۸۰۸ ترکیب مسیر و متد HTTP.
- CriticalControllers: ۱۶۳۹؛ ControllerBatch110: ۲۴۲۴؛ ControllerBatch132: ۱۰۷۹؛ ControllerBatch151: ۵۵۲ بررسی موفق.
- اختصاصی این مرحله: ۲۰۷۹ بررسی موفق؛ ۷۷ امضای عمومی، ۵۳۹ سناریوی وابستگی و ۳۲۵ سناریوی SQL برای ۲۲ عملیات تراکنشی. هیچ مسیر موفقی صرفاً با مقایسهٔ دو خطای fixture پوشش داده نشده است.
- تطبیق ساختاری این مرحله: ۱۴۰۹ بررسی؛ ۱۴۲ متد کامل، ۱۰۸ متد مصرف‌کننده، ۳۰ نسخهٔ SQL آزمایشی و کنترل مستقل EstateInfoController که انتخاب نشده بود. حذف هر ۷۵ قرارداد قبلی و نبود مسیر excel/sti و ~ نیز بررسی می‌شود.
- دو مرحلهٔ قبلی دوباره اجرا شدند: Critical10 برابر ۱۹۶۳ بررسی رفتاری و ۶۹۸ بررسی ساختاری؛ Auxiliary10 برابر ۱۶۰۱ بررسی رفتاری و ۴۹۰ بررسی ساختاری.
- جمع کل بررسی‌های موفق این نوبت: ۱۷۸۱۴.
- هش رمز و توکن، دو زمان انقضای متفاوت توکن، refresh نامعتبر، حالت پیش‌نمایش بدون نوشتن SQL، شکست هر فرمان، لغو، خطای open/begin/commit، disposal و عدم ادامه پس از خطا کنترل شدند.

نسخه‌های قبل فقط در پروژهٔ تست نگهداری می‌شوند. تست ساختاری، بدنهٔ Handlerها را با Git HEAD و مصرف‌کننده‌ها را با تصویر بلافاصله پیش از همین مرحله تطبیق می‌دهد؛ Login قبلاً تغییرات تأییدشده داشت و مبنا عمداً HEAD نیست.

## محدودیت‌ها و ایرادهای قبلی

- تست‌ها بدون SQL Server واقعی، ارسال پیامک، یا اجرای job واقعی انجام می‌شوند. provider ضبط‌کننده، فرمان‌های واقعی Dapper را ثبت می‌کند؛ این تست جایگزین تست یکپارچه روی دیتابیس آزمایشی نیست.
- در ChangePasswordValidate، شیء ChangePasswordValidationException ساخته می‌شود ولی throw نمی‌شود؛ در نتیجه عدم تطابق رمز و تکرار آن در کد قبلی نیز جلوی تغییر رمز را نمی‌گیرد. این رفتار در ادغام اصلاح نشده و نیازمند تغییر مستقل است.
- SubscriptionAssignment.UpdateAsync در کد قبلی نیز سه بار getterِ _sqlReportConnection را می‌خواند؛ getter هر بار SqlConnection تازه می‌سازد و دو command service به connection متفاوت از transaction متصل می‌شوند. این ایراد قبلی در ادغام حفظ شده است. تست سخت‌گیرانه با connection تازه در هر getter، ناسازگاری connection/transaction را در هر دو نسخه بازتولید کرد؛ سناریوی ضبط فرمان با connection مشترک به معنی تأیید اجرای واقعی این مسیر نیست.
- مجموعهٔ قدیمی MergedControllers در اجرای عمومی، mismatch قبلی fixture مسیر CollectBills/get را دارد؛ نتیجهٔ تست اختصاصی این مرحله و مجموعه‌های مستقل بالا جدا گزارش می‌شود.

فرمان‌های بازاجرا:

```powershell
dotnet run --project tests/Aban360.CriticalServiceStructure.RegressionTests -- --management20 --verify-head
dotnet run --project tests/Aban360.MergedControllers.SmokeTests -- --management20
```
