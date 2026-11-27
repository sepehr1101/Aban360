USE [Aban360]
GO
INSERT [TaxPool].[MaaherInvoiceStatus] ( [Title], [Description]) VALUES (N'VALIDATING', N'در حال بررسی صحت اطلاعات وارد شده در سیستم معتمد')
GO
INSERT [TaxPool].[MaaherInvoiceStatus] ( [Title], [Description]) VALUES (N'VALIDATION_FAILED', N'فرایند صحت اطلاعات با شکست مواجد شد. دلایل شکست در قسمت errors وجود دارند')
GO
INSERT [TaxPool].[MaaherInvoiceStatus] ( [Title], [Description]) VALUES (N'PERSISTING', N'در حال ذخیره سازی اطلاعات در سیستم معتمد')
GO
INSERT [TaxPool].[MaaherInvoiceStatus] ( [Title], [Description]) VALUES (N'PERSISTING_FAILED', N'فرایند ذخیره سازی اطلاعات در سیستم معتمد با شکست مواجه شد. دلایل شکست در قسمت errors وجود دارند')
GO
INSERT [TaxPool].[MaaherInvoiceStatus] ( [Title], [Description]) VALUES (N'QUEUED', N'در صف ارسال معتمد قرار گرفت')
GO
INSERT [TaxPool].[MaaherInvoiceStatus] ( [Title], [Description]) VALUES (N'POSTED', N'به هنگام ارسال فاکتور برای سیستم مالیاتی سیستم با شکست مواجه شد. دلایل شکست در قسمت errors وجود دارند')
GO
INSERT [TaxPool].[MaaherInvoiceStatus] ( [Title], [Description]) VALUES (N'FAILED_WHILE_SENDING', N'فاکتور برای سیستم امور مالیاتی ارسال شد در این مرحله شناسه ثبت فاکتور در کلید reference_number در ریکوست وجود دارد')
GO
INSERT [TaxPool].[MaaherInvoiceStatus] ( [Title], [Description]) VALUES (N'FAILED_WHILE_CHECKING', N'در حال بررسی وضعیت فاکتور از سمت سازمان')
GO
INSERT [TaxPool].[MaaherInvoiceStatus] ( [Title], [Description]) VALUES (N'PENDING', NULL)
GO
INSERT [TaxPool].[MaaherInvoiceStatus] ( [Title], [Description]) VALUES (N'SUCCESS', N'فاکتور با موفقیت در سیستم مالیاتی مورد تایید قرار گرفت')
GO
INSERT [TaxPool].[MaaherInvoiceStatus] ( [Title], [Description]) VALUES (N'FAILED', N'فاکتور در سیستم مالیاتی مورد تایید قرار نگرفت. دلایل عدم تایید در قسمت tax_errors و tax_warnings وجود دارند')
GO