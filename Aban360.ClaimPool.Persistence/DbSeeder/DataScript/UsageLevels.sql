USE [Aban360]
GO
INSERT [ClaimPool].[UsageLevel1] ([Id], [Title]) VALUES (1, N'خانگی')
GO
INSERT [ClaimPool].[UsageLevel1] ([Id], [Title]) VALUES (2, N'غیر خانگی')
GO
INSERT [ClaimPool].[UsageLevel2] ([Id], [Title], [UsageLevel1Id]) VALUES (10, N'خانگی', 1)
GO
INSERT [ClaimPool].[UsageLevel2] ([Id], [Title], [UsageLevel1Id]) VALUES (15, N'اقامتگاه غیر دائم', 1)
GO
INSERT [ClaimPool].[UsageLevel2] ([Id], [Title], [UsageLevel1Id]) VALUES (20, N'صنعتی', 2)
GO
INSERT [ClaimPool].[UsageLevel2] ([Id], [Title], [UsageLevel1Id]) VALUES (25, N'عمومی و دستگاه اجرایی', 2)
GO
INSERT [ClaimPool].[UsageLevel2] ([Id], [Title], [UsageLevel1Id]) VALUES (30, N'تجاری و مراکز خدمات غیر دولتی', 2)
GO
INSERT [ClaimPool].[UsageLevel2] ([Id], [Title], [UsageLevel1Id]) VALUES (35, N'آب آزاد و بنایی', 2)
GO
INSERT [ClaimPool].[UsageLevel2] ([Id], [Title], [UsageLevel1Id]) VALUES (40, N'آموزشی و اماکن مذهبی', 2)
GO
INSERT [ClaimPool].[UsageLevel2] ([Id], [Title], [UsageLevel1Id]) VALUES (45, N'آب شیرین کن', 2)
GO
INSERT [ClaimPool].[UsageLevel2] ([Id], [Title], [UsageLevel1Id]) VALUES (50, N'گرمابه', 2)
GO
INSERT [ClaimPool].[UsageLevel2] ([Id], [Title], [UsageLevel1Id]) VALUES (55, N'موارد خاص', 2)
GO
INSERT [ClaimPool].[UsageLevel2] ([Id], [Title], [UsageLevel1Id]) VALUES (60, N'سایر', 2)
GO
INSERT [ClaimPool].[UsageLevel3] ([Id], [Title], [UsageLevel2Id]) VALUES (100, N'خانگی', 10)
GO
INSERT [ClaimPool].[UsageLevel3] ([Id], [Title], [UsageLevel2Id]) VALUES (150, N'اقامتگاه غیر دائم', 15)
GO
INSERT [ClaimPool].[UsageLevel3] ([Id], [Title], [UsageLevel2Id]) VALUES (200, N'صنایع', 20)
GO
INSERT [ClaimPool].[UsageLevel3] ([Id], [Title], [UsageLevel2Id]) VALUES (205, N'گردشگری', 20)
GO
INSERT [ClaimPool].[UsageLevel3] ([Id], [Title], [UsageLevel2Id]) VALUES (250, N'عمومی', 25)
GO
INSERT [ClaimPool].[UsageLevel3] ([Id], [Title], [UsageLevel2Id]) VALUES (255, N'دستگاه اجرایی', 25)
GO
INSERT [ClaimPool].[UsageLevel3] ([Id], [Title], [UsageLevel2Id]) VALUES (260, N'فضای سبز', 25)
GO
INSERT [ClaimPool].[UsageLevel3] ([Id], [Title], [UsageLevel2Id]) VALUES (265, N'نانوایی', 25)
GO
INSERT [ClaimPool].[UsageLevel3] ([Id], [Title], [UsageLevel2Id]) VALUES (270, N'نیروی مسلح', 25)
GO
INSERT [ClaimPool].[UsageLevel3] ([Id], [Title], [UsageLevel2Id]) VALUES (300, N'تجاری', 30)
GO
INSERT [ClaimPool].[UsageLevel3] ([Id], [Title], [UsageLevel2Id]) VALUES (305, N'مراکز خدمات غیر دولتی', 30)
GO
INSERT [ClaimPool].[UsageLevel3] ([Id], [Title], [UsageLevel2Id]) VALUES (310, N'علمک برداشت عشایری', 30)
GO
INSERT [ClaimPool].[UsageLevel3] ([Id], [Title], [UsageLevel2Id]) VALUES (350, N'آب آزاد', 35)
GO
INSERT [ClaimPool].[UsageLevel3] ([Id], [Title], [UsageLevel2Id]) VALUES (355, N'بنایی', 35)
GO
INSERT [ClaimPool].[UsageLevel3] ([Id], [Title], [UsageLevel2Id]) VALUES (400, N'آموزشی', 40)
GO
INSERT [ClaimPool].[UsageLevel3] ([Id], [Title], [UsageLevel2Id]) VALUES (405, N'اماکن مذهبی', 40)
GO
INSERT [ClaimPool].[UsageLevel3] ([Id], [Title], [UsageLevel2Id]) VALUES (410, N'درمانی و آموزشی', 40)
GO
INSERT [ClaimPool].[UsageLevel3] ([Id], [Title], [UsageLevel2Id]) VALUES (415, N'تربیتی', 40)
GO
INSERT [ClaimPool].[UsageLevel3] ([Id], [Title], [UsageLevel2Id]) VALUES (450, N'آب شیرین کن (ایستگاه توزیع و ظرف)', 45)
GO
INSERT [ClaimPool].[UsageLevel3] ([Id], [Title], [UsageLevel2Id]) VALUES (455, N'آب شیرین کن (بخش خصوص با شبکه توزیع)', 45)
GO
INSERT [ClaimPool].[UsageLevel3] ([Id], [Title], [UsageLevel2Id]) VALUES (500, N'گرمابه', 50)
GO
INSERT [ClaimPool].[UsageLevel3] ([Id], [Title], [UsageLevel2Id]) VALUES (550, N'فروش به کشتی و شناورها', 55)
GO
INSERT [ClaimPool].[UsageLevel3] ([Id], [Title], [UsageLevel2Id]) VALUES (555, N'فروش به تانکرهای خدماتی', 55)
GO
INSERT [ClaimPool].[UsageLevel3] ([Id], [Title], [UsageLevel2Id]) VALUES (560, N'فروش به مناطق آزاد (تحویل عمده)', 55)
GO
INSERT [ClaimPool].[UsageLevel3] ([Id], [Title], [UsageLevel2Id]) VALUES (600, N'شیر آتش نشانی', 60)
GO
INSERT [ClaimPool].[UsageLevel3] ([Id], [Title], [UsageLevel2Id]) VALUES (605, N'تبادل فی مابین شرکت های آب و فاضلاب', 60)
GO
INSERT [ClaimPool].[UsageLevel4] ([Id], [Title], [UsageLevel3Id]) VALUES (1000, N'خانگی', 100)
GO
INSERT [ClaimPool].[UsageLevel4] ([Id], [Title], [UsageLevel3Id]) VALUES (1500, N'اقامتگاه غیر دائم', 150)
GO
INSERT [ClaimPool].[UsageLevel4] ([Id], [Title], [UsageLevel3Id]) VALUES (2000, N'صنایع', 200)
GO
INSERT [ClaimPool].[UsageLevel4] ([Id], [Title], [UsageLevel3Id]) VALUES (2050, N'گردشگری', 205)
GO
INSERT [ClaimPool].[UsageLevel4] ([Id], [Title], [UsageLevel3Id]) VALUES (2500, N'عمومی', 250)
GO
INSERT [ClaimPool].[UsageLevel4] ([Id], [Title], [UsageLevel3Id]) VALUES (2550, N'دستگاه اجرایی', 255)
GO
INSERT [ClaimPool].[UsageLevel4] ([Id], [Title], [UsageLevel3Id]) VALUES (2600, N'فضای سبز', 260)
GO
INSERT [ClaimPool].[UsageLevel4] ([Id], [Title], [UsageLevel3Id]) VALUES (2650, N'سنتی', 265)
GO
INSERT [ClaimPool].[UsageLevel4] ([Id], [Title], [UsageLevel3Id]) VALUES (2652, N'صنعتی', 265)
GO
INSERT [ClaimPool].[UsageLevel4] ([Id], [Title], [UsageLevel3Id]) VALUES (2700, N'ارتش', 270)
GO
INSERT [ClaimPool].[UsageLevel4] ([Id], [Title], [UsageLevel3Id]) VALUES (2702, N'نیروی انتظامی', 270)
GO
INSERT [ClaimPool].[UsageLevel4] ([Id], [Title], [UsageLevel3Id]) VALUES (2704, N'سپاه', 270)
GO
INSERT [ClaimPool].[UsageLevel4] ([Id], [Title], [UsageLevel3Id]) VALUES (2706, N'وزارت دفاع', 270)
GO
INSERT [ClaimPool].[UsageLevel4] ([Id], [Title], [UsageLevel3Id]) VALUES (2708, N'بسیج', 270)
GO
INSERT [ClaimPool].[UsageLevel4] ([Id], [Title], [UsageLevel3Id]) VALUES (2710, N'سایر', 270)
GO
INSERT [ClaimPool].[UsageLevel4] ([Id], [Title], [UsageLevel3Id]) VALUES (3000, N'تجاری', 300)
GO
INSERT [ClaimPool].[UsageLevel4] ([Id], [Title], [UsageLevel3Id]) VALUES (3050, N'مراکز خدمات غیر دولتی', 305)
GO
INSERT [ClaimPool].[UsageLevel4] ([Id], [Title], [UsageLevel3Id]) VALUES (3100, N'علمک برداشت عشایری', 310)
GO
INSERT [ClaimPool].[UsageLevel4] ([Id], [Title], [UsageLevel3Id]) VALUES (3500, N'آب آزاد', 350)
GO
INSERT [ClaimPool].[UsageLevel4] ([Id], [Title], [UsageLevel3Id]) VALUES (3550, N'بنایی', 355)
GO
INSERT [ClaimPool].[UsageLevel4] ([Id], [Title], [UsageLevel3Id]) VALUES (4000, N'مدارس', 400)
GO
INSERT [ClaimPool].[UsageLevel4] ([Id], [Title], [UsageLevel3Id]) VALUES (4002, N'دانشگاهی', 400)
GO
INSERT [ClaimPool].[UsageLevel4] ([Id], [Title], [UsageLevel3Id]) VALUES (4004, N'ورزشی', 400)
GO
INSERT [ClaimPool].[UsageLevel4] ([Id], [Title], [UsageLevel3Id]) VALUES (4006, N'آموزش فنی و حرفه ای', 400)
GO
INSERT [ClaimPool].[UsageLevel4] ([Id], [Title], [UsageLevel3Id]) VALUES (4008, N'موزه و تاریخی', 400)
GO
INSERT [ClaimPool].[UsageLevel4] ([Id], [Title], [UsageLevel3Id]) VALUES (4010, N'خوابگاه دانشجویی', 400)
GO
INSERT [ClaimPool].[UsageLevel4] ([Id], [Title], [UsageLevel3Id]) VALUES (4012, N'سایر', 400)
GO
INSERT [ClaimPool].[UsageLevel4] ([Id], [Title], [UsageLevel3Id]) VALUES (4050, N'اماکن مذهبی', 405)
GO
INSERT [ClaimPool].[UsageLevel4] ([Id], [Title], [UsageLevel3Id]) VALUES (4100, N'نگهداری معلولین و ایتام و ...', 410)
GO
INSERT [ClaimPool].[UsageLevel4] ([Id], [Title], [UsageLevel3Id]) VALUES (4102, N'بیمارستان آموزشی', 410)
GO
INSERT [ClaimPool].[UsageLevel4] ([Id], [Title], [UsageLevel3Id]) VALUES (4104, N'درمانی بیماران خاص', 410)
GO
INSERT [ClaimPool].[UsageLevel4] ([Id], [Title], [UsageLevel3Id]) VALUES (4106, N'سایر', 410)
GO
INSERT [ClaimPool].[UsageLevel4] ([Id], [Title], [UsageLevel3Id]) VALUES (4150, N'زندان', 415)
GO
INSERT [ClaimPool].[UsageLevel4] ([Id], [Title], [UsageLevel3Id]) VALUES (4152, N'مراکز اقدامات تامینی و تربیتی', 415)
GO
INSERT [ClaimPool].[UsageLevel4] ([Id], [Title], [UsageLevel3Id]) VALUES (4154, N'سایر', 415)
GO
INSERT [ClaimPool].[UsageLevel4] ([Id], [Title], [UsageLevel3Id]) VALUES (4500, N'آب شیرین کن (ایستگاه توزیع و ظرف)', 450)
GO
INSERT [ClaimPool].[UsageLevel4] ([Id], [Title], [UsageLevel3Id]) VALUES (4550, N'آب شیرین کن (بخش خصوص با شبکه توزیع)', 455)
GO
INSERT [ClaimPool].[UsageLevel4] ([Id], [Title], [UsageLevel3Id]) VALUES (5000, N'گرمابه', 500)
GO
INSERT [ClaimPool].[UsageLevel4] ([Id], [Title], [UsageLevel3Id]) VALUES (5500, N'فروش به کشتی و شناورها', 550)
GO
INSERT [ClaimPool].[UsageLevel4] ([Id], [Title], [UsageLevel3Id]) VALUES (5550, N'فروش به تانکرهای خدماتی', 555)
GO
INSERT [ClaimPool].[UsageLevel4] ([Id], [Title], [UsageLevel3Id]) VALUES (5600, N'فروش به مناطق آزاد (تحویل عمده)', 560)
GO
INSERT [ClaimPool].[UsageLevel4] ([Id], [Title], [UsageLevel3Id]) VALUES (6000, N'شیر آتش نشانی', 600)
GO
INSERT [ClaimPool].[UsageLevel4] ([Id], [Title], [UsageLevel3Id]) VALUES (6050, N'تبادل فی مابین شرکت های آب و فاضلاب', 605)
GO