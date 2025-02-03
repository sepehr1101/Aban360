USE [Aban360]
GO
SET IDENTITY_INSERT [dbo].[Country] ON 
GO
INSERT [dbo].[Country] ([Id], [Title]) VALUES (1, N'ایران')
GO
SET IDENTITY_INSERT [dbo].[Country] OFF
GO
SET IDENTITY_INSERT [dbo].[CordinalDirection] ON 
GO
INSERT [dbo].[CordinalDirection] ([Id], [Title]) VALUES (1, N'مرکز')
GO
INSERT [dbo].[CordinalDirection] ([Id], [Title]) VALUES (2, N'شمال')
GO
INSERT [dbo].[CordinalDirection] ([Id], [Title]) VALUES (3, N'جنوب')
GO
INSERT [dbo].[CordinalDirection] ([Id], [Title]) VALUES (4, N'شرق')
GO
INSERT [dbo].[CordinalDirection] ([Id], [Title]) VALUES (5, N'غرب')
GO
SET IDENTITY_INSERT [dbo].[CordinalDirection] OFF
GO
INSERT [dbo].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (1, N'آذربایجان شرقی', 1, 1)
GO
INSERT [dbo].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (2, N'آذربایجان غربی', 1, 1)
GO
INSERT [dbo].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (3, N'اردبیل', 1, 1)
GO
INSERT [dbo].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (4, N'خوزستان', 1, 1)
GO
INSERT [dbo].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (5, N'البرز', 1, 1)
GO
INSERT [dbo].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (6, N'ایلام', 1, 1)
GO
INSERT [dbo].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (7, N'بوشهر', 1, 1)
GO
INSERT [dbo].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (8, N'تهران', 1, 1)
GO
INSERT [dbo].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (9, N'چهارمحال و بختیاری', 1, 1)
GO
INSERT [dbo].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (10, N'خراسان جنوبی', 1, 1)
GO
INSERT [dbo].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (11, N'خراسان رضوی', 1, 1)
GO
INSERT [dbo].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (12, N'خراسان شمالی', 1, 1)
GO
INSERT [dbo].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (13, N'اصفهان', 1, 1)
GO
INSERT [dbo].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (14, N'زنجان', 1, 1)
GO
INSERT [dbo].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (15, N'سمنان', 1, 1)
GO
INSERT [dbo].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (16, N'سیستان و بلوچستان', 1, 1)
GO
INSERT [dbo].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (17, N'فارس', 1, 1)
GO
INSERT [dbo].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (18, N'قزوین', 1, 1)
GO
INSERT [dbo].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (19, N'قم', 1, 1)
GO
INSERT [dbo].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (20, N'کردستان', 1, 1)
GO
INSERT [dbo].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (21, N'کرمان', 1, 1)
GO
INSERT [dbo].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (22, N'کرمانشاه', 1, 1)
GO
INSERT [dbo].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (23, N'کهگیلویه و بویراحمد', 1, 1)
GO
INSERT [dbo].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (24, N'گلستان', 1, 1)
GO
INSERT [dbo].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (25, N'لرستان', 1, 1)
GO
INSERT [dbo].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (26, N'گیلان', 1, 1)
GO
INSERT [dbo].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (27, N'مازندران', 1, 1)
GO
INSERT [dbo].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (28, N'مرکزی', 1, 1)
GO
INSERT [dbo].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (29, N'هرمزگان', 1, 1)
GO
INSERT [dbo].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (30, N'همدان', 1, 1)
GO
INSERT [dbo].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (31, N'یزد', 1, 1)
GO
INSERT [dbo].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (1, N'آبفا استان اصفهان', 13)
GO
INSERT [dbo].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (2, N'آبفا کاشان', 13)
GO
INSERT [dbo].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (3, N'آذربایجان شرقی', 1)
GO
INSERT [dbo].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (4, N'آذربایجان غربی', 2)
GO
INSERT [dbo].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (5, N'اردبیل', 3)
GO
INSERT [dbo].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (6, N'البرز', 5)
GO
INSERT [dbo].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (7, N'ایلام', 6)
GO
INSERT [dbo].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (8, N'بوشهر', 7)
GO
INSERT [dbo].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (9, N'تهران', 8)
GO
INSERT [dbo].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (10, N'چهارمحال و بختیاری', 9)
GO
INSERT [dbo].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (11, N'خراسان جنوبی', 10)
GO
INSERT [dbo].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (12, N'خراسان رضوی', 11)
GO
INSERT [dbo].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (13, N'خراسان شمالی', 12)
GO
INSERT [dbo].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (14, N'خوزستان', 4)
GO
INSERT [dbo].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (15, N'زنجان', 14)
GO
INSERT [dbo].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (16, N'سمنان', 15)
GO
INSERT [dbo].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (17, N'سیستان و بلوچستان', 16)
GO
INSERT [dbo].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (18, N'فارس', 17)
GO
INSERT [dbo].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (19, N'قزوین', 18)
GO
INSERT [dbo].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (20, N'قم', 19)
GO
INSERT [dbo].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (21, N'کردستان', 20)
GO
INSERT [dbo].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (22, N'کرمان', 21)
GO
INSERT [dbo].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (23, N'کرمانشاه', 22)
GO
INSERT [dbo].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (24, N'کهگیلویه و بویراحمد', 23)
GO
INSERT [dbo].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (25, N'گلستان', 24)
GO
INSERT [dbo].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (26, N'لرستان', 25)
GO
INSERT [dbo].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (27, N'گیلان', 26)
GO
INSERT [dbo].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (28, N'مازندران', 27)
GO
INSERT [dbo].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (29, N'مرکزی', 28)
GO
INSERT [dbo].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (30, N'هرمزگان', 27)
GO
INSERT [dbo].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (31, N'همدان', 28)
GO
INSERT [dbo].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (32, N'یزد', 29)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1, N'آذربایجان شرقی', 3)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (2, N'آذربایجان غربی', 4)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (3, N'اردبیل', 5)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (4, N'البرز', 6)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (5, N'ایلام', 7)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (6, N'بوشهر', 8)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (7, N'تهران', 9)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (8, N'چهارمحال و بختیاری', 10)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (9, N'خراسان جنوبی', 11)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (10, N'خراسان رضوی', 12)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (11, N'خراسان شمالی', 13)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (12, N'خوزستان', 14)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (13, N'زنجان', 15)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (14, N'سمنان', 16)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (15, N'سیستان و بلوچستان', 17)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (16, N'فارس', 18)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (17, N'قزوین', 19)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (18, N'قم', 20)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (19, N'کردستان', 21)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (20, N'کرمان', 22)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (21, N'کرمانشاه', 23)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (22, N'کهگیلویه و بویراحمد', 24)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (23, N'گلستان', 25)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (24, N'لرستان', 26)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (25, N'گیلان', 27)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (26, N'مازندران', 28)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (27, N'مرکزی', 29)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (28, N'هرمزگان', 30)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (29, N'همدان', 31)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (30, N'یزد', 32)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1312, N'اردستان', 1)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1314, N'شاهين شهر', 1)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1315, N'جرقويه', 1)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1316, N'خوانسار', 1)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1317, N'خميني شهر', 1)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1318, N'خوراسگان', 1)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1319, N'سميرم', 1)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1320, N'شهرضا', 1)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1321, N'فريدونشهر', 1)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1322, N'فلاورجان', 1)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1323, N'فريدن', 1)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1324, N'كوهپايه', 1)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1325, N'گلپايگان', 1)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1326, N'لنجان', 1)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1327, N'مباركه', 1)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1328, N'نائين', 1)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1329, N'نجف آباد', 1)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1330, N'نطنز', 1)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1331, N'ورزنه', 1)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1332, N'جلگه', 1)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1333, N'تيران', 1)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1334, N'فولادشهر', 1)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1336, N'بادرود', 1)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1337, N'چادگان', 1)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1338, N'دهاقان', 1)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1339, N'خور و بيابانك', 1)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1340, N'دولت آباد', 1)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1341, N'بوئين مياندشت', 1)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1342, N'میمه', 1)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1343, N'قهجاورستان', 1)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1344, N'براآن و کراچ', 1)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1345, N'مهردشت', 1)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1390, N'بهارستان', 1)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1515, N'مجلسی', 1)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (131301, N'1- منطقه یک', 1)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (131302, N'2- منطقه دو', 1)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (131303, N'3- منطقه سه', 1)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (131304, N'4- منطقه چهار', 1)
GO
INSERT [dbo].[Region] ([Id], [Title], [HeadquartersId]) VALUES (131305, N'5- منطقه پنج', 1)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (131211, N'اردستان', 1312)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (131212, N'زواره', 1312)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (131213, N'مهاباد', 1312)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (131301, N'1  - منطقه يک', 131301)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (131302, N'2 - منطقه  دو', 131302)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (131303, N'3 - منطقه  سه', 131303)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (131304, N'4 -  منطقه  چهار', 131304)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (131305, N'5 -  منطقه  پنج', 131305)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (131411, N'شاهين شهر', 1314)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (131412, N'گز', 1314)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (131420, N'گرگاب', 1314)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (131511, N'نيک آباد', 1315)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (131512, N'محمد آباد', 1315)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (131513, N'حسن آباد', 1315)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (131514, N'نصر آباد', 1315)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (131611, N'خوانسار', 1316)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (131701, N'خميني شهر', 1317)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (131702, N'درچه', 1317)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (131703, N'کوشک', 1317)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (131704, N'اصغر آباد', 1317)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (131811, N'خوراسگان', 1318)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (131911, N'سميرم', 1319)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (131912, N'حنا', 1319)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (131913, N'ونک', 1319)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (131914, N'کمه', 1319)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (132011, N'شهرضا', 1320)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (132012, N'منظريه', 1320)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (132111, N'فريدون شهر', 1321)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (132112, N'برف انبار', 1321)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (132211, N'فلاورجان', 1322)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (132212, N'کليشاد وسودرجان', 1322)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (132213, N'قهدريجان', 1322)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (132214, N'ابريشم', 1322)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (132215, N'پيربکران', 1322)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (132216, N'مينادشت', 1322)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (132217, N'بهاران', 1322)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (132218, N'اشترجان', 1322)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (132219, N'زازران', 1322)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (132220, N'روستاهای فلاورجان', 1322)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (132311, N'داران', 1323)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (132315, N'دامنه', 1323)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (132411, N'کوهپايه', 1324)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (132412, N'سجزي', 1324)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (132413, N'تودشک', 1324)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (132511, N'گلپايگان', 1325)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (132512, N'گوگد', 1325)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (132513, N'گلشهر', 1325)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (132611, N'زرين شهر', 1326)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (132612, N'ورنامخواست', 1326)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (132613, N'سده لنجان', 1326)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (132614, N'چرمهين', 1326)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (132615, N'باغبهادران', 1326)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (132616, N'چمگردان', 1326)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (132617, N'زاينده رود', 1326)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (132618, N'باغشاد', 1326)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (132711, N'مبارکه', 1327)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (132712, N'ديزيچه', 1327)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (132713, N'طالخونچه', 1327)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (132714, N'زيبا شهر', 1327)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (132715, N'کرکوند', 1327)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (132811, N'نائين', 1328)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (132812, N'انارک', 1328)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (132813, N'بافران', 1328)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (132911, N'نجف آباد', 1329)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (132914, N'گلدشت', 1329)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (132915, N'کهريزسنگ', 1329)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (132916, N'جوزدان', 1329)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (133011, N'نطنز', 1330)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (133012, N'طرق', 1330)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (133111, N'ورزنه', 1331)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (133211, N'هرند', 1332)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (133212, N'اژيه', 1332)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (133311, N'تيران', 1333)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (133312, N'رضوان شهر', 1333)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (133313, N'عسگران', 1333)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (133411, N'فولادشهر', 1334)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (133611, N'بادرود', 1336)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (133711, N'چادگان', 1337)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (133712, N'رزوه', 1337)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (133811, N'دهاقان', 1338)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (133812, N'گلشن', 1338)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (133911, N'خور', 1339)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (133912, N'جندق', 1339)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (133913, N'فرخي', 1339)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (134013, N'دولت آباد', 1340)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (134014, N'خورزوق', 1340)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (134015, N'دستگرد', 1340)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (134016, N'حبيب آباد', 1340)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (134019, N'کمشچه', 1340)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (134111, N'بوئين مياندشت', 1341)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (134112, N'افوس', 1341)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (134211, N'ميمه', 1342)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (134212, N'وزوان', 1342)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (134213, N'لاي بيد', 1342)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (134311, N'قهجاورستان', 1343)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (134411, N'زیار', 1344)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (134511, N'دهق', 1345)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (134512, N'علویجه', 1345)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (139011, N'بهارستان', 1390)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (141211, N'اردستان-ر', 1312)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (141212, N'زواره-ر', 1312)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (141213, N'مهاباد-ر', 1312)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (141304, N'4 منطقه چهار- ر', 131304)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (141411, N'شاهين شهر-ر', 1314)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (141412, N'گز-ر', 1314)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (141420, N'گرگاب-ر', 1314)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (141511, N'نيک آباد-ر', 1315)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (141512, N'محمد آباد-ر', 1315)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (141513, N'حسن آباد-ر', 1315)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (141514, N'نصر آباد-ر', 1315)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (141611, N'خوانسار-ر', 1316)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (141701, N'خميني شهر-ر', 1317)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (141702, N'درچه-ر', 1317)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (141703, N'کوشک-ر', 1317)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (141704, N'اصغر آباد-ر', 1317)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (141811, N'خوراسگان-ر', 1318)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (141911, N'سميرم-ر', 1319)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (141912, N'حنا-ر', 1319)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (141913, N'ونک-ر', 1319)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (141914, N'کمه-ر', 1319)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (142011, N'شهرضا-ر', 1320)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (142012, N'منظريه-ر', 1320)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (142111, N'فريدون شهر-ر', 1321)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (142112, N'برف انبار-ر', 1321)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (142211, N'فلاورجان-ر', 1322)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (142212, N'کليشاد وسودرجان-ر', 1322)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (142213, N'قهدريجان-ر', 1322)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (142214, N'ابريشم-ر', 1322)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (142215, N'پيربکران-ر', 1322)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (142216, N'مينادشت-ر', 1322)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (142217, N'بهاران-ر', 1322)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (142218, N'اشترجان-ر', 1322)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (142219, N'زازران-ر', 1322)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (142220, N'روستاهای فلاورجان-ر', 1322)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (142311, N'داران-ر', 1323)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (142315, N'دامنه-ر', 1323)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (142411, N'کوهپايه-ر', 1324)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (142412, N'سجزي-ر', 1324)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (142413, N'تودشک-ر', 1324)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (142511, N'گلپايگان-ر', 1325)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (142512, N'گوگد-ر', 1325)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (142513, N'گلشهر-ر', 1325)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (142611, N'زرين شهر-ر', 1326)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (142612, N'ورنامخواست-ر', 1326)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (142613, N'سده لنجان-ر', 1326)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (142614, N'چرمهين-ر', 1326)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (142615, N'باغبهادران-ر', 1326)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (142616, N'چمگردان-ر', 1326)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (142617, N'زاينده رود-ر', 1326)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (142618, N'باغشاد-ر', 1326)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (142711, N'مبارکه-ر', 1327)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (142712, N'ديزيچه-ر', 1327)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (142713, N'طالخونچه-ر', 1327)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (142714, N'زيبا شهر-ر', 1327)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (142715, N'کرکوند-ر', 1327)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (142811, N'نائين-ر', 1328)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (142812, N'انارک-ر', 1328)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (142813, N'بافران-ر', 1328)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (142911, N'نجف آباد-ر', 1329)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (142914, N'گلدشت-ر', 1329)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (142915, N'کهريزسنگ-ر', 1329)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (142916, N'جوزدان-ر', 1329)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (143011, N'نطنز-ر', 1330)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (143012, N'طرق-ر', 1330)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (143111, N'ورزنه-ر', 1331)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (143211, N'هرند-ر', 1332)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (143212, N'اژيه-ر', 1332)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (143311, N'تيران-ر', 1333)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (143312, N'رضوان شهر-ر', 1333)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (143313, N'عسگران-ر', 1333)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (143411, N'فولادشهر-ر', 1334)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (143611, N'بادرود-ر', 1336)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (143711, N'چادگان-ر', 1337)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (143712, N'رزوه-ر', 1337)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (143811, N'دهاقان-ر', 1338)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (143812, N'گلشن-ر', 1338)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (143911, N'خور-ر', 1339)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (143912, N'جندق-ر', 1339)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (143913, N'فرخي-ر', 1339)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (144013, N'دولت آباد-ر', 1340)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (144014, N'خورزوق-ر', 1340)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (144015, N'دستگرد-ر', 1340)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (144016, N'حبيب آباد-ر', 1340)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (144019, N'کمشچه-ر', 1340)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (144111, N'بوئين مياندشت-ر', 1341)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (144112, N'افوس-ر', 1341)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (144211, N'ميمه-ر', 1342)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (144212, N'وزوان-ر', 1342)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (144213, N'لاي بيد-ر', 1342)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (144311, N'قهجاورستان-ر', 1343)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (144411, N'زیار-ر', 1344)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (144511, N'دهق-ر', 1345)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (144512, N'علویجه-ر', 1345)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (149011, N'بهارستان-ر', 1390)
GO
INSERT [dbo].[Zone] ([Id], [Title], [RegionId]) VALUES (151511, N'شهر جديد مجلسي-ر', 1515)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10010001, N'اشینه', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10010003, N'قارنه', 141512, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10010004, N'ارجنک', 141611, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10010005, N'تنگ خشک', 141911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10010006, N'دره بید', 142315, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10010007, N'بزمه', 142112, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10010008, N'پلارت', 142215, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10010009, N'کهرویه', 142011, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10010011, N'آرجان', 142512, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10010012, N'آيدوغميش', 142615, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10010013, N'چوپانان', 142812, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10010014, N'رحمت آباد', 142916, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10010015, N'اریسمان', 143611, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10010016, N'جهاد آباد', 141411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10010017, N'ابرو', 142713, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10010018, N'آبپونه', 143312, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10010020, N'علی آباد', 143711, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10010022, N'جعفرآباد', 143911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10010023, N'پروانه', 144016, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10010024, N'آبچوییه', 142413, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10010025, N'تیرانچی', 141703, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10010026, N'آرتیجان', 144111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10010027, N'گلستانه', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10020001, N'خیر آباد', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10020003, N'گنج آباد', 141512, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10020004, N'حاجی آباد', 141611, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10020005, N'چهارراه', 141911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10020006, N'قفر', 142311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10020007, N'بیجگرد', 142112, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10020008, N'تمندگان', 142215, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10020009, N'یحی آباد', 142012, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10020011, N'دستجرده', 142512, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10020012, N'برنجگان', 142615, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10020013, N'کبرآباد', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10020014, N'فیلور', 142916, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10020015, N'صالح اباد', 143011, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10020016, N'بیدشک', 141411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10020017, N'حوض ماهی', 142713, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10020018, N'حسن آبادآبریزه', 143312, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10020020, N'فراموشجان', 143711, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10020022, N'چاهملک', 143911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10020023, N'علی آباد چی', 144016, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10020024, N'چیرمان', 142413, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10020026, N'حسین آباد', 144111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10020027, N'جار', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10030001, N'طوران', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10030003, N'سیان', 141512, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10030004, N'حسن آباد', 141611, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10030005, N'روداباد', 141911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10030006, N'نهرخلج', 142311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10030007, N'چقا', 142112, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10030008, N'چهاربرج', 142215, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10030009, N'اسفرجان', 142012, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10030011, N'دم اسمان', 142513, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10030012, N'حاجي الوان', 142615, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10030013, N'فيض آباد معدن', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10030015, N'ده اباد', 143611, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10030016, N'دهلر', 141411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10030017, N'قلعه سفید', 142713, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10030018, N'حسن آبادکهنه', 143312, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10030020, N'چهل چشمه', 143711, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10030022, N'قادراباد', 143911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10030023, N'دنبی', 144016, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10030024, N'جشوقان', 142413, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10030026, N'ده رجب', 144111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10030027, N'يفران', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10040001, N'فران', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10040003, N'حسین آباد', 141512, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10040004, N'خشکرود', 141611, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10040005, N'سادات پادنا', 141914, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10040006, N'گنجه', 142311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10040007, N'خویگان علیا', 142112, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10040008, N'خوانسارک', 142215, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10040009, N'هونجان', 142012, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10040011, N'رباط ملکی', 142512, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10040012, N'خشوئيه', 142615, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10040013, N'مزرعچه', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10040015, N'فمی', 143611, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10040016, N'کلهرود', 141411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10040017, N'لاو', 142713, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10040018, N'قلعه عرب', 143312, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10040020, N'ورباد', 143711, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10040021, N'محمود آباد', 143812, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10040023, N'مرغ', 144016, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10040024, N'دلگشا', 142413, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10040026, N'ماربر', 144111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10040027, N'ايچي', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10050001, N'قهساره', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10050003, N'آذرخواران', 141512, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10050004, N'کهرت', 141611, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10050005, N'دنگزلو', 141914, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10050006, N'بادجان', 142311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10050007, N'دهسور سفلی', 142112, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10050008, N'دارافشان', 142215, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10050009, N'امامزاده علی اکبر', 142011, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10050011, N'سراور', 142512, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10050012, N'دورك', 142615, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10050013, N'كدنوئيه', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10050014, N'حاجی آباد', 142911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10050015, N'متین اباد', 143611, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10050016, N'مورچه خورت', 141411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10050017, N'بداغ اباد', 142711, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10050018, N'قره تپه', 143312, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10050020, N'لگاله', 143711, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10050021, N'علی آباد گچی', 143812, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10050022, N'ابراهیم آباد', 143911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10050023, N'شورچه', 144016, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10050024, N'اروجه', 142413, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10050026, N'قره بلطاق', 144111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10050027, N'کرچگان', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10060001, N'كچي', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10060003, N'پیکان', 141511, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10060004, N'ویست', 141611, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10060005, N'نقل', 141914, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10060006, N'قوهک', 142311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10060007, N'سروشجان', 142112, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10060008, N'رارا', 142215, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10060009, N'اسفه', 142011, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10060011, N'شادگان', 142513, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10060012, N'رحمتاباد', 142615, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10060013, N'حاجی آباد', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10060014, N'اشن', 144511, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10060015, N'بیدهند', 143011, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10060016, N'باغمیران', 141411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10060017, N'مزرعچه', 142712, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10060018, N'عزیزآباد', 143312, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10060020, N'مشهدکاوه', 143711, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10060021, N'کره', 143811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10060022, N'آبادان', 143911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10060023, N'سنگلاخ', 144016, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10060024, N'وج', 142413, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10060026, N'خلیلی', 144111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10060027, N'زغمار', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10070001, N'كهنگ', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10070003, N'سعادت آباد', 141511, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10070004, N'تیدجان', 141611, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10070005, N'نورابادپادنا', 141914, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10070006, N'خلج', 142311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10070007, N'میلاگرد', 142112, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10070008, N'رحیم آباد', 142215, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10070009, N'مهیار', 142011, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10070011, N'شیداباد', 142512, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10070012, N'زردخشوييه', 142615, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10070013, N'مجتمع جزلان', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10070014, N'خیرآباد', 144511, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10070015, N'ولوجرد', 143011, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10070016, N'لایبد', 141411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10070017, N'حسن اباد بیدکان', 142715, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10070018, N'قلعه موسی خان', 143312, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10070020, N'درک آباد', 143711, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10070021, N'لاریچه', 143811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10070022, N'خرمدشت', 143911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10070024, N'باد افشان', 142413, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10070026, N'تخماقلو', 144111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10070027, N'هرمدان', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10080001, N'مار', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10080003, N'حیدرآباد', 141511, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10080004, N'دوشخراط', 141611, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10080005, N'کره دان', 141914, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10080006, N'چهلخانه', 142311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10080007, N'چقادر', 142112, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10080008, N'سمسان', 142215, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10080009, N'باغ سرخ', 142012, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10080011, N'فاویان', 142513, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10080012, N'سعيداباد', 142615, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10080014, N'دماب', 144511, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10080015, N'هنجن', 143011, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10080016, N'لوشاب', 144211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10080018, N'تندران', 143311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10080020, N'کمیتک', 143712, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10080021, N'دزج', 143811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10080022, N'مصر', 143912, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10080024, N'برز آباد', 142413, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10080026, N'بسینان', 144111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10080027, N'عباس آباد', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10090001, N'مارچوبه', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10090003, N'مزرعه عرب', 141511, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10090004, N'سنگ سفید', 141611, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10090005, N'شهید', 141914, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10090006, N'حصور', 142311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10090007, N'دهسور علیا', 142112, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10090008, N'سیاه افشار', 142215, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10090009, N'بوان', 142012, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10090011, N'وارنیان', 142512, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10090012, N'صادق اباد', 142615, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10090014, N'گلدره', 144511, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10090015, N'یارند', 143011, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10090016, N'حسن رباط', 144211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10090017, N'کرکوند', 142715, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10090018, N'جاجا', 143311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10090020, N'پرمه سفلی', 143711, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10090021, N'همگین', 143811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10090022, N'ایراج', 143911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10090024, N'جندابه', 142413, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10090026, N'کرچ', 144111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10090027, N'ازيران', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10100001, N'مباركه', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10100003, N'کمال آباد', 141513, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10100004, N'صفادشت', 141611, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10100005, N'کیفته حسینی', 141914, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10100006, N'ازونبلاغ', 142311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10100007, N'بهرام آباد', 142111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10100008, N'صادق آباد', 142215, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10100009, N'دهک', 142012, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10100011, N'استهلک', 142511, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10100012, N'قلعه تركي', 142615, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10100014, N'حسین آباد', 144512, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10100015, N'باغستان بالا', 143012, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10100016, N'موته', 144211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10100017, N'احمد اباد', 142714, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10100018, N'جعفرآباد', 143311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10100020, N'آبادچی پایین', 143711, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10100021, N'قمیشلو', 143811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10100022, N'گرمه', 143911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10100024, N'مشکنان', 142413, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10100026, N'هلاغره', 144111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10100027, N'ليان', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10110001, N'نهوج', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10110003, N'دستجرد', 141513, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10110004, N'قلعه بابامحمد', 141611, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10110005, N'دیدجان', 141914, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10110006, N'درختک', 142311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10110007, N'دهنو', 142111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10110008, N'علی آباد', 142215, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10110009, N'زیارتگاه', 142012, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10110011, N'اسفاجرد', 142512, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10110012, N'كته شور', 142615, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10110014, N'خونداب', 144511, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10110016, N'ازان', 144211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10110017, N'اراضی', 142714, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10110018, N'خرمنان', 143311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10110020, N'تقی آباد', 143711, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10110021, N'پوده', 143811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10110022, N'مهرجان', 143911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10110024, N'امامزاده قاسم', 142411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10110026, N'قلعه بهمن', 144111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10110027, N'کروه', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10120001, N'نيسيان', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10120003, N'خارا', 141513, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10120004, N'قودجان', 141611, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10120005, N'قنات', 141914, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10120006, N'دهق', 142311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10120007, N'تزره', 142111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10120008, N'قلعه سرخ', 142215, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10120009, N'سولار', 142012, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10120011, N'حاجیله', 142511, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10120012, N'كرچگان', 142615, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10120014, N'علی آباد', 144511, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10120015, N'طار', 143011, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10120016, N'خسروآباد', 144211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10120017, N'اسد اباد', 142714, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10120018, N'خمیران', 143311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10120020, N'حیدر آباد', 143711, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10120021, N'قهه', 143811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10120022, N'اردیب', 143911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10120024, N'جزه', 142411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10120026, N'بتلیجه', 144111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10120027, N'پيله وران', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10130001, N'ديزيچه', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10130003, N'اله آباد', 141513, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10130004, N'لایجند', 141611, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10130005, N'گیوسین', 141914, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10130006, N'قودجانک', 142311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10130007, N'خشک آبخور', 142111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10130008, N'کلیسان', 142215, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10130009, N'قصرچم', 142011, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10130011, N'درب امامزاده', 142511, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10130012, N'موركان', 142615, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10130014, N'هسنیجه', 144512, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10130015, N'کشه', 143012, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10130016, N'سعید آباد', 144211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10130017, N'اکبر اباد', 142714, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10130018, N'خیرآباد', 143311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10130020, N'پرمه علیا', 143711, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10130021, N'گنجقباد', 143811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10130022, N'بیاضه', 143911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10130024, N'کمال بیک', 142411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10130026, N'دره سوخته', 144111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10130027, N'برکان', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10140001, N'اونج', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10140003, N'مالواجرد', 141513, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10140004, N'شهرک های یاسر', 141611, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10140005, N'مورک', 141914, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10140006, N'نماگرد', 142311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10140007, N'راچه', 142111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10140008, N'مهرگان', 142215, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10140009, N'قوام آباد', 142012, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10140011, N'دیزیجان', 142511, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10140012, N'همام', 142615, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10140014, N'جلال آباد', 142911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10140015, N'مزده', 143011, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10140016, N'ونداده', 144211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10140017, N'بارچان', 142714, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10140018, N'ورپشت', 143311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10140020, N'دولت آباد گل سفید', 143711, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10140021, N'قمبوان', 143811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10140022, N'حسین آباد هفتومان', 143911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10140024, N'سهر', 142411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10140026, N'هندوکش', 144111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10140027, N'ازوار', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10150001, N'باقرابادعليا', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10150003, N'مبارکه', 141513, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10150004, N'شهرک صنعتی', 141611, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10150005, N'سادات دیدجان', 141914, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10150006, N'سفتجان', 142311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10150007, N'زردفهره', 142111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10150008, N'نودرآمد', 142215, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10150009, N'ماران', 142012, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10150011, N'رباط ابوالقاسم', 142511, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10150012, N'چم تقي', 142615, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10150014, N'نهضت آباد', 142911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10150015, N'نیه', 143011, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10150016, N'سه', 141411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10150017, N'باغملک', 142714, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10150018, N'آبگرم', 143313, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10150020, N'دهباد سفلی', 143711, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10150021, N'جمبزه', 143811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10150022, N'هفتومان', 143911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10150024, N'میر جعفر', 142411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10150026, N'نوغان علیا', 144111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10150027, N'کلارتان', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10160001, N'بغم', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10160003, N'رامشه', 141513, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10160004, N'تجره', 141611, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10160005, N'قلعه اسلام اباد', 141912, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10160006, N'چیگان', 142311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10160007, N'سخی آباد', 142111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10160008, N'وزیرآباد', 142215, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10160009, N'مسینه', 142012, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10160011, N'رباط سرخ', 142511, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10160012, N'حاجت اقا', 142615, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10160014, N'کوه لطف', 144511, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10160015, N'یحیی اباد', 143012, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10160017, N'بروزاد', 142714, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10160018, N'افجان', 143311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10160020, N'هرمانک سفلی', 143711, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10160021, N'علی آباد جمبزه', 143811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10160022, N'عروسان', 143911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10160024, N'دخر آباد', 142411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10160026, N'نوغان سفلی', 144111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10160027, N'زيار ( شهر)', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10170001, N'جنبه', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10170004, N'حاج بلاغ', 141611, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10170005, N'چشمه خونی', 141912, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10170006, N'غرغن', 142311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10170007, N'سکان', 142111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10170008, N'پلارتگان', 142215, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10170009, N'مقصود بیک', 142012, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10170011, N'رباط قالقان', 142512, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10170012, N'ركن اباد', 142615, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10170015, N'باغستان پایین', 143012, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10170017, N'جوشان', 142714, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10170018, N'بودان', 143311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10170020, N'اصفهانک مشاعی', 143711, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10170022, N'نصرآباد', 143913, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10170024, N'علون آباد', 142411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10170026, N'ماهورک', 144111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10170027, N'اندلان', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10180001, N'چاهريسه', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10180004, N'خم پیچ', 141611, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10180005, N'شیخعلی', 141911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10180006, N'سواران', 142311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10180007, N'اسلام آباد', 142111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10180008, N'دارگان', 142215, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10180009, N'میر آباد', 142012, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10180011, N'سعید آباد', 142511, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10180012, N'زمان آباد', 142614, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10180015, N'ورگوران', 143011, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10180017, N'جوهرستان', 142714, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10180018, N'حسن آبادعلیا', 143313, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10180020, N'اصفهانک علیا', 143711, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10180022, N'فرح زاد', 143912, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10180024, N'کیچی', 142411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10180026, N'دره ساری', 144111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10180027, N'رحيم آباد', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10190001, N'خاصه تراش', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10190004, N'رحمت آباد', 141611, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10190005, N'گرموک', 141911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10190006, N'قلعه ملک', 142311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10190007, N'مکدین علیا', 142111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10190008, N'علیشاهدان', 142215, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10190009, N'وشاره', 142012, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10190011, N'ضامن اباد', 142511, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10190012, N'قلعه پائین', 142614, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10190013, N'بنويدسفلي', 142813, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10190015, N'ابیازن', 143011, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10190017, N'دهسرخ', 142714, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10190018, N'حسین آباد', 143311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10190020, N'اصفهانک ساکی', 143711, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10190024, N'آبخارک', 142411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10190026, N'تیرکرت', 144111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10190027, N'روران', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10200001, N'ديزلو', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10200004, N'مهرآباد', 141611, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10200005, N'هست', 141911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10200006, N'ننادگان', 142311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10200007, N'مزرعه میر', 142111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10200008, N'بجگرد', 142215, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10200009, N'ولندان', 142012, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10200011, N'عباس اباد', 142511, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10200012, N'قلعه اقا', 142615, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10200013, N'سپرو', 142813, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10200015, N'اسفیدان', 143011, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10200017, N'زودان', 142714, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10200018, N'سوران', 143311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10200020, N'اصفهانک سفلی', 143711, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10200024, N'کرد آباد', 142411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10200026, N'قلعه اخلاص', 144111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10200027, N'شيدان', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10210001, N'عباس ابادعليا', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10210005, N'اسکان', 141912, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10210006, N'خویگان', 142311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10210007, N'وهرگان', 142111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10210008, N'سهروفیروزان', 142215, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10210009, N'امین آباد', 142012, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10210011, N'غرقاب', 142511, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10210012, N'قلعه لاي بيد', 142615, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10210013, N'سهيل', 142813, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10210015, N'اوره', 143011, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10210017, N'فخر اباد', 142714, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10210018, N'علی آباد', 143311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10210020, N'اصفهانک عبدل', 143711, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10210024, N'زفره', 142412, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10210026, N'بلطاق', 144111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10210027, N'هرمزآباد', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10220001, N'فسخود', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10220005, N'ضرغام اباد', 141911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10220006, N'گل امیر', 142311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10220007, N'سیبک', 142111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10220008, N'طاد', 142215, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10220011, N'فرج آباد', 142511, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10220012, N'كچوييه بالا', 142614, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10220013, N'فرح آباد', 142813, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10220015, N'جریان', 143011, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10220017, N'کوشکیچه', 142714, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10220018, N'محمدیه', 143311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10220020, N'پرزگان سفلی', 143711, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10220024, N'مزرعه عبدا...', 142412, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10220026, N'ازناوله', 144111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10220027, N'کبوترآباد', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10230001, N'گل و مل', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10230005, N'اسلام اباد', 141911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10230006, N'اسکندری', 142311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10230007, N'نهضت آباد', 142111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10230008, N'فیلرگان', 142215, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10230011, N'فقستان', 142512, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10230012, N'كليشادرخ', 142615, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10230013, N'هندوچوب', 142813, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10230015, N'جزن', 143011, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10230017, N'میر اباد', 142714, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10230018, N'میرآباد', 143311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10230020, N'پرزگان خراج', 143711, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10230024, N'ورتون', 142412, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10230026, N'زرنه', 144111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10230027, N'چم زيار', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10240001, N'ماستبندي', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10240005, N'آغداش سفلی', 141911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10240006, N'نسار', 142311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10240007, N'سرداب سفلی', 142111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10240008, N'گلگون', 142215, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10240011, N'قالقان', 142512, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10240012, N'هاردنگ', 142614, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10240013, N'بنويدعليا', 142813, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10240015, N'خفر', 143011, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10240017, N'هراتمه', 142714, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10240018, N'نسیم آباد', 143311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10240020, N'چشمندگان سفلی', 143711, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10240024, N'یک لنگی', 142412, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10240026, N'اغچه', 144111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10240027, N'دهکرم', 143212, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10250001, N'ومكان', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10250005, N'پیراسفنه', 141911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10250006, N'سینگرد', 142311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10250007, N'سرداب علیا', 142111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10250008, N'نرگان', 142215, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10250011, N'قرغن', 142511, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10250012, N'پركستان', 142615, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10250013, N'مزرعه احمد', 142813, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10250015, N'دستجرد', 143011, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10250017, N'سورچه بالا', 142715, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10250018, N'مهدی آباد', 143311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10250020, N'چشمندگان مجید', 143711, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10250024, N'کلیشاد', 143211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10250026, N'حاج فتحعلی', 144111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10250027, N'جمبزه', 143212, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10260001, N'حسن ابادشور', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10260005, N'جلال اباد', 141911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10260006, N'عادگان', 142311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10260007, N'دره سیب', 142111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10260008, N'ونهر', 142215, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10260012, N'چم حيدر', 142615, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10260013, N'مزرعه حاج حسين', 142813, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10260015, N'طامه', 143011, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10260017, N'اردوگاه', 142714, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10260018, N'گلاب', 143313, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10260020, N'قلعه زنبور', 143711, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10260024, N'سیچی', 143211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10260026, N'آقاگل', 144111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10260027, N'مارچي', 143212, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10270001, N'مونيه', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10270005, N'حاجی اباد', 141911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10270006, N'طرار', 142311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10270007, N'قلعه سرخ', 142111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10270008, N'کرسگان', 142211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10270011, N'لالان', 142511, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10270012, N'چم نور)چم گاو(', 142615, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10270013, N'جزن اباد', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10270015, N'میلاجرد', 143011, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10270018, N'الور', 143313, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10270020, N'چشمندگان علیا', 143711, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10270024, N'مادرکان', 143211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10270026, N'هزارجریب', 144111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10270027, N'قلعه عبداله', 143212, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10280001, N'علي ابادلاسيب', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10280005, N'حیدرابادعباسی', 141911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10280007, N'میدانک اول', 142111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10280008, N'دشتچی کرسگان', 142211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10280011, N'ماکوله', 142511, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10280012, N'جعفراباد', 142615, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10280013, N'جوي اباد', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10280015, N'نسران', 143011, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10280018, N'چشمه احمدرضا', 143313, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10280020, N'کلیچه', 143711, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10280024, N'خرم', 143211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10280026, N'قايم آباد', 144111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10280027, N'برسيان', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10290001, N'لاسيب', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10290005, N'ده عاشوری', 141911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10290007, N'میدانک دوم', 142111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10290008, N'کارویه', 142213, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10290011, N'مرغ', 142511, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10290012, N'چم طاق', 142615, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10290013, N'خاروان', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10290015, N'ابکشه', 143012, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10290018, N'دره بید', 143313, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10290020, N'دهباد علیا', 143711, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10290024, N'خورچان', 143211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10290026, N'قلعه خواجه', 144111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10290027, N'تيميارت', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10300001, N'توتكان', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10300005, N'دهکرد', 141911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10300007, N'برداسیاب', 142111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10300008, N'زفره', 142211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10300011, N'مزرعه', 142512, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10300012, N'چم عليشاه', 142615, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10300013, N'فيض ابادحاج كاظم', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10300015, N'برزرود', 143011, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10300018, N'دوتو', 143313, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10300020, N'اورگان', 143711, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10300024, N'هلارته', 143211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10300026, N'داشکسن', 144111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10300027, N'فساران', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10310001, N'پنج', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10310005, N'سبزاباد', 141911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10310007, N'چقیورت', 142111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10310008, N'کاویان', 142211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10310011, N'ملازجان', 142512, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10310012, N'چم كهريز', 142615, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10310013, N'كجان', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10310018, N'دولت آباد', 143313, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10310020, N'هرمانک علیا', 143711, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10310024, N'سکان', 143211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10310026, N'میرآباد', 144111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10310027, N'جور', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10320001, N'سرابه', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10320005, N'سادات اباد وردشت', 141911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10320007, N'دره بادام علیا', 142111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10320008, N'محمدیه', 142211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10320011, N'هنده', 142511, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10320012, N'چم يوسفعلي', 142615, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10320013, N'گلستان', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10320018, N'قلعه ناظر', 143313, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10320020, N'سرچشمه', 143711, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10320024, N'سنوچی', 143211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10320026, N'جوزار', 144111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10320027, N'اسفيناء', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10330001, N'زفرقند', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10330005, N'علی اباد', 141911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10330007, N'دره بادام سفلی', 142111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10330008, N'مهرنجان اتراک', 142211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10330011, N'غرقه', 142511, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10330012, N'اشيان', 142612, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10330013, N'ونديش', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10330018, N'کردسفلی', 143313, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10330020, N'دولت آباد', 143711, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10330024, N'پاچیک آباد', 143211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10330027, N'دستجاء', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10340001, N'كاشانك', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10340005, N'فتح اباد', 141911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10340007, N'خسرو آباد', 142111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10340008, N'بوستان لارگان', 142211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10340011, N'هرستانه', 142511, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10340012, N'كاريز', 142612, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10340013, N'ملكان', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10340018, N'گنهران', 143313, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10340020, N'خرسانک سفلی', 143711, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10340024, N'کمندان', 143211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10340027, N'کندلان', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10350001, N'كچومثقال', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10350005, N'قره قاچ', 141911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10350007, N'مزرعه کیماس', 142111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10350008, N'خیرآباد', 142211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10350011, N'رباط محمود', 142512, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10350012, N'مبارک آباد', 142612, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10350013, N'صفي اباد', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10350018, N'تقی آباد', 143313, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10350020, N'خرسانک علیا', 143711, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10350024, N'سریان', 143211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10350027, N'کوهان', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10360001, N'مهراندو', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10360005, N'کزن', 141911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10360007, N'مزرعه سیب', 142111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10360008, N'مهرنجان ارامنه', 142211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10360011, N'علی آباد', 142512, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10360012, N'قرق اقا', 142612, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10360013, N'نيستانك', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10360018, N'هومان', 143311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10360020, N'علی عرب', 143712, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10360024, N'قلعه ساربان', 143211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10360027, N'حاجي آباد', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10370001, N'ميشاب', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10370005, N'موروک', 141911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10370007, N'حاجی آباد', 142111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10370008, N'بوستان درچه عابد', 142211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10370011, N'خاکه', 142512, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10370012, N'باغشاه', 142618, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10370013, N'مهرادران', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10370018, N'قاسم آباد', 143311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10370020, N'ده کلبعلی', 143712, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10370024, N'کلیل', 143211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10370027, N'دستگردمار', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10380001, N'هندواباد', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10380005, N'مهراباد', 141911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10380007, N'دره نورالدین', 142111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10380008, N'بوستان حاجی آباد', 142211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10380011, N'قشلاق', 142511, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10380012, N'مديسه', 142618, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10380013, N'بلان', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10380018, N'حسن آبادوسطی', 143311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10380020, N'معروف آباد', 143712, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10380024, N'صیدان', 143211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10380027, N'ارم پشت', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10390001, N'دره باغ', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10390005, N'مهرگرد', 141911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10390007, N'لطف آباد', 142111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10390008, N'دستناء', 142215, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10390011, N'لالانک', 142511, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10390012, N'نوگوران', 142618, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10390013, N'حسين ابادعاشق', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10390018, N'اله آباد', 143312, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10390020, N'حجت آباد', 143711, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10390024, N'امامزاده عبدالعزیز', 143211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10390027, N'ديده ران', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10400001, N'جعفر اباد گرمسير', 141213, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10400005, N'ورق', 141911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10400008, N'جیلاب', 142211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10400011, N'تیکن', 142512, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10400012, N'زاینده رود', 142617, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10400013, N'ورپاي عليا', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10400018, N'عسگران', 143313, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10400020, N'انالوجه', 143712, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10400024, N'فیض آباد', 143211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10400027, N'کاروانچي', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10410001, N'حيدراباد', 141213, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10410005, N'چشمه رحمن', 141911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10410008, N'حسین آباد', 142211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10410011, N'در', 142512, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10410012, N'صادق آبادكاريز', 142612, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10410013, N'يورتكا', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10410018, N'مبارکه', 143311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10410020, N'قرقر', 143712, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10410024, N'شریف آباد', 143211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10410027, N'جاجا', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10420001, N'خشك اياد', 141213, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10420005, N'حیدراباد', 141911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10420008, N'جولرستان', 142211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10420011, N'زرنجان', 142512, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10420012, N'سيبه', 142612, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10420013, N'معين آباد', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10420018, N'کردعلیا', 143313, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10420020, N'منصوریه', 143711, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10420024, N'گیشی', 143212, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10420027, N'يحيي آباد', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10430001, N'دولت اباد', 141213, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10430005, N'ده نسا علیا', 141911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10430008, N'چمرود', 142215, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10430011, N'شرکت زراعی', 142512, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10430012, N'يال بلند', 142615, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10430013, N'اسدآباد', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10430020, N'گشنیزجان', 143712, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10430024, N'قمشان', 143212, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10430027, N'کنجوان', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10440001, N'رحمت اباد', 141213, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10440005, N'بردکان', 141911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10440008, N'اردال', 142215, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10440011, N'شورچه', 142512, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10440012, N'چم پير', 142615, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10440013, N'بلااباد', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10440020, N'آبادچی علیا', 143711, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10440024, N'قلعه بالا', 143212, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10440027, N'باچه', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10450001, N'سرهنگچه', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10450005, N'ملاقلی', 141911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10450008, N'امام زاده شمس الدین', 142215, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10450011, N'نیوان سوق', 142512, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10450012, N'شورجه', 142618, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10450013, N'جهان اباد', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10450020, N'درکان', 143711, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10450024, N'سیان', 143212, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10450027, N'دولاب', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10460001, N'سريجهن', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10460005, N'کاکابادعلیا', 141911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10460011, N'وانشان', 142511, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10460012, N'چم آسمان', 142618, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10460013, N'دولت آباد شيخ', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10460020, N'باغ ناظر', 143711, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10460024, N'قهی', 143211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10460027, N'اسپارت', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10470001, N'كسوج', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10470005, N'نظراباد', 141911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10470011, N'نیوان نار', 142512, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10470012, N'فتح آباد', 142615, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10470013, N'رحيم اباد', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10470020, N'هجرت آباد', 143711, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10470024, N'طهمورثات', 143111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10470027, N'منشيان', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10480001, N'حسن اباد گرمسير', 141213, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10480005, N'سرچغاسفلی', 141911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10480011, N'امیریه', 142511, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10480013, N'رسول اباد', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10480020, N'محمود آباد', 143711, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10480024, N'ابوالخیر', 143111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10480027, N'حسين آباد', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10490001, N'حسين اباد گرمسير', 141213, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10490005, N'اسداباد', 141911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10490011, N'آزادان', 142512, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10490013, N'زمان اباد', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10490020, N'جلال آباد', 143711, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10490024, N'رنگینده', 143111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10490027, N'جوزدان', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10500001, N'زيارتگاه', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10500005, N'حاجی اباد شورچمن', 141911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10500011, N'رباط گوگدی', 142511, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10500013, N'سرشك', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10500020, N'ابوالقاسم آباد', 143711, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10500024, N'فارفان', 143111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10510001, N'شمس اباد', 141213, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10510005, N'دیزجان', 141911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10510013, N'سيف آباد', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10510020, N'قهرمان آباد', 143711, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10510024, N'کفران', 143111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10520001, N'موغار', 141213, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10520005, N'ده نساسفلی', 141911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10520013, N'عفيف آباد', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10520020, N'ظاهر آباد', 143711, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10520024, N'کفرود', 143111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10530001, N'گلستان بهشتي', 141213, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10530005, N'نور ابادوردشت', 141911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10530013, N'فوداز', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10530020, N'جعفر آباد', 143711, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10530024, N'جندان', 143111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10540001, N'آستانه', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10540005, N'نرمه', 141911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10540013, N'قلعه دار', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10540020, N'حسین آباد', 143711, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10540024, N'بزم', 143111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10550001, N'بند آستانه', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10550005, N'قلعه قدم', 141913, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10550013, N'هاشم آباد', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10550020, N'ونک سفلی', 143711, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10550024, N'سهران', 143111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10560001, N'داوران', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10560005, N'علی اباد اغداش', 141911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10560013, N'مظفراباد', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10560020, N'ورباد سفلی', 143711, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10560024, N'قلعه امام', 143111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10570001, N'شيرازان', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10570005, N'اسلام اباداغداش', 141911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10570013, N'مهراباد', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10570020, N'دره مهدیقلی', 143711, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10570024, N'اشکهران', 143111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10580001, N'كچوسنگ', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10580005, N'مهدی اباد اغداش', 141911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10580013, N'نصراباد', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10580020, N'ونک علیا', 143711, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10580024, N'بلان', 143111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10590001, N'گونيان', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10590005, N'چشمه سرد', 141911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10590013, N'اوشن عليا', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10590020, N'دره آلوچه', 143711, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10590024, N'قورتان', 143111, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10600001, N'همبر', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10600005, N'سرچغاعلیا', 141911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10600013, N'فهيه', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10600024, N'شریف آباد', 143211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10610001, N'همسار', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10610005, N'بیده', 141914, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10610013, N'ماندگي عليا', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10610024, N'باقرآباد', 143211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10620001, N'گل شكنان', 141212, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10620005, N'لرکش', 141914, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10620013, N'مرغچوئيه', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10620024, N'کبریت', 143211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10630001, N'سفيده', 141212, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10630005, N'گنجگان', 141914, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10630013, N'هماابادعليا', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10630024, N'هاشم آباد', 143211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10640001, N'شهراب', 141212, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10640005, N'کاسگان سفلی', 141911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10640013, N'خرمدشت', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10640024, N'مند آباد', 142411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10650001, N'همت اباد', 141212, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10650005, N'حسین اباد', 141911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10650013, N'اله آباد', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10650024, N'هماگران', 142411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10660001, N'مزداباد', 141212, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10660005, N'بارند سفلی', 141914, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10660013, N'خيرآباد', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10660024, N'میرهمایون', 142411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10670001, N'احمد اباد', 141213, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10670005, N'صادق اباد', 141911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10670013, N'اميرآباد', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10670024, N'سسن آباد', 142411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10680001, N'اسلام اباد', 141213, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10680005, N'ماندگان', 141911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10680013, N'ماندگي سفلي', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10680024, N'لوتری', 142411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10690001, N'امير اباد', 141212, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10690005, N'سرباز', 141914, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10690013, N'سپيده', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10690024, N'هریزه', 142411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10700001, N'اميران', 141212, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10700005, N'حسن آباد', 141914, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10700013, N'موسي آباد', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10700024, N'تینجان', 142411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10710001, N'تلك اباد', 141212, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10710002, N'كاج', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10710005, N'پهلو شکن', 141914, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10710013, N'هماآبادسفلي', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10710024, N'فشارک', 142411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10720001, N'جعفراباد ريگستان', 141212, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10720005, N'شیبانی', 141914, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10720013, N'خارزن', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10720024, N'آبفا تودشک', 142413, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10730001, N'جهان اباد', 141212, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10730005, N'بارند علیا', 141914, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10730013, N'مزرعه نو', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10730024, N'پایگاه هوایی هاشم آباد', 142413, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10740001, N'حسين اباد ريگستان', 141212, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10740005, N'دورجان علیا', 141914, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10740013, N'معصوم آباد', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10740024, N'مزرعه یزدی', 142413, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10750001, N'علي اباد ريگستان', 141212, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10750005, N'تل محمد', 141914, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10750013, N'كوشكوئيه', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10750024, N'سیدآباد', 142413, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10760001, N'كچورستاق', 141212, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10760005, N'دره ماسون', 141914, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10760013, N'نرگور', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10760024, N'مهدی آباد', 142412, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10770001, N'كريم اباد', 141212, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10770005, N'دورجان سفلی', 141914, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10770013, N'حسين آبادنرگور', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10770024, N'علی آباد', 142413, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10780001, N'گلزارمحمد', 141213, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10780002, N'مهدي آباد', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10780005, N'بی بی سیدان', 141911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10780013, N'نوشي', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10790001, N'شهيد رجايي', 141212, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10790002, N'سروشبادران', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10790005, N'رهیز', 141914, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10790013, N'بلابادچه', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10800001, N'گلستان مهدي', 141213, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10800002, N'امين اباد', 144311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10800005, N'کهنگان', 141914, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10800013, N'نوجوك', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10810001, N'معين اباد', 141212, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10810002, N'جلادران', 144311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10820001, N'نير اباد', 141212, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10820002, N'جيلان آباد', 144311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10820005, N'نادرآباد', 141911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10820013, N'سنجدو', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10830001, N'علي اباد منصور', 141212, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10830002, N'حسن اباد', 144311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10830005, N'کاسگان علیا', 141911, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10830013, N'راحت آباد', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10840001, N'دهنو', 141212, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10840002, N'سوسارت', 144311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10840013, N'عشرت آباد', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10850001, N'سيد اباد', 141213, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10850002, N'فيروزاباد', 144311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10850005, N'ذبیح آباد', 141914, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10850013, N'كمال آباد', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10860001, N'نجف اباد ريگ', 141212, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10860002, N'كلمنجان', 144311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10860013, N'ورپشت', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10870001, N'حبيب اباد ريگ', 141212, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10870002, N'محمداباد', 144311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10870013, N'ورچم', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10880001, N'عباس اباد ريگ', 141212, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10880002, N'مزرعه گورت', 144311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10880013, N'مزرعه نو حاج قنبري', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10890001, N'باقر اباد ريگ', 141212, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10890002, N'هتم آباد', 144311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10890013, N'مزيک', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10900001, N'حاجي اباد ريگ', 141212, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10900002, N'اندوان', 144311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10900013, N'حسين آباد حاج کاظم', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10910001, N'بلهور', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10910002, N'بهاران', 144311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10910013, N'نجف آباد', 142811, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10920001, N'حسن اباد مظاهري', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10920002, N'جلمرز', 144311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10930001, N'كماسه', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10930002, N'گيان', 144311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10940001, N'حسن اباد ريگ', 141212, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10940002, N'مورنان', 144311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10950001, N'نصر اباد', 141212, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10950002, N'زمان آباد', 141304, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10960001, N'خالق اباد', 141212, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10960002, N'دينان', 144311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10970001, N'يافت اباد', 141212, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10970002, N'مولنجان', 141304, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10980001, N'پنارت', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10980002, N'آمرزيدآباد', 144311, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10990001, N'ميرزاعلي', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (11000001, N'بيدشك', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (11010001, N'ونين', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (11010002, N'دشتي', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (11020001, N'سهامیه', 141213, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (11020002, N'قلعه شور', 149011, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (11030001, N'امیدیه', 141213, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (11040001, N'دشت آزادگان', 141213, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (11040002, N'كبجوان', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (11050001, N'تل آباد', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (11060001, N'سهرویه', 141211, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (11060002, N'كيچي كراج', 149011, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (11070001, N'خرم آباد', 141213, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (11090002, N'شهرك فجر', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (11100002, N'شهرك سرو', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (11120002, N'كوي راه حق', 149011, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (11130002, N'حيدرآبادكراج', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (11590002, N'اشكاوند', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (11600002, N'قلعه چوم', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (11610002, N'اصفهانك', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (11630002, N'چيريان', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (11640002, N'زردنجان', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (11660002, N'راشنان', 144411, 1)
GO
INSERT [dbo].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (11680002, N'هفشوئيه', 144311, 1)
GO
/*INSERT [dbo].[CaptchaDisplayMode] ([Id], [DisplayModeEnumId], [Name], [Tite]) VALUES (1, 0, N'NumberToWord', N'تبدیل حروف به ارقام')
GO
INSERT [dbo].[CaptchaDisplayMode] ([Id], [DisplayModeEnumId], [Name], [Tite]) VALUES (2, 1, N'ShowDigits', N'نمایش ارقام')
GO
INSERT [dbo].[CaptchaDisplayMode] ([Id], [DisplayModeEnumId], [Name], [Tite]) VALUES (3, 2, N'SumOfTwoNumbers', N'جمع دو عدد')
GO
INSERT [dbo].[CaptchaDisplayMode] ([Id], [DisplayModeEnumId], [Name], [Tite]) VALUES (4, 3, N'SumOfTwoNumbersToWords', N'جمع دو عدد با حروف')
GO
INSERT [dbo].[CaptchaLanguage] ([Id], [LanguageId], [Name], [Title]) VALUES (1, 1, N'Persian', N'فارسی')
GO
INSERT [dbo].[CaptchaLanguage] ([Id], [LanguageId], [Name], [Title]) VALUES (2, 0, N'English', N'English')
GO
INSERT [dbo].[CaptchaLanguage] ([Id], [LanguageId], [Name], [Title]) VALUES (3, 4, N'Turkish', N'Türkçe')
GO
INSERT [dbo].[CaptchaLanguage] ([Id], [LanguageId], [Name], [Title]) VALUES (4, 5, N'Arabic', N'عربی')
GO
SET IDENTITY_INSERT [dbo].[Captcha] ON 
GO
INSERT [dbo].[Captcha] ([Id], [CaptchaLanguageId], [CaptchaDisplayModeId], [ShowThousandSeperator], [FontName], [FontSize], [ForeColor], [BackColor], [ExpiresAfter], [RateLimit], [Noise], [EncryptionKey], [NonceKey], [Direction], [Min], [Max], [IsSelected]) VALUES (1, 1, 4, 1, N'Tahoma', 18, N'#111111', N'#f7f3f3', 7, 10, N'putNoiserAsYouCan', N'This is my secret', N'nonceKey', N'ltr', 0, 99, 1)
GO
SET IDENTITY_INSERT [dbo].[Captcha] OFF
GO
INSERT [dbo].[User] ([Id], [FullName], [DisplayName], [Username], [Password], [Mobile], [MobileConfirmed], [HasTwoStepVerification], [InvalidLoginAttemptCount], [SerialNumber], [LatestLoginDateTime], [LockTimespan], [PreviousId], [ValidFrom], [ValidTo], [InsertLogInfo], [RemoveLogInfo], [Hash]) VALUES (N'6bbd331b-2a40-4966-b0bc-6610a75b79b8', N'برنامه نویس', N'برنامه نویس', N'programmer', N'ujJTh2rta8ItSm/1PYQGxq2GQZXtFEq1yHYhtsIztUi66uaVbfNG7IwX9eoQ817jy8UUeX7X3dMUVGTioLq0Ew==', N'09130000000', 0, 0, 0, N'5e9bfa188bf8488e98e66d8ff986b0b4', NULL, NULL, NULL, CAST(N'2025-02-03T09:51:13.1630926' AS DateTime2), NULL, N'{"Device":{"Title":null,"IsBot":false,"IsMobile":false,"IsTablet":false,"IsTouchEnabled":false,"IsDesktop":false,"IsBrwoser":false,"Model":null,"Brand":null},"Os":{"ShortName":null,"Name":null,"Family":null,"Platform":null},"Client":{"Name":null,"Type":null,"Platform":null,"App":null,"Architecture":null,"Version":null},"Bot":{"Url":null,"Category":null,"Name":null,"Producer":null}}', NULL, N'RBNvo1WzZ4oRRq0W9+hknpT7T8If536DEMBg9hyq/4o=')
GO
SET IDENTITY_INSERT [dbo].[Role] ON 
GO
INSERT [dbo].[Role] ([Id], [Name], [Title], [DefaultClaims], [SensitiveInfo], [IsRemovable], [PreviousId], [ValidFrom], [ValidTo], [InsertLogInfo], [RemoveLogInfo], [Hash]) VALUES (1, N'Admin', N'راهبر سامانه', NULL, 0, 0, NULL, CAST(N'2025-02-03T09:51:13.2383682' AS DateTime2), NULL, N'{"Device":{"Title":null,"IsBot":false,"IsMobile":false,"IsTablet":false,"IsTouchEnabled":false,"IsDesktop":false,"IsBrwoser":false,"Model":null,"Brand":null},"Os":{"ShortName":null,"Name":null,"Family":null,"Platform":null},"Client":{"Name":null,"Type":null,"Platform":null,"App":null,"Architecture":null,"Version":null},"Bot":{"Url":null,"Category":null,"Name":null,"Producer":null}}', NULL, N'RBNvo1WzZ4oRRq0W9+hknpT7T8If536DEMBg9hyq/4o=')
GO
INSERT [dbo].[Role] ([Id], [Name], [Title], [DefaultClaims], [SensitiveInfo], [IsRemovable], [PreviousId], [ValidFrom], [ValidTo], [InsertLogInfo], [RemoveLogInfo], [Hash]) VALUES (2, N'Ai', N'دستیار هوشمند', NULL, 0, 0, NULL, CAST(N'2025-02-03T09:51:13.2383315' AS DateTime2), NULL, N'{"Device":{"Title":null,"IsBot":false,"IsMobile":false,"IsTablet":false,"IsTouchEnabled":false,"IsDesktop":false,"IsBrwoser":false,"Model":null,"Brand":null},"Os":{"ShortName":null,"Name":null,"Family":null,"Platform":null},"Client":{"Name":null,"Type":null,"Platform":null,"App":null,"Architecture":null,"Version":null},"Bot":{"Url":null,"Category":null,"Name":null,"Producer":null}}', NULL, N'RBNvo1WzZ4oRRq0W9+hknpT7T8If536DEMBg9hyq/4o=')
GO
INSERT [dbo].[Role] ([Id], [Name], [Title], [DefaultClaims], [SensitiveInfo], [IsRemovable], [PreviousId], [ValidFrom], [ValidTo], [InsertLogInfo], [RemoveLogInfo], [Hash]) VALUES (3, N'Programmer', N'برنامه نویس', NULL, 0, 0, NULL, CAST(N'2025-02-03T09:51:13.2376572' AS DateTime2), NULL, N'{"Device":{"Title":null,"IsBot":false,"IsMobile":false,"IsTablet":false,"IsTouchEnabled":false,"IsDesktop":false,"IsBrwoser":false,"Model":null,"Brand":null},"Os":{"ShortName":null,"Name":null,"Family":null,"Platform":null},"Client":{"Name":null,"Type":null,"Platform":null,"App":null,"Architecture":null,"Version":null},"Bot":{"Url":null,"Category":null,"Name":null,"Producer":null}}', NULL, N'RBNvo1WzZ4oRRq0W9+hknpT7T8If536DEMBg9hyq/4o=')
GO
SET IDENTITY_INSERT [dbo].[Role] OFF
GO
INSERT [dbo].[InvalidLoginReason] ([Id], [Title]) VALUES (1, N'نام کاربری نادرست')
GO
INSERT [dbo].[InvalidLoginReason] ([Id], [Title]) VALUES (2, N'پسورد نادرست')
GO
INSERT [dbo].[InvalidLoginReason] ([Id], [Title]) VALUES (3, N'کد دو مرحله ای نادرست')
GO
INSERT [dbo].[InvalidLoginReason] ([Id], [Title]) VALUES (4, N'کد دو مرحله ای منقضی شده')
GO
INSERT [dbo].[InvalidLoginReason] ([Id], [Title]) VALUES (5, N'تلاش پس از قفل')
GO
INSERT [dbo].[InvalidLoginReason] ([Id], [Title]) VALUES (6, N'تلاش کاربر غیرفعال')
GO
INSERT [dbo].[LogoutReason] ([Id], [Title]) VALUES (1, N'توسط کاربر')
GO
INSERT [dbo].[LogoutReason] ([Id], [Title]) VALUES (2, N'توسط ادمین')
GO
INSERT [dbo].[LogoutReason] ([Id], [Title]) VALUES (3, N'تغیر پسورد')
GO
INSERT [dbo].[LogoutReason] ([Id], [Title]) VALUES (4, N'ویرایش توسط ادمین')
GO
INSERT [dbo].[LogoutReason] ([Id], [Title]) VALUES (5, N'انقضای توکن')
GO
INSERT [dbo].[LogoutReason] ([Id], [Title]) VALUES (6, N'تغیر IP در جلسه جاری')
GO
INSERT [dbo].[LogoutReason] ([Id], [Title]) VALUES (7, N'تغییر مشخصات کلاینت')
GO
INSERT [dbo].[LogoutReason] ([Id], [Title]) VALUES (8, N'لاگین همزمان')
GO
SET IDENTITY_INSERT [dbo].[Logs] ON 
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (1, N'Start installing Hangfire SQL objects...', N'{State:l}', N'Information', CAST(N'2025-02-03T09:51:13.683' AS DateTime), NULL, N'<properties><property key=''State''>Start installing Hangfire SQL objects...</property><property key=''SourceContext''>Hangfire.SqlServer.SqlServerObjectsInstaller</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (2, N'Hangfire SQL objects installed.', N'{State:l}', N'Information', CAST(N'2025-02-03T09:51:14.247' AS DateTime), NULL, N'<properties><property key=''State''>Hangfire SQL objects installed.</property><property key=''SourceContext''>Hangfire.SqlServer.SqlServerObjectsInstaller</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (3, N'Registered model binder providers, in the following order: ["Microsoft.AspNetCore.Mvc.ModelBinding.Binders.BinderTypeModelBinderProvider", "Microsoft.AspNetCore.Mvc.ModelBinding.Binders.ServicesModelBinderProvider", "Microsoft.AspNetCore.Mvc.ModelBinding.Binders.BodyModelBinderProvider", "Microsoft.AspNetCore.Mvc.ModelBinding.Binders.HeaderModelBinderProvider", "Microsoft.AspNetCore.Mvc.ModelBinding.Binders.FloatingPointTypeModelBinderProvider", "Microsoft.AspNetCore.Mvc.ModelBinding.Binders.EnumTypeModelBinderProvider", "Microsoft.AspNetCore.Mvc.ModelBinding.Binders.DateTimeModelBinderProvider", "Microsoft.AspNetCore.Mvc.ModelBinding.Binders.SimpleTypeModelBinderProvider", "Microsoft.AspNetCore.Mvc.ModelBinding.Binders.TryParseModelBinderProvider", "Microsoft.AspNetCore.Mvc.ModelBinding.Binders.CancellationTokenModelBinderProvider", "Microsoft.AspNetCore.Mvc.ModelBinding.Binders.ByteArrayModelBinderProvider", "Microsoft.AspNetCore.Mvc.ModelBinding.Binders.FormFileModelBinderProvider", "Microsoft.AspNetCore.Mvc.ModelBinding.Binders.FormCollectionModelBinderProvider", "Microsoft.AspNetCore.Mvc.ModelBinding.Binders.KeyValuePairModelBinderProvider", "Microsoft.AspNetCore.Mvc.ModelBinding.Binders.DictionaryModelBinderProvider", "Microsoft.AspNetCore.Mvc.ModelBinding.Binders.ArrayModelBinderProvider", "Microsoft.AspNetCore.Mvc.ModelBinding.Binders.CollectionModelBinderProvider", "Microsoft.AspNetCore.Mvc.ModelBinding.Binders.ComplexObjectModelBinderProvider"]', N'Registered model binder providers, in the following order: {ModelBinderProviders}', N'Debug', CAST(N'2025-02-03T09:51:14.293' AS DateTime), NULL, N'<properties><property key=''ModelBinderProviders''><sequence><item>Microsoft.AspNetCore.Mvc.ModelBinding.Binders.BinderTypeModelBinderProvider</item><item>Microsoft.AspNetCore.Mvc.ModelBinding.Binders.ServicesModelBinderProvider</item><item>Microsoft.AspNetCore.Mvc.ModelBinding.Binders.BodyModelBinderProvider</item><item>Microsoft.AspNetCore.Mvc.ModelBinding.Binders.HeaderModelBinderProvider</item><item>Microsoft.AspNetCore.Mvc.ModelBinding.Binders.FloatingPointTypeModelBinderProvider</item><item>Microsoft.AspNetCore.Mvc.ModelBinding.Binders.EnumTypeModelBinderProvider</item><item>Microsoft.AspNetCore.Mvc.ModelBinding.Binders.DateTimeModelBinderProvider</item><item>Microsoft.AspNetCore.Mvc.ModelBinding.Binders.SimpleTypeModelBinderProvider</item><item>Microsoft.AspNetCore.Mvc.ModelBinding.Binders.TryParseModelBinderProvider</item><item>Microsoft.AspNetCore.Mvc.ModelBinding.Binders.CancellationTokenModelBinderProvider</item><item>Microsoft.AspNetCore.Mvc.ModelBinding.Binders.ByteArrayModelBinderProvider</item><item>Microsoft.AspNetCore.Mvc.ModelBinding.Binders.FormFileModelBinderProvider</item><item>Microsoft.AspNetCore.Mvc.ModelBinding.Binders.FormCollectionModelBinderProvider</item><item>Microsoft.AspNetCore.Mvc.ModelBinding.Binders.KeyValuePairModelBinderProvider</item><item>Microsoft.AspNetCore.Mvc.ModelBinding.Binders.DictionaryModelBinderProvider</item><item>Microsoft.AspNetCore.Mvc.ModelBinding.Binders.ArrayModelBinderProvider</item><item>Microsoft.AspNetCore.Mvc.ModelBinding.Binders.CollectionModelBinderProvider</item><item>Microsoft.AspNetCore.Mvc.ModelBinding.Binders.ComplexObjectModelBinderProvider</item></sequence></property><property key=''EventId''><structure type=''''><property key=''Id''>12</property><property key=''Name''>RegisteredModelBinderProviders</property></structure></property><property key=''SourceContext''>Microsoft.AspNetCore.Mvc.ModelBinding.ModelBinderFactory</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (4, N'Hosting starting', N'Hosting starting', N'Debug', CAST(N'2025-02-03T09:51:14.350' AS DateTime), NULL, N'<properties><property key=''EventId''><structure type=''''><property key=''Id''>1</property><property key=''Name''>Starting</property></structure></property><property key=''SourceContext''>Microsoft.Extensions.Hosting.Internal.Host</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (5, N'User profile is available. Using ''"C:\Users\zahra\AppData\Local\ASP.NET\DataProtection-Keys"'' as key repository and Windows DPAPI to encrypt keys at rest.', N'User profile is available. Using ''{FullName}'' as key repository and Windows DPAPI to encrypt keys at rest.', N'Information', CAST(N'2025-02-03T09:51:14.360' AS DateTime), NULL, N'<properties><property key=''FullName''>C:\Users\zahra\AppData\Local\ASP.NET\DataProtection-Keys</property><property key=''EventId''><structure type=''''><property key=''Id''>63</property><property key=''Name''>UsingProfileAsKeyRepositoryWithDPAPI</property></structure></property><property key=''SourceContext''>Microsoft.AspNetCore.DataProtection.KeyManagement.XmlKeyManager</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (6, N'Reading data from file ''"C:\Users\zahra\AppData\Local\ASP.NET\DataProtection-Keys\key-0faf8820-b8a8-4f83-85df-101e90ae7dc1.xml"''.', N'Reading data from file ''{FullPath}''.', N'Debug', CAST(N'2025-02-03T09:51:14.377' AS DateTime), NULL, N'<properties><property key=''FullPath''>C:\Users\zahra\AppData\Local\ASP.NET\DataProtection-Keys\key-0faf8820-b8a8-4f83-85df-101e90ae7dc1.xml</property><property key=''EventId''><structure type=''''><property key=''Id''>37</property><property key=''Name''>ReadingDataFromFile</property></structure></property><property key=''SourceContext''>Microsoft.AspNetCore.DataProtection.Repositories.FileSystemXmlRepository</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (7, N'Reading data from file ''"C:\Users\zahra\AppData\Local\ASP.NET\DataProtection-Keys\key-69864b2a-a4e0-4492-8314-ddeda3c1eb89.xml"''.', N'Reading data from file ''{FullPath}''.', N'Debug', CAST(N'2025-02-03T09:51:14.383' AS DateTime), NULL, N'<properties><property key=''FullPath''>C:\Users\zahra\AppData\Local\ASP.NET\DataProtection-Keys\key-69864b2a-a4e0-4492-8314-ddeda3c1eb89.xml</property><property key=''EventId''><structure type=''''><property key=''Id''>37</property><property key=''Name''>ReadingDataFromFile</property></structure></property><property key=''SourceContext''>Microsoft.AspNetCore.DataProtection.Repositories.FileSystemXmlRepository</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (8, N'Reading data from file ''"C:\Users\zahra\AppData\Local\ASP.NET\DataProtection-Keys\key-d0932e56-40f6-47df-9383-e466f22625b2.xml"''.', N'Reading data from file ''{FullPath}''.', N'Debug', CAST(N'2025-02-03T09:51:14.390' AS DateTime), NULL, N'<properties><property key=''FullPath''>C:\Users\zahra\AppData\Local\ASP.NET\DataProtection-Keys\key-d0932e56-40f6-47df-9383-e466f22625b2.xml</property><property key=''EventId''><structure type=''''><property key=''Id''>37</property><property key=''Name''>ReadingDataFromFile</property></structure></property><property key=''SourceContext''>Microsoft.AspNetCore.DataProtection.Repositories.FileSystemXmlRepository</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (9, N'Found key {0faf8820-b8a8-4f83-85df-101e90ae7dc1}.', N'Found key {KeyId:B}.', N'Debug', CAST(N'2025-02-03T09:51:14.393' AS DateTime), NULL, N'<properties><property key=''KeyId''>0faf8820-b8a8-4f83-85df-101e90ae7dc1</property><property key=''EventId''><structure type=''''><property key=''Id''>18</property><property key=''Name''>FoundKey</property></structure></property><property key=''SourceContext''>Microsoft.AspNetCore.DataProtection.KeyManagement.XmlKeyManager</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (10, N'Found key {69864b2a-a4e0-4492-8314-ddeda3c1eb89}.', N'Found key {KeyId:B}.', N'Debug', CAST(N'2025-02-03T09:51:14.397' AS DateTime), NULL, N'<properties><property key=''KeyId''>69864b2a-a4e0-4492-8314-ddeda3c1eb89</property><property key=''EventId''><structure type=''''><property key=''Id''>18</property><property key=''Name''>FoundKey</property></structure></property><property key=''SourceContext''>Microsoft.AspNetCore.DataProtection.KeyManagement.XmlKeyManager</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (11, N'Found key {d0932e56-40f6-47df-9383-e466f22625b2}.', N'Found key {KeyId:B}.', N'Debug', CAST(N'2025-02-03T09:51:14.397' AS DateTime), NULL, N'<properties><property key=''KeyId''>d0932e56-40f6-47df-9383-e466f22625b2</property><property key=''EventId''><structure type=''''><property key=''Id''>18</property><property key=''Name''>FoundKey</property></structure></property><property key=''SourceContext''>Microsoft.AspNetCore.DataProtection.KeyManagement.XmlKeyManager</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (12, N'Considering key {69864b2a-a4e0-4492-8314-ddeda3c1eb89} with expiration date 2025-03-26 17:15:22Z as default key.', N'Considering key {KeyId:B} with expiration date {ExpirationDate:u} as default key.', N'Debug', CAST(N'2025-02-03T09:51:14.400' AS DateTime), NULL, N'<properties><property key=''KeyId''>69864b2a-a4e0-4492-8314-ddeda3c1eb89</property><property key=''ExpirationDate''>3/26/2025 5:15:22 PM +00:00</property><property key=''EventId''><structure type=''''><property key=''Id''>13</property><property key=''Name''>ConsideringKeyWithExpirationDateAsDefaultKey</property></structure></property><property key=''SourceContext''>Microsoft.AspNetCore.DataProtection.KeyManagement.DefaultKeyResolver</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (13, N'Forwarded activator type request from "Microsoft.AspNetCore.DataProtection.XmlEncryption.DpapiXmlDecryptor, Microsoft.AspNetCore.DataProtection, Version=5.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60" to "Microsoft.AspNetCore.DataProtection.XmlEncryption.DpapiXmlDecryptor, Microsoft.AspNetCore.DataProtection, Culture=neutral, PublicKeyToken=adb9793829ddae60"', N'Forwarded activator type request from {FromType} to {ToType}', N'Debug', CAST(N'2025-02-03T09:51:14.403' AS DateTime), NULL, N'<properties><property key=''FromType''>Microsoft.AspNetCore.DataProtection.XmlEncryption.DpapiXmlDecryptor, Microsoft.AspNetCore.DataProtection, Version=5.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60</property><property key=''ToType''>Microsoft.AspNetCore.DataProtection.XmlEncryption.DpapiXmlDecryptor, Microsoft.AspNetCore.DataProtection, Culture=neutral, PublicKeyToken=adb9793829ddae60</property><property key=''SourceContext''>Microsoft.AspNetCore.DataProtection.TypeForwardingActivator</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (14, N'Decrypting secret element using Windows DPAPI.', N'Decrypting secret element using Windows DPAPI.', N'Debug', CAST(N'2025-02-03T09:51:14.407' AS DateTime), NULL, N'<properties><property key=''EventId''><structure type=''''><property key=''Id''>51</property><property key=''Name''>DecryptingSecretElementUsingWindowsDPAPI</property></structure></property><property key=''SourceContext''>Microsoft.AspNetCore.DataProtection.XmlEncryption.DpapiXmlDecryptor</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (15, N'Forwarded activator type request from "Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel.AuthenticatedEncryptorDescriptorDeserializer, Microsoft.AspNetCore.DataProtection, Version=5.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60" to "Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel.AuthenticatedEncryptorDescriptorDeserializer, Microsoft.AspNetCore.DataProtection, Culture=neutral, PublicKeyToken=adb9793829ddae60"', N'Forwarded activator type request from {FromType} to {ToType}', N'Debug', CAST(N'2025-02-03T09:51:14.410' AS DateTime), NULL, N'<properties><property key=''FromType''>Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel.AuthenticatedEncryptorDescriptorDeserializer, Microsoft.AspNetCore.DataProtection, Version=5.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60</property><property key=''ToType''>Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel.AuthenticatedEncryptorDescriptorDeserializer, Microsoft.AspNetCore.DataProtection, Culture=neutral, PublicKeyToken=adb9793829ddae60</property><property key=''SourceContext''>Microsoft.AspNetCore.DataProtection.TypeForwardingActivator</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (16, N'Opening CNG algorithm ''"AES"'' from provider ''null'' with chaining mode CBC.', N'Opening CNG algorithm ''{EncryptionAlgorithm}'' from provider ''{EncryptionAlgorithmProvider}'' with chaining mode CBC.', N'Debug', CAST(N'2025-02-03T09:51:14.413' AS DateTime), NULL, N'<properties><property key=''EncryptionAlgorithm''>AES</property><property key=''EncryptionAlgorithmProvider''></property><property key=''EventId''><structure type=''''><property key=''Id''>4</property><property key=''Name''>OpeningCNGAlgorithmFromProviderWithChainingModeCBC</property></structure></property><property key=''SourceContext''>Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.CngCbcAuthenticatedEncryptorFactory</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (17, N'Opening CNG algorithm ''"SHA256"'' from provider ''null'' with HMAC.', N'Opening CNG algorithm ''{HashAlgorithm}'' from provider ''{HashAlgorithmProvider}'' with HMAC.', N'Debug', CAST(N'2025-02-03T09:51:14.413' AS DateTime), NULL, N'<properties><property key=''HashAlgorithm''>SHA256</property><property key=''HashAlgorithmProvider''></property><property key=''EventId''><structure type=''''><property key=''Id''>3</property><property key=''Name''>OpeningCNGAlgorithmFromProviderWithHMAC</property></structure></property><property key=''SourceContext''>Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.CngCbcAuthenticatedEncryptorFactory</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (18, N'Using key {69864b2a-a4e0-4492-8314-ddeda3c1eb89} as the default key.', N'Using key {KeyId:B} as the default key.', N'Debug', CAST(N'2025-02-03T09:51:14.417' AS DateTime), NULL, N'<properties><property key=''KeyId''>69864b2a-a4e0-4492-8314-ddeda3c1eb89</property><property key=''EventId''><structure type=''''><property key=''Id''>2</property><property key=''Name''>UsingKeyAsDefaultKey</property></structure></property><property key=''SourceContext''>Microsoft.AspNetCore.DataProtection.KeyManagement.KeyRingProvider</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (19, N'Key ring with default key {69864b2a-a4e0-4492-8314-ddeda3c1eb89} was loaded during application startup.', N'Key ring with default key {KeyId:B} was loaded during application startup.', N'Debug', CAST(N'2025-02-03T09:51:14.417' AS DateTime), NULL, N'<properties><property key=''KeyId''>69864b2a-a4e0-4492-8314-ddeda3c1eb89</property><property key=''EventId''><structure type=''''><property key=''Id''>65</property><property key=''Name''>KeyRingWasLoadedOnStartup</property></structure></property><property key=''SourceContext''>Microsoft.AspNetCore.DataProtection.Internal.DataProtectionHostedService</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (20, N'Now listening on: "https://localhost:7239"', N'Now listening on: {address}', N'Information', CAST(N'2025-02-03T09:51:14.513' AS DateTime), NULL, N'<properties><property key=''address''>https://localhost:7239</property><property key=''EventId''><structure type=''''><property key=''Id''>14</property><property key=''Name''>ListeningOnAddress</property></structure></property><property key=''SourceContext''>Microsoft.Hosting.Lifetime</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (21, N'Now listening on: "http://localhost:5239"', N'Now listening on: {address}', N'Information', CAST(N'2025-02-03T09:51:14.527' AS DateTime), NULL, N'<properties><property key=''address''>http://localhost:5239</property><property key=''EventId''><structure type=''''><property key=''Id''>14</property><property key=''Name''>ListeningOnAddress</property></structure></property><property key=''SourceContext''>Microsoft.Hosting.Lifetime</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (22, N'Connection id ""0HNA43BR1NAJ5"" received FIN.', N'Connection id "{ConnectionId}" received FIN.', N'Debug', CAST(N'2025-02-03T09:51:14.533' AS DateTime), NULL, N'<properties><property key=''ConnectionId''>0HNA43BR1NAJ5</property><property key=''EventId''><structure type=''''><property key=''Id''>6</property><property key=''Name''>ConnectionReadFin</property></structure></property><property key=''SourceContext''>Microsoft.AspNetCore.Server.Kestrel.Transport.Sockets</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (23, N'Connection id ""0HNA43BR1NAJ4"" received FIN.', N'Connection id "{ConnectionId}" received FIN.', N'Debug', CAST(N'2025-02-03T09:51:14.533' AS DateTime), NULL, N'<properties><property key=''ConnectionId''>0HNA43BR1NAJ4</property><property key=''EventId''><structure type=''''><property key=''Id''>6</property><property key=''Name''>ConnectionReadFin</property></structure></property><property key=''SourceContext''>Microsoft.AspNetCore.Server.Kestrel.Transport.Sockets</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (24, N'Loaded hosting startup assembly "Aban360.Api"', N'Loaded hosting startup assembly {assemblyName}', N'Debug', CAST(N'2025-02-03T09:51:14.537' AS DateTime), NULL, N'<properties><property key=''assemblyName''>Aban360.Api</property><property key=''EventId''><structure type=''''><property key=''Id''>13</property><property key=''Name''>HostingStartupAssemblyLoaded</property></structure></property><property key=''SourceContext''>Microsoft.AspNetCore.Hosting.Diagnostics</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (25, N'Connection id ""0HNA43BR1NAJ4"" accepted.', N'Connection id "{ConnectionId}" accepted.', N'Debug', CAST(N'2025-02-03T09:51:14.543' AS DateTime), NULL, N'<properties><property key=''ConnectionId''>0HNA43BR1NAJ4</property><property key=''EventId''><structure type=''''><property key=''Id''>39</property><property key=''Name''>ConnectionAccepted</property></structure></property><property key=''SourceContext''>Microsoft.AspNetCore.Server.Kestrel.Connections</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (26, N'Loaded hosting startup assembly "Microsoft.WebTools.ApiEndpointDiscovery"', N'Loaded hosting startup assembly {assemblyName}', N'Debug', CAST(N'2025-02-03T09:51:14.543' AS DateTime), NULL, N'<properties><property key=''assemblyName''>Microsoft.WebTools.ApiEndpointDiscovery</property><property key=''EventId''><structure type=''''><property key=''Id''>13</property><property key=''Name''>HostingStartupAssemblyLoaded</property></structure></property><property key=''SourceContext''>Microsoft.AspNetCore.Hosting.Diagnostics</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (27, N'Connection id ""0HNA43BR1NAJ5"" accepted.', N'Connection id "{ConnectionId}" accepted.', N'Debug', CAST(N'2025-02-03T09:51:14.543' AS DateTime), NULL, N'<properties><property key=''ConnectionId''>0HNA43BR1NAJ5</property><property key=''EventId''><structure type=''''><property key=''Id''>39</property><property key=''Name''>ConnectionAccepted</property></structure></property><property key=''SourceContext''>Microsoft.AspNetCore.Server.Kestrel.Connections</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (28, N'Loaded hosting startup assembly "Microsoft.AspNetCore.Watch.BrowserRefresh"', N'Loaded hosting startup assembly {assemblyName}', N'Debug', CAST(N'2025-02-03T09:51:14.547' AS DateTime), NULL, N'<properties><property key=''assemblyName''>Microsoft.AspNetCore.Watch.BrowserRefresh</property><property key=''EventId''><structure type=''''><property key=''Id''>13</property><property key=''Name''>HostingStartupAssemblyLoaded</property></structure></property><property key=''SourceContext''>Microsoft.AspNetCore.Hosting.Diagnostics</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (29, N'Connection id ""0HNA43BR1NAJ4"" started.', N'Connection id "{ConnectionId}" started.', N'Debug', CAST(N'2025-02-03T09:51:14.547' AS DateTime), NULL, N'<properties><property key=''ConnectionId''>0HNA43BR1NAJ4</property><property key=''EventId''><structure type=''''><property key=''Id''>1</property><property key=''Name''>ConnectionStart</property></structure></property><property key=''SourceContext''>Microsoft.AspNetCore.Server.Kestrel.Connections</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (30, N'Loaded hosting startup assembly "Microsoft.WebTools.BrowserLink.Net"', N'Loaded hosting startup assembly {assemblyName}', N'Debug', CAST(N'2025-02-03T09:51:14.557' AS DateTime), NULL, N'<properties><property key=''assemblyName''>Microsoft.WebTools.BrowserLink.Net</property><property key=''EventId''><structure type=''''><property key=''Id''>13</property><property key=''Name''>HostingStartupAssemblyLoaded</property></structure></property><property key=''SourceContext''>Microsoft.AspNetCore.Hosting.Diagnostics</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (31, N'Connection id ""0HNA43BR1NAJ5"" started.', N'Connection id "{ConnectionId}" started.', N'Debug', CAST(N'2025-02-03T09:51:14.553' AS DateTime), NULL, N'<properties><property key=''ConnectionId''>0HNA43BR1NAJ5</property><property key=''EventId''><structure type=''''><property key=''Id''>1</property><property key=''Name''>ConnectionStart</property></structure></property><property key=''SourceContext''>Microsoft.AspNetCore.Server.Kestrel.Connections</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (32, N'Failed to authenticate HTTPS connection.', N'Failed to authenticate HTTPS connection.', N'Debug', CAST(N'2025-02-03T09:51:14.577' AS DateTime), N'System.IO.IOException: Received an unexpected EOF or 0 bytes from the transport stream.
   at System.Net.Security.SslStream.ReceiveHandshakeFrameAsync[TIOAdapter](CancellationToken cancellationToken)
   at System.Net.Security.SslStream.ForceAuthenticationAsync[TIOAdapter](Boolean receiveFirst, Byte[] reAuthenticationData, CancellationToken cancellationToken)
   at System.Net.Security.SslStream.ProcessAuthenticationWithTelemetryAsync(Boolean isAsync, CancellationToken cancellationToken)
   at Microsoft.AspNetCore.Server.Kestrel.Https.Internal.HttpsConnectionMiddleware.OnConnectionAsync(ConnectionContext context)', N'<properties><property key=''EventId''><structure type=''''><property key=''Id''>1</property><property key=''Name''>AuthenticationFailed</property></structure></property><property key=''SourceContext''>Microsoft.AspNetCore.Server.Kestrel.Https.Internal.HttpsConnectionMiddleware</property><property key=''ConnectionId''>0HNA43BR1NAJ5</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (33, N'Failed to authenticate HTTPS connection.', N'Failed to authenticate HTTPS connection.', N'Debug', CAST(N'2025-02-03T09:51:14.577' AS DateTime), N'System.IO.IOException: Received an unexpected EOF or 0 bytes from the transport stream.
   at System.Net.Security.SslStream.ReceiveHandshakeFrameAsync[TIOAdapter](CancellationToken cancellationToken)
   at System.Net.Security.SslStream.ForceAuthenticationAsync[TIOAdapter](Boolean receiveFirst, Byte[] reAuthenticationData, CancellationToken cancellationToken)
   at System.Net.Security.SslStream.ProcessAuthenticationWithTelemetryAsync(Boolean isAsync, CancellationToken cancellationToken)
   at Microsoft.AspNetCore.Server.Kestrel.Https.Internal.HttpsConnectionMiddleware.OnConnectionAsync(ConnectionContext context)', N'<properties><property key=''EventId''><structure type=''''><property key=''Id''>1</property><property key=''Name''>AuthenticationFailed</property></structure></property><property key=''SourceContext''>Microsoft.AspNetCore.Server.Kestrel.Https.Internal.HttpsConnectionMiddleware</property><property key=''ConnectionId''>0HNA43BR1NAJ4</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (34, N'Connection id ""0HNA43BR1NAJ5"" stopped.', N'Connection id "{ConnectionId}" stopped.', N'Debug', CAST(N'2025-02-03T09:51:14.593' AS DateTime), NULL, N'<properties><property key=''ConnectionId''>0HNA43BR1NAJ5</property><property key=''EventId''><structure type=''''><property key=''Id''>2</property><property key=''Name''>ConnectionStop</property></structure></property><property key=''SourceContext''>Microsoft.AspNetCore.Server.Kestrel.Connections</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (35, N'Connection id ""0HNA43BR1NAJ4"" stopped.', N'Connection id "{ConnectionId}" stopped.', N'Debug', CAST(N'2025-02-03T09:51:14.600' AS DateTime), NULL, N'<properties><property key=''ConnectionId''>0HNA43BR1NAJ4</property><property key=''EventId''><structure type=''''><property key=''Id''>2</property><property key=''Name''>ConnectionStop</property></structure></property><property key=''SourceContext''>Microsoft.AspNetCore.Server.Kestrel.Connections</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (36, N'Connection id ""0HNA43BR1NAJ5"" sending FIN because: ""The Socket transport''s send loop completed gracefully.""', N'Connection id "{ConnectionId}" sending FIN because: "{Reason}"', N'Debug', CAST(N'2025-02-03T09:51:14.607' AS DateTime), NULL, N'<properties><property key=''ConnectionId''>0HNA43BR1NAJ5</property><property key=''Reason''>The Socket transport''s send loop completed gracefully.</property><property key=''EventId''><structure type=''''><property key=''Id''>7</property><property key=''Name''>ConnectionWriteFin</property></structure></property><property key=''SourceContext''>Microsoft.AspNetCore.Server.Kestrel.Transport.Sockets</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (37, N'Connection id ""0HNA43BR1NAJ4"" sending FIN because: ""The Socket transport''s send loop completed gracefully.""', N'Connection id "{ConnectionId}" sending FIN because: "{Reason}"', N'Debug', CAST(N'2025-02-03T09:51:14.607' AS DateTime), NULL, N'<properties><property key=''ConnectionId''>0HNA43BR1NAJ4</property><property key=''Reason''>The Socket transport''s send loop completed gracefully.</property><property key=''EventId''><structure type=''''><property key=''Id''>7</property><property key=''Name''>ConnectionWriteFin</property></structure></property><property key=''SourceContext''>Microsoft.AspNetCore.Server.Kestrel.Transport.Sockets</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (38, N'Starting Hangfire Server using job storage: ''SQL Server: .@Aban360''', N'{State:l}', N'Information', CAST(N'2025-02-03T09:51:14.620' AS DateTime), NULL, N'<properties><property key=''State''>Starting Hangfire Server using job storage: ''SQL Server: .@Aban360''</property><property key=''SourceContext''>Hangfire.BackgroundJobServer</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (39, N'Using the following options for SQL Server job storage: Queue poll interval: 00:00:00.', N'{State:l}', N'Information', CAST(N'2025-02-03T09:51:14.623' AS DateTime), NULL, N'<properties><property key=''State''>Using the following options for SQL Server job storage: Queue poll interval: 00:00:00.</property><property key=''SourceContext''>Hangfire.BackgroundJobServer</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (40, N'Using the following options for Hangfire Server:
    Worker count: 20
    Listening queues: ''default''
    Shutdown timeout: 00:00:15
    Schedule polling interval: 00:00:15', N'{State:l}', N'Information', CAST(N'2025-02-03T09:51:14.627' AS DateTime), NULL, N'<properties><property key=''State''>Using the following options for Hangfire Server:
    Worker count: 20
    Listening queues: ''default''
    Shutdown timeout: 00:00:15
    Schedule polling interval: 00:00:15</property><property key=''SourceContext''>Hangfire.BackgroundJobServer</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (41, N'Execution loop BackgroundServerProcess:97b92c09 has started in 7.3602 ms', N'{State:l}', N'Debug', CAST(N'2025-02-03T09:51:14.640' AS DateTime), NULL, N'<properties><property key=''State''>Execution loop BackgroundServerProcess:97b92c09 has started in 7.3602 ms</property><property key=''SourceContext''>Hangfire.Processing.BackgroundExecution</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (42, N'Application started. Press Ctrl+C to shut down.', N'Application started. Press Ctrl+C to shut down.', N'Information', CAST(N'2025-02-03T09:51:14.647' AS DateTime), NULL, N'<properties><property key=''SourceContext''>Microsoft.Hosting.Lifetime</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (43, N'Hosting environment: "Development"', N'Hosting environment: {EnvName}', N'Information', CAST(N'2025-02-03T09:51:14.647' AS DateTime), NULL, N'<properties><property key=''EnvName''>Development</property><property key=''SourceContext''>Microsoft.Hosting.Lifetime</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (44, N'Content root path: "E:\Abfa\Aban360\Aban360.Api"', N'Content root path: {ContentRoot}', N'Information', CAST(N'2025-02-03T09:51:14.650' AS DateTime), NULL, N'<properties><property key=''ContentRoot''>E:\Abfa\Aban360\Aban360.Api</property><property key=''SourceContext''>Microsoft.Hosting.Lifetime</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (45, N'Hosting started', N'Hosting started', N'Debug', CAST(N'2025-02-03T09:51:14.650' AS DateTime), NULL, N'<properties><property key=''EventId''><structure type=''''><property key=''Id''>2</property><property key=''Name''>Started</property></structure></property><property key=''SourceContext''>Microsoft.Extensions.Hosting.Internal.Host</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (46, N'Server z_e:14528:7b476ccb successfully announced in 33.5651 ms', N'{State:l}', N'Information', CAST(N'2025-02-03T09:51:14.683' AS DateTime), NULL, N'<properties><property key=''State''>Server z_e:14528:7b476ccb successfully announced in 33.5651 ms</property><property key=''SourceContext''>Hangfire.Server.BackgroundServerProcess</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (47, N'Server z_e:14528:7b476ccb is starting the registered dispatchers: ServerWatchdog, ServerJobCancellationWatcher, ExpirationManager, CountersAggregator, SqlServerHeartbeatProcess, Worker, DelayedJobScheduler, RecurringJobScheduler...', N'{State:l}', N'Information', CAST(N'2025-02-03T09:51:14.690' AS DateTime), NULL, N'<properties><property key=''State''>Server z_e:14528:7b476ccb is starting the registered dispatchers: ServerWatchdog, ServerJobCancellationWatcher, ExpirationManager, CountersAggregator, SqlServerHeartbeatProcess, Worker, DelayedJobScheduler, RecurringJobScheduler...</property><property key=''SourceContext''>Hangfire.Server.BackgroundServerProcess</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (48, N'Execution loop ServerHeartbeatProcess:3ef6b0fc has started in 4.4369 ms', N'{State:l}', N'Debug', CAST(N'2025-02-03T09:51:14.690' AS DateTime), NULL, N'<properties><property key=''State''>Execution loop ServerHeartbeatProcess:3ef6b0fc has started in 4.4369 ms</property><property key=''SourceContext''>Hangfire.Processing.BackgroundExecution</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (49, N'Execution loop ServerWatchdog:03fe2efa has started in 4.038 ms', N'{State:l}', N'Debug', CAST(N'2025-02-03T09:51:14.697' AS DateTime), NULL, N'<properties><property key=''State''>Execution loop ServerWatchdog:03fe2efa has started in 4.038 ms</property><property key=''SourceContext''>Hangfire.Processing.BackgroundExecution</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (50, N'Execution loop ServerJobCancellationWatcher:b2047830 has started in 6.9221 ms', N'{State:l}', N'Debug', CAST(N'2025-02-03T09:51:14.700' AS DateTime), NULL, N'<properties><property key=''State''>Execution loop ServerJobCancellationWatcher:b2047830 has started in 6.9221 ms</property><property key=''SourceContext''>Hangfire.Processing.BackgroundExecution</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (51, N'Execution loop ExpirationManager:5c6febdf has started in 9.4213 ms', N'{State:l}', N'Debug', CAST(N'2025-02-03T09:51:14.710' AS DateTime), NULL, N'<properties><property key=''State''>Execution loop ExpirationManager:5c6febdf has started in 9.4213 ms</property><property key=''SourceContext''>Hangfire.Processing.BackgroundExecution</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (52, N'Execution loop CountersAggregator:23e90646 has started in 11.4013 ms', N'{State:l}', N'Debug', CAST(N'2025-02-03T09:51:14.713' AS DateTime), NULL, N'<properties><property key=''State''>Execution loop CountersAggregator:23e90646 has started in 11.4013 ms</property><property key=''SourceContext''>Hangfire.Processing.BackgroundExecution</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (53, N'Removing outdated records from the ''AggregatedCounter'' table...', N'{State:l}', N'Debug', CAST(N'2025-02-03T09:51:14.733' AS DateTime), NULL, N'<properties><property key=''State''>Removing outdated records from the ''AggregatedCounter'' table...</property><property key=''SourceContext''>Hangfire.SqlServer.ExpirationManager</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (54, N'Aggregating records in ''Counter'' table...', N'{State:l}', N'Debug', CAST(N'2025-02-03T09:51:14.757' AS DateTime), NULL, N'<properties><property key=''State''>Aggregating records in ''Counter'' table...</property><property key=''SourceContext''>Hangfire.SqlServer.CountersAggregator</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (55, N'Execution loop SqlServerHeartbeatProcess:0d0a350d has started in 11.8256 ms', N'{State:l}', N'Debug', CAST(N'2025-02-03T09:51:14.717' AS DateTime), NULL, N'<properties><property key=''State''>Execution loop SqlServerHeartbeatProcess:0d0a350d has started in 11.8256 ms</property><property key=''SourceContext''>Hangfire.Processing.BackgroundExecution</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (56, N'Execution loop Worker:66053948 has started in 12.1154 ms', N'{State:l}', N'Debug', CAST(N'2025-02-03T09:51:14.727' AS DateTime), NULL, N'<properties><property key=''State''>Execution loop Worker:66053948 has started in 12.1154 ms</property><property key=''SourceContext''>Hangfire.Processing.BackgroundExecution</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (57, N'Execution loop Worker:61b5ab8b has started in 15.8241 ms', N'{State:l}', N'Debug', CAST(N'2025-02-03T09:51:14.730' AS DateTime), NULL, N'<properties><property key=''State''>Execution loop Worker:61b5ab8b has started in 15.8241 ms</property><property key=''SourceContext''>Hangfire.Processing.BackgroundExecution</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (58, N'Execution loop Worker:f6321f1a has started in 20.228 ms', N'{State:l}', N'Debug', CAST(N'2025-02-03T09:51:14.733' AS DateTime), NULL, N'<properties><property key=''State''>Execution loop Worker:f6321f1a has started in 20.228 ms</property><property key=''SourceContext''>Hangfire.Processing.BackgroundExecution</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (59, N'Execution loop Worker:5f314dec has started in 28.985 ms', N'{State:l}', N'Debug', CAST(N'2025-02-03T09:51:14.743' AS DateTime), NULL, N'<properties><property key=''State''>Execution loop Worker:5f314dec has started in 28.985 ms</property><property key=''SourceContext''>Hangfire.Processing.BackgroundExecution</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (60, N'Server z_e:14528:7b476ccb all the dispatchers started', N'{State:l}', N'Information', CAST(N'2025-02-03T09:51:14.813' AS DateTime), NULL, N'<properties><property key=''State''>Server z_e:14528:7b476ccb all the dispatchers started</property><property key=''SourceContext''>Hangfire.Server.BackgroundServerProcess</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (61, N'Removing outdated records from the ''Job'' table...', N'{State:l}', N'Debug', CAST(N'2025-02-03T09:51:14.810' AS DateTime), NULL, N'<properties><property key=''State''>Removing outdated records from the ''Job'' table...</property><property key=''SourceContext''>Hangfire.SqlServer.ExpirationManager</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (62, N'Execution loop Worker:5803cd39 has started in 33.6844 ms', N'{State:l}', N'Debug', CAST(N'2025-02-03T09:51:14.747' AS DateTime), NULL, N'<properties><property key=''State''>Execution loop Worker:5803cd39 has started in 33.6844 ms</property><property key=''SourceContext''>Hangfire.Processing.BackgroundExecution</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (63, N'Execution loop Worker:118fe1cb has started in 36.4396 ms', N'{State:l}', N'Debug', CAST(N'2025-02-03T09:51:14.750' AS DateTime), NULL, N'<properties><property key=''State''>Execution loop Worker:118fe1cb has started in 36.4396 ms</property><property key=''SourceContext''>Hangfire.Processing.BackgroundExecution</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (64, N'Execution loop Worker:2665bf59 has started in 40.9484 ms', N'{State:l}', N'Debug', CAST(N'2025-02-03T09:51:14.753' AS DateTime), NULL, N'<properties><property key=''State''>Execution loop Worker:2665bf59 has started in 40.9484 ms</property><property key=''SourceContext''>Hangfire.Processing.BackgroundExecution</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (65, N'Execution loop Worker:347c0458 has started in 44.7524 ms', N'{State:l}', N'Debug', CAST(N'2025-02-03T09:51:14.757' AS DateTime), NULL, N'<properties><property key=''State''>Execution loop Worker:347c0458 has started in 44.7524 ms</property><property key=''SourceContext''>Hangfire.Processing.BackgroundExecution</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (66, N'Execution loop Worker:07124fac has started in 50.3532 ms', N'{State:l}', N'Debug', CAST(N'2025-02-03T09:51:14.763' AS DateTime), NULL, N'<properties><property key=''State''>Execution loop Worker:07124fac has started in 50.3532 ms</property><property key=''SourceContext''>Hangfire.Processing.BackgroundExecution</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (67, N'Execution loop Worker:d0a76868 has started in 54.7865 ms', N'{State:l}', N'Debug', CAST(N'2025-02-03T09:51:14.767' AS DateTime), NULL, N'<properties><property key=''State''>Execution loop Worker:d0a76868 has started in 54.7865 ms</property><property key=''SourceContext''>Hangfire.Processing.BackgroundExecution</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (68, N'Execution loop Worker:cd02c87c has started in 58.6184 ms', N'{State:l}', N'Debug', CAST(N'2025-02-03T09:51:14.773' AS DateTime), NULL, N'<properties><property key=''State''>Execution loop Worker:cd02c87c has started in 58.6184 ms</property><property key=''SourceContext''>Hangfire.Processing.BackgroundExecution</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (69, N'Execution loop Worker:8605263f has started in 64.0501 ms', N'{State:l}', N'Debug', CAST(N'2025-02-03T09:51:14.777' AS DateTime), NULL, N'<properties><property key=''State''>Execution loop Worker:8605263f has started in 64.0501 ms</property><property key=''SourceContext''>Hangfire.Processing.BackgroundExecution</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (70, N'Execution loop Worker:e073286f has started in 68.2926 ms', N'{State:l}', N'Debug', CAST(N'2025-02-03T09:51:14.783' AS DateTime), NULL, N'<properties><property key=''State''>Execution loop Worker:e073286f has started in 68.2926 ms</property><property key=''SourceContext''>Hangfire.Processing.BackgroundExecution</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (71, N'Execution loop Worker:bab110c4 has started in 71.2402 ms', N'{State:l}', N'Debug', CAST(N'2025-02-03T09:51:14.783' AS DateTime), NULL, N'<properties><property key=''State''>Execution loop Worker:bab110c4 has started in 71.2402 ms</property><property key=''SourceContext''>Hangfire.Processing.BackgroundExecution</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (72, N'Execution loop Worker:70e6f234 has started in 74.9424 ms', N'{State:l}', N'Debug', CAST(N'2025-02-03T09:51:14.787' AS DateTime), NULL, N'<properties><property key=''State''>Execution loop Worker:70e6f234 has started in 74.9424 ms</property><property key=''SourceContext''>Hangfire.Processing.BackgroundExecution</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (73, N'Execution loop Worker:2f11b691 has started in 79.6937 ms', N'{State:l}', N'Debug', CAST(N'2025-02-03T09:51:14.793' AS DateTime), NULL, N'<properties><property key=''State''>Execution loop Worker:2f11b691 has started in 79.6937 ms</property><property key=''SourceContext''>Hangfire.Processing.BackgroundExecution</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (74, N'Execution loop Worker:02ca9501 has started in 83.467 ms', N'{State:l}', N'Debug', CAST(N'2025-02-03T09:51:14.797' AS DateTime), NULL, N'<properties><property key=''State''>Execution loop Worker:02ca9501 has started in 83.467 ms</property><property key=''SourceContext''>Hangfire.Processing.BackgroundExecution</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (75, N'Execution loop Worker:df55611f has started in 89.3043 ms', N'{State:l}', N'Debug', CAST(N'2025-02-03T09:51:14.803' AS DateTime), NULL, N'<properties><property key=''State''>Execution loop Worker:df55611f has started in 89.3043 ms</property><property key=''SourceContext''>Hangfire.Processing.BackgroundExecution</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (76, N'Execution loop Worker:31d38ac8 has started in 89.2278 ms', N'{State:l}', N'Debug', CAST(N'2025-02-03T09:51:14.803' AS DateTime), NULL, N'<properties><property key=''State''>Execution loop Worker:31d38ac8 has started in 89.2278 ms</property><property key=''SourceContext''>Hangfire.Processing.BackgroundExecution</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (77, N'Execution loop Worker:f2ab1846 has started in 94.4409 ms', N'{State:l}', N'Debug', CAST(N'2025-02-03T09:51:14.807' AS DateTime), NULL, N'<properties><property key=''State''>Execution loop Worker:f2ab1846 has started in 94.4409 ms</property><property key=''SourceContext''>Hangfire.Processing.BackgroundExecution</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (78, N'Execution loop DelayedJobScheduler:9ed977ca has started in 9.1772 ms', N'{State:l}', N'Debug', CAST(N'2025-02-03T09:51:14.810' AS DateTime), NULL, N'<properties><property key=''State''>Execution loop DelayedJobScheduler:9ed977ca has started in 9.1772 ms</property><property key=''SourceContext''>Hangfire.Processing.BackgroundExecution</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (79, N'Execution loop RecurringJobScheduler:f3806315 has started in 8.8693 ms', N'{State:l}', N'Debug', CAST(N'2025-02-03T09:51:14.817' AS DateTime), NULL, N'<properties><property key=''State''>Execution loop RecurringJobScheduler:f3806315 has started in 8.8693 ms</property><property key=''SourceContext''>Hangfire.Processing.BackgroundExecution</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (80, N'Removing outdated records from the ''List'' table...', N'{State:l}', N'Debug', CAST(N'2025-02-03T09:51:14.887' AS DateTime), NULL, N'<properties><property key=''State''>Removing outdated records from the ''List'' table...</property><property key=''SourceContext''>Hangfire.SqlServer.ExpirationManager</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (81, N'Removing outdated records from the ''Set'' table...', N'{State:l}', N'Debug', CAST(N'2025-02-03T09:51:14.903' AS DateTime), NULL, N'<properties><property key=''State''>Removing outdated records from the ''Set'' table...</property><property key=''SourceContext''>Hangfire.SqlServer.ExpirationManager</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (82, N'Removing outdated records from the ''Hash'' table...', N'{State:l}', N'Debug', CAST(N'2025-02-03T09:51:14.907' AS DateTime), NULL, N'<properties><property key=''State''>Removing outdated records from the ''Hash'' table...</property><property key=''SourceContext''>Hangfire.SqlServer.ExpirationManager</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (83, N'Connection id ""0HNA43BR1NAJ6"" accepted.', N'Connection id "{ConnectionId}" accepted.', N'Debug', CAST(N'2025-02-03T09:51:15.053' AS DateTime), NULL, N'<properties><property key=''ConnectionId''>0HNA43BR1NAJ6</property><property key=''EventId''><structure type=''''><property key=''Id''>39</property><property key=''Name''>ConnectionAccepted</property></structure></property><property key=''SourceContext''>Microsoft.AspNetCore.Server.Kestrel.Connections</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (84, N'Connection id ""0HNA43BR1NAJ6"" started.', N'Connection id "{ConnectionId}" started.', N'Debug', CAST(N'2025-02-03T09:51:15.057' AS DateTime), NULL, N'<properties><property key=''ConnectionId''>0HNA43BR1NAJ6</property><property key=''EventId''><structure type=''''><property key=''Id''>1</property><property key=''Name''>ConnectionStart</property></structure></property><property key=''SourceContext''>Microsoft.AspNetCore.Server.Kestrel.Connections</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (85, N'Connection "0HNA43BR1NAJ6" established using the following protocol: Tls13', N'Connection {ConnectionId} established using the following protocol: {Protocol}', N'Debug', CAST(N'2025-02-03T09:51:15.113' AS DateTime), NULL, N'<properties><property key=''ConnectionId''>0HNA43BR1NAJ6</property><property key=''Protocol''>Tls13</property><property key=''EventId''><structure type=''''><property key=''Id''>3</property><property key=''Name''>HttpsConnectionEstablished</property></structure></property><property key=''SourceContext''>Microsoft.AspNetCore.Server.Kestrel.Https.Internal.HttpsConnectionMiddleware</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (86, N'Request starting "HTTP/2" "GET" "https"://"localhost:7239""""/swagger""" - null null', N'Request starting {Protocol} {Method} {Scheme}://{Host}{PathBase}{Path}{QueryString} - {ContentType} {ContentLength}', N'Information', CAST(N'2025-02-03T09:51:15.173' AS DateTime), NULL, N'<properties><property key=''Protocol''>HTTP/2</property><property key=''Method''>GET</property><property key=''ContentType''></property><property key=''ContentLength''></property><property key=''Scheme''>https</property><property key=''Host''>localhost:7239</property><property key=''PathBase''></property><property key=''Path''>/swagger</property><property key=''QueryString''></property><property key=''EventId''><structure type=''''><property key=''Id''>1</property></structure></property><property key=''SourceContext''>Microsoft.AspNetCore.Hosting.Diagnostics</property><property key=''RequestId''>0HNA43BR1NAJ6:00000003</property><property key=''RequestPath''>/swagger</property><property key=''ConnectionId''>0HNA43BR1NAJ6</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (87, N'Request starting "HTTP/2" "GET" "https"://"localhost:7239""""/swagger/index.html""" - null null', N'Request starting {Protocol} {Method} {Scheme}://{Host}{PathBase}{Path}{QueryString} - {ContentType} {ContentLength}', N'Information', CAST(N'2025-02-03T09:51:15.230' AS DateTime), NULL, N'<properties><property key=''Protocol''>HTTP/2</property><property key=''Method''>GET</property><property key=''ContentType''></property><property key=''ContentLength''></property><property key=''Scheme''>https</property><property key=''Host''>localhost:7239</property><property key=''PathBase''></property><property key=''Path''>/swagger/index.html</property><property key=''QueryString''></property><property key=''EventId''><structure type=''''><property key=''Id''>1</property></structure></property><property key=''SourceContext''>Microsoft.AspNetCore.Hosting.Diagnostics</property><property key=''RequestId''>0HNA43BR1NAJ6:00000005</property><property key=''RequestPath''>/swagger/index.html</property><property key=''ConnectionId''>0HNA43BR1NAJ6</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (88, N'Wildcard detected, all requests with hosts will be allowed.', N'Wildcard detected, all requests with hosts will be allowed.', N'Debug', CAST(N'2025-02-03T09:51:15.330' AS DateTime), NULL, N'<properties><property key=''EventId''><structure type=''''><property key=''Name''>WildcardDetected</property></structure></property><property key=''SourceContext''>Microsoft.AspNetCore.HostFiltering.HostFilteringMiddleware</property><property key=''RequestId''>0HNA43BR1NAJ6:00000005</property><property key=''RequestPath''>/swagger/index.html</property><property key=''ConnectionId''>0HNA43BR1NAJ6</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (89, N'Wildcard detected, all requests with hosts will be allowed.', N'Wildcard detected, all requests with hosts will be allowed.', N'Debug', CAST(N'2025-02-03T09:51:15.330' AS DateTime), NULL, N'<properties><property key=''EventId''><structure type=''''><property key=''Name''>WildcardDetected</property></structure></property><property key=''SourceContext''>Microsoft.AspNetCore.HostFiltering.HostFilteringMiddleware</property><property key=''RequestId''>0HNA43BR1NAJ6:00000003</property><property key=''RequestPath''>/swagger</property><property key=''ConnectionId''>0HNA43BR1NAJ6</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (90, N'Request finished "HTTP/2" "GET" "https"://"localhost:7239""""/swagger""" - 499 null null 169.1917ms', N'Request finished {Protocol} {Method} {Scheme}://{Host}{PathBase}{Path}{QueryString} - {StatusCode} {ContentLength} {ContentType} {ElapsedMilliseconds}ms', N'Information', CAST(N'2025-02-03T09:51:15.337' AS DateTime), NULL, N'<properties><property key=''ElapsedMilliseconds''>169.1917</property><property key=''StatusCode''>499</property><property key=''ContentType''></property><property key=''ContentLength''></property><property key=''Protocol''>HTTP/2</property><property key=''Method''>GET</property><property key=''Scheme''>https</property><property key=''Host''>localhost:7239</property><property key=''PathBase''></property><property key=''Path''>/swagger</property><property key=''QueryString''></property><property key=''EventId''><structure type=''''><property key=''Id''>2</property></structure></property><property key=''SourceContext''>Microsoft.AspNetCore.Hosting.Diagnostics</property><property key=''RequestId''>0HNA43BR1NAJ6:00000003</property><property key=''RequestPath''>/swagger</property><property key=''ConnectionId''>0HNA43BR1NAJ6</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (91, N'Response markup is scheduled to include Browser Link script injection.', N'Response markup is scheduled to include Browser Link script injection.', N'Debug', CAST(N'2025-02-03T09:51:15.373' AS DateTime), NULL, N'<properties><property key=''EventId''><structure type=''''><property key=''Id''>1</property><property key=''Name''>SetUpResponseForBrowserLink</property></structure></property><property key=''SourceContext''>Microsoft.WebTools.BrowserLink.Net.BrowserLinkMiddleware</property><property key=''RequestId''>0HNA43BR1NAJ6:00000005</property><property key=''RequestPath''>/swagger/index.html</property><property key=''ConnectionId''>0HNA43BR1NAJ6</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (92, N'Response markup is scheduled to include browser refresh script injection.', N'Response markup is scheduled to include browser refresh script injection.', N'Debug', CAST(N'2025-02-03T09:51:15.373' AS DateTime), NULL, N'<properties><property key=''EventId''><structure type=''''><property key=''Id''>1</property><property key=''Name''>SetUpResponseForBrowserRefresh</property></structure></property><property key=''SourceContext''>Microsoft.AspNetCore.Watch.BrowserRefresh.BrowserRefreshMiddleware</property><property key=''RequestId''>0HNA43BR1NAJ6:00000005</property><property key=''RequestPath''>/swagger/index.html</property><property key=''ConnectionId''>0HNA43BR1NAJ6</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (93, N'Response markup was updated to include Browser Link script injection.', N'Response markup was updated to include Browser Link script injection.', N'Debug', CAST(N'2025-02-03T09:51:15.380' AS DateTime), NULL, N'<properties><property key=''EventId''><structure type=''''><property key=''Id''>2</property><property key=''Name''>BrowserConfiguredForBrowserLink</property></structure></property><property key=''SourceContext''>Microsoft.WebTools.BrowserLink.Net.BrowserLinkMiddleware</property><property key=''RequestId''>0HNA43BR1NAJ6:00000005</property><property key=''RequestPath''>/swagger/index.html</property><property key=''ConnectionId''>0HNA43BR1NAJ6</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (94, N'Response markup was updated to include browser refresh script injection.', N'Response markup was updated to include browser refresh script injection.', N'Debug', CAST(N'2025-02-03T09:51:15.383' AS DateTime), NULL, N'<properties><property key=''EventId''><structure type=''''><property key=''Id''>2</property><property key=''Name''>BrowserConfiguredForRefreshes</property></structure></property><property key=''SourceContext''>Microsoft.AspNetCore.Watch.BrowserRefresh.BrowserRefreshMiddleware</property><property key=''RequestId''>0HNA43BR1NAJ6:00000005</property><property key=''RequestPath''>/swagger/index.html</property><property key=''ConnectionId''>0HNA43BR1NAJ6</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (95, N'Request finished "HTTP/2" "GET" "https"://"localhost:7239""""/swagger/index.html""" - 200 null "text/html;charset=utf-8" 154.7664ms', N'Request finished {Protocol} {Method} {Scheme}://{Host}{PathBase}{Path}{QueryString} - {StatusCode} {ContentLength} {ContentType} {ElapsedMilliseconds}ms', N'Information', CAST(N'2025-02-03T09:51:15.383' AS DateTime), NULL, N'<properties><property key=''ElapsedMilliseconds''>154.7664</property><property key=''StatusCode''>200</property><property key=''ContentType''>text/html;charset=utf-8</property><property key=''ContentLength''></property><property key=''Protocol''>HTTP/2</property><property key=''Method''>GET</property><property key=''Scheme''>https</property><property key=''Host''>localhost:7239</property><property key=''PathBase''></property><property key=''Path''>/swagger/index.html</property><property key=''QueryString''></property><property key=''EventId''><structure type=''''><property key=''Id''>2</property></structure></property><property key=''SourceContext''>Microsoft.AspNetCore.Hosting.Diagnostics</property><property key=''RequestId''>0HNA43BR1NAJ6:00000005</property><property key=''RequestPath''>/swagger/index.html</property><property key=''ConnectionId''>0HNA43BR1NAJ6</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (96, N'Request starting "HTTP/2" "GET" "https"://"localhost:7239""""/swagger/swagger-ui.css""" - null null', N'Request starting {Protocol} {Method} {Scheme}://{Host}{PathBase}{Path}{QueryString} - {ContentType} {ContentLength}', N'Information', CAST(N'2025-02-03T09:51:15.443' AS DateTime), NULL, N'<properties><property key=''Protocol''>HTTP/2</property><property key=''Method''>GET</property><property key=''ContentType''></property><property key=''ContentLength''></property><property key=''Scheme''>https</property><property key=''Host''>localhost:7239</property><property key=''PathBase''></property><property key=''Path''>/swagger/swagger-ui.css</property><property key=''QueryString''></property><property key=''EventId''><structure type=''''><property key=''Id''>1</property></structure></property><property key=''SourceContext''>Microsoft.AspNetCore.Hosting.Diagnostics</property><property key=''RequestId''>0HNA43BR1NAJ6:00000007</property><property key=''RequestPath''>/swagger/swagger-ui.css</property><property key=''ConnectionId''>0HNA43BR1NAJ6</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (97, N'Request starting "HTTP/2" "GET" "https"://"localhost:7239""""/swagger/index.js""" - null null', N'Request starting {Protocol} {Method} {Scheme}://{Host}{PathBase}{Path}{QueryString} - {ContentType} {ContentLength}', N'Information', CAST(N'2025-02-03T09:51:15.443' AS DateTime), NULL, N'<properties><property key=''Protocol''>HTTP/2</property><property key=''Method''>GET</property><property key=''ContentType''></property><property key=''ContentLength''></property><property key=''Scheme''>https</property><property key=''Host''>localhost:7239</property><property key=''PathBase''></property><property key=''Path''>/swagger/index.js</property><property key=''QueryString''></property><property key=''EventId''><structure type=''''><property key=''Id''>1</property></structure></property><property key=''SourceContext''>Microsoft.AspNetCore.Hosting.Diagnostics</property><property key=''RequestId''>0HNA43BR1NAJ6:00000009</property><property key=''RequestPath''>/swagger/index.js</property><property key=''ConnectionId''>0HNA43BR1NAJ6</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (98, N'Request starting "HTTP/2" "GET" "https"://"localhost:7239""""/_vs/browserLink""" - null null', N'Request starting {Protocol} {Method} {Scheme}://{Host}{PathBase}{Path}{QueryString} - {ContentType} {ContentLength}', N'Information', CAST(N'2025-02-03T09:51:15.447' AS DateTime), NULL, N'<properties><property key=''Protocol''>HTTP/2</property><property key=''Method''>GET</property><property key=''ContentType''></property><property key=''ContentLength''></property><property key=''Scheme''>https</property><property key=''Host''>localhost:7239</property><property key=''PathBase''></property><property key=''Path''>/_vs/browserLink</property><property key=''QueryString''></property><property key=''EventId''><structure type=''''><property key=''Id''>1</property></structure></property><property key=''SourceContext''>Microsoft.AspNetCore.Hosting.Diagnostics</property><property key=''RequestId''>0HNA43BR1NAJ6:0000000D</property><property key=''RequestPath''>/_vs/browserLink</property><property key=''ConnectionId''>0HNA43BR1NAJ6</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (99, N'Request starting "HTTP/2" "GET" "https"://"localhost:7239""""/_framework/aspnetcore-browser-refresh.js""" - null null', N'Request starting {Protocol} {Method} {Scheme}://{Host}{PathBase}{Path}{QueryString} - {ContentType} {ContentLength}', N'Information', CAST(N'2025-02-03T09:51:15.443' AS DateTime), NULL, N'<properties><property key=''Protocol''>HTTP/2</property><property key=''Method''>GET</property><property key=''ContentType''></property><property key=''ContentLength''></property><property key=''Scheme''>https</property><property key=''Host''>localhost:7239</property><property key=''PathBase''></property><property key=''Path''>/_framework/aspnetcore-browser-refresh.js</property><property key=''QueryString''></property><property key=''EventId''><structure type=''''><property key=''Id''>1</property></structure></property><property key=''SourceContext''>Microsoft.AspNetCore.Hosting.Diagnostics</property><property key=''RequestId''>0HNA43BR1NAJ6:0000000B</property><property key=''RequestPath''>/_framework/aspnetcore-browser-refresh.js</property><property key=''ConnectionId''>0HNA43BR1NAJ6</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (100, N'Request finished "HTTP/2" "GET" "https"://"localhost:7239""""/swagger/index.js""" - 200 null "application/javascript;charset=utf-8" 7.4614ms', N'Request finished {Protocol} {Method} {Scheme}://{Host}{PathBase}{Path}{QueryString} - {StatusCode} {ContentLength} {ContentType} {ElapsedMilliseconds}ms', N'Information', CAST(N'2025-02-03T09:51:15.450' AS DateTime), NULL, N'<properties><property key=''ElapsedMilliseconds''>7.4614</property><property key=''StatusCode''>200</property><property key=''ContentType''>application/javascript;charset=utf-8</property><property key=''ContentLength''></property><property key=''Protocol''>HTTP/2</property><property key=''Method''>GET</property><property key=''Scheme''>https</property><property key=''Host''>localhost:7239</property><property key=''PathBase''></property><property key=''Path''>/swagger/index.js</property><property key=''QueryString''></property><property key=''EventId''><structure type=''''><property key=''Id''>2</property></structure></property><property key=''SourceContext''>Microsoft.AspNetCore.Hosting.Diagnostics</property><property key=''RequestId''>0HNA43BR1NAJ6:00000009</property><property key=''RequestPath''>/swagger/index.js</property><property key=''ConnectionId''>0HNA43BR1NAJ6</property></properties>')
GO
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (101, N'The file transmission was cancelled', N'The file transmission was cancelled', N'Debug', CAST(N'2025-02-03T09:51:15.460' AS DateTime), N'System.OperationCanceledException: The operation was canceled.
   at System.Threading.CancellationToken.ThrowOperationCanceledException()
   at System.Threading.CancellationToken.ThrowIfCancellationRequested()
   at Microsoft.AspNetCore.Http.SendFileResponseExtensions.SendFileAsyncCore(HttpResponse response, IFileInfo file, Int64 offset, Nullable`1 count, CancellationToken cancellationToken)
   at Microsoft.AspNetCore.Http.SendFileResponseExtensions.SendFileAsyncCore(HttpResponse response, IFileInfo file, Int64 offset, Nullable`1 count, CancellationToken cancellationToken)
   at Microsoft.AspNetCore.StaticFiles.StaticFileContext.SendAsync()', N'<properties><property key=''EventId''><structure type=''''><property key=''Id''>14</property><property key=''Name''>WriteCancelled</property></structure></property><property key=''SourceContext''>Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware</property><property key=''RequestId''>0HNA43BR1NAJ6:00000007</property><property key=''RequestPath''>/swagger/swagger-ui.css</property><property key=''ConnectionId''>0HNA43BR1NAJ6</property></properties>')
GO
SET IDENTITY_INSERT [dbo].[Logs] OFF
GO
INSERT [dbo].[VersionInfo] ([Version], [AppliedOn], [Description]) VALUES (14031110, CAST(N'2025-02-03T06:21:12.000' AS DateTime), N'DbInitialDesign')
GO
INSERT [dbo].[VersionInfo] ([Version], [AppliedOn], [Description]) VALUES (1403082101, CAST(N'2025-02-03T06:21:12.000' AS DateTime), N'DbInitialDesign')
GO
INSERT [HangFire].[Schema] ([Version]) VALUES (9)
GO
INSERT [HangFire].[Server] ([Id], [Data], [LastHeartbeat]) VALUES (N'z_e:14528:7b476ccb-8224-4f9b-90de-c2bc8e4d742b', N'{"WorkerCount":20,"Queues":["default"],"StartedAt":"2025-02-03T06:21:14.6495586Z"}', CAST(N'2025-02-03T06:21:14.663' AS DateTime))
GO
*/