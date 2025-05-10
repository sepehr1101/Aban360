USE [Aban360]
GO
SET IDENTITY_INSERT [UserPool].[App] ON 
GO
INSERT [UserPool].[App] ( [Title], [Style], [InMenu], [LogicalOrder], [IsActive]) VALUES ( N'امنیت', N'', 1, 1, 1)
GO
INSERT [UserPool].[App] ( [Title], [Style], [InMenu], [LogicalOrder], [IsActive]) VALUES ( N'اطلاعات مشترکین', N'', 1, 1, 1)
GO
INSERT [UserPool].[App] ( [Title], [Style], [InMenu], [LogicalOrder], [IsActive]) VALUES ( N'محاسبات', N'', 1, 1, 1)
GO
INSERT [UserPool].[App] ( [Title], [Style], [InMenu], [LogicalOrder], [IsActive]) VALUES ( N'بایگانی الکترونیک', N'', 1, 1, 1)
GO
INSERT [UserPool].[App] ( [Title], [Style], [InMenu], [LogicalOrder], [IsActive]) VALUES ( N'قرائت کنتور', N'', 1, 1, 1)
GO
INSERT [UserPool].[App] ( [Title], [Style], [InMenu], [LogicalOrder], [IsActive]) VALUES ( N'نصب', N'', 1, 1, 1)
GO
INSERT [UserPool].[App] ( [Title], [Style], [InMenu], [LogicalOrder], [IsActive]) VALUES ( N'پرداخت', N'', 1, 1, 1)
GO
INSERT [UserPool].[App] ( [Title], [Style], [InMenu], [LogicalOrder], [IsActive]) VALUES ( N'گزارشات', N'', 1, 1, 1)
GO
INSERT [UserPool].[App] ( [Title], [Style], [InMenu], [LogicalOrder], [IsActive]) VALUES ( N'اطلاعات سیستم', N'', 1, 1, 1)
GO
INSERT [UserPool].[App] ( [Title], [Style], [InMenu], [LogicalOrder], [IsActive]) VALUES ( N'Workflow', N'', 1, 1, 1)
GO
SET IDENTITY_INSERT [UserPool].[App] OFF
GO
SET IDENTITY_INSERT [UserPool].[Module] ON 
GO
INSERT [UserPool].[Module] ( [AppId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 1, N'مدیریت کاربران', N'', 1, 1, N'', 1)
GO
INSERT [UserPool].[Module] ( [AppId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 1, N'تصاویر امنیتی', N'', 1, 2, N'', 1)
GO
INSERT [UserPool].[Module] ( [AppId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 1, N'مدیریت نقش ها', N'', 1, 3, N'', 1)
GO
INSERT [UserPool].[Module] ( [AppId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 2, N'مدیریت سیفون', N'', 1, 1, N'', 1)
GO
INSERT [UserPool].[Module] ( [AppId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 2, N'مدیریت کنتور', N'', 1, 2, N'', 1)
GO
INSERT [UserPool].[Module] ( [AppId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 2, N'مدیریت ملک', N'', 1, 3, N'', 1)
GO
INSERT [UserPool].[Module] ( [AppId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 2, N'مدیریت اشخاص', N'', 4, 1, N'', 1)
GO
INSERT [UserPool].[Module] ( [AppId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 2, N'مدیریت نواحی', N'', 5, 1, N'', 1)
GO
INSERT [UserPool].[Module] ( [AppId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 2, N'انشعاب', N'', 1, 6, N'', 1)
GO
INSERT [UserPool].[Module] ( [AppId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 2, N'درخواست های مشترکین', N'', 1, 7, N'', 1)--10
GO
INSERT [UserPool].[Module] ( [AppId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 3, N'صورت حساب', N'', 1, 1, N'', 1)
GO
INSERT [UserPool].[Module] ( [AppId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 3, N'قواعد محاسبه', N'', 1, 2, N'', 1)
GO
INSERT [UserPool].[Module] ( [AppId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 3, N'مدیریت', N'', 1, 3, N'', 1)--13
GO
INSERT [UserPool].[Module] ( [AppId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 4, N'مدیریت مدارک', N'', 1, 1, N'', 1)
GO
INSERT [UserPool].[Module] ( [AppId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 5, N'تعاریف', N'', 1, 1, N'', 1)--15
GO
INSERT [UserPool].[Module] ( [AppId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 6, N'تعاریف', N'', 1, 1, N'', 1)
GO
INSERT [UserPool].[Module] ( [AppId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 7, N'اطلاعات بانک', N'', 1, 1, N'', 1)
GO
INSERT [UserPool].[Module] ( [AppId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 7, N'دریافت مدارک', N'', 1, 2, N'', 1)--18
GO
INSERT [UserPool].[Module] ( [AppId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 8, N'گزارش ساز پویا', N'', 1, 1, N'', 1)
GO
INSERT [UserPool].[Module] ( [AppId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 8, N'اطلاعات پایه', N'', 1, 2, N'', 1)--20
GO
INSERT [UserPool].[Module] ( [AppId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 9, N'اطلاعات سیستم', N'', 1, 1, N'', 1)
GO
INSERT [UserPool].[Module] ( [AppId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 10, N'Workflow', N'', 1, 1, N'', 1)--22
GO
SET IDENTITY_INSERT [UserPool].[Module] OFF
GO
SET IDENTITY_INSERT [UserPool].[SubModule] ON 
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 1, N'جدول کاربران', N'', 1, 1, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 1, N'مدیریت و افزودن کاربر', N'', 1, 2, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 2, N'مدیریت و افزودن', N'', 1, 1, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 2, N'زبان', N'', 1, 2, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 2, N'حالت', N'', 1, 3, N'', 1)--5
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 3, N'جدول نقش ها', N'', 1, 1, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 3, N'افزودن', N'', 1, 2, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 4, N'نوع سیفون', N'', 1, 1, N'', 1)--8
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 4, N'جنس سیفون', N'', 1, 2, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 4, N'قطر سیفون', N'', 1, 3, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 4, N'سیفون', N'', 1, 4, N'', 1)--11
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 5, N'نوع کنتور', N'', 1, 1, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 5, N'جنس  کنتور', N'', 1, 2, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 5, N'قطر کنتور', N'', 1, 3, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 5, N'نوع استفاده', N'', 1, 4, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 5, N'سازنده کنتور', N'', 1, 5, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 5, N'مدیریت برچسب', N'', 1, 6, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 5, N'انتساب برچسب', N'', 1, 7, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 5, N'وضعیت کنتور', N'', 1, 8, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 5, N'کنتور', N'', 1, 9, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 5, N'نوع اشتراک', N'', 1, 10, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 5, N'نحوه نصب', N'', 1, 11, N'', 1)--22
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 6, N'نوع سازنده', N'', 1, 1, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 6, N'کاربری', N'', 1, 2, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 6, N'ملک', N'', 1, 3, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 6, N'واحد', N'', 1, 4, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 6, N'شاخص محاسبه ظرفیت', N'', 1, 5, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 6, N'نوع محدوده', N'', 1, 6, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 6, N'ارتباط ملک و منبع آب', N'', 1, 7, N'', 1)--29
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 6, N'Guild', N'', 1, 8, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 6, N'Profession', N'', 1, 9, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 6, N'واگذاری', N'', 1, 10, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 6, N'روزهای تعطیلات رسمی', N'', 1, 11, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 6, N'سطح دسترسی 1', N'', 1, 12, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 6, N'سطح دسترسی 2', N'', 1, 13, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 6, N'سطح دسترسی 3', N'', 1, 14, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 6, N'سطح دسترسی 4', N'', 1, 15, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 6, N'خروج کاربر', N'', 1, 16, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 6, N'UserWorkday', N'', 1, 17, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 6, N'منابع آب', N'', 1, 18, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 7, N'نوع شخص', N'', 1, 1, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 7, N'رابطه', N'', 1, 2, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 7, N'اشخاص', N'', 1, 3, N'', 1)--43
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 7, N'مدیریت برچسب', N'', 1, 4, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 7, N'انتساب برچسب', N'', 1, 5, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 7, N'نوع تخفیف', N'', 1, 6, N'', 1)--46
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 8, N'کشور', N'', 1, 1, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 8, N'جهت', N'', 1, 2, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 8, N'استان', N'', 1, 3, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 8, N'نام شرکت', N'', 1, 4, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 8, N'منطقه', N'', 1, 5, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 8, N'ناحیه', N'', 1, 6, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 8, N'شهر/روستا/شهرداری', N'', 1, 7, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 8, N'محدوده اشتراکی', N'', 1, 8, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 8, N'بلوک اشتراکی', N'', 1, 9, N'', 1)--55
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 9, N'مدیریت', N'', 1, 1, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 10, N'درخواست اشتراک', N'', 1, 1, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 10, N'ملک', N'', 1, 2, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 10, N'واحد', N'', 1, 3, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 10, N'توع تخفیف اشخاص', N'', 1, 4, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 10, N'انتساب تگ های اشخاص', N'', 1, 5, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 10, N'شخص', N'', 1, 6, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 10, N'ارتباط ملک و شخص', N'', 1, 7, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 10, N'سیفون', N'', 1, 8, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 10, N'کنتور', N'', 1, 9, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 10, N'انتساب تگ ها کنتور', N'', 1, 10, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 10, N'ارتباط کنتور و سیفون', N'', 1, 11, N'', 1)--67
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 11, N'خدمات', N'', 1, 1, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 11, N'نوع خدمات', N'', 1, 2, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 11, N'اقلام-خدمات', N'', 1, 3, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 11, N'علامت', N'', 1, 4, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 11, N'صورتحساب', N'', 1, 5, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 11, N'اقساط', N'', 1, 6, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 11, N'نوع ورود صورتحساب', N'', 1, 7, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 11, N'وضعیت ثبت صورتحساب', N'', 1, 8, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 11, N'نوع صورتحساب', N'', 1, 9, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 11, N'وضعیت صورتحساب', N'', 1, 10, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 11, N'ردیف هزینه', N'', 1, 11, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 11, N'گروه ردیف هزینه', N'', 1, 12, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 11, N'صورت حساب بهمراه نوع ورود و اقساط', N'', 1, 13, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 11, N'تاریخچه تغیر رقم کنتور', N'', 1, 14, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 12, N'اقلام', N'', 1, 1, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 12, N'گروه اقلام', N'', 1, 2, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 12, N'واحد اقلام', N'', 1, 3, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 12, N'عملگر قابل استفاده', N'', 1, 4, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 12, N'فیلد های قابل استفاده', N'', 1, 5, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 12, N'تعرفه', N'', 1, 6, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 12, N'نوع محاسبه تعرفه', N'', 1, 7, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 12, N'ثوابت', N'', 1, 8, N'', 1)--88
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 13, N'تعرفه گروهی', N'', 1, 1, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 13, N'تعرفه بهمراه جزئیات', N'', 1, 2, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 13, N'تست تعرفه فرضی', N'', 1, 3, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 13, N'محاسبه تعرفه', N'', 1, 4, N'', 1)--92
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 14, N'سند', N'', 1, 1, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 14, N'دسته بندی سند', N'', 1, 2, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 14, N'نوع مدارک', N'', 1, 3, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 14, N'مدارک', N'', 1, 4, N'', 1)--96
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 14, N'مدارک قابل اجرا', N'', 1, 5, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 14, N'MimetypeCategory', N'', 1, 6, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 15, N'وضعیت کنتور', N'', 1, 1, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 15, N'تنظیمات پیشفرض', N'', 1, 2, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 15, N'نوع دوره قرائت', N'', 1, 3, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 15, N'دوره قرائت', N'', 1, 4, N'', 1)--102
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 16, N'کارگزار لوازم انشعاب', N'', 1, 1, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 16, N'کارگزار مناطق', N'', 1, 2, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES (17 , N'حساب بانکی', N'', 1, 1, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES (17 , N'ساختار فایل بانکی', N'', 1, 2, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES (17 , N'بانک', N'', 1, 3, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES (17 , N'روش پرداخت', N'', 1, 4, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES (18 , N'Credit', N'', 1, 1, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES (19 , N'گزارش', N'', 1, 1, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES (20 , N'شخص', N'', 1, 1, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES (20 , N'ملک', N'', 1, 2, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES (20 , N'واحد', N'', 1, 3, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES (20 , N'سیفون', N'', 1, 4, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES (20 , N'کنتور', N'', 1, 5, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES (20 , N'اشتراک', N'', 1, 6, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES (20 , N'قبض آب', N'', 1, 7, N'', 1)--117
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES (21 , N'اطلاعات سیستم عامل', N'', 1, 1, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES (21 , N'اطلاعات دیسک', N'', 1, 2, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES (22 , N'State', N'', 1, 1, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES (22 , N'Workflow', N'', 1, 2, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES (22 , N'WorkflowStatus', N'', 1, 3, N'', 1)
GO
SET IDENTITY_INSERT [UserPool].[SubModule] OFF
GO
SET IDENTITY_INSERT [UserPool].[Endpoint] ON 
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 1, N'حذف', N'', 1, 1, N'UserDeleteManager.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 1, N'فعال سازی', N'', 1, 2, N'UserUnLockManager.UnLock', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 1, N'غیرفعال سازی', N'', 1, 3, N'UserLockManager.Lock', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 1, N'بازنشانی گذرواژه', N'', 1, 4, N'UserResetPasswordManager.ResetPassword', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 1, N'مشاهده همه جدولی', N'', 1, 5, N'UserAllQuery.Get', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 1, N'مشاهده بصورت Paigination', N'', 1, 6, N'UserGridifyQuery.GetUsersByQuery', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 1, N'مشاهده جدولی', N'', 1, 7, N'UserGetSingleQuery.GetInfo', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 1, N'مشاهده پارامتر های ایجاد', N'', 1, 8, N'UserQueryParamsOfCreate.GetParamsOfCreate', 1)
GO

INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 2, N'ذخیره', N'', 1, 2, N'UserCreate.Trigger', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 2, N'مشاهده همه مقادیر', N'', 1, 2, N'', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 2, N'ویرایش', N'UserUpdate.Update', 1, 3, N'', 1)
GO

INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 3, N'مشاهده', N'', 1, 1, N'CaptchaGetSignleQuery.Read', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 3, N'ویرایش', N'', 1, 2, N'CaptchaUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 3, N'دیکشنری', N'', 1, 3, N'CaptchaDictionary.Get', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 3, N'پارامترها', N'', 1, 4, N'CaptchaParameter.Read', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 3, N'ایجاد', N'', 1, 4, N'CaptchaCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 3, N'مشاهده لیست', N'', 1, 4, N'CaptchaList.Read', 1)
GO

INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 4, N'مشاهده', N'', 1, 1, N'CaptchaLanguage.Get', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 5, N'مشاهده', N'', 1, 1, N'CaptchaDisplayMode.Get', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 6, N'مشاهده همه', N'', 1, 1, N'RoleGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 6, N'حذف', N'', 1, 2, N'RoleDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 6, N'ویرایش', N'', 1, 3, N'RoleUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 6, N'مشاهده', N'', 1, 4, N'RoleGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 7, N'مشاهده مقادیر ایجاد', N'', 1, 1, N'RoleQueryParamCreate.GetRoleParamsOfCreate', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 7, N'ایجاد', N'', 1, 2, N'RoleCreate.Create', 1)
GO

INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 8, N'ایجاد', N'', 1, 1, N'SiphonTypeCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 8, N'مشاهده لیست', N'', 1, 1, N'SiphonTypeGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 8, N'مشاهده', N'', 1, 2, N'SiphonTypeGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 8, N'حذف', N'', 1, 3, N'SiphonTypeDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 8, N'ویرایش', N'', 1, 4, N'SiphonTypeUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 9, N'ایجاد', N'', 1, 1, N'SiphonMaterialCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 9, N'مشاهده لیست', N'', 1, 1, N'SiphonMaterialGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 9, N'مشاهده', N'', 1, 2, N'SiphonMaterialGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 9, N'حذف', N'', 1, 3, N'SiphonMaterialDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 9, N'ویرایش', N'', 1, 4, N'SiphonMaterialUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 10, N'ایجاد', N'', 1, 1, N'SiphonDiameterCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 10, N'مشاهده لیست', N'', 1, 1, N'SiphonDiameterGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 10, N'مشاهده', N'', 1, 2, N'SiphonDiameterGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 10, N'حذف', N'', 1, 3, N'SiphonDiameterDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 10, N'ویرایش', N'', 1, 4, N'SiphonDiameterUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 11, N'ایجاد', N'', 1, 1, N'SiphonCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 11, N'مشاهده لیست', N'', 1, 1, N'SiphonGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 11, N'مشاهده', N'', 1, 2, N'SiphonGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 11, N'حذف', N'', 1, 3, N'SiphonDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 11, N'ویرایش', N'', 1, 4, N'SiphonUpdate.Update', 1)
GO

INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 12, N'ایجاد', N'', 1, 1, N'MeterTypeCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 12, N'مشاهده لیست', N'', 1, 1, N'MeterTypeGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 12, N'مشاهده', N'', 1, 2, N'MeterTypeGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 12, N'حذف', N'', 1, 3, N'MeterTypeDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 12, N'ویرایش', N'', 1, 4, N'MeterTypeUpdate.Update', 1)
GO

INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 13, N'ایجاد', N'', 1, 1, N'MeterMaterialCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 13, N'مشاهده لیست', N'', 1, 1, N'MeterMaterialGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 13, N'مشاهده', N'', 1, 2, N'MeterMaterialGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 13, N'حذف', N'', 1, 3, N'MeterMaterialDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 13, N'ویرایش', N'', 1, 4, N'MeterMaterialUpdate.Update', 1)
GO

INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 14, N'ایجاد', N'', 1, 1, N'MeterDiameterCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 14, N'مشاهده لیست', N'', 1, 1, N'MeterDiameterGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 14, N'مشاهده', N'', 1, 2, N'MeterDiameterGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 14, N'حذف', N'', 1, 3, N'MeterDiameterDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 14, N'ویرایش', N'', 1, 4, N'MeterDiameterUpdate.Update', 1)
GO

INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 15, N'ایجاد', N'', 1, 1, N'MeterUseTypeCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 15, N'مشاهده لیست', N'', 1, 1, N'MeterUseTypeGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 15, N'مشاهده', N'', 1, 2, N'MeterUseTypeGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 15, N'حذف', N'', 1, 3, N'MeterUseTypeDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 15, N'ویرایش', N'', 1, 4, N'MeterUseTypeUpdate.Update', 1)
GO

INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 16, N'ایجاد', N'', 1, 1, N'MeterProducerCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 16, N'مشاهده لیست', N'', 1, 1, N'MeterProducerGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 16, N'مشاهده', N'', 1, 2, N'MeterProducerGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 16, N'حذف', N'', 1, 3, N'MeterProducerDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 16, N'ویرایش', N'', 1, 4, N'MeterProducerUpdate.Update', 1)
GO


INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 17, N'ایجاد', N'', 1, 1, N'WaterMeterTagDefinitionCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 17, N'مشاهده لیست', N'', 1, 1, N'WaterMeterTagDefinitionGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 17, N'مشاهده', N'', 1, 2, N'WaterMeterTagDefinitionGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 17, N'حذف', N'', 1, 3, N'WaterMeterTagDefinitionDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 17, N'ویرایش', N'', 1, 4, N'WaterMeterTagDefinitionUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 18, N'ایجاد', N'', 1, 1, N'WaterMeterTagCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 18, N'مشاهده لیست', N'', 1, 1, N'WaterMeterTagGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 18, N'مشاهده', N'', 1, 2, N'WaterMeterTagGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 18, N'حذف', N'', 1, 3, N'WaterMeterTagDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 18, N'ویرایش', N'', 1, 4, N'WaterMeterTagUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 18, N'جستجو', N'', 1, 5, N'WaterMeterTagGetSingleBySearchInput.Search', 1)
GO

INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 19, N'ایجاد', N'', 1, 1, N'UseStateCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 19, N'مشاهده لیست', N'', 1, 1, N'UseStateGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 19, N'مشاهده', N'', 1, 2, N'UseStateGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 19, N'حذف', N'', 1, 3, N'UseStateDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 19, N'ویرایش', N'', 1, 4, N'UseStateUpdate.Update', 1)
GO

INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 20, N'ایجاد', N'', 1, 1, N'WaterMeterCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 20, N'مشاهده لیست', N'', 1, 1, N'WaterMeterGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 20, N'مشاهده', N'', 1, 2, N'WaterMeterGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (20, N'حذف', N'', 1, 3, N'WaterMeterDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (20, N'ویرایش', N'', 1, 4, N'WaterMeterUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 21, N'ایجاد', N'', 1, 1, N'SubscriptionTypeGetAll.GetAll', 1)
GO

INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 21, N'ایجاد', N'', 1, 1, N'SubscriptionTypeCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 21, N'مشاهده لیست', N'', 1, 1, N'SubscriptionTypeGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 21, N'مشاهده', N'', 1, 2, N'SubscriptionTypeGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 21, N'حذف', N'', 1, 3, N'SubscriptionTypeDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 21, N'ویرایش', N'', 1, 4, N'SubscriptionTypeUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 22, N'ایجاد', N'', 1, 1, N'WaterMeterInstallationMethodCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 22, N'مشاهده لیست', N'', 1, 1, N'WaterMeterInstallationMethodGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 22, N'مشاهده', N'', 1, 2, N'WaterMeterInstallationMethodGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 22, N'حذف', N'', 1, 3, N'WaterMeterInstallationMethodDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 22, N'ویرایش', N'', 1, 4, N'WaterMeterInstallationMethodUpdate.Update', 1)
GO

INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (23, N'ایجاد', N'', 1, 1, N'ConstructionTypeCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (23, N'مشاهده لیست', N'', 1, 1, N'ConstructionTypeGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (23, N'مشاهده', N'', 1, 2, N'ConstructionTypeGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (23, N'حذف', N'', 1, 3, N'ConstructionTypeDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (23, N'ویرایش', N'', 1, 4, N'ConstructionTypeUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (24, N'ایجاد', N'', 1, 1, N'UsageCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (24, N'مشاهده لیست', N'', 1, 1, N'UsageGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (24, N'مشاهده', N'', 1, 2, N'UsageGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (24, N'حذف', N'', 1, 3, N'UsageDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (24, N'ویرایش', N'', 1, 4, N'UsageUpdate.Update', 1)
GO

INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (25, N'ایجاد', N'', 1, 1, N'EstateCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (25, N'مشاهده لیست', N'', 1, 1, N'EstateGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (25, N'مشاهده', N'', 1, 2, N'EstateGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (25, N'حذف', N'', 1, 3, N'EstateDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (25, N'ویرایش', N'', 1, 4, N'EstateUpdate.Update', 1)
GO

INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (26, N'ایجاد', N'', 1, 1, N'FlatCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (26, N'مشاهده لیست', N'', 1, 1, N'FlatGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (26, N'مشاهده', N'', 1, 2, N'FlatGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (26, N'حذف', N'', 1, 3, N'FlatDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (26, N'ویرایش', N'', 1, 4, N'FlatUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 27, N'ایجاد', N'', 1, 1, N'CapacityCalculationIndexCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 27, N'مشاهده لیست', N'', 1, 1, N'CapacityCalculationIndexGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 27, N'مشاهده', N'', 1, 2, N'CapacityCalculationIndexGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 27, N'حذف', N'', 1, 3, N'CapacityCalculationIndexDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 27, N'ویرایش', N'', 1, 4, N'CapacityCalculationIndexUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 28, N'ایجاد', N'', 1, 1, N'EstateBoundTypeCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 28, N'مشاهده لیست', N'', 1, 1, N'EstateBoundTypeGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 28, N'مشاهده', N'', 1, 2, N'EstateBoundTypeGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 28, N'حذف', N'', 1, 3, N'EstateBoundTypeDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 28, N'ویرایش', N'', 1, 4, N'EstateBoundTypeUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 29, N'ایجاد', N'', 1, 1, N'EstateWaterResourceCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 29, N'مشاهده لیست', N'', 1, 1, N'EstateWaterResourceGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 29, N'مشاهده', N'', 1, 2, N'EstateWaterResourceGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 29, N'حذف', N'', 1, 3, N'EstateWaterResourceDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 29, N'ویرایش', N'', 1, 4, N'EstateWaterResourceUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 30, N'ایجاد', N'', 1, 1, N'GuildCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 30, N'مشاهده لیست', N'', 1, 2, N'GuildGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 30, N'مشاهده', N'', 1, 3, N'GuildGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 30, N'حذف', N'', 1, 4, N'GuildDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 30, N'ویرایش', N'', 1, 5, N'GuildUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 31, N'ایجاد', N'', 1, 1, N'ProfessionCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 31, N'مشاهده لیست', N'', 1, 2, N'ProfessionGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 31, N'مشاهده', N'', 1, 3, N'ProfessionGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 31, N'حذف', N'', 1, 4, N'ProfessionDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 31, N'ویرایش', N'', 1, 5, N'ProfessionUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 32, N'ایجاد', N'', 1, 1, N'HandoverCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 32, N'مشاهده لیست', N'', 1, 2, N'HandoverGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 32, N'مشاهده', N'', 1, 3, N'HandoverGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 32, N'حذف', N'', 1, 4, N'HandoverDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 32, N'ویرایش', N'', 1, 5, N'HandoverUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 33, N'ایجاد', N'', 1, 1, N'OfficialHolidayCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 33, N'مشاهده لیست', N'', 1, 2, N'OfficialHolidayGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 33, N'مشاهده', N'', 1, 3, N'OfficialHolidayGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 33, N'حذف', N'', 1, 4, N'OfficialHolidayDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 33, N'ویرایش', N'', 1, 5, N'OfficialHolidayUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 34, N'ایجاد', N'', 1, 1, N'UsageLevel1Create.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 34, N'مشاهده لیست', N'', 1, 2, N'UsageLevel1GetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 34, N'مشاهده', N'', 1, 3, N'UsageLevel1GetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 34, N'حذف', N'', 1, 4, N'UsageLevel1Delete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 34, N'ویرایش', N'', 1, 5, N'UsageLevel1Update.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 35, N'ایجاد', N'', 1, 1, N'UsageLevel2Create.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 35, N'مشاهده لیست', N'', 1, 2, N'UsageLevel2GetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 35, N'مشاهده', N'', 1, 3, N'UsageLevel2GetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 35, N'حذف', N'', 1, 4, N'UsageLevel2Delete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 35, N'ویرایش', N'', 1, 5, N'UsageLevel2Update.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 36, N'ایجاد', N'', 1, 1, N'UsageLevel3Create.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 36, N'مشاهده لیست', N'', 1, 2, N'UsageLevel3GetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 36, N'مشاهده', N'', 1, 3, N'UsageLevel3GetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 36, N'حذف', N'', 1, 4, N'UsageLevel3Delete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 36, N'ویرایش', N'', 1, 5, N'UsageLevel3Update.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 37, N'ایجاد', N'', 1, 1, N'UsageLevel4Create.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 37, N'مشاهده لیست', N'', 1, 2, N'UsageLevel4GetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 37, N'مشاهده', N'', 1, 3, N'UsageLevel4GetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 37, N'حذف', N'', 1, 4, N'UsageLevel4Delete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 37, N'ویرایش', N'', 1, 5, N'UsageLevel4Update.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 38, N'ایجاد', N'', 1, 1, N'UserLeaveCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 38, N'مشاهده لیست', N'', 1, 2, N'UserLeaveGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 38, N'مشاهده', N'', 1, 3, N'UserLeaveGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 38, N'حذف', N'', 1, 4, N'UserLeaveDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 38, N'ویرایش', N'', 1, 5, N'UserLeaveUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 39, N'ایجاد', N'', 1, 1, N'UserWorkdayCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 39, N'مشاهده لیست', N'', 1, 2, N'UserWorkdayGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 39, N'مشاهده', N'', 1, 3, N'UserWorkdayGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 39, N'حذف', N'', 1, 4, N'UserWorkdayDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 39, N'ویرایش', N'', 1, 5, N'UserWorkdayUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 40, N'ایجاد', N'', 1, 1, N'WaterResourceGCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 40, N'مشاهده لیست', N'', 1, 2, N'WaterResourceGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 40, N'مشاهده', N'', 1, 3, N'WaterResourceGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 40, N'حذف', N'', 1, 4, N'WaterResourceDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 40, N'ویرایش', N'', 1, 5, N'WaterResourceUpdate.Update', 1)
GO


INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (41, N'مشاهده لیست', N'', 1, 1, N'IndividualTypeGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (41, N'مشاهده', N'', 1, 2, N'IndividualTypeGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (41, N'حذف', N'', 1, 3, N'IndividualTypeDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (41, N'ویرایش', N'', 1, 4, N'IndividualTypeUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (42, N'مشاهده لیست', N'', 1, 1, N'IndividualEstateRelationTypeGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (42, N'مشاهده', N'', 1, 2, N'IndividualEstateRelationTypeGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (42, N'حذف', N'', 1, 3, N'IndividualEstateRelationTypeDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (42, N'ویرایش', N'', 1, 4, N'IndividualEstateRelationTypeUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (43, N'مشاهده لیست', N'', 1, 1, N'IndividualGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (43, N'مشاهده', N'', 1, 2, N'IndividualGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (43, N'حذف', N'', 1, 3, N'IndividualDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (43, N'ویرایش', N'', 1, 4, N'IndividualUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (44, N'مشاهده لیست', N'', 1, 1, N'IndividualTagDefinitionGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (44, N'مشاهده', N'', 1, 2, N'IndividualTagDefinitionGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (44, N'حذف', N'', 1, 3, N'IndividualTagDefinitionDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (44, N'ویرایش', N'', 1, 4, N'IndividualTagDefinitionUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (45, N'مشاهده لیست', N'', 1, 1, N'IndividualTagGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (45, N'مشاهده', N'', 1, 2, N'IndividualTagGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (45, N'حذف', N'', 1, 3, N'IndividualTagDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (45, N'ویرایش', N'', 1, 4, N'IndividualTagUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (45, N'جستجو', N'', 1, 5, N'IndividualTagGetSingleBySearchInput.Search', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 46, N'ایجاد', N'', 1, 1, N'IndividualDiscountTypeCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 46, N'مشاهده لیست', N'', 1, 2, N'IndividualDiscountTypeGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 46, N'مشاهده', N'', 1, 3, N'IndividualDiscountTypeGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 46, N'حذف', N'', 1, 4, N'IndividualDiscountTypeDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 46, N'ویرایش', N'', 1, 5, N'IndividualDiscountTypeUpdate.Update', 1)
GO

INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (47, N'مشاهده لیست', N'', 1, 1, N'CountryGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (47, N'مشاهده', N'', 1, 2, N'CountryGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (47, N'حذف', N'', 1, 3, N'CountryDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (47, N'ویرایش', N'', 1, 4, N'CountryUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (48, N'مشاهده لیست', N'', 1, 1, N'CordinalDirectionGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (48, N'مشاهده', N'', 1, 2, N'CordinalDirectionGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (48, N'حذف', N'', 1, 3, N'CordinalDirectionDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (48, N'ویرایش', N'', 1, 4, N'CordinalDirectionUpdate.Update', 1)
GO

INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (49, N'مشاهده لیست', N'', 1, 1, N'ProvinceGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (49, N'مشاهده', N'', 1, 2, N'ProvinceGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (49, N'حذف', N'', 1, 3, N'ProvinceDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (49, N'ویرایش', N'', 1, 4, N'ProvinceUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (50, N'مشاهده لیست', N'', 1, 1, N'HeadquartersGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (50, N'مشاهده', N'', 1, 2, N'HeadquartersGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (50, N'حذف', N'', 1, 3, N'HeadquartersDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (50, N'ویرایش', N'', 1, 4, N'HeadquartersUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (51, N'مشاهده لیست', N'', 1, 1, N'RegionGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (51, N'مشاهده', N'', 1, 2, N'RegionGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (51, N'حذف', N'', 1, 3, N'RegionDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (51, N'ویرایش', N'', 1, 4, N'RegionUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (52, N'مشاهده لیست', N'', 1, 1, N'ZoneGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (52, N'مشاهده', N'', 1, 2, N'ZoneGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (52, N'حذف', N'', 1, 3, N'ZoneDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (52, N'ویرایش', N'', 1, 4, N'ZoneUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (53, N'مشاهده لیست', N'', 1, 1, N'MunicipalityGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (53, N'مشاهده', N'', 1, 2, N'MunicipalityGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (53, N'حذف', N'', 1, 3, N'MunicipalityDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (53, N'ویرایش', N'', 1, 4, N'MunicipalityUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (54, N'مشاهده لیست', N'', 1, 1, N'ReadingBoundGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (54, N'مشاهده', N'', 1, 2, N'ReadingBoundGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (54, N'حذف', N'', 1, 3, N'ReadingBoundDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (54, N'ویرایش', N'', 1, 4, N'ReadingBoundUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (55, N'مشاهده لیست', N'', 1, 1, N'ReadingBlockGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (55, N'مشاهده', N'', 1, 2, N'ReadingBlockGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (55, N'حذف', N'', 1, 3, N'ReadingBlockDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (55, N'ویرایش', N'', 1, 4, N'ReadingBlockUpdate.Update', 1)
GO

INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (56, N'ایجاد', N'', 1, 4, N'TotalApiCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (57, N'ایجاد', N'', 1, 4, N'RequestSubscriptionCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 58, N'ایجاد', N'', 1, 1, N'RequestEstateCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 58, N'مشاهده لیست', N'', 1, 2, N'RequestEstateGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 58, N'مشاهده', N'', 1, 3, N'RequestEstateGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 58, N'حذف', N'', 1, 4, N'RequestEstateDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 58, N'ویرایش', N'', 1, 5, N'RequestEstateUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 59, N'ایجاد', N'', 1, 1, N'RequestFlatCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 59, N'مشاهده لیست', N'', 1, 2, N'RequestFlatGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 59, N'مشاهده', N'', 1, 3, N'RequestFlatGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 59, N'حذف', N'', 1, 4, N'RequestFlatDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 59, N'ویرایش', N'', 1, 5, N'RequestFlatUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 60, N'ایجاد', N'', 1, 1, N'RequestIndividualDiscountTypeCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 60, N'مشاهده لیست', N'', 1, 2, N'RequestIndividualDiscountTypeGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 60, N'مشاهده', N'', 1, 3, N'RequestIndividualDiscountTypeGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 60, N'حذف', N'', 1, 4, N'RequestIndividualDiscountTypeDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 60, N'ویرایش', N'', 1, 5, N'RequestIndividualDiscountTypeUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 61, N'ایجاد', N'', 1, 1, N'RequestIndividualTagCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 61, N'مشاهده لیست', N'', 1, 2, N'RequestIndividualTagGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 61, N'مشاهده', N'', 1, 3, N'RequestIndividualTagGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 61, N'حذف', N'', 1, 4, N'RequestIndividualTagDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 61, N'ویرایش', N'', 1, 5, N'RequestIndividualTagUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 62, N'ایجاد', N'', 1, 1, N'RequestIndividualCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 62, N'مشاهده لیست', N'', 1, 2, N'RequestIndividualGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 62, N'مشاهده', N'', 1, 3, N'RequestIndividualGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 62, N'حذف', N'', 1, 4, N'RequestIndividualDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 62, N'ویرایش', N'', 1, 5, N'RequestIndividualUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 63, N'ایجاد', N'', 1, 1, N'RequestIndividualEstateCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 63, N'مشاهده لیست', N'', 1, 2, N'RequestIndividualEstateGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 63, N'مشاهده', N'', 1, 3, N'RequestIndividualEstateGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 63, N'حذف', N'', 1, 4, N'RequestIndividualEstateDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 63, N'ویرایش', N'', 1, 5, N'RequestIndividualEstateUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 64, N'ایجاد', N'', 1, 1, N'RequestSiphonCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 64, N'مشاهده لیست', N'', 1, 2, N'RequestSiphonGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 64, N'مشاهده', N'', 1, 3, N'RequestSiphonGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 64, N'حذف', N'', 1, 4, N'RequestSiphonDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 64, N'ویرایش', N'', 1, 5, N'RequestSiphonUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 65, N'ایجاد', N'', 1, 1, N'RequestWaterMeterCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 65, N'مشاهده لیست', N'', 1, 2, N'RequestWaterMeterGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 65, N'مشاهده', N'', 1, 3, N'RequestWaterMeterGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 65, N'حذف', N'', 1, 4, N'RequestWaterMeterDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 65, N'ویرایش', N'', 1, 5, N'RequestWaterMeterUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 66, N'ایجاد', N'', 1, 1, N'RequestWaterMeterTagCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 66, N'مشاهده لیست', N'', 1, 2, N'RequestWaterMeterTagGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 66, N'مشاهده', N'', 1, 3, N'RequestWaterMeterTagGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 66, N'حذف', N'', 1, 4, N'RequestWaterMeterTagDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 66, N'ویرایش', N'', 1, 5, N'RequestWaterMeterTagUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 67, N'ایجاد', N'', 1, 1, N'RequestWaterMeterSiphonCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 67, N'مشاهده لیست', N'', 1, 2, N'RequestWaterMeterSiphonGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 67, N'مشاهده', N'', 1, 3, N'RequestWaterMeterSiphonGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 67, N'حذف', N'', 1, 4, N'RequestWaterMeterSiphonDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 67, N'ویرایش', N'', 1, 5, N'RequestWaterMeterSiphonUpdate.Update', 1)
GO




INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (67, N'ایجاد', N'', 1, 1, N'CompanyServiceCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (67, N'مشاهده لیست', N'', 1, 2, N'CompanyServiceGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (67, N'مشاهده', N'', 1, 3, N'CompanyServiceGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (67, N'حذف', N'', 1, 4, N'CompanyServiceDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (67, N'ویرایش', N'', 1, 5, N'CompanyServiceUpdate.Update', 1)
GO

INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (68, N'ایجاد', N'', 1, 1, N'CompanyServiceTypeCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (68, N'مشاهده لیست', N'', 1, 2, N'CompanyServiceTypeGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (68, N'مشاهده', N'', 1, 3, N'CompanyServiceTypeGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (68, N'حذف', N'', 1, 4, N'CompanyServiceTypeDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (68, N'ویرایش', N'', 1, 5, N'CompanyServiceTypeUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (69, N'ایجاد', N'', 1, 1, N'CompanyServiceOfferingCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (69, N'مشاهده لیست', N'', 1, 2, N'CompanyServiceOfferingGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (69, N'مشاهده', N'', 1, 3, N'CompanyServiceOfferingGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (69, N'حذف', N'', 1, 4, N'CompanyServiceOfferingDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (69, N'ویرایش', N'', 1, 5, N'CompanyServiceOfferingUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (70, N'ایجاد', N'', 1, 1, N'ImpactSignCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (70, N'مشاهده لیست', N'', 1, 2, N'ImpactSignGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (70, N'مشاهده', N'', 1, 3, N'ImpactSignGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (70, N'حذف', N'', 1, 4, N'ImpactSignDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (70, N'ویرایش', N'', 1, 5, N'ImpactSignUpdate.Update', 1)
GO

INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (71, N'مشاهده لیست', N'', 1, 2, N'InvoiceGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (71, N'مشاهده', N'', 1, 3, N'InvoiceGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (71, N'حذف', N'', 1, 4, N'InvoiceDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (71, N'ویرایش', N'', 1, 5, N'InvoiceUpdate.Update', 1)
GO

INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (72, N'مشاهده لیست', N'', 1, 2, N'InvoiceInstallmentGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (72, N'مشاهده', N'', 1, 3, N'InvoiceInstallmentGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (72, N'حذف', N'', 1, 4, N'InvoiceInstallmentDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (72, N'ویرایش', N'', 1, 5, N'InvoiceInstallmentUpdate.Update', 1)
GO

INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (73, N'مشاهده لیست', N'', 1, 2, N'InvoiceLineItemGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (73, N'مشاهده', N'', 1, 3, N'InvoiceLineItemGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (73, N'حذف', N'', 1, 4, N'InvoiceLineItemDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (73, N'ویرایش', N'', 1, 5, N'InvoiceLineItemUpdate.Update', 1)
GO

INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (74, N'ایجاد', N'', 1, 1, N'InvoiceLineItemInsertModeCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (74, N'مشاهده لیست', N'', 1, 2, N'InvoiceLineItemInsertModeGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (74, N'مشاهده', N'', 1, 3, N'InvoiceLineItemInsertModeGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (74, N'حذف', N'', 1, 4, N'InvoiceLineItemInsertModeDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (74, N'ویرایش', N'', 1, 5, N'InvoiceLineItemInsertModeUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (75, N'ایجاد', N'', 1, 1, N'InvoiceTypeCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (75, N'مشاهده لیست', N'', 1, 2, N'InvoiceTypeGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (75, N'مشاهده', N'', 1, 3, N'InvoiceTypeGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (75, N'حذف', N'', 1, 4, N'InvoiceTypeDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (75, N'ویرایش', N'', 1, 5, N'InvoiceTypeUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (76, N'ایجاد', N'', 1, 1, N'InvoiceStatusCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (76, N'مشاهده لیست', N'', 1, 2, N'InvoiceStatusGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (76, N'مشاهده', N'', 1, 3, N'InvoiceStatusGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (76, N'حذف', N'', 1, 4, N'InvoiceStatusDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (76, N'ویرایش', N'', 1, 5, N'InvoiceStatusUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (77, N'ایجاد', N'', 1, 1, N'LineItemTypeCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (77, N'مشاهده لیست', N'', 1, 2, N'LineItemTypeGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (77, N'مشاهده', N'', 1, 3, N'LineItemTypeGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (77, N'حذف', N'', 1, 4, N'LineItemTypeDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (77, N'ویرایش', N'', 1, 5, N'LineItemTypeUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (78, N'ایجاد', N'', 1, 1, N'LineItemTypeGroupCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (78, N'مشاهده لیست', N'', 1, 2, N'LineItemTypeGroupGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (78, N'مشاهده', N'', 1, 3, N'LineItemTypeGroupGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (78, N'حذف', N'', 1, 4, N'LineItemTypeGroupDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (78, N'ویرایش', N'', 1, 5, N'LineItemTypeGroupUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (79, N'ایجاد', N'', 1, 1, N'InvoiceTogetherCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (80, N'مشاهده لیست', N'', 1, 2, N'WaterMeterChangeNumberHistoryGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (80, N'مشاهده', N'', 1, 3, N'WaterMeterChangeNumberHistoryGetSingle.GetSingle', 1)
GO

INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (81, N'ایجاد', N'', 1, 1, N'OfferingCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (81, N'مشاهده لیست', N'', 1, 2, N'OfferingGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (81, N'مشاهده', N'', 1, 3, N'OfferingGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (81, N'حذف', N'', 1, 4, N'OfferingDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (81, N'ویرایش', N'', 1, 5, N'OfferingUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (82, N'ایجاد', N'', 1, 1, N'OfferingGroupCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (82, N'مشاهده لیست', N'', 1, 2, N'OfferingGroupGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (82, N'مشاهده', N'', 1, 3, N'OfferingGroupGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (82, N'حذف', N'', 1, 4, N'OfferingGroupDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (82, N'ویرایش', N'', 1, 5, N'OfferingGroupUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (83, N'ایجاد', N'', 1, 1, N'OfferingUnitCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (83, N'مشاهده لیست', N'', 1, 2, N'OfferingUnitGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (83, N'مشاهده', N'', 1, 3, N'OfferingUnitGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (83, N'حذف', N'', 1, 4, N'OfferingUnitDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (83, N'ویرایش', N'', 1, 5, N'OfferingUnitUpdate.Update', 1)
GO

INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (84, N'مشاهده لیست', N'', 1, 2, N'SupportedOperatorGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (84, N'مشاهده', N'', 1, 3, N'SupportedOperatorGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (85, N'مشاهده لیست', N'', 1, 2, N'SupportedFieldGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (85, N'مشاهده', N'', 1, 3, N'SupportedFieldGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (86, N'ایجاد', N'', 1, 1, N'TariffCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (86, N'مشاهده لیست', N'', 1, 2, N'TariffGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (86, N'مشاهده', N'', 1, 3, N'TariffGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (86, N'حذف', N'', 1, 4, N'TariffDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (86, N'ویرایش', N'', 1, 5, N'TariffUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (87, N'ایجاد', N'', 1, 1, N'TariffCalculationModeCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (87, N'مشاهده لیست', N'', 1, 2, N'TariffCalculationModeGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (87, N'مشاهده', N'', 1, 3, N'TariffCalculationModeGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (87, N'حذف', N'', 1, 4, N'TariffCalculationModeDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (87, N'ویرایش', N'', 1, 5, N'TariffCalculationModeUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (88, N'ایجاد', N'', 1, 1, N'TariffConstantCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (88, N'مشاهده لیست', N'', 1, 2, N'TariffConstantGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (88, N'مشاهده', N'', 1, 3, N'TariffConstantGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (88, N'حذف', N'', 1, 4, N'TariffConstantDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (88, N'ویرایش', N'', 1, 5, N'TariffConstantUpdate.Update', 1)
GO

INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (89, N'تعرفه گروهی', N'', 1, 5, N'TariffBatchCalculationManager.Test', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (90, N'تعرفه بهمراه جزئیات', N'', 1, 5, N'TariffByDetailManager.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (91, N'تست تعرفه فرضی', N'', 1, 5, N'TariffCalculationImaginaryManager.Test', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (92, N'محاسبه تعرفه', N'', 1, 5, N'TariffCalculationImaginaryManager.Test', 1)
GO


INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (93, N'ایجاد', N'', 1, 1, N'DocumentEntityCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (93, N'مشاهده لیست', N'', 1, 2, N'DocumentEntityGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (93, N'مشاهده', N'', 1, 3, N'DocumentEntityGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (93, N'حذف', N'', 1, 4, N'DocumentEntityDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (93, N'ویرایش', N'', 1, 5, N'DocumentEntityUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (94, N'ایجاد', N'', 1, 1, N'DocumentCategoryCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (94, N'مشاهده لیست', N'', 1, 2, N'DocumentCategoryGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (94, N'مشاهده', N'', 1, 3, N'DocumentCategoryGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (94, N'حذف', N'', 1, 4, N'DocumentCategoryDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (94, N'ویرایش', N'', 1, 5, N'DocumentCategoryUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (94, N'مشاهده با شناسه قبض', N'', 1, 6, N'DocumentCategoryGetAllByBillId.ByBillId', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (95, N'ایجاد', N'', 1, 1, N'DocumentTypeCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (95, N'مشاهده لیست', N'', 1, 2, N'DocumentTypeGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (95, N'مشاهده', N'', 1, 3, N'DocumentTypeGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (95, N'حذف', N'', 1, 4, N'DocumentTypeDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (95, N'ویرایش', N'', 1, 5, N'DocumentTypeUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (96, N'ایجاد', N'', 1, 1, N'DocumentCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (96, N'مشاهده لیست', N'', 1, 2, N'DocumentGetAllByBillIdCategoryId.GetByBillIdCategoryId', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (96, N'مشاهده', N'', 1, 3, N'DocumentGet.Get', 1)
GO

INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (97, N'ایجاد', N'', 1, 1, N'ExecutableMimetypeCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (97, N'مشاهده لیست', N'', 1, 2, N'ExecutableMimetypeGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (97, N'مشاهده', N'', 1, 3, N'ExecutableMimetypeGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (97, N'حذف', N'', 1, 4, N'ExecutableMimetypeDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (97, N'ویرایش', N'', 1, 5, N'ExecutableMimetypeUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (98, N'ایجاد', N'', 1, 1, N'MimetypeCategoryCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (98, N'مشاهده لیست', N'', 1, 2, N'MimetypeCategoryGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (98, N'مشاهده', N'', 1, 3, N'MimetypeCategoryGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (98, N'حذف', N'', 1, 4, N'MimetypeCategoryDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (98, N'ویرایش', N'', 1, 5, N'MimetypeCategoryUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 99, N'ایجاد', N'', 1, 1, N'CounterStateCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 99, N'مشاهده لیست', N'', 1, 2, N'CounterStateGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 99, N'مشاهده', N'', 1, 3, N'CounterStateGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 99, N'حذف', N'', 1, 4, N'CounterStateDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 99, N'ویرایش', N'', 1, 5, N'CounterStateUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 100, N'ایجاد', N'', 1, 1, N'ReadingConfigDefaultCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 100, N'مشاهده لیست', N'', 1, 2, N'ReadingConfigDefaultGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 100, N'مشاهده', N'', 1, 3, N'ReadingConfigDefaultGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 100, N'حذف', N'', 1, 4, N'ReadingConfigDefaultDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 100, N'ویرایش', N'', 1, 5, N'ReadingConfigDefaultUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 101, N'ایجاد', N'', 1, 1, N'ReadingPeriodTypeCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 101, N'مشاهده لیست', N'', 1, 2, N'ReadingPeriodTypeGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 101, N'مشاهده', N'', 1, 3, N'ReadingPeriodTypeGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 101, N'حذف', N'', 1, 4, N'ReadingPeriodTypeDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 101, N'ویرایش', N'', 1, 5, N'ReadingPeriodTypeUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 102, N'ایجاد', N'', 1, 1, N'ReadingPeriodCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 102, N'مشاهده لیست', N'', 1, 2, N'ReadingPeriodGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 102, N'مشاهده', N'', 1, 3, N'ReadingPeriodGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 102, N'حذف', N'', 1, 4, N'ReadingPeriodDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 102, N'ویرایش', N'', 1, 5, N'ReadingPeriodUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 103, N'ایجاد', N'', 1, 1, N'EquipmentBrokerCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 103, N'مشاهده لیست', N'', 1, 2, N'EquipmentBrokerGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 103, N'مشاهده', N'', 1, 3, N'EquipmentBrokerGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 103, N'حذف', N'', 1, 4, N'EquipmentBrokerDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 103, N'ویرایش', N'', 1, 5, N'EquipmentBrokerUpdate.Update', 1)
GO	
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 104, N'ایجاد', N'', 1, 1, N'EquipmentBrokerZoneCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 104, N'مشاهده لیست', N'', 1, 2, N'EquipmentBrokerZoneGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 104, N'مشاهده', N'', 1, 3, N'EquipmentBrokerZoneGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 104, N'حذف', N'', 1, 4, N'EquipmentBrokerZoneDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 104, N'ویرایش', N'', 1, 5, N'EquipmentBrokerZoneUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 105, N'ایجاد', N'', 1, 1, N'BankAccountCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 105, N'مشاهده لیست', N'', 1, 2, N'BankAccountGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 105, N'مشاهده', N'', 1, 3, N'BankAccountGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 105, N'حذف', N'', 1, 4, N'BankAccountDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 105, N'ویرایش', N'', 1, 5, N'BankAccountUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 106, N'ایجاد', N'', 1, 1, N'BankFileStructureCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 106, N'مشاهده لیست', N'', 1, 2, N'BankFileStructureGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 106, N'مشاهده', N'', 1, 3, N'BankFileStructureGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 106, N'حذف', N'', 1, 4, N'BankFileStructureDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 106, N'ویرایش', N'', 1, 5, N'BankFileStructureUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 107, N'مشاهده لیست', N'', 1, 2, N'BankGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 107, N'مشاهده', N'', 1, 3, N'BankGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 108, N'مشاهده لیست', N'', 1, 2, N'PaymentMethodGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 108, N'مشاهده', N'', 1, 3, N'PaymentMethodGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 109, N'ایجاد', N'', 1, 1, N'CreditByDocumentCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 109, N'مشاهده لیست', N'', 1, 2, N'CreditByDocumentGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 109, N'مشاهده', N'', 1, 3, N'CreditByDocumentGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 109, N'حذف', N'', 1, 4, N'CreditByDocumentDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 109, N'ویرایش', N'', 1, 5, N'CreditByDocumentUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 110, N'مشاهده لیست', N'', 1, 2, N'DynamicReportGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 110, N'View', N'', 1, 3, N'?', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 110, N'حذف', N'', 1, 4, N'DynamicReportDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 110, N'Design', N'', 1, 5, N'?', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 111, N'مشاهده گزارش', N'', 1, 1, N'IndividualSummeryInfo.OwnerShipSummary', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 112, N'مشاهده گزارش', N'', 1, 1, N'RealmEstateSummeryInfo.Get', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 113, N'مشاهده گزارش', N'', 1, 1, N'RealmFlatSummeryInfo.Get', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 114, N'مشاهده گزارش', N'', 1, 1, N'SiphonSummeryInfo.Get', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 115, N'مشاهده گزارش', N'', 1, 1, N'WaterMeterSummeryInfo.ConsumptionSummary', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 116, N'مشاهده گزارش', N'', 1, 1, N'SubscriptionSummaryInfo.GetSummaryInfo', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 116, N'EventSummery', N'', 1, 1, N'SubscriptionEventsSummaryInfo.GetEventsSummaryInfo', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 117, N'مشاهده گزارش', N'', 1, 1, N'WaterInvoiceSummeryInfo.GetSummary', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 118, N'مشاهده', N'OsInfoGet.Get', 1, 1, N'', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 119, N'مشاهده', N'DiskInfoGe.Get', 1, 1, N'', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 120, N'ایجاد', N'', 1, 1, N'StateCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 120, N'مشاهده لیست', N'', 1, 2, N'StateGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 120, N'مشاهده', N'', 1, 3, N'StateGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 120, N'حذف', N'', 1, 4, N'StateDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 120, N'ویرایش', N'', 1, 5, N'StateUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 121, N'ایجاد', N'', 1, 1, N'WorkflowCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 121, N'مشاهده لیست', N'', 1, 2, N'WorkflowGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 121, N'مشاهده', N'', 1, 3, N'WorkflowGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 121, N'حذف', N'', 1, 4, N'WorkflowDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 121, N'ویرایش', N'', 1, 5, N'WorkflowUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 121, N'مشاهده Master', N'', 1, 5, N'WorkflowGetMaster.GetMaster', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 122, N'مشاهده لیست', N'', 1, 5, N'WorkflowStatusGetAll.GetAll', 1)
GO
SET IDENTITY_INSERT [UserPool].[Endpoint] OFF
GO