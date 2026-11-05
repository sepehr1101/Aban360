# تصاویر اپلیکیشن قرائت کنتور

## بارگذاری

```http
POST {apiBaseUrl}/v1/open-km/add-reading-image
Authorization: Bearer {accessToken}
Content-Type: multipart/form-data
```

فقط دو فیلد فرم لازم است:

| فیلد | نوع | توضیح |
| --- | --- | --- |
| `billId` | string | شناسه قبض |
| `file` | IFormFile | تصویر JPEG یا PNG |

نمونه فرانت:

```javascript
const form = new FormData();
form.append("billId", billId);
form.append("file", imageFile);

const response = await fetch(`${apiBaseUrl}/v1/open-km/add-reading-image`, {
  method: "POST",
  headers: { Authorization: `Bearer ${accessToken}` },
  body: form
});
```

هنگام استفاده از FormData، هدر Content-Type را دستی تنظیم نکنید تا boundary به‌درستی ساخته شود. نام کاربر، تاریخ و نام فایل از کلاینت گرفته نمی‌شوند.

پاسخ موفق در envelope معمول API و از نوع `AddFileDto` است؛ `data.uuid` شناسه سند و `data.size` اندازه پاسخ‌داده‌شده توسط بایگانی است.

## اعتبارسنجی و ذخیره

- ساختار، طول و رقم کنترل شناسه قبض با `TransactionIdGenerator.IsBillIdStructureValid` موجود بررسی می‌شود.
- وجود شناسه قبض در اطلاعات مشترکین و دسترسی کاربر به منطقه همان مشترک نیز بررسی می‌شود.
- تصویر خالی یا بزرگ‌تر از ۲۰ مگابایت، محتوای غیرتصویری و فرمت غیر از JPEG/PNG پذیرفته نمی‌شود.
- ابعاد تصویر حداقل ۱۶۰ در ۱۲۰ و حداکثر ۲۵ مگاپیکسل است. تصویر پذیرفته‌شده کوچک‌سازی نمی‌شود.
- جهت EXIF عکس تلفن همراه پیش از درج واترمارک اصلاح می‌شود.
- پوشه قبض و سپس زیرپوشه `reading`، فقط در صورت نبودن ایجاد می‌شوند؛ از handler موجود ساخت پوشه استفاده می‌شود.
- فایل زیر مسیر نسبی `<billId>/reading/<fileName>` در بایگانی ذخیره می‌شود. مسیر پایه همان `OpenKm.BaseDirectoryPath` فعلی است.
- تصویر خروجی JPEG با کیفیت ۹۰ است. فایل اصلی در ریشه قبض ذخیره نمی‌شود و فایل‌های قبلی جابه‌جا یا بازنویسی نمی‌شوند.
- هر بارگذاری نام یکتا دارد. نام فایل ارسالی کلاینت در ساخت مسیر استفاده نمی‌شود.

قالب نام فایل:

```text
{yyyy-MM-dd شمسی}_{HH-mm-ss-fff}_{billId}_{guid}.jpg
```

تاریخ و زمان، زمان محلی سرور هنگام بارگذاری‌اند، نه تاریخ EXIF دوربین. پسوند GUID از تداخل نام دو بارگذاری در یک میلی‌ثانیه جلوگیری می‌کند.

واترمارک کوچک در پایینِ سمت راست تصویر شامل این موارد است:

```text
تاریخ: {تاریخ شمسی و ساعت بارگذاری}
شناسه قبض: {billId}
کاربر: {نام کامل کاربر احراز هویت‌شده}
```

اگر نام کامل کاربر خالی باشد، نام کاربری استفاده می‌شود. تبدیل تاریخ در نام فایل و واترمارک فقط با DNT موجود پروژه انجام می‌شود و برای یک فایل، زمان یکسانی مبناست.

ثبت عملیات پس از ذخیره انجام می‌شود. اگر خود ذخیره شکست بخورد، پاسخ موفق داده نمی‌شود. اگر فایل ذخیره شده باشد اما ثبت گزارش عملیات خطا بدهد، خطای ثبت در logger درج می‌شود و موفقیت آپلود پنهان نمی‌شود تا کلاینت به‌اشتباه همان فایل را دوباره ارسال نکند.

## نمایش تصاویر قرائت

در `GetDirectoryTree2Controller`:

```http
GET {apiBaseUrl}/v1/open-km/reading-directory-tree2?billId={billId}
Authorization: Bearer {accessToken}
```

شناسه قبض و دسترسی منطقه در این مسیر نیز بررسی می‌شود. فقط اسناد پوشه `<billId>/reading` خوانده می‌شوند. اگر پوشه هنوز وجود نداشته باشد، `data.document` آرایه خالی است؛ درخواست GET پوشه جدید ایجاد نمی‌کند.

پاسخ از نوع `FileListWithTitlesResponse` است. تمام مشخصات هر سند حفظ و `fileTitle` برابر «تصویر قرائت کنتور» قرار داده می‌شود. چون این پوشه مخصوص تصاویر قرائت است و API آپلود فقط دو ورودی دارد، documentTypeId فرضی یا مقدار تازه‌ای در جدول انواع سند ایجاد نشده است و خواندن metadata تک‌تک تصاویر نیز لازم نیست.

برای خود تصویر، همان مسیر موجود استفاده شود:

```http
GET {apiBaseUrl}/v1/open-km/document-binary?documentId={uuid}&isThumbnail=false
```

## سازگاری و اجرا

- APIها و handlerهای قبلی آپلود و فهرست بایگانی تغییر رفتاری ندارند. در کنترلرها فقط action جدید اضافه شده و سرویس جدید با `[FromServices]` در همان action دریافت می‌شود؛ سازنده کنترلرهای قبلی تغییر نکرده است.
- هیچ PackageReference یا کتابخانه جدیدی اضافه نشده و اسکریپت دیتابیس جدید لازم نیست.
- واترمارک از `System.Drawing.Common` موجود پروژه استفاده می‌کند. سرور Windows بودن توسط کاربر تأیید شده است؛ محدودیت کتابخانه در .NET جدید در [مستندات Microsoft](https://learn.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/6.0/system-drawing-common-windows-only) توضیح داده شده است.
- آزمون تصویر واقعی روی Windows اجرا می‌شود؛ ارتباط OpenKM و اطلاعات مشترکین در آزمون شبیه‌سازی شده است. درخواست واقعی به بایگانی اصلی یا بارگذاری واقعی روی آن در این بررسی انجام نمی‌شود.

## آزمون‌ها

```powershell
dotnet run --project tests/Aban360.ReadingImages.SmokeTests/Aban360.ReadingImages.SmokeTests.csproj
dotnet run --project tests/Aban360.ArchiveDirectory.SmokeTests/Aban360.ArchiveDirectory.SmokeTests.csproj
dotnet run --project tests/Aban360.OpenKm.SmokeTests/Aban360.OpenKm.SmokeTests.csproj
```

در آزمون جدید، صحت شناسه و رقم کنترل، وجود مشترک، دسترسی منطقه، رد تصویر نامعتبر قبل از نوشتن در بایگانی، ساخت پوشه‌ها، استفاده مجدد از پوشه، ساخت JPEG واقعی با واترمارک، نام فایل، نمایش فقط پوشه reading، UUID لازم برای binary، ثبت عملیات و حفظ سازنده قبلی کنترلر بررسی می‌شوند.

نتیجه اجرای بررسی‌ها روی Windows: ۲۵ بررسی بخش تصاویر قرائت، ۴۸ بررسی directory-tree2 و ۴۲ بررسی OpenKM موفق بودند. ثبت سرویس‌ها با تنظیمات واقعی پروژه و اصلاح جهت تصویر موبایل بر اساس EXIF نیز بررسی شدند. پروژه API و بخش‌های تغییرکرده بدون خطای کامپایل ساخته شدند.
