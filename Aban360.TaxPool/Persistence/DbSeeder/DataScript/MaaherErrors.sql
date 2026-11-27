USE [Aban360]
GO
INSERT [TaxPool].[MaaherErrors] ([ErrorCode], [HttpStatus], [Description]) VALUES (1, 200, N'درخواست موفق')
GO
INSERT [TaxPool].[MaaherErrors] ([ErrorCode], [HttpStatus], [Description]) VALUES (40001, 400, N'خطای اعتبارسنجی داده‌ها')
GO
INSERT [TaxPool].[MaaherErrors] ([ErrorCode], [HttpStatus], [Description]) VALUES (40002, 400, N'خطای فرمت UUID')
GO
INSERT [TaxPool].[MaaherErrors] ([ErrorCode], [HttpStatus], [Description]) VALUES (40003, 400, N'خطای شناسه مالیاتی')
GO
INSERT [TaxPool].[MaaherErrors] ([ErrorCode], [HttpStatus], [Description]) VALUES (40004, 400, N'خطای الگوی ناشناخته')
GO
INSERT [TaxPool].[MaaherErrors] ([ErrorCode], [HttpStatus], [Description]) VALUES (40101, 401, N'توکن احراز هویت لازم است')
GO
INSERT [TaxPool].[MaaherErrors] ([ErrorCode], [HttpStatus], [Description]) VALUES (40102, 401, N'توکن نامعتبر')
GO
INSERT [TaxPool].[MaaherErrors] ([ErrorCode], [HttpStatus], [Description]) VALUES (40103, 401, N'خطای احراز هویت مؤدی مالی')
GO
INSERT [TaxPool].[MaaherErrors] ([ErrorCode], [HttpStatus], [Description]) VALUES (40104, 401, N'خطای شناسه احراز هویت')
GO
INSERT [TaxPool].[MaaherErrors] ([ErrorCode], [HttpStatus], [Description]) VALUES (40301, 403, N'دسترسی ممنوع')
GO
INSERT [TaxPool].[MaaherErrors] ([ErrorCode], [HttpStatus], [Description]) VALUES (40401, 404, N'منبع یافت نشد')
GO
INSERT [TaxPool].[MaaherErrors] ([ErrorCode], [HttpStatus], [Description]) VALUES (42901, 429, N'محدودیت نرخ فراخوانی')
GO
INSERT [TaxPool].[MaaherErrors] ([ErrorCode], [HttpStatus], [Description]) VALUES (42902, 429, N'محدودیت نرخ OTP')
GO
INSERT [TaxPool].[MaaherErrors] ([ErrorCode], [HttpStatus], [Description]) VALUES (50001, 500, N'خطای بازیابی سفارشی')
GO
INSERT [TaxPool].[MaaherErrors] ([ErrorCode], [HttpStatus], [Description]) VALUES (50002, 500, N'خطای داخلی سرور')
GO