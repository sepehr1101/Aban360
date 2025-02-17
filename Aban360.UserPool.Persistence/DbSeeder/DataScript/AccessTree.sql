USE [Aban360]
GO
SET IDENTITY_INSERT [dbo].[App] ON 
GO
INSERT [dbo].[App] (  [Title], [Style], [InMenu], [LogicalOrder], [IsActive]) values ( N'امنیت', N'', 1, 1, 1)
GO
INSERT [dbo].[App] (  [Title], [Style], [InMenu], [LogicalOrder], [IsActive]) values ( N'اطلاعات مشترکین', N'', 1, 1, 1)
GO
SET IDENTITY_INSERT [dbo].[App] OFF
GO
SET IDENTITY_INSERT [dbo].[Module] ON 
GO
INSERT [dbo].[Module] (  [AppId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) values ( 1, N'مدیریت کاربران', N'', 1, 1, N'', 1)
GO
INSERT [dbo].[Module] (  [AppId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) values ( 1, N'تصاویر امنیتی', N'', 1, 1, N'', 1)
GO
INSERT [dbo].[Module] (  [AppId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) values ( 1, N'مدیریت نقش ها', N'', 1, 1, N'', 1)
GO
INSERT [dbo].[Module] (  [AppId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) values ( 2, N'مدیریت سیفون', N'', 1, 1, N'', 1)
GO
INSERT [dbo].[Module] (  [AppId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) values ( 2, N'مدیریت کنتور', N'', 1, 1, N'', 1)
GO
INSERT [dbo].[Module] (  [AppId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) values ( 2, N'مدیریت ملک', N'', 1, 1, N'', 1)
GO
INSERT [dbo].[Module] (  [AppId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) values ( 2, N'مدیریت اشخاص', N'', 1, 1, N'', 1)
GO
INSERT [dbo].[Module] (  [AppId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) values ( 2, N'مدیریت نواحی', N'', 1, 1, N'', 1)
GO
INSERT [dbo].[Module] (  [AppId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) values ( 2, N'انشعاب', N'', 1, 1, N'', 1)
GO
SET IDENTITY_INSERT [dbo].[Module] OFF
GO
SET IDENTITY_INSERT [dbo].[SubModule] ON 
GO
INSERT [dbo].[SubModule] (  [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) values ( 1, N'جدول کاربران', N'', 1, 1, N'', 1)
GO
INSERT [dbo].[SubModule] (  [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) values ( 1, N'افزودن کاربر', N'', 1, 2, N'', 1)
GO
INSERT [dbo].[SubModule] (  [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) values ( 2, N'مدیریت و افزودن', N'', 1, 1, N'', 1)
GO
INSERT [dbo].[SubModule] (  [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) values ( 2, N'زبان', N'', 1, 2, N'', 1)
GO
INSERT [dbo].[SubModule] (  [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) values ( 2, N'حالت', N'', 1, 3, N'', 1)
GO
INSERT [dbo].[SubModule] (  [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) values ( 3, N'جدول نقش ها', N'', 1, 1, N'', 1)
GO
INSERT [dbo].[SubModule] (  [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) values ( 3, N'افزودن', N'', 1, 2, N'', 1)
GO
INSERT [dbo].[SubModule] (  [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) values ( 4, N'نوع سیفون', N'', 1, 1, N'', 1)
GO
INSERT [dbo].[SubModule] (  [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) values ( 4, N'جنس سیفون', N'', 1, 2, N'', 1)
GO
INSERT [dbo].[SubModule] (  [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) values ( 4, N'قطر سیفون', N'', 1, 3, N'', 1)
GO
INSERT [dbo].[SubModule] (  [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) values ( 5, N'نوع کنتور', N'', 1, 1, N'', 1)
GO
INSERT [dbo].[SubModule] (  [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) values ( 5, N'جنس  کنتور', N'', 1, 2, N'', 1)
GO
INSERT [dbo].[SubModule] (  [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) values ( 5, N'قطر کنتور', N'', 1, 3, N'', 1)
GO
INSERT [dbo].[SubModule] (  [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) values ( 5, N'نوع استفاده', N'', 1, 4, N'', 1)
GO
INSERT [dbo].[SubModule] (  [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) values ( 5, N'سازنده کنتور', N'', 1, 5, N'', 1)
GO
INSERT [dbo].[SubModule] (  [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) values ( 5, N'مدیریت برچسب', N'', 1, 6, N'', 1)
GO
INSERT [dbo].[SubModule] (  [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) values ( 5, N'انتساب برچسب', N'', 1, 7, N'', 1)
GO
INSERT [dbo].[SubModule] (  [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) values ( 5, N'وضعیت کنتور', N'', 1, 8, N'', 1)
GO
INSERT [dbo].[SubModule] (  [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) values ( 5, N'کنتور', N'', 1, 9, N'', 1)
GO
INSERT [dbo].[SubModule] (  [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) values ( 6, N'نوع سازنده', N'', 1, 1, N'', 1)
GO
INSERT [dbo].[SubModule] (  [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) values ( 6, N'کاربری', N'', 1, 2, N'', 1)
GO
INSERT [dbo].[SubModule] (  [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) values ( 6, N'ملک', N'', 1, 3, N'', 1)
GO
INSERT [dbo].[SubModule] (  [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) values ( 6, N'واحد', N'', 1, 4, N'', 1)
GO
INSERT [dbo].[SubModule] (  [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) values ( 7, N'نوع شخص', N'', 1, 1, N'', 1)
GO
INSERT [dbo].[SubModule] (  [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) values ( 7, N'رابطه', N'', 1, 2, N'', 1)
GO
INSERT [dbo].[SubModule] (  [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) values ( 7, N'اشخاص', N'', 1, 3, N'', 1)
GO
INSERT [dbo].[SubModule] (  [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) values ( 7, N'مدیریت برچسب', N'', 1, 4, N'', 1)
GO
INSERT [dbo].[SubModule] (  [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) values ( 7, N'انتساب برچسب', N'', 1, 5, N'', 1)
GO
INSERT [dbo].[SubModule] (  [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) values ( 8, N'کشور', N'', 1, 1, N'', 1)
GO
INSERT [dbo].[SubModule] (  [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) values ( 8, N'جهت', N'', 1, 2, N'', 1)
GO
INSERT [dbo].[SubModule] (  [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) values ( 8, N'استان', N'', 1, 3, N'', 1)
GO
INSERT [dbo].[SubModule] (  [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) values ( 8, N'نام شرکت', N'', 1, 4, N'', 1)
GO
INSERT [dbo].[SubModule] (  [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) values ( 8, N'منطقه', N'', 1, 5, N'', 1)
GO
INSERT [dbo].[SubModule] (  [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) values ( 8, N'ناحیه', N'', 1, 6, N'', 1)
GO
INSERT [dbo].[SubModule] (  [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) values ( 8, N'شهر/روستا/شهرداری', N'', 1, 7, N'', 1)
GO
INSERT [dbo].[SubModule] (  [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) values ( 8, N'محدوده اشتراکی', N'', 1, 8, N'', 1)
GO
INSERT [dbo].[SubModule] (  [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) values ( 8, N'بلوک اشتراکی', N'', 1, 9, N'', 1)
GO
INSERT [dbo].[SubModule] (  [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) values ( 9, N'مدیریت', N'', 1, 1, N'', 1)
GO
SET IDENTITY_INSERT [dbo].[SubModule] OFF
GO
SET IDENTITY_INSERT [dbo].[Endpoint] ON 
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) values ( 1, N'مشاهده', N'', 1, 1, N'', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) values ( 1, N'حذف', N'', 1, 2, N'UserDeleteManager.Delete', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) values ( 1, N'فعال سازی', N'', 1, 3, N'UserUnLockManager.UnLock', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) values ( 1, N'غیرفعال سازی', N'', 1, 4, N'UserLockManager.Lock', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) values ( 1, N'بازنشانی گذرواژه', N'', 1, 5, N'UserResetPasswordManager.ResetPassword', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) values ( 1, N'مشاهده جدولی', N'', 1, 6, N'UserAllQuery.Get', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) values ( 1, N'مشاهده بصورت Paigination', N'', 1, 7, N'UserGridifyQuery.GetUsersByQuery', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) values ( 2, N'ذخیره', N'', 1, 2, N'UserCreate.Trigger', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) values ( 2, N'مشاهده همه مقادیر', N'', 1, 2, N'', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) values ( 3, N'مشاهده', N'', 1, 1, N'CaptchaList.Read', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) values ( 3, N'ویرایش', N'', 1, 2, N'CaptchaUpdate.Update', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) values ( 3, N'دیکشنری', N'', 1, 3, N'CaptchaDictionary.Get', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) values ( 3, N'پارامترها', N'', 1, 4, N'CaptchaParameter.Read', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) values ( 4, N'مشاهده', N'', 1, 1, N'CaptchaLanguage.Get', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) values ( 5, N'مشاهده', N'', 1, 1, N'CaptchaDisplayMode.Get', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) values ( 6, N'مشاهده همه', N'', 1, 1, N'RoleGetAll.GetAll', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) values ( 6, N'حذف', N'', 1, 2, N'RoleDelete.Delete', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) values ( 6, N'ویرایش', N'', 1, 3, N'RoleUpdate.Update', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) values ( 6, N'مشاهده', N'', 1, 2, N'RoleGetSingle.GetSingle', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) values ( 7, N'مشاهده مقادیر ایجاد', N'', 1, 1, N'RoleQueryParamCreate.GetRoleParamsOfCreate', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) values ( 7, N'ایجاد', N'', 1, 2, N'RoleCreate.Create', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) values ( 8, N'مشاهده لیست', N'', 1, 1, N'SiphonTypeGetAll.GetAll', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) values ( 8, N'مشاهده', N'', 1, 2, N'SiphonTypeGetSingle.GetSingle', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) values ( 8, N'حذف', N'', 1, 3, N'SiphonTypeDelete.Delete', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) values ( 8, N'ویرایش', N'', 1, 4, N'SiphonTypeUpdate.Update', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) values ( 9, N'مشاهده لیست', N'', 1, 1, N'SiphonMaterialGetAll.GetAll', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) values ( 9, N'مشاهده', N'', 1, 2, N'SiphonMaterialGetSingle.GetSingle', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) values ( 9, N'حذف', N'', 1, 3, N'SiphonMaterialDelete.Delete', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) values ( 9, N'ویرایش', N'', 1, 4, N'SiphonMaterialUpdate.Update', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) values ( 10, N'مشاهده لیست', N'', 1, 1, N'SiphonDiameterGetAll.GetAll', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) values ( 10, N'مشاهده', N'', 1, 2, N'SiphonDiameterGetSingle.GetSingle', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) values ( 10, N'حذف', N'', 1, 3, N'SiphonDiameterDelete.Delete', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) values ( 10, N'ویرایش', N'', 1, 4, N'SiphonDiameterUpdate.Update', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) values ( 11, N'مشاهده لیست', N'', 1, 1, N'MeterTypeGetAll.GetAll', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) values ( 11, N'مشاهده', N'', 1, 2, N'MeterTypeGetSingle.GetSingle', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) values ( 11, N'حذف', N'', 1, 3, N'MeterTypeDelete.Delete', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) values ( 11, N'ویرایش', N'', 1, 4, N'MeterTypeUpdate.Update', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) values ( 11, N'ویرایش', N'', 1, 4, N'MeterTypeUpdate.Update', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (12, N'مشاهده لیست', N'', 1, 1, N'MeterMaterialGetAll.GetAll', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (12, N'مشاهده', N'', 1, 2, N'MeterMaterialGetSingle.GetSingle', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (12, N'حذف', N'', 1, 3, N'MeterMaterialDelete.Delete', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (12, N'ویرایش', N'', 1, 4, N'MeterMaterialUpdate.Update', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (13, N'مشاهده لیست', N'', 1, 1, N'MeterDiameterGetAll.GetAll', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (13, N'مشاهده', N'', 1, 2, N'MeterDiameterGetSingle.GetSingle', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (13, N'حذف', N'', 1, 3, N'MeterDiameterDelete.Delete', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (13, N'ویرایش', N'', 1, 4, N'MeterDiameterUpdate.Update', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (14, N'مشاهده لیست', N'', 1, 1, N'MeterUseTypeGetAll.GetAll', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (14, N'مشاهده', N'', 1, 2, N'MeterUseTypeGetSingle.GetSingle', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (14, N'حذف', N'', 1, 3, N'MeterUseTypeDelete.Delete', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (14, N'ویرایش', N'', 1, 4, N'MeterUseTypeUpdate.Update', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (15, N'مشاهده لیست', N'', 1, 1, N'MeterProducerGetAll.GetAll', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (15, N'مشاهده', N'', 1, 2, N'MeterProducerGetSingle.GetSingle', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (15, N'حذف', N'', 1, 3, N'MeterProducerDelete.Delete', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (15, N'ویرایش', N'', 1, 4, N'MeterProducerUpdate.Update', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (16, N'مشاهده لیست', N'', 1, 1, N'WaterMeterTagDefinitionGetAll.GetAll', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (16, N'مشاهده', N'', 1, 2, N'WaterMeterTagDefinitionGetSingle.GetSingle', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (16, N'حذف', N'', 1, 3, N'WaterMeterTagDefinitionDelete.Delete', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (16, N'ویرایش', N'', 1, 4, N'WaterMeterTagDefinitionUpdate.Update', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (17, N'مشاهده لیست', N'', 1, 1, N'WaterMeterTagGetAll.GetAll', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (17, N'مشاهده', N'', 1, 2, N'WaterMeterTagGetSingle.GetSingle', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (17, N'حذف', N'', 1, 3, N'WaterMeterTagDelete.Delete', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (17, N'ویرایش', N'', 1, 4, N'WaterMeterTagUpdate.Update', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (17, N'جستجو', N'', 1, 5, N'WaterMeterTagGetSingleBySearchInput.Search', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (18, N'مشاهده لیست', N'', 1, 1, N'UseStateGetAll.GetAll', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (18, N'مشاهده', N'', 1, 2, N'UseStateGetSingle.GetSingle', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (18, N'حذف', N'', 1, 3, N'UseStateDelete.Delete', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (18, N'ویرایش', N'', 1, 4, N'UseStateUpdate.Update', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (19, N'مشاهده لیست', N'', 1, 1, N'WaterMeterGetAll.GetAll', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (19, N'مشاهده', N'', 1, 2, N'WaterMeterGetSingle.GetSingle', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (19, N'حذف', N'', 1, 3, N'WaterMeterDelete.Delete', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (19, N'ویرایش', N'', 1, 4, N'WaterMeterUpdate.Update', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (20, N'مشاهده لیست', N'', 1, 1, N'ConstructionTypeGetAll.GetAll', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (20, N'مشاهده', N'', 1, 2, N'ConstructionTypeGetSingle.GetSingle', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (20, N'حذف', N'', 1, 3, N'ConstructionTypeDelete.Delete', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (20, N'ویرایش', N'', 1, 4, N'ConstructionTypeUpdate.Update', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (21, N'مشاهده لیست', N'', 1, 1, N'UsageGetAll.GetAll', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (21, N'مشاهده', N'', 1, 2, N'UsageGetSingle.GetSingle', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (21, N'حذف', N'', 1, 3, N'UsageDelete.Delete', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (21, N'ویرایش', N'', 1, 4, N'UsageUpdate.Update', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (22, N'مشاهده لیست', N'', 1, 1, N'EstateGetAll.GetAll', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (22, N'مشاهده', N'', 1, 2, N'EstateGetSingle.GetSingle', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (22, N'حذف', N'', 1, 3, N'EstateDelete.Delete', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (22, N'ویرایش', N'', 1, 4, N'EstateUpdate.Update', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (23, N'مشاهده لیست', N'', 1, 1, N'FlatGetAll.GetAll', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (23, N'مشاهده', N'', 1, 2, N'FlatGetSingle.GetSingle', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (23, N'حذف', N'', 1, 3, N'FlatDelete.Delete', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (23, N'ویرایش', N'', 1, 4, N'FlatUpdate.Update', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (24, N'مشاهده لیست', N'', 1, 1, N'IndividualTypeGetAll.GetAll', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (24, N'مشاهده', N'', 1, 2, N'IndividualTypeGetSingle.GetSingle', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (24, N'حذف', N'', 1, 3, N'IndividualTypeDelete.Delete', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (24, N'ویرایش', N'', 1, 4, N'IndividualTypeUpdate.Update', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (25, N'مشاهده لیست', N'', 1, 1, N'IndividualEstateRelationTypeGetAll.GetAll', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (25, N'مشاهده', N'', 1, 2, N'IndividualEstateRelationTypeGetSingle.GetSingle', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (25, N'حذف', N'', 1, 3, N'IndividualEstateRelationTypeDelete.Delete', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (25, N'ویرایش', N'', 1, 4, N'IndividualEstateRelationTypeUpdate.Update', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (26, N'مشاهده لیست', N'', 1, 1, N'IndividualGetAll.GetAll', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (26, N'مشاهده', N'', 1, 2, N'IndividualGetSingle.GetSingle', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (26, N'حذف', N'', 1, 3, N'IndividualDelete.Delete', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 26, N'ویرایش', N'', 1, 4, N'IndividualUpdate.Update', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 27, N'مشاهده لیست', N'', 1, 1, N'IndividualTagDefinitionGetAll.GetAll', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 27, N'مشاهده', N'', 1, 2, N'IndividualTagDefinitionGetSingle.GetSingle', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 27, N'حذف', N'', 1, 3, N'IndividualTagDefinitionDelete.Delete', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 27, N'ویرایش', N'', 1, 4, N'IndividualTagDefinitionUpdate.Update', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 28, N'مشاهده لیست', N'', 1, 1, N'IndividualTagGetAll.GetAll', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 28, N'مشاهده', N'', 1, 2, N'IndividualTagGetSingle.GetSingle', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 28, N'حذف', N'', 1, 3, N'IndividualTagDelete.Delete', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 28, N'ویرایش', N'', 1, 4, N'IndividualTagUpdate.Update', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 28, N'جستجو', N'', 1, 5, N'IndividualTagGetSingleBySearchInput.Search', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 29, N'مشاهده لیست', N'', 1, 1, N'CountryGetAll.GetAll', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 29, N'مشاهده', N'', 1, 2, N'CountryGetSingle.GetSingle', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 29, N'حذف', N'', 1, 3, N'CountryDelete.Delete', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 29, N'ویرایش', N'', 1, 4, N'CountryUpdate.Update', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 30, N'مشاهده لیست', N'', 1, 1, N'CordinalDirectionGetAll.GetAll', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 30, N'مشاهده', N'', 1, 2, N'CordinalDirectionGetSingle.GetSingle', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 30, N'حذف', N'', 1, 3, N'CordinalDirectionDelete.Delete', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 30, N'ویرایش', N'', 1, 4, N'CordinalDirectionUpdate.Update', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 31, N'مشاهده لیست', N'', 1, 1, N'ProvinceGetAll.GetAll', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 31, N'مشاهده', N'', 1, 2, N'ProvinceGetSingle.GetSingle', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 31, N'حذف', N'', 1, 3, N'ProvinceDelete.Delete', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 31, N'ویرایش', N'', 1, 4, N'ProvinceUpdate.Update', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 32, N'مشاهده لیست', N'', 1, 1, N'HeadquartersGetAll.GetAll', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 32, N'مشاهده', N'', 1, 2, N'HeadquartersGetSingle.GetSingle', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 32, N'حذف', N'', 1, 3, N'HeadquartersDelete.Delete', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 32, N'ویرایش', N'', 1, 4, N'HeadquartersUpdate.Update', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 33, N'مشاهده لیست', N'', 1, 1, N'RegionGetAll.GetAll', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 33, N'مشاهده', N'', 1, 2, N'RegionGetSingle.GetSingle', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 33, N'حذف', N'', 1, 3, N'RegionDelete.Delete', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 33, N'ویرایش', N'', 1, 4, N'RegionUpdate.Update', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 34, N'مشاهده لیست', N'', 1, 1, N'ZoneGetAll.GetAll', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 34, N'مشاهده', N'', 1, 2, N'ZoneGetSingle.GetSingle', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 34, N'حذف', N'', 1, 3, N'ZoneDelete.Delete', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 34, N'ویرایش', N'', 1, 4, N'ZoneUpdate.Update', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 35, N'مشاهده لیست', N'', 1, 1, N'MunicipalityGetAll.GetAll', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 35, N'مشاهده', N'', 1, 2, N'MunicipalityGetSingle.GetSingle', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 35, N'حذف', N'', 1, 3, N'MunicipalityDelete.Delete', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 35, N'ویرایش', N'', 1, 4, N'MunicipalityUpdate.Update', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 36, N'مشاهده لیست', N'', 1, 1, N'ReadingBoundGetAll.GetAll', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 36, N'مشاهده', N'', 1, 2, N'ReadingBoundGetSingle.GetSingle', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 36, N'حذف', N'', 1, 3, N'ReadingBoundDelete.Delete', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 36, N'ویرایش', N'', 1, 4, N'ReadingBoundUpdate.Update', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 37, N'مشاهده لیست', N'', 1, 1, N'ReadingBlockGetAll.GetAll', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 37, N'مشاهده', N'', 1, 2, N'ReadingBlockGetSingle.GetSingle', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 37, N'حذف', N'', 1, 3, N'ReadingBlockDelete.Delete', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 37, N'ویرایش', N'', 1, 4, N'ReadingBlockUpdate.Update', 1)
GO
INSERT [dbo].[Endpoint] (  [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 38, N'ایجاد', N'', 1, 4, N'TotalApiCreate.Create', 1)
GO
SET IDENTITY_INSERT [dbo].[Endpoint] OFF
GO