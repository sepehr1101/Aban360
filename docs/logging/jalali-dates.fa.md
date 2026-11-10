# تغییرات تاریخ در خروجی لاگ‌ها

فیلدهای میلادی قبلی حفظ شده‌اند. معادل شمسی هر فیلد با همان نام و پسوند `Jalali` به خروجی JSON اضافه شده است:

| مسیر | فیلد قبلی | فیلد جدید |
| --- | --- | --- |
| `/v1/logging/http-requests/slowest` | `timeStamp` | `timeStampJalali` |
| `/v1/logging/http-requests/summary`، `users` و `routes` | `timeStamp` (بدون مقدار در گزارش تجمیعی) | `timeStampJalali` (بدون مقدار) |
| `/v1/logging/errors/{id}` | `timeStamp` | `timeStampJalali` |
| `/v1/op-log/get` | `registerDateTime` | `registerDateTimeJalali` |

قالب مقدار جدید `yyyy/MM/dd HH:mm:ss` است؛ برای مثال `1405/08/17 17:45:30`. ساعت به‌صورت ۲۴ساعته است. تبدیل تقویم، منطقهٔ زمانی را تغییر نمی‌دهد. برای زمان ناموجود یا خارج از محدودهٔ تقویم شمسی مقدار `null` برمی‌گردد.

در لاگ عمومی (`/v1/logging/get`) و فهرست خطاها (`/v1/logging/errors`) تاریخ از قبل در `dateJalali` و ساعت در `time` برمی‌گردد. لاگ عملیات همچنان `registerDateJalali` قبلی را نیز دارد. لاگ‌های ارزیابی و اپلیکیشن کنتور از قبل `insertDateTimeJalali` دارند. فیلدهای شمسی موجود تغییر نکرده‌اند. مدت‌زمان‌های پردازش مانند `elapsedMilliseconds` تاریخ نیستند و تبدیل نمی‌شوند؛ متن خام لاگ‌ها و فایل‌های دانلودی نیز بازنویسی نمی‌شوند.
