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
INSERT [UserPool].[App] ( [Title], [Style], [InMenu], [LogicalOrder], [IsActive]) VALUES ( N'درخواست های مشترکین', N'', 1, 1, 1)--module
GO
INSERT [UserPool].[App] ( [Title], [Style], [InMenu], [LogicalOrder], [IsActive]) VALUES ( N'قرائت کنتور', N'', 1, 1, 1)
GO
SET IDENTITY_INSERT [UserPool].[App] OFF
GO
SET IDENTITY_INSERT [UserPool].[Module] ON 
GO
INSERT [UserPool].[Module] ( [AppId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 1, N'مدیریت کاربران', N'', 1, 1, N'', 1)
GO
INSERT [UserPool].[Module] ( [AppId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 1, N'تصاویر امنیتی', N'', 1, 1, N'', 1)
GO
INSERT [UserPool].[Module] ( [AppId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 1, N'مدیریت نقش ها', N'', 1, 1, N'', 1)
GO
INSERT [UserPool].[Module] ( [AppId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 2, N'مدیریت سیفون', N'', 1, 1, N'', 1)
GO
INSERT [UserPool].[Module] ( [AppId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 2, N'مدیریت کنتور', N'', 1, 1, N'', 1)
GO
INSERT [UserPool].[Module] ( [AppId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 2, N'مدیریت ملک', N'', 1, 1, N'', 1)
GO
INSERT [UserPool].[Module] ( [AppId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 2, N'مدیریت اشخاص', N'', 1, 1, N'', 1)
GO
INSERT [UserPool].[Module] ( [AppId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 2, N'مدیریت نواحی', N'', 1, 1, N'', 1)
GO
INSERT [UserPool].[Module] ( [AppId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 2, N'انشعاب', N'', 1, 1, N'', 1)
GO
INSERT [UserPool].[Module] ( [AppId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 3, N'صورت حساب', N'', 1, 1, N'', 1)
GO
INSERT [UserPool].[Module] ( [AppId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 3, N'قواعد محاسبه', N'', 1, 2, N'', 1)
GO
INSERT [UserPool].[Module] ( [AppId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 4, N'مدیریت مدارک', N'', 1, 1, N'', 1)
GO
INSERT [UserPool].[Module] ( [AppId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 5, N'درخواست های مشترکین', N'', 1, 1, N'', 1)--
GO
INSERT [UserPool].[Module] ( [AppId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 6, N'تعاریف', N'', 1, 1, N'', 1)
GO
SET IDENTITY_INSERT [UserPool].[Module] OFF
GO
SET IDENTITY_INSERT [UserPool].[SubModule] ON 
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 1, N'جدول کاربران', N'', 1, 1, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 1, N'افزودن کاربر', N'', 1, 2, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 2, N'مدیریت و افزودن', N'', 1, 1, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 2, N'زبان', N'', 1, 2, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 2, N'حالت', N'', 1, 3, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 3, N'جدول نقش ها', N'', 1, 1, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 3, N'افزودن', N'', 1, 2, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 4, N'نوع سیفون', N'', 1, 1, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 4, N'جنس سیفون', N'', 1, 2, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 4, N'قطر سیفون', N'', 1, 3, N'', 1)
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
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 6, N'نوع سازنده', N'', 1, 1, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 6, N'کاربری', N'', 1, 2, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 6, N'ملک', N'', 1, 3, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 6, N'واحد', N'', 1, 4, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 7, N'نوع شخص', N'', 1, 1, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 7, N'رابطه', N'', 1, 2, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 7, N'اشخاص', N'', 1, 3, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 7, N'مدیریت برچسب', N'', 1, 4, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 7, N'انتساب برچسب', N'', 1, 5, N'', 1)
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
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 8, N'بلوک اشتراکی', N'', 1, 9, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 9, N'مدیریت', N'', 1, 1, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 10, N'خدمات', N'', 1, 1, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 10, N'نوع خدمات', N'', 1, 2, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 10, N'اقلام-خدمات', N'', 1, 3, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 10, N'علامت', N'', 1, 4, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 10, N'صورتحساب', N'', 1, 5, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 10, N'اقساط', N'', 1, 6, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 10, N'نوع ورود صورتحساب', N'', 1, 7, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 10, N'وضعیت ثبت صورتحساب', N'', 1, 8, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 10, N'نوع صورتحساب', N'', 1, 9, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 10, N'وضعیت صورتحساب', N'', 1, 10, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 10, N'ردیف هزینه', N'', 1, 11, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 10, N'گروه ردیف هزینه', N'', 1, 12, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 11, N'اقلام', N'', 1, 1, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 11, N'گروه اقلام', N'', 1, 2, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 11, N'واحد اقلام', N'', 1, 3, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 11, N'عملگر قابل استفاده', N'', 1, 4, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 11, N'فیلد های قابل استفاده', N'', 1, 5, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 11, N'تعرفه', N'', 1, 6, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 11, N'نوع محاسبه تعرفه', N'', 1, 7, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 11, N'ثوابت', N'', 1, 8, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 12, N'سند', N'', 1, 1, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 12, N'دسته بندی سند', N'', 1, 2, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 12, N'نوع مدارک', N'', 1, 3, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 12, N'مدارک قابل اجرا', N'', 1, 4, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 12, N'Mimetype', N'', 1, 5, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 13, N'وضعیت کنتور', N'', 1, 1, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 13, N'نوع دوره قرائت', N'', 1, 2, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 13, N'دوره قرائت', N'', 1, 3, N'', 1)
GO
INSERT [UserPool].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 13, N'تنظیمات پیشفرض', N'', 1, 4, N'', 1)
GO
SET IDENTITY_INSERT [UserPool].[SubModule] OFF
GO
SET IDENTITY_INSERT [UserPool].[Endpoint] ON 
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 1, N'مشاهده', N'', 1, 1, N'', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 1, N'حذف', N'', 1, 2, N'UserDeleteManager.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 1, N'فعال سازی', N'', 1, 3, N'UserUnLockManager.UnLock', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 1, N'غیرفعال سازی', N'', 1, 4, N'UserLockManager.Lock', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 1, N'بازنشانی گذرواژه', N'', 1, 5, N'UserResetPasswordManager.ResetPassword', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 1, N'مشاهده جدولی', N'', 1, 6, N'UserAllQuery.Get', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 1, N'مشاهده بصورت Paigination', N'', 1, 7, N'UserGridifyQuery.GetUsersByQuery', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 2, N'ذخیره', N'', 1, 2, N'UserCreate.Trigger', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 2, N'مشاهده همه مقادیر', N'', 1, 2, N'', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 3, N'مشاهده', N'', 1, 1, N'CaptchaList.Read', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 3, N'ویرایش', N'', 1, 2, N'CaptchaUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 3, N'دیکشنری', N'', 1, 3, N'CaptchaDictionary.Get', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 3, N'پارامترها', N'', 1, 4, N'CaptchaParameter.Read', 1)
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
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 6, N'مشاهده', N'', 1, 2, N'RoleGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 7, N'مشاهده مقادیر ایجاد', N'', 1, 1, N'RoleQueryParamCreate.GetRoleParamsOfCreate', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 7, N'ایجاد', N'', 1, 2, N'RoleCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 8, N'مشاهده لیست', N'', 1, 1, N'SiphonTypeGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 8, N'مشاهده', N'', 1, 2, N'SiphonTypeGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 8, N'حذف', N'', 1, 3, N'SiphonTypeDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 8, N'ویرایش', N'', 1, 4, N'SiphonTypeUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 9, N'مشاهده لیست', N'', 1, 1, N'SiphonMaterialGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 9, N'مشاهده', N'', 1, 2, N'SiphonMaterialGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 9, N'حذف', N'', 1, 3, N'SiphonMaterialDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 9, N'ویرایش', N'', 1, 4, N'SiphonMaterialUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 10, N'مشاهده لیست', N'', 1, 1, N'SiphonDiameterGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 10, N'مشاهده', N'', 1, 2, N'SiphonDiameterGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 10, N'حذف', N'', 1, 3, N'SiphonDiameterDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 10, N'ویرایش', N'', 1, 4, N'SiphonDiameterUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 11, N'مشاهده لیست', N'', 1, 1, N'MeterTypeGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 11, N'مشاهده', N'', 1, 2, N'MeterTypeGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 11, N'حذف', N'', 1, 3, N'MeterTypeDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 11, N'ویرایش', N'', 1, 4, N'MeterTypeUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 11, N'ویرایش', N'', 1, 4, N'MeterTypeUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 12, N'مشاهده لیست', N'', 1, 1, N'MeterMaterialGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 12, N'مشاهده', N'', 1, 2, N'MeterMaterialGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 12, N'حذف', N'', 1, 3, N'MeterMaterialDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 12, N'ویرایش', N'', 1, 4, N'MeterMaterialUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 13, N'مشاهده لیست', N'', 1, 1, N'MeterDiameterGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 13, N'مشاهده', N'', 1, 2, N'MeterDiameterGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 13, N'حذف', N'', 1, 3, N'MeterDiameterDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 13, N'ویرایش', N'', 1, 4, N'MeterDiameterUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 14, N'مشاهده لیست', N'', 1, 1, N'MeterUseTypeGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 14, N'مشاهده', N'', 1, 2, N'MeterUseTypeGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 14, N'حذف', N'', 1, 3, N'MeterUseTypeDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 14, N'ویرایش', N'', 1, 4, N'MeterUseTypeUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 15, N'مشاهده لیست', N'', 1, 1, N'MeterProducerGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 15, N'مشاهده', N'', 1, 2, N'MeterProducerGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 15, N'حذف', N'', 1, 3, N'MeterProducerDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 15, N'ویرایش', N'', 1, 4, N'MeterProducerUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 16, N'مشاهده لیست', N'', 1, 1, N'WaterMeterTagDefinitionGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 16, N'مشاهده', N'', 1, 2, N'WaterMeterTagDefinitionGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 16, N'حذف', N'', 1, 3, N'WaterMeterTagDefinitionDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 16, N'ویرایش', N'', 1, 4, N'WaterMeterTagDefinitionUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 17, N'مشاهده لیست', N'', 1, 1, N'WaterMeterTagGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 17, N'مشاهده', N'', 1, 2, N'WaterMeterTagGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 17, N'حذف', N'', 1, 3, N'WaterMeterTagDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 17, N'ویرایش', N'', 1, 4, N'WaterMeterTagUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 17, N'جستجو', N'', 1, 5, N'WaterMeterTagGetSingleBySearchInput.Search', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 18, N'مشاهده لیست', N'', 1, 1, N'UseStateGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 18, N'مشاهده', N'', 1, 2, N'UseStateGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 18, N'حذف', N'', 1, 3, N'UseStateDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 18, N'ویرایش', N'', 1, 4, N'UseStateUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 19, N'مشاهده لیست', N'', 1, 1, N'WaterMeterGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 19, N'مشاهده', N'', 1, 2, N'WaterMeterGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (19, N'حذف', N'', 1, 3, N'WaterMeterDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (19, N'ویرایش', N'', 1, 4, N'WaterMeterUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (20, N'مشاهده لیست', N'', 1, 1, N'ConstructionTypeGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (20, N'مشاهده', N'', 1, 2, N'ConstructionTypeGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (20, N'حذف', N'', 1, 3, N'ConstructionTypeDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (20, N'ویرایش', N'', 1, 4, N'ConstructionTypeUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (21, N'مشاهده لیست', N'', 1, 1, N'UsageGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (21, N'مشاهده', N'', 1, 2, N'UsageGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (21, N'حذف', N'', 1, 3, N'UsageDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (21, N'ویرایش', N'', 1, 4, N'UsageUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (22, N'مشاهده لیست', N'', 1, 1, N'EstateGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (22, N'مشاهده', N'', 1, 2, N'EstateGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (22, N'حذف', N'', 1, 3, N'EstateDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (22, N'ویرایش', N'', 1, 4, N'EstateUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (23, N'مشاهده لیست', N'', 1, 1, N'FlatGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (23, N'مشاهده', N'', 1, 2, N'FlatGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (23, N'حذف', N'', 1, 3, N'FlatDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (23, N'ویرایش', N'', 1, 4, N'FlatUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (24, N'مشاهده لیست', N'', 1, 1, N'IndividualTypeGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (24, N'مشاهده', N'', 1, 2, N'IndividualTypeGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (24, N'حذف', N'', 1, 3, N'IndividualTypeDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (24, N'ویرایش', N'', 1, 4, N'IndividualTypeUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (25, N'مشاهده لیست', N'', 1, 1, N'IndividualEstateRelationTypeGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (25, N'مشاهده', N'', 1, 2, N'IndividualEstateRelationTypeGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (25, N'حذف', N'', 1, 3, N'IndividualEstateRelationTypeDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (25, N'ویرایش', N'', 1, 4, N'IndividualEstateRelationTypeUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (26, N'مشاهده لیست', N'', 1, 1, N'IndividualGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (26, N'مشاهده', N'', 1, 2, N'IndividualGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (26, N'حذف', N'', 1, 3, N'IndividualDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (26, N'ویرایش', N'', 1, 4, N'IndividualUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (27, N'مشاهده لیست', N'', 1, 1, N'IndividualTagDefinitionGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (27, N'مشاهده', N'', 1, 2, N'IndividualTagDefinitionGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (27, N'حذف', N'', 1, 3, N'IndividualTagDefinitionDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (27, N'ویرایش', N'', 1, 4, N'IndividualTagDefinitionUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (28, N'مشاهده لیست', N'', 1, 1, N'IndividualTagGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (28, N'مشاهده', N'', 1, 2, N'IndividualTagGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (28, N'حذف', N'', 1, 3, N'IndividualTagDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (28, N'ویرایش', N'', 1, 4, N'IndividualTagUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (28, N'جستجو', N'', 1, 5, N'IndividualTagGetSingleBySearchInput.Search', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (29, N'مشاهده لیست', N'', 1, 1, N'CountryGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (29, N'مشاهده', N'', 1, 2, N'CountryGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (29, N'حذف', N'', 1, 3, N'CountryDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (29, N'ویرایش', N'', 1, 4, N'CountryUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (30, N'مشاهده لیست', N'', 1, 1, N'CordinalDirectionGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (30, N'مشاهده', N'', 1, 2, N'CordinalDirectionGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (30, N'حذف', N'', 1, 3, N'CordinalDirectionDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (30, N'ویرایش', N'', 1, 4, N'CordinalDirectionUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (31, N'مشاهده لیست', N'', 1, 1, N'ProvinceGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (31, N'مشاهده', N'', 1, 2, N'ProvinceGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (31, N'حذف', N'', 1, 3, N'ProvinceDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (31, N'ویرایش', N'', 1, 4, N'ProvinceUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (32, N'مشاهده لیست', N'', 1, 1, N'HeadquartersGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (32, N'مشاهده', N'', 1, 2, N'HeadquartersGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (32, N'حذف', N'', 1, 3, N'HeadquartersDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (32, N'ویرایش', N'', 1, 4, N'HeadquartersUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (33, N'مشاهده لیست', N'', 1, 1, N'RegionGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (33, N'مشاهده', N'', 1, 2, N'RegionGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (33, N'حذف', N'', 1, 3, N'RegionDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (33, N'ویرایش', N'', 1, 4, N'RegionUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (34, N'مشاهده لیست', N'', 1, 1, N'ZoneGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (34, N'مشاهده', N'', 1, 2, N'ZoneGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (34, N'حذف', N'', 1, 3, N'ZoneDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (34, N'ویرایش', N'', 1, 4, N'ZoneUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (35, N'مشاهده لیست', N'', 1, 1, N'MunicipalityGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (35, N'مشاهده', N'', 1, 2, N'MunicipalityGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (35, N'حذف', N'', 1, 3, N'MunicipalityDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (35, N'ویرایش', N'', 1, 4, N'MunicipalityUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (36, N'مشاهده لیست', N'', 1, 1, N'ReadingBoundGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (36, N'مشاهده', N'', 1, 2, N'ReadingBoundGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (36, N'حذف', N'', 1, 3, N'ReadingBoundDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (36, N'ویرایش', N'', 1, 4, N'ReadingBoundUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (37, N'مشاهده لیست', N'', 1, 1, N'ReadingBlockGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (37, N'مشاهده', N'', 1, 2, N'ReadingBlockGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (37, N'حذف', N'', 1, 3, N'ReadingBlockDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (37, N'ویرایش', N'', 1, 4, N'ReadingBlockUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (38, N'ایجاد', N'', 1, 4, N'TotalApiCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (39, N'ایجاد', N'', 1, 1, N'CompanyServiceCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (39, N'مشاهده لیست', N'', 1, 2, N'CompanyServiceGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (39, N'مشاهده', N'', 1, 3, N'CompanyServiceGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (39, N'حذف', N'', 1, 4, N'CompanyServiceDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (39, N'ویرایش', N'', 1, 5, N'CompanyServiceUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (40, N'ایجاد', N'', 1, 1, N'CompanyServiceTypeCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (40, N'مشاهده لیست', N'', 1, 2, N'CompanyServiceTypeGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (40, N'مشاهده', N'', 1, 3, N'CompanyServiceTypeGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (40, N'حذف', N'', 1, 4, N'CompanyServiceTypeDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (40, N'ویرایش', N'', 1, 5, N'CompanyServiceTypeUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (41, N'ایجاد', N'', 1, 1, N'CompanyServiceOfferingCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (41, N'مشاهده لیست', N'', 1, 2, N'CompanyServiceOfferingGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (41, N'مشاهده', N'', 1, 3, N'CompanyServiceOfferingGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (41, N'حذف', N'', 1, 4, N'CompanyServiceOfferingDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (41, N'ویرایش', N'', 1, 5, N'CompanyServiceOfferingUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (42, N'ایجاد', N'', 1, 1, N'ImpactSignCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (42, N'مشاهده لیست', N'', 1, 2, N'ImpactSignGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (42, N'مشاهده', N'', 1, 3, N'ImpactSignGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (42, N'حذف', N'', 1, 4, N'ImpactSignDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (42, N'ویرایش', N'', 1, 5, N'ImpactSignUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (43, N'ایجاد', N'', 1, 1, N'InvoiceCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (43, N'مشاهده لیست', N'', 1, 2, N'InvoiceGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (43, N'مشاهده', N'', 1, 3, N'InvoiceGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (43, N'حذف', N'', 1, 4, N'InvoiceDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (43, N'ویرایش', N'', 1, 5, N'InvoiceUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (44, N'ایجاد', N'', 1, 1, N'InvoiceInstallmentCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (44, N'مشاهده لیست', N'', 1, 2, N'InvoiceInstallmentGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (44, N'مشاهده', N'', 1, 3, N'InvoiceInstallmentGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (44, N'حذف', N'', 1, 4, N'InvoiceInstallmentDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (44, N'ویرایش', N'', 1, 5, N'InvoiceInstallmentUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (45, N'ایجاد', N'', 1, 1, N'InvoiceLineItemCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (45, N'مشاهده لیست', N'', 1, 2, N'InvoiceLineItemGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (45, N'مشاهده', N'', 1, 3, N'InvoiceLineItemGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (45, N'حذف', N'', 1, 4, N'InvoiceLineItemDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (45, N'ویرایش', N'', 1, 5, N'InvoiceLineItemUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (46, N'ایجاد', N'', 1, 1, N'InsertModeCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (46, N'مشاهده لیست', N'', 1, 2, N'InsertModeGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (46, N'مشاهده', N'', 1, 3, N'InsertModeGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (46, N'حذف', N'', 1, 4, N'InsertModeDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (46, N'ویرایش', N'', 1, 5, N'InsertModeUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (47, N'ایجاد', N'', 1, 1, N'InvoiceTypeCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (47, N'مشاهده لیست', N'', 1, 2, N'InvoiceTypeGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (47, N'مشاهده', N'', 1, 3, N'InvoiceTypeGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (47, N'حذف', N'', 1, 4, N'InvoiceTypeDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (47, N'ویرایش', N'', 1, 5, N'InvoiceTypeUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (48, N'ایجاد', N'', 1, 1, N'InvoiceStatusCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (48, N'مشاهده لیست', N'', 1, 2, N'InvoiceStatusGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (48, N'مشاهده', N'', 1, 3, N'InvoiceStatusGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (48, N'حذف', N'', 1, 4, N'InvoiceStatusDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (48, N'ویرایش', N'', 1, 5, N'InvoiceStatusUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (49, N'ایجاد', N'', 1, 1, N'LineItemTypeCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (49, N'مشاهده لیست', N'', 1, 2, N'LineItemTypeGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (49, N'مشاهده', N'', 1, 3, N'LineItemTypeGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (49, N'حذف', N'', 1, 4, N'LineItemTypeDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (49, N'ویرایش', N'', 1, 5, N'LineItemTypeUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (50, N'ایجاد', N'', 1, 1, N'LineItemTypeGroupCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (50, N'مشاهده لیست', N'', 1, 2, N'LineItemTypeGroupGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (50, N'مشاهده', N'', 1, 3, N'LineItemTypeGroupGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (50, N'حذف', N'', 1, 4, N'LineItemTypeGroupDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (50, N'ویرایش', N'', 1, 5, N'LineItemTypeGroupUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (51, N'ایجاد', N'', 1, 1, N'OfferingCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (51, N'مشاهده لیست', N'', 1, 2, N'OfferingGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (51, N'مشاهده', N'', 1, 3, N'OfferingGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (51, N'حذف', N'', 1, 4, N'OfferingDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (51, N'ویرایش', N'', 1, 5, N'OfferingUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (52, N'ایجاد', N'', 1, 1, N'OfferingGroupCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (52, N'مشاهده لیست', N'', 1, 2, N'OfferingGroupGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (52, N'مشاهده', N'', 1, 3, N'OfferingGroupGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (52, N'حذف', N'', 1, 4, N'OfferingGroupDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (52, N'ویرایش', N'', 1, 5, N'OfferingGroupUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (53, N'ایجاد', N'', 1, 1, N'OfferingUnitCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (53, N'مشاهده لیست', N'', 1, 2, N'OfferingUnitGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (53, N'مشاهده', N'', 1, 3, N'OfferingUnitGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (53, N'حذف', N'', 1, 4, N'OfferingUnitDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (53, N'ویرایش', N'', 1, 5, N'OfferingUnitUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (54, N'ایجاد', N'', 1, 1, N'SupportedOperatorCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (54, N'مشاهده لیست', N'', 1, 2, N'SupportedOperatorGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (54, N'مشاهده', N'', 1, 3, N'SupportedOperatorGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (54, N'حذف', N'', 1, 4, N'SupportedOperatorDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (54, N'ویرایش', N'', 1, 5, N'SupportedOperatorUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (55, N'ایجاد', N'', 1, 1, N'SupportedFieldCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (55, N'مشاهده لیست', N'', 1, 2, N'SupportedFieldGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (55, N'مشاهده', N'', 1, 3, N'SupportedFieldGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (55, N'حذف', N'', 1, 4, N'SupportedFieldDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (55, N'ویرایش', N'', 1, 5, N'SupportedFieldUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (56, N'ایجاد', N'', 1, 1, N'TariffCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (56, N'مشاهده لیست', N'', 1, 2, N'TariffGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (56, N'مشاهده', N'', 1, 3, N'TariffGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (56, N'حذف', N'', 1, 4, N'TariffDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (56, N'ویرایش', N'', 1, 5, N'TariffUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (57, N'ایجاد', N'', 1, 1, N'TariffCalculationModeCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (57, N'مشاهده لیست', N'', 1, 2, N'TariffCalculationModeGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (57, N'مشاهده', N'', 1, 3, N'TariffCalculationModeGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (57, N'حذف', N'', 1, 4, N'TariffCalculationModeDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (57, N'ویرایش', N'', 1, 5, N'TariffCalculationModeUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (58, N'ایجاد', N'', 1, 1, N'TariffConstantCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (58, N'مشاهده لیست', N'', 1, 2, N'TariffConstantGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (58, N'مشاهده', N'', 1, 3, N'TariffConstantGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (58, N'حذف', N'', 1, 4, N'TariffConstantDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (58, N'ویرایش', N'', 1, 5, N'TariffConstantUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (59, N'ایجاد', N'', 1, 1, N'DocumentEntityCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (59, N'مشاهده لیست', N'', 1, 2, N'DocumentEntityGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (59, N'مشاهده', N'', 1, 3, N'DocumentEntityGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (59, N'حذف', N'', 1, 4, N'DocumentEntityDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (59, N'ویرایش', N'', 1, 5, N'DocumentEntityUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (60, N'ایجاد', N'', 1, 1, N'CounterStateCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (60, N'مشاهده لیست', N'', 1, 2, N'CounterStateGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (60, N'مشاهده', N'', 1, 3, N'CounterStateGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (60, N'حذف', N'', 1, 4, N'CounterStateDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (60, N'ویرایش', N'', 1, 5, N'CounterStateUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (61, N'ایجاد', N'', 1, 1, N'ReadingPeriodTypeCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (61, N'مشاهده لیست', N'', 1, 2, N'ReadingPeriodTypeGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (61, N'مشاهده', N'', 1, 3, N'ReadingPeriodTypeGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (61, N'حذف', N'', 1, 4, N'ReadingPeriodTypeDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (61, N'ویرایش', N'', 1, 5, N'ReadingPeriodTypeUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (62, N'ایجاد', N'', 1, 1, N'ReadingPeriodCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (62, N'مشاهده لیست', N'', 1, 2, N'ReadingPeriodGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (62, N'مشاهده', N'', 1, 3, N'ReadingPeriodGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (62, N'حذف', N'', 1, 4, N'ReadingPeriodDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (62, N'ویرایش', N'', 1, 5, N'ReadingPeriodUpdate.Update', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (63, N'ایجاد', N'', 1, 1, N'ReadingConfigDefaultCreate.Create', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (63, N'مشاهده لیست', N'', 1, 2, N'ReadingConfigDefaultGetAll.GetAll', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (63, N'مشاهده', N'', 1, 3, N'ReadingConfigDefaultGetSingle.GetSingle', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (63, N'حذف', N'', 1, 4, N'ReadingConfigDefaultDelete.Delete', 1)
GO
INSERT [UserPool].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (63, N'ویرایش', N'', 1, 5, N'ReadingConfigDefaultUpdate.Update', 1)
GO
SET IDENTITY_INSERT [UserPool].[Endpoint] OFF
GO