USE [Aban360]
GO
SET IDENTITY_INSERT [dbo].[App] ON 
GO
INSERT [dbo].[App] ( [Title], [Style], [InMenu], [LogicalOrder], [IsActive]) VALUES ( N'امنیت', N'', 1, 1, 1)
GO
SET IDENTITY_INSERT [dbo].[App] OFF
GO
SET IDENTITY_INSERT [dbo].[Module] ON 
GO
INSERT [dbo].[Module] ( [AppId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 1, N'مدیریت کاربران', N'', 1, 1, N'', 1)
GO
INSERT [dbo].[Module] ( [AppId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 1, N'تصاویر امنیتی', N'', 1, 1, N'', 1)
GO
INSERT [dbo].[Module] ( [AppId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 1, N'مدیریت نقش ها', N'', 1, 1, N'', 1)
GO
SET IDENTITY_INSERT [dbo].[Module] OFF
GO
SET IDENTITY_INSERT [dbo].[SubModule] ON 
GO
INSERT [dbo].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 1, N'جدول کاربران', N'', 1, 1, N'', 1)
GO
INSERT [dbo].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 1, N'افزودن کاربر', N'', 1, 2, N'', 1)
GO
INSERT [dbo].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 2, N'مدیریت و افزودن', N'', 1, 1, N'', 1)
GO
INSERT [dbo].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 2, N'زبان', N'', 1, 2, N'', 1)
GO
INSERT [dbo].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 2, N'حالت', N'', 1, 3, N'', 1)
GO
INSERT [dbo].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 3, N'جدول نقش ها', N'', 1, 1, N'', 1)
GO
INSERT [dbo].[SubModule] ( [ModuleId], [Title], [Style], [InMenu], [LogicalOrder], [ClientRoute], [IsActive]) VALUES ( 3, N'افزودن', N'', 1, 2, N'', 1)
GO
SET IDENTITY_INSERT [dbo].[SubModule] OFF
GO
SET IDENTITY_INSERT [dbo].[Endpoint] ON 
GO
INSERT [dbo].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (1, N'مشاهده', N'', 1, 1, N'', 1)
GO
INSERT [dbo].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 1, N'حذف', N'', 1, 2, N'UserDeleteManager.Delete', 1)
GO
INSERT [dbo].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 1, N'فعال سازی', N'', 1, 3, N'UserUnLockManager.UnLock', 1)
GO
INSERT [dbo].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 1, N'غیرفعال سازی', N'', 1, 4, N'UserLockManager.Lock', 1)
GO
INSERT [dbo].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 1, N'بازنشانی گذرواژه', N'', 1, 5, N'UserResetPasswordManager.ResetPassword', 1)
GO
INSERT [dbo].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 1, N'مشاهده جدولی', N'', 1, 6, N'UserAllQuery.Get', 1)
GO
INSERT [dbo].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 1, N'مشاهده بصورت Paigination', N'', 1, 7, N'UserGridifyQuery.GetUsersByQuery', 1)
GO
INSERT [dbo].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 2, N'ذخیره', N'', 1, 2, N'UserCreate.Trigger', 1)
GO
INSERT [dbo].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 2, N'مشاهده همه مقادیر', N'', 1, 2, N'', 1)
GO
INSERT [dbo].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 3, N'مشاهده', N'', 1, 1, N'CaptchaList.Read', 1)
GO
INSERT [dbo].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 3, N'ویرایش', N'', 1, 2, N'CaptchaUpdate.Update', 1)
GO
INSERT [dbo].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 3, N'دیکشنری', N'', 1, 3, N'CaptchaDictionary.Get', 1)
GO
INSERT [dbo].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES (3, N'پارامترها', N'', 1, 4, N'CaptchaParameter.Read', 1)
GO
INSERT [dbo].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 4, N'مشاهده', N'', 1, 1, N'CaptchaLanguage.Get', 1)
GO
INSERT [dbo].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 5, N'مشاهده', N'', 1, 1, N'CaptchaDisplayMode.Get', 1)
GO
INSERT [dbo].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 6, N'مشاهده', N'', 1, 1, N'RoleGetAll.GetAll', 1)
GO
INSERT [dbo].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 6, N'حذف', N'', 1, 2, N'RoleDelete.Delete', 1)
GO
INSERT [dbo].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 6, N'ویرایش', N'', 1, 3, N'RoleUpdate.Update', 1)
GO
INSERT [dbo].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 7, N'مشاهده مقادیر', N'', 1, 1, N'', 1)
GO
INSERT [dbo].[Endpoint] ( [SubModuleId], [Title], [Style], [InMenu], [LogicalOrder], [AuthValue], [IsActive]) VALUES ( 7, N'ذخیره', N'', 1, 2, N'RoleCreate.Create', 1)
GO
SET IDENTITY_INSERT [dbo].[Endpoint] OFF
GO
