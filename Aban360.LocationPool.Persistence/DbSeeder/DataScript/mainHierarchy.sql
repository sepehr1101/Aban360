USE [Aban360]
GO
INSERT [LocationPool].[Country] ( [Title]) VALUES ( N'ایران')
GO
INSERT [LocationPool].[CordinalDirection] ( [Title]) VALUES ( N'مرکز')
GO
INSERT [LocationPool].[CordinalDirection] ( [Title]) VALUES ( N'شمال')
GO
INSERT [LocationPool].[CordinalDirection] ( [Title]) VALUES ( N'جنوب')
GO
INSERT [LocationPool].[CordinalDirection] ( [Title]) VALUES ( N'شرق')
GO
INSERT [LocationPool].[CordinalDirection] ( [Title]) VALUES ( N'غرب')
GO
INSERT [LocationPool].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (1, N'آذربایجان شرقی', 1, 1)
GO
INSERT [LocationPool].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (2, N'آذربایجان غربی', 1, 1)
GO
INSERT [LocationPool].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (3, N'اردبیل', 1, 1)
GO
INSERT [LocationPool].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (4, N'خوزستان', 1, 1)
GO
INSERT [LocationPool].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (5, N'البرز', 1, 1)
GO
INSERT [LocationPool].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (6, N'ایلام', 1, 1)
GO
INSERT [LocationPool].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (7, N'بوشهر', 1, 1)
GO
INSERT [LocationPool].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (8, N'تهران', 1, 1)
GO
INSERT [LocationPool].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (9, N'چهارمحال و بختیاری', 1, 1)
GO
INSERT [LocationPool].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (10, N'خراسان جنوبی', 1, 1)
GO
INSERT [LocationPool].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (11, N'خراسان رضوی', 1, 1)
GO
INSERT [LocationPool].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (12, N'خراسان شمالی', 1, 1)
GO
INSERT [LocationPool].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (13, N'اصفهان', 1, 1)
GO
INSERT [LocationPool].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (14, N'زنجان', 1, 1)
GO
INSERT [LocationPool].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (15, N'سمنان', 1, 1)
GO
INSERT [LocationPool].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (16, N'سیستان و بلوچستان', 1, 1)
GO
INSERT [LocationPool].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (17, N'فارس', 1, 1)
GO
INSERT [LocationPool].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (18, N'قزوین', 1, 1)
GO
INSERT [LocationPool].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (19, N'قم', 1, 1)
GO
INSERT [LocationPool].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (20, N'کردستان', 1, 1)
GO
INSERT [LocationPool].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (21, N'کرمان', 1, 1)
GO
INSERT [LocationPool].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (22, N'کرمانشاه', 1, 1)
GO
INSERT [LocationPool].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (23, N'کهگیلویه و بویراحمد', 1, 1)
GO
INSERT [LocationPool].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (24, N'گلستان', 1, 1)
GO
INSERT [LocationPool].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (25, N'لرستان', 1, 1)
GO
INSERT [LocationPool].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (26, N'گیلان', 1, 1)
GO
INSERT [LocationPool].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (27, N'مازندران', 1, 1)
GO
INSERT [LocationPool].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (28, N'مرکزی', 1, 1)
GO
INSERT [LocationPool].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (29, N'هرمزگان', 1, 1)
GO
INSERT [LocationPool].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (30, N'همدان', 1, 1)
GO
INSERT [LocationPool].[Province] ([Id], [Title], [CordinalDirectionId], [CountryId]) VALUES (31, N'یزد', 1, 1)
GO
INSERT [LocationPool].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (1, N'آبفا استان اصفهان', 13)
GO
INSERT [LocationPool].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (2, N'آبفا کاشان', 13)
GO
INSERT [LocationPool].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (3, N'آذربایجان شرقی', 1)
GO
INSERT [LocationPool].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (4, N'آذربایجان غربی', 2)
GO
INSERT [LocationPool].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (5, N'اردبیل', 3)
GO
INSERT [LocationPool].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (6, N'البرز', 5)
GO
INSERT [LocationPool].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (7, N'ایلام', 6)
GO
INSERT [LocationPool].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (8, N'بوشهر', 7)
GO
INSERT [LocationPool].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (9, N'تهران', 8)
GO
INSERT [LocationPool].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (10, N'چهارمحال و بختیاری', 9)
GO
INSERT [LocationPool].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (11, N'خراسان جنوبی', 10)
GO
INSERT [LocationPool].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (12, N'خراسان رضوی', 11)
GO
INSERT [LocationPool].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (13, N'خراسان شمالی', 12)
GO
INSERT [LocationPool].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (14, N'خوزستان', 4)
GO
INSERT [LocationPool].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (15, N'زنجان', 14)
GO
INSERT [LocationPool].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (16, N'سمنان', 15)
GO
INSERT [LocationPool].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (17, N'سیستان و بلوچستان', 16)
GO
INSERT [LocationPool].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (18, N'فارس', 17)
GO
INSERT [LocationPool].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (19, N'قزوین', 18)
GO
INSERT [LocationPool].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (20, N'قم', 19)
GO
INSERT [LocationPool].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (21, N'کردستان', 20)
GO
INSERT [LocationPool].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (22, N'کرمان', 21)
GO
INSERT [LocationPool].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (23, N'کرمانشاه', 22)
GO
INSERT [LocationPool].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (24, N'کهگیلویه و بویراحمد', 23)
GO
INSERT [LocationPool].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (25, N'گلستان', 24)
GO
INSERT [LocationPool].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (26, N'لرستان', 25)
GO
INSERT [LocationPool].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (27, N'گیلان', 26)
GO
INSERT [LocationPool].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (28, N'مازندران', 27)
GO
INSERT [LocationPool].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (29, N'مرکزی', 28)
GO
INSERT [LocationPool].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (30, N'هرمزگان', 27)
GO
INSERT [LocationPool].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (31, N'همدان', 28)
GO
INSERT [LocationPool].[Headquarters] ([Id], [Title], [ProvinceId]) VALUES (32, N'یزد', 29)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1, N'آذربایجان شرقی', 3)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (2, N'آذربایجان غربی', 4)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (3, N'اردبیل', 5)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (4, N'البرز', 6)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (5, N'ایلام', 7)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (6, N'بوشهر', 8)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (7, N'تهران', 9)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (8, N'چهارمحال و بختیاری', 10)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (9, N'خراسان جنوبی', 11)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (10, N'خراسان رضوی', 12)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (11, N'خراسان شمالی', 13)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (12, N'خوزستان', 14)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (13, N'زنجان', 15)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (14, N'سمنان', 16)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (15, N'سیستان و بلوچستان', 17)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (16, N'فارس', 18)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (17, N'قزوین', 19)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (18, N'قم', 20)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (19, N'کردستان', 21)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (20, N'کرمان', 22)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (21, N'کرمانشاه', 23)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (22, N'کهگیلویه و بویراحمد', 24)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (23, N'گلستان', 25)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (24, N'لرستان', 26)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (25, N'گیلان', 27)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (26, N'مازندران', 28)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (27, N'مرکزی', 29)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (28, N'هرمزگان', 30)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (29, N'همدان', 31)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (30, N'یزد', 32)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1312, N'اردستان', 1)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1314, N'شاهين شهر', 1)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1315, N'جرقويه', 1)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1316, N'خوانسار', 1)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1317, N'خميني شهر', 1)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1318, N'خوراسگان', 1)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1319, N'سميرم', 1)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1320, N'شهرضا', 1)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1321, N'فريدونشهر', 1)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1322, N'فلاورجان', 1)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1323, N'فريدن', 1)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1324, N'كوهپايه', 1)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1325, N'گلپايگان', 1)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1326, N'لنجان', 1)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1327, N'مباركه', 1)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1328, N'نائين', 1)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1329, N'نجف آباد', 1)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1330, N'نطنز', 1)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1331, N'ورزنه', 1)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1332, N'جلگه', 1)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1333, N'تيران', 1)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1334, N'فولادشهر', 1)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1336, N'بادرود', 1)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1337, N'چادگان', 1)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1338, N'دهاقان', 1)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1339, N'خور و بيابانك', 1)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1340, N'دولت آباد', 1)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1341, N'بوئين مياندشت', 1)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1342, N'میمه', 1)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1343, N'قهجاورستان', 1)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1344, N'براآن و کراچ', 1)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1345, N'مهردشت', 1)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1390, N'بهارستان', 1)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (1515, N'مجلسی', 1)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (131301, N'1- منطقه یک', 1)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (131302, N'2- منطقه دو', 1)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (131303, N'3- منطقه سه', 1)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (131304, N'4- منطقه چهار', 1)
GO
INSERT [LocationPool].[Region] ([Id], [Title], [HeadquartersId]) VALUES (131305, N'5- منطقه پنج', 1)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (131211, N'اردستان', 1312, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (131212, N'زواره', 1312, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (131213, N'مهاباد', 1312, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (131301, N'1  - منطقه یک', 131301, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (131302, N'2 - منطقه  دو', 131302, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (131303, N'3 - منطقه  سه', 131303, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (131304, N'4 -  منطقه  چهار', 131304, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (131305, N'5 -  منطقه  پنج', 131305, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (131411, N'شاهین شهر', 1314, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (131412, N'گز', 1314, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (131420, N'گرگاب', 1314, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (131511, N'نیک آباد', 1315, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (131512, N'محمد آباد', 1315, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (131513, N'حسن آباد', 1315, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (131514, N'نصر آباد', 1315, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (131611, N'خوانسار', 1316, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (131701, N'خمینی شهر', 1317, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (131702, N'درچه', 1317, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (131703, N'کوشک', 1317, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (131704, N'اصغر آباد', 1317, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (131811, N'خوراسگان', 1318, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (131911, N'سمیرم', 1319, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (131912, N'حنا', 1319, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (131913, N'ونک', 1319, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (131914, N'کمه', 1319, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (132011, N'شهرضا', 1320, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (132012, N'منظریه', 1320, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (132111, N'فریدون شهر', 1321, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (132112, N'برف انبار', 1321, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (132211, N'فلاورجان', 1322, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (132212, N'کلیشاد وسودرجان', 1322, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (132213, N'قهدریجان', 1322, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (132214, N'ابریشم', 1322, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (132215, N'پیربکران', 1322, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (132216, N'مینادشت', 1322, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (132217, N'بهاران', 1322, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (132218, N'اشترجان', 1322, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (132219, N'زازران', 1322, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (132220, N'روستاهای فلاورجان', 1322, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (132311, N'داران', 1323, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (132315, N'دامنه', 1323, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (132411, N'کوهپایه', 1324, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (132412, N'سجزی', 1324, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (132413, N'تودشک', 1324, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (132511, N'گلپایگان', 1325, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (132512, N'گوگد', 1325, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (132513, N'گلشهر', 1325, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (132611, N'زرین شهر', 1326, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (132612, N'ورنامخواست', 1326, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (132613, N'سده لنجان', 1326, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (132614, N'چرمهین', 1326, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (132615, N'باغبهادران', 1326, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (132616, N'چمگردان', 1326, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (132617, N'زاینده رود', 1326, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (132618, N'باغشاد', 1326, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (132711, N'مبارکه', 1327, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (132712, N'دیزیچه', 1327, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (132713, N'طالخونچه', 1327, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (132714, N'زیبا شهر', 1327, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (132715, N'کرکوند', 1327, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (132811, N'نائین', 1328, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (132812, N'انارک', 1328, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (132813, N'بافران', 1328, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (132911, N'نجف آباد', 1329, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (132914, N'گلدشت', 1329, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (132915, N'کهریزسنگ', 1329, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (132916, N'جوزدان', 1329, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (133011, N'نطنز', 1330, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (133012, N'طرق', 1330, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (133111, N'ورزنه', 1331, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (133211, N'هرند', 1332, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (133212, N'اژیه', 1332, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (133311, N'تیران', 1333, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (133312, N'رضوان شهر', 1333, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (133313, N'عسگران', 1333, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (133411, N'فولادشهر', 1334, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (133611, N'بادرود', 1336, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (133711, N'چادگان', 1337, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (133712, N'رزوه', 1337, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (133811, N'دهاقان', 1338, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (133812, N'گلشن', 1338, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (133911, N'خور', 1339, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (133912, N'جندق', 1339, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (133913, N'فرخی', 1339, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (134013, N'دولت آباد', 1340, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (134014, N'خورزوق', 1340, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (134015, N'دستگرد', 1340, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (134016, N'حبیب آباد', 1340, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (134019, N'کمشچه', 1340, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (134111, N'بوئین میاندشت', 1341, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (134112, N'افوس', 1341, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (134211, N'میمه', 1342, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (134212, N'وزوان', 1342, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (134213, N'لای بید', 1342, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (134311, N'قهجاورستان', 1343, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (134411, N'زیار', 1344, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (134511, N'دهق', 1345, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (134512, N'علویجه', 1345, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (139011, N'بهارستان', 1390, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (141211, N'اردستان-ر', 1312, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (141212, N'زواره-ر', 1312, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (141213, N'مهاباد-ر', 1312, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (141304, N'4 -  منطقه  چهار-ر', 131304, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (141411, N'شاهین شهر-ر', 1314, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (141412, N'گز-ر', 1314, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (141420, N'گرگاب-ر', 1314, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (141511, N'نیک آباد-ر', 1315, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (141512, N'محمد آباد-ر', 1315, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (141513, N'حسن آباد-ر', 1315, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (141514, N'نصر آباد-ر', 1315, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (141611, N'خوانسار-ر', 1316, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (141701, N'خمینی شهر-ر', 1317, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (141702, N'درچه-ر', 1317, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (141703, N'کوشک-ر', 1317, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (141704, N'اصغر آباد-ر', 1317, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (141811, N'خوراسگان-ر', 1318, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (141911, N'سمیرم-ر', 1319, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (141912, N'حنا-ر', 1319, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (141913, N'ونک-ر', 1319, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (141914, N'کمه-ر', 1319, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (142011, N'شهرضا-ر', 1320, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (142012, N'منظریه-ر', 1320, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (142111, N'فریدون شهر-ر', 1321, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (142112, N'برف انبار-ر', 1321, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (142211, N'فلاورجان-ر', 1322, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (142212, N'کلیشاد وسودرجان-ر', 1322, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (142213, N'قهدریجان-ر', 1322, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (142214, N'ابریشم-ر', 1322, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (142215, N'پیربکران-ر', 1322, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (142216, N'مینادشت-ر', 1322, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (142217, N'بهاران-ر', 1322, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (142218, N'اشترجان-ر', 1322, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (142219, N'زازران-ر', 1322, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (142220, N'روستاهای فلاورجان-ر', 1322, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (142311, N'داران-ر', 1323, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (142315, N'دامنه-ر', 1323, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (142411, N'کوهپایه-ر', 1324, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (142412, N'سجزی-ر', 1324, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (142413, N'تودشک-ر', 1324, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (142511, N'گلپایگان-ر', 1325, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (142512, N'گوگد-ر', 1325, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (142513, N'گلشهر-ر', 1325, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (142611, N'زرین شهر-ر', 1326, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (142612, N'ورنامخواست-ر', 1326, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (142613, N'سده لنجان-ر', 1326, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (142614, N'چرمهین-ر', 1326, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (142615, N'باغبهادران-ر', 1326, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (142616, N'چمگردان-ر', 1326, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (142617, N'زاینده رود-ر', 1326, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (142618, N'باغشاد-ر', 1326, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (142711, N'مبارکه-ر', 1327, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (142712, N'دیزیچه-ر', 1327, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (142713, N'طالخونچه-ر', 1327, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (142714, N'زیبا شهر-ر', 1327, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (142715, N'کرکوند-ر', 1327, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (142811, N'نائین-ر', 1328, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (142812, N'انارک-ر', 1328, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (142813, N'بافران-ر', 1328, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (142911, N'نجف آباد-ر', 1329, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (142914, N'گلدشت-ر', 1329, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (142915, N'کهریزسنگ-ر', 1329, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (142916, N'جوزدان-ر', 1329, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (143011, N'نطنز-ر', 1330, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (143012, N'طرق-ر', 1330, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (143111, N'ورزنه-ر', 1331, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (143211, N'هرند-ر', 1332, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (143212, N'اژیه-ر', 1332, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (143311, N'تیران-ر', 1333, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (143312, N'رضوان شهر-ر', 1333, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (143313, N'عسگران-ر', 1333, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (143411, N'فولادشهر-ر', 1334, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (143611, N'بادرود-ر', 1336, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (143711, N'چادگان-ر', 1337, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (143712, N'رزوه-ر', 1337, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (143811, N'دهاقان-ر', 1338, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (143812, N'گلشن-ر', 1338, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (143911, N'خور-ر', 1339, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (143912, N'جندق-ر', 1339, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (143913, N'فرخی-ر', 1339, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (144013, N'دولت آباد-ر', 1340, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (144014, N'خورزوق-ر', 1340, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (144015, N'دستگرد-ر', 1340, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (144016, N'حبیب آباد-ر', 1340, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (144019, N'کمشچه-ر', 1340, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (144111, N'بوئین میاندشت-ر', 1341, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (144112, N'افوس-ر', 1341, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (144211, N'میمه-ر', 1342, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (144212, N'وزوان-ر', 1342, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (144213, N'لای بید-ر', 1342, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (144311, N'قهجاورستان-ر', 1343, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (144411, N'زیار-ر', 1344, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (144511, N'دهق-ر', 1345, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (144512, N'علویجه-ر', 1345, NULL)
GO
INSERT [LocationPool].[Zone] ([Id], [Title], [RegionId], [UnstandardCode]) VALUES (149011, N'بهارستان-ر', 1390, NULL)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10010001, N'اشینه', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10010003, N'قارنه', 131512, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10010004, N'ارجنک', 131611, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10010005, N'تنگ خشک', 131911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10010006, N'دره بید', 132315, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10010007, N'بزمه', 132112, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10010008, N'پلارت', 132215, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10010009, N'کهرویه', 132011, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10010011, N'آرجان', 132512, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10010012, N'آيدوغميش', 132615, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10010013, N'چوپانان', 132812, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10010014, N'رحمت آباد', 132916, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10010015, N'اریسمان', 133611, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10010016, N'جهاد آباد', 131411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10010017, N'ابرو', 132713, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10010018, N'آبپونه', 133312, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10010020, N'علی آباد', 133711, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10010022, N'جعفرآباد', 133911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10010023, N'پروانه', 134016, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10010024, N'آبچوییه', 132413, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10010025, N'تیرانچی', 131703, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10010026, N'آرتیجان', 134111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10010027, N'گلستانه', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10020001, N'خیر آباد', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10020003, N'گنج آباد', 131512, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10020004, N'حاجی آباد', 131611, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10020005, N'چهارراه', 131911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10020006, N'قفر', 132311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10020007, N'بیجگرد', 132112, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10020008, N'تمندگان', 132215, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10020009, N'یحی آباد', 132012, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10020011, N'دستجرده', 132512, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10020012, N'برنجگان', 132615, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10020013, N'کبرآباد', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10020014, N'فیلور', 132916, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10020015, N'صالح اباد', 133011, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10020016, N'بیدشک', 131411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10020017, N'حوض ماهی', 132713, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10020018, N'حسن آبادآبریزه', 133312, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10020020, N'فراموشجان', 133711, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10020022, N'چاهملک', 133911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10020023, N'علی آباد چی', 134016, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10020024, N'چیرمان', 132413, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10020026, N'حسین آباد', 134111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10020027, N'جار', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10030001, N'طوران', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10030003, N'سیان', 131512, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10030004, N'حسن آباد', 131611, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10030005, N'روداباد', 131911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10030006, N'نهرخلج', 132311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10030007, N'چقا', 132112, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10030008, N'چهاربرج', 132215, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10030009, N'اسفرجان', 132012, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10030011, N'دم اسمان', 132513, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10030012, N'حاجي الوان', 132615, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10030013, N'فيض آباد معدن', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10030015, N'ده اباد', 133611, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10030016, N'دهلر', 131411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10030017, N'قلعه سفید', 132713, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10030018, N'حسن آبادکهنه', 133312, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10030020, N'چهل چشمه', 133711, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10030022, N'قادراباد', 133911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10030023, N'دنبی', 134016, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10030024, N'جشوقان', 132413, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10030026, N'ده رجب', 134111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10030027, N'يفران', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10040001, N'فران', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10040003, N'حسین آباد', 131512, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10040004, N'خشکرود', 131611, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10040005, N'سادات پادنا', 131914, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10040006, N'گنجه', 132311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10040007, N'خویگان علیا', 132112, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10040008, N'خوانسارک', 132215, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10040009, N'هونجان', 132012, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10040011, N'رباط ملکی', 132512, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10040012, N'خشوئيه', 132615, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10040013, N'مزرعچه', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10040015, N'فمی', 133611, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10040016, N'کلهرود', 131411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10040017, N'لاو', 132713, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10040018, N'قلعه عرب', 133312, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10040020, N'ورباد', 133711, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10040021, N'محمود آباد', 133812, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10040023, N'مرغ', 134016, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10040024, N'دلگشا', 132413, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10040026, N'ماربر', 134111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10040027, N'ايچي', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10050001, N'قهساره', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10050003, N'آذرخواران', 131512, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10050004, N'کهرت', 131611, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10050005, N'دنگزلو', 131914, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10050006, N'بادجان', 132311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10050007, N'دهسور سفلی', 132112, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10050008, N'دارافشان', 132215, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10050009, N'امامزاده علی اکبر', 132011, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10050011, N'سراور', 132512, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10050012, N'دورك', 132615, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10050013, N'كدنوئيه', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10050014, N'حاجی آباد', 132911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10050015, N'متین اباد', 133611, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10050016, N'مورچه خورت', 131411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10050017, N'بداغ اباد', 132711, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10050018, N'قره تپه', 133312, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10050020, N'لگاله', 133711, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10050021, N'علی آباد گچی', 133812, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10050022, N'ابراهیم آباد', 133911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10050023, N'شورچه', 134016, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10050024, N'اروجه', 132413, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10050026, N'قره بلطاق', 134111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10050027, N'کرچگان', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10060001, N'كچي', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10060003, N'پیکان', 131511, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10060004, N'ویست', 131611, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10060005, N'نقل', 131914, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10060006, N'قوهک', 132311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10060007, N'سروشجان', 132112, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10060008, N'رارا', 132215, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10060009, N'اسفه', 132011, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10060011, N'شادگان', 132513, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10060012, N'رحمتاباد', 132615, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10060013, N'حاجی آباد', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10060014, N'اشن', 134511, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10060015, N'بیدهند', 133011, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10060016, N'باغمیران', 131411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10060017, N'مزرعچه', 132712, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10060018, N'عزیزآباد', 133312, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10060020, N'مشهدکاوه', 133711, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10060021, N'کره', 133811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10060022, N'آبادان', 133911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10060023, N'سنگلاخ', 134016, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10060024, N'وج', 132413, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10060026, N'خلیلی', 134111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10060027, N'زغمار', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10070001, N'كهنگ', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10070003, N'سعادت آباد', 131511, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10070004, N'تیدجان', 131611, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10070005, N'نورابادپادنا', 131914, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10070006, N'خلج', 132311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10070007, N'میلاگرد', 132112, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10070008, N'رحیم آباد', 132215, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10070009, N'مهیار', 132011, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10070011, N'شیداباد', 132512, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10070012, N'زردخشوييه', 132615, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10070013, N'مجتمع جزلان', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10070014, N'خیرآباد', 134511, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10070015, N'ولوجرد', 133011, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10070016, N'لایبد', 131411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10070017, N'حسن اباد بیدکان', 132715, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10070018, N'قلعه موسی خان', 133312, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10070020, N'درک آباد', 133711, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10070021, N'لاریچه', 133811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10070022, N'خرمدشت', 133911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10070024, N'باد افشان', 132413, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10070026, N'تخماقلو', 134111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10070027, N'هرمدان', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10080001, N'مار', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10080003, N'حیدرآباد', 131511, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10080004, N'دوشخراط', 131611, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10080005, N'کره دان', 131914, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10080006, N'چهلخانه', 132311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10080007, N'چقادر', 132112, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10080008, N'سمسان', 132215, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10080009, N'باغ سرخ', 132012, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10080011, N'فاویان', 132513, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10080012, N'سعيداباد', 132615, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10080014, N'دماب', 134511, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10080015, N'هنجن', 133011, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10080016, N'لوشاب', 134211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10080018, N'تندران', 133311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10080020, N'کمیتک', 133712, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10080021, N'دزج', 133811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10080022, N'مصر', 133912, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10080024, N'برز آباد', 132413, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10080026, N'بسینان', 134111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10080027, N'عباس آباد', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10090001, N'مارچوبه', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10090003, N'مزرعه عرب', 131511, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10090004, N'سنگ سفید', 131611, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10090005, N'شهید', 131914, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10090006, N'حصور', 132311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10090007, N'دهسور علیا', 132112, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10090008, N'سیاه افشار', 132215, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10090009, N'بوان', 132012, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10090011, N'وارنیان', 132512, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10090012, N'صادق اباد', 132615, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10090014, N'گلدره', 134511, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10090015, N'یارند', 133011, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10090016, N'حسن رباط', 134211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10090017, N'کرکوند', 132715, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10090018, N'جاجا', 133311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10090020, N'پرمه سفلی', 133711, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10090021, N'همگین', 133811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10090022, N'ایراج', 133911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10090024, N'جندابه', 132413, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10090026, N'کرچ', 134111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10090027, N'ازيران', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10100001, N'مباركه', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10100003, N'کمال آباد', 131513, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10100004, N'صفادشت', 131611, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10100005, N'کیفته حسینی', 131914, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10100006, N'ازونبلاغ', 132311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10100007, N'بهرام آباد', 132111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10100008, N'صادق آباد', 132215, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10100009, N'دهک', 132012, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10100011, N'استهلک', 132511, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10100012, N'قلعه تركي', 132615, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10100014, N'حسین آباد', 134512, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10100015, N'باغستان بالا', 133012, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10100016, N'موته', 134211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10100017, N'احمد اباد', 132714, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10100018, N'جعفرآباد', 133311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10100020, N'آبادچی پایین', 133711, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10100021, N'قمیشلو', 133811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10100022, N'گرمه', 133911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10100024, N'مشکنان', 132413, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10100026, N'هلاغره', 134111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10100027, N'ليان', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10110001, N'نهوج', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10110003, N'دستجرد', 131513, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10110004, N'قلعه بابامحمد', 131611, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10110005, N'دیدجان', 131914, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10110006, N'درختک', 132311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10110007, N'دهنو', 132111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10110008, N'علی آباد', 132215, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10110009, N'زیارتگاه', 132012, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10110011, N'اسفاجرد', 132512, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10110012, N'كته شور', 132615, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10110014, N'خونداب', 134511, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10110016, N'ازان', 134211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10110017, N'اراضی', 132714, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10110018, N'خرمنان', 133311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10110020, N'تقی آباد', 133711, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10110021, N'پوده', 133811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10110022, N'مهرجان', 133911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10110024, N'امامزاده قاسم', 132411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10110026, N'قلعه بهمن', 134111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10110027, N'کروه', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10120001, N'نيسيان', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10120003, N'خارا', 131513, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10120004, N'قودجان', 131611, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10120005, N'قنات', 131914, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10120006, N'دهق', 132311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10120007, N'تزره', 132111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10120008, N'قلعه سرخ', 132215, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10120009, N'سولار', 132012, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10120011, N'حاجیله', 132511, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10120012, N'كرچگان', 132615, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10120014, N'علی آباد', 134511, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10120015, N'طار', 133011, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10120016, N'خسروآباد', 134211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10120017, N'اسد اباد', 132714, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10120018, N'خمیران', 133311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10120020, N'حیدر آباد', 133711, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10120021, N'قهه', 133811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10120022, N'اردیب', 133911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10120024, N'جزه', 132411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10120026, N'بتلیجه', 134111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10120027, N'پيله وران', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10130001, N'ديزيچه', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10130003, N'اله آباد', 131513, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10130004, N'لایجند', 131611, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10130005, N'گیوسین', 131914, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10130006, N'قودجانک', 132311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10130007, N'خشک آبخور', 132111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10130008, N'کلیسان', 132215, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10130009, N'قصرچم', 132011, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10130011, N'درب امامزاده', 132511, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10130012, N'موركان', 132615, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10130014, N'هسنیجه', 134512, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10130015, N'کشه', 133012, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10130016, N'سعید آباد', 134211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10130017, N'اکبر اباد', 132714, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10130018, N'خیرآباد', 133311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10130020, N'پرمه علیا', 133711, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10130021, N'گنجقباد', 133811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10130022, N'بیاضه', 133911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10130024, N'کمال بیک', 132411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10130026, N'دره سوخته', 134111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10130027, N'برکان', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10140001, N'اونج', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10140003, N'مالواجرد', 131513, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10140004, N'شهرک های یاسر', 131611, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10140005, N'مورک', 131914, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10140006, N'نماگرد', 132311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10140007, N'راچه', 132111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10140008, N'مهرگان', 132215, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10140009, N'قوام آباد', 132012, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10140011, N'دیزیجان', 132511, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10140012, N'همام', 132615, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10140014, N'جلال آباد', 132911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10140015, N'مزده', 133011, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10140016, N'ونداده', 134211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10140017, N'بارچان', 132714, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10140018, N'ورپشت', 133311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10140020, N'دولت آباد گل سفید', 133711, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10140021, N'قمبوان', 133811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10140022, N'حسین آباد هفتومان', 133911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10140024, N'سهر', 132411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10140026, N'هندوکش', 134111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10140027, N'ازوار', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10150001, N'باقرابادعليا', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10150003, N'مبارکه', 131513, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10150004, N'شهرک صنعتی', 131611, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10150005, N'سادات دیدجان', 131914, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10150006, N'سفتجان', 132311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10150007, N'زردفهره', 132111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10150008, N'نودرآمد', 132215, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10150009, N'ماران', 132012, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10150011, N'رباط ابوالقاسم', 132511, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10150012, N'چم تقي', 132615, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10150014, N'نهضت آباد', 132911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10150015, N'نیه', 133011, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10150016, N'سه', 131411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10150017, N'باغملک', 132714, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10150018, N'آبگرم', 133313, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10150020, N'دهباد سفلی', 133711, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10150021, N'جمبزه', 133811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10150022, N'هفتومان', 133911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10150024, N'میر جعفر', 132411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10150026, N'نوغان علیا', 134111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10150027, N'کلارتان', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10160001, N'بغم', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10160003, N'رامشه', 131513, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10160004, N'تجره', 131611, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10160005, N'قلعه اسلام اباد', 131912, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10160006, N'چیگان', 132311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10160007, N'سخی آباد', 132111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10160008, N'وزیرآباد', 132215, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10160009, N'مسینه', 132012, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10160011, N'رباط سرخ', 132511, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10160012, N'حاجت اقا', 132615, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10160014, N'کوه لطف', 134511, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10160015, N'یحیی اباد', 133012, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10160017, N'بروزاد', 132714, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10160018, N'افجان', 133311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10160020, N'هرمانک سفلی', 133711, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10160021, N'علی آباد جمبزه', 133811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10160022, N'عروسان', 133911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10160024, N'دخر آباد', 132411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10160026, N'نوغان سفلی', 134111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10160027, N'زيار ( شهر)', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10170001, N'جنبه', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10170004, N'حاج بلاغ', 131611, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10170005, N'چشمه خونی', 131912, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10170006, N'غرغن', 132311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10170007, N'سکان', 132111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10170008, N'پلارتگان', 132215, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10170009, N'مقصود بیک', 132012, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10170011, N'رباط قالقان', 132512, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10170012, N'ركن اباد', 132615, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10170015, N'باغستان پایین', 133012, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10170017, N'جوشان', 132714, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10170018, N'بودان', 133311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10170020, N'اصفهانک مشاعی', 133711, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10170022, N'نصرآباد', 133913, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10170024, N'علون آباد', 132411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10170026, N'ماهورک', 134111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10170027, N'اندلان', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10180001, N'چاهريسه', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10180004, N'خم پیچ', 131611, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10180005, N'شیخعلی', 131911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10180006, N'سواران', 132311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10180007, N'اسلام آباد', 132111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10180008, N'دارگان', 132215, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10180009, N'میر آباد', 132012, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10180011, N'سعید آباد', 132511, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10180012, N'زمان آباد', 132614, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10180015, N'ورگوران', 133011, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10180017, N'جوهرستان', 132714, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10180018, N'حسن آبادعلیا', 133313, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10180020, N'اصفهانک علیا', 133711, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10180022, N'فرح زاد', 133912, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10180024, N'کیچی', 132411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10180026, N'دره ساری', 134111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10180027, N'رحيم آباد', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10190001, N'خاصه تراش', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10190004, N'رحمت آباد', 131611, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10190005, N'گرموک', 131911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10190006, N'قلعه ملک', 132311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10190007, N'مکدین علیا', 132111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10190008, N'علیشاهدان', 132215, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10190009, N'وشاره', 132012, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10190011, N'ضامن اباد', 132511, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10190012, N'قلعه پائین', 132614, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10190013, N'بنويدسفلي', 132813, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10190015, N'ابیازن', 133011, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10190017, N'دهسرخ', 132714, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10190018, N'حسین آباد', 133311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10190020, N'اصفهانک ساکی', 133711, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10190024, N'آبخارک', 132411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10190026, N'تیرکرت', 134111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10190027, N'روران', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10200001, N'ديزلو', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10200004, N'مهرآباد', 131611, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10200005, N'هست', 131911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10200006, N'ننادگان', 132311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10200007, N'مزرعه میر', 132111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10200008, N'بجگرد', 132215, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10200009, N'ولندان', 132012, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10200011, N'عباس اباد', 132511, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10200012, N'قلعه اقا', 132615, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10200013, N'سپرو', 132813, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10200015, N'اسفیدان', 133011, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10200017, N'زودان', 132714, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10200018, N'سوران', 133311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10200020, N'اصفهانک سفلی', 133711, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10200024, N'کرد آباد', 132411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10200026, N'قلعه اخلاص', 134111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10200027, N'شيدان', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10210001, N'عباس ابادعليا', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10210005, N'اسکان', 131912, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10210006, N'خویگان', 132311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10210007, N'وهرگان', 132111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10210008, N'سهروفیروزان', 132215, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10210009, N'امین آباد', 132012, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10210011, N'غرقاب', 132511, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10210012, N'قلعه لاي بيد', 132615, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10210013, N'سهيل', 132813, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10210015, N'اوره', 133011, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10210017, N'فخر اباد', 132714, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10210018, N'علی آباد', 133311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10210020, N'اصفهانک عبدل', 133711, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10210024, N'زفره', 132412, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10210026, N'بلطاق', 134111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10210027, N'هرمزآباد', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10220001, N'فسخود', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10220005, N'ضرغام اباد', 131911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10220006, N'گل امیر', 132311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10220007, N'سیبک', 132111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10220008, N'طاد', 132215, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10220011, N'فرج آباد', 132511, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10220012, N'كچوييه بالا', 132614, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10220013, N'فرح آباد', 132813, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10220015, N'جریان', 133011, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10220017, N'کوشکیچه', 132714, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10220018, N'محمدیه', 133311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10220020, N'پرزگان سفلی', 133711, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10220024, N'مزرعه عبدا...', 132412, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10220026, N'ازناوله', 134111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10220027, N'کبوترآباد', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10230001, N'گل و مل', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10230005, N'اسلام اباد', 131911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10230006, N'اسکندری', 132311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10230007, N'نهضت آباد', 132111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10230008, N'فیلرگان', 132215, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10230011, N'فقستان', 132512, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10230012, N'كليشادرخ', 132615, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10230013, N'هندوچوب', 132813, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10230015, N'جزن', 133011, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10230017, N'میر اباد', 132714, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10230018, N'میرآباد', 133311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10230020, N'پرزگان خراج', 133711, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10230024, N'ورتون', 132412, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10230026, N'زرنه', 134111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10230027, N'چم زيار', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10240001, N'ماستبندي', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10240005, N'آغداش سفلی', 131911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10240006, N'نسار', 132311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10240007, N'سرداب سفلی', 132111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10240008, N'گلگون', 132215, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10240011, N'قالقان', 132512, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10240012, N'هاردنگ', 132614, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10240013, N'بنويدعليا', 132813, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10240015, N'خفر', 133011, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10240017, N'هراتمه', 132714, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10240018, N'نسیم آباد', 133311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10240020, N'چشمندگان سفلی', 133711, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10240024, N'یک لنگی', 132412, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10240026, N'اغچه', 134111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10240027, N'دهکرم', 133212, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10250001, N'ومكان', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10250005, N'پیراسفنه', 131911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10250006, N'سینگرد', 132311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10250007, N'سرداب علیا', 132111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10250008, N'نرگان', 132215, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10250011, N'قرغن', 132511, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10250012, N'پركستان', 132615, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10250013, N'مزرعه احمد', 132813, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10250015, N'دستجرد', 133011, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10250017, N'سورچه بالا', 132715, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10250018, N'مهدی آباد', 133311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10250020, N'چشمندگان مجید', 133711, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10250024, N'کلیشاد', 133211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10250026, N'حاج فتحعلی', 134111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10250027, N'جمبزه', 133212, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10260001, N'حسن ابادشور', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10260005, N'جلال اباد', 131911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10260006, N'عادگان', 132311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10260007, N'دره سیب', 132111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10260008, N'ونهر', 132215, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10260012, N'چم حيدر', 132615, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10260013, N'مزرعه حاج حسين', 132813, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10260015, N'طامه', 133011, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10260017, N'اردوگاه', 132714, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10260018, N'گلاب', 133313, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10260020, N'قلعه زنبور', 133711, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10260024, N'سیچی', 133211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10260026, N'آقاگل', 134111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10260027, N'مارچي', 133212, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10270001, N'مونيه', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10270005, N'حاجی اباد', 131911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10270006, N'طرار', 132311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10270007, N'قلعه سرخ', 132111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10270008, N'کرسگان', 132211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10270011, N'لالان', 132511, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10270012, N'چم نور)چم گاو(', 132615, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10270013, N'جزن اباد', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10270015, N'میلاجرد', 133011, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10270018, N'الور', 133313, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10270020, N'چشمندگان علیا', 133711, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10270024, N'مادرکان', 133211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10270026, N'هزارجریب', 134111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10270027, N'قلعه عبداله', 133212, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10280001, N'علي ابادلاسيب', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10280005, N'حیدرابادعباسی', 131911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10280007, N'میدانک اول', 132111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10280008, N'دشتچی کرسگان', 132211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10280011, N'ماکوله', 132511, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10280012, N'جعفراباد', 132615, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10280013, N'جوي اباد', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10280015, N'نسران', 133011, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10280018, N'چشمه احمدرضا', 133313, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10280020, N'کلیچه', 133711, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10280024, N'خرم', 133211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10280026, N'قايم آباد', 134111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10280027, N'برسيان', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10290001, N'لاسيب', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10290005, N'ده عاشوری', 131911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10290007, N'میدانک دوم', 132111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10290008, N'کارویه', 132213, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10290011, N'مرغ', 132511, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10290012, N'چم طاق', 132615, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10290013, N'خاروان', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10290015, N'ابکشه', 133012, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10290018, N'دره بید', 133313, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10290020, N'دهباد علیا', 133711, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10290024, N'خورچان', 133211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10290026, N'قلعه خواجه', 134111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10290027, N'تيميارت', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10300001, N'توتكان', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10300005, N'دهکرد', 131911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10300007, N'برداسیاب', 132111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10300008, N'زفره', 132211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10300011, N'مزرعه', 132512, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10300012, N'چم عليشاه', 132615, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10300013, N'فيض ابادحاج كاظم', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10300015, N'برزرود', 133011, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10300018, N'دوتو', 133313, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10300020, N'اورگان', 133711, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10300024, N'هلارته', 133211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10300026, N'داشکسن', 134111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10300027, N'فساران', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10310001, N'پنج', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10310005, N'سبزاباد', 131911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10310007, N'چقیورت', 132111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10310008, N'کاویان', 132211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10310011, N'ملازجان', 132512, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10310012, N'چم كهريز', 132615, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10310013, N'كجان', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10310018, N'دولت آباد', 133313, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10310020, N'هرمانک علیا', 133711, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10310024, N'سکان', 133211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10310026, N'میرآباد', 134111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10310027, N'جور', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10320001, N'سرابه', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10320005, N'سادات اباد وردشت', 131911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10320007, N'دره بادام علیا', 132111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10320008, N'محمدیه', 132211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10320011, N'هنده', 132511, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10320012, N'چم يوسفعلي', 132615, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10320013, N'گلستان', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10320018, N'قلعه ناظر', 133313, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10320020, N'سرچشمه', 133711, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10320024, N'سنوچی', 133211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10320026, N'جوزار', 134111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10320027, N'اسفيناء', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10330001, N'زفرقند', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10330005, N'علی اباد', 131911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10330007, N'دره بادام سفلی', 132111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10330008, N'مهرنجان اتراک', 132211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10330011, N'غرقه', 132511, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10330012, N'اشيان', 132612, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10330013, N'ونديش', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10330018, N'کردسفلی', 133313, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10330020, N'دولت آباد', 133711, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10330024, N'پاچیک آباد', 133211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10330027, N'دستجاء', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10340001, N'كاشانك', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10340005, N'فتح اباد', 131911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10340007, N'خسرو آباد', 132111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10340008, N'بوستان لارگان', 132211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10340011, N'هرستانه', 132511, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10340012, N'كاريز', 132612, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10340013, N'ملكان', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10340018, N'گنهران', 133313, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10340020, N'خرسانک سفلی', 133711, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10340024, N'کمندان', 133211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10340027, N'کندلان', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10350001, N'كچومثقال', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10350005, N'قره قاچ', 131911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10350007, N'مزرعه کیماس', 132111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10350008, N'خیرآباد', 132211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10350011, N'رباط محمود', 132512, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10350012, N'مبارک آباد', 132612, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10350013, N'صفي اباد', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10350018, N'تقی آباد', 133313, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10350020, N'خرسانک علیا', 133711, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10350024, N'سریان', 133211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10350027, N'کوهان', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10360001, N'مهراندو', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10360005, N'کزن', 131911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10360007, N'مزرعه سیب', 132111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10360008, N'مهرنجان ارامنه', 132211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10360011, N'علی آباد', 132512, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10360012, N'قرق اقا', 132612, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10360013, N'نيستانك', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10360018, N'هومان', 133311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10360020, N'علی عرب', 133712, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10360024, N'قلعه ساربان', 133211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10360027, N'حاجي آباد', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10370001, N'ميشاب', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10370005, N'موروک', 131911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10370007, N'حاجی آباد', 132111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10370008, N'بوستان درچه عابد', 132211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10370011, N'خاکه', 132512, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10370012, N'باغشاه', 132618, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10370013, N'مهرادران', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10370018, N'قاسم آباد', 133311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10370020, N'ده کلبعلی', 133712, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10370024, N'کلیل', 133211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10370027, N'دستگردمار', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10380001, N'هندواباد', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10380005, N'مهراباد', 131911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10380007, N'دره نورالدین', 132111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10380008, N'بوستان حاجی آباد', 132211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10380011, N'قشلاق', 132511, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10380012, N'مديسه', 132618, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10380013, N'بلان', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10380018, N'حسن آبادوسطی', 133311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10380020, N'معروف آباد', 133712, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10380024, N'صیدان', 133211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10380027, N'ارم پشت', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10390001, N'دره باغ', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10390005, N'مهرگرد', 131911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10390007, N'لطف آباد', 132111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10390008, N'دستناء', 132215, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10390011, N'لالانک', 132511, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10390012, N'نوگوران', 132618, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10390013, N'حسين ابادعاشق', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10390018, N'اله آباد', 133312, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10390020, N'حجت آباد', 133711, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10390024, N'امامزاده عبدالعزیز', 133211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10390027, N'ديده ران', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10400001, N'جعفر اباد گرمسير', 131213, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10400005, N'ورق', 131911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10400008, N'جیلاب', 132211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10400011, N'تیکن', 132512, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10400012, N'زاینده رود', 132617, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10400013, N'ورپاي عليا', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10400018, N'عسگران', 133313, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10400020, N'انالوجه', 133712, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10400024, N'فیض آباد', 133211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10400027, N'کاروانچي', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10410001, N'حيدراباد', 131213, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10410005, N'چشمه رحمن', 131911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10410008, N'حسین آباد', 132211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10410011, N'در', 132512, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10410012, N'صادق آبادكاريز', 132612, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10410013, N'يورتكا', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10410018, N'مبارکه', 133311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10410020, N'قرقر', 133712, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10410024, N'شریف آباد', 133211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10410027, N'جاجا', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10420001, N'خشك اياد', 131213, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10420005, N'حیدراباد', 131911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10420008, N'جولرستان', 132211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10420011, N'زرنجان', 132512, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10420012, N'سيبه', 132612, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10420013, N'معين آباد', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10420018, N'کردعلیا', 133313, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10420020, N'منصوریه', 133711, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10420024, N'گیشی', 133212, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10420027, N'يحيي آباد', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10430001, N'دولت اباد', 131213, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10430005, N'ده نسا علیا', 131911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10430008, N'چمرود', 132215, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10430011, N'شرکت زراعی', 132512, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10430012, N'يال بلند', 132615, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10430013, N'اسدآباد', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10430020, N'گشنیزجان', 133712, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10430024, N'قمشان', 133212, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10430027, N'کنجوان', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10440001, N'رحمت اباد', 131213, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10440005, N'بردکان', 131911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10440008, N'اردال', 132215, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10440011, N'شورچه', 132512, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10440012, N'چم پير', 132615, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10440013, N'بلااباد', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10440020, N'آبادچی علیا', 133711, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10440024, N'قلعه بالا', 133212, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10440027, N'باچه', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10450001, N'سرهنگچه', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10450005, N'ملاقلی', 131911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10450008, N'امام زاده شمس الدین', 132215, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10450011, N'نیوان سوق', 132512, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10450012, N'شورجه', 132618, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10450013, N'جهان اباد', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10450020, N'درکان', 133711, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10450024, N'سیان', 133212, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10450027, N'دولاب', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10460001, N'سريجهن', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10460005, N'کاکابادعلیا', 131911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10460011, N'وانشان', 132511, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10460012, N'چم آسمان', 132618, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10460013, N'دولت آباد شيخ', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10460020, N'باغ ناظر', 133711, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10460024, N'قهی', 133211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10460027, N'اسپارت', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10470001, N'كسوج', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10470005, N'نظراباد', 131911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10470011, N'نیوان نار', 132512, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10470012, N'فتح آباد', 132615, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10470013, N'رحيم اباد', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10470020, N'هجرت آباد', 133711, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10470024, N'طهمورثات', 133111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10470027, N'منشيان', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10480001, N'حسن اباد گرمسير', 131213, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10480005, N'سرچغاسفلی', 131911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10480011, N'امیریه', 132511, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10480013, N'رسول اباد', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10480020, N'محمود آباد', 133711, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10480024, N'ابوالخیر', 133111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10480027, N'حسين آباد', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10490001, N'حسين اباد گرمسير', 131213, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10490005, N'اسداباد', 131911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10490011, N'آزادان', 132512, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10490013, N'زمان اباد', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10490020, N'جلال آباد', 133711, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10490024, N'رنگینده', 133111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10490027, N'جوزدان', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10500001, N'زيارتگاه', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10500005, N'حاجی اباد شورچمن', 131911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10500011, N'رباط گوگدی', 132511, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10500013, N'سرشك', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10500020, N'ابوالقاسم آباد', 133711, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10500024, N'فارفان', 133111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10510001, N'شمس اباد', 131213, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10510005, N'دیزجان', 131911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10510013, N'سيف آباد', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10510020, N'قهرمان آباد', 133711, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10510024, N'کفران', 133111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10520001, N'موغار', 131213, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10520005, N'ده نساسفلی', 131911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10520013, N'عفيف آباد', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10520020, N'ظاهر آباد', 133711, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10520024, N'کفرود', 133111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10530001, N'گلستان بهشتي', 131213, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10530005, N'نور ابادوردشت', 131911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10530013, N'فوداز', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10530020, N'جعفر آباد', 133711, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10530024, N'جندان', 133111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10540001, N'آستانه', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10540005, N'نرمه', 131911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10540013, N'قلعه دار', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10540020, N'حسین آباد', 133711, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10540024, N'بزم', 133111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10550001, N'بند آستانه', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10550005, N'قلعه قدم', 131913, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10550013, N'هاشم آباد', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10550020, N'ونک سفلی', 133711, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10550024, N'سهران', 133111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10560001, N'داوران', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10560005, N'علی اباد اغداش', 131911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10560013, N'مظفراباد', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10560020, N'ورباد سفلی', 133711, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10560024, N'قلعه امام', 133111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10570001, N'شيرازان', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10570005, N'اسلام اباداغداش', 131911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10570013, N'مهراباد', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10570020, N'دره مهدیقلی', 133711, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10570024, N'اشکهران', 133111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10580001, N'كچوسنگ', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10580005, N'مهدی اباد اغداش', 131911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10580013, N'نصراباد', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10580020, N'ونک علیا', 133711, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10580024, N'بلان', 133111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10590001, N'گونيان', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10590005, N'چشمه سرد', 131911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10590013, N'اوشن عليا', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10590020, N'دره آلوچه', 133711, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10590024, N'قورتان', 133111, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10600001, N'همبر', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10600005, N'سرچغاعلیا', 131911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10600013, N'فهيه', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10600024, N'شریف آباد', 133211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10610001, N'همسار', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10610005, N'بیده', 131914, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10610013, N'ماندگي عليا', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10610024, N'باقرآباد', 133211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10620001, N'گل شكنان', 131212, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10620005, N'لرکش', 131914, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10620013, N'مرغچوئيه', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10620024, N'کبریت', 133211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10630001, N'سفيده', 131212, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10630005, N'گنجگان', 131914, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10630013, N'هماابادعليا', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10630024, N'هاشم آباد', 133211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10640001, N'شهراب', 131212, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10640005, N'کاسگان سفلی', 131911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10640013, N'خرمدشت', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10640024, N'مند آباد', 132411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10650001, N'همت اباد', 131212, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10650005, N'حسین اباد', 131911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10650013, N'اله آباد', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10650024, N'هماگران', 132411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10660001, N'مزداباد', 131212, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10660005, N'بارند سفلی', 131914, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10660013, N'خيرآباد', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10660024, N'میرهمایون', 132411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10670001, N'احمد اباد', 131213, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10670005, N'صادق اباد', 131911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10670013, N'اميرآباد', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10670024, N'سسن آباد', 132411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10680001, N'اسلام اباد', 131213, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10680005, N'ماندگان', 131911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10680013, N'ماندگي سفلي', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10680024, N'لوتری', 132411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10690001, N'امير اباد', 131212, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10690005, N'سرباز', 131914, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10690013, N'سپيده', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10690024, N'هریزه', 132411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10700001, N'اميران', 131212, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10700005, N'حسن آباد', 131914, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10700013, N'موسي آباد', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10700024, N'تینجان', 132411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10710001, N'تلك اباد', 131212, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10710002, N'كاج', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10710005, N'پهلو شکن', 131914, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10710013, N'هماآبادسفلي', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10710024, N'فشارک', 132411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10720001, N'جعفراباد ريگستان', 131212, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10720005, N'شیبانی', 131914, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10720013, N'خارزن', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10720024, N'آبفا تودشک', 132413, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10730001, N'جهان اباد', 131212, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10730005, N'بارند علیا', 131914, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10730013, N'مزرعه نو', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10730024, N'پایگاه هوایی هاشم آباد', 132413, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10740001, N'حسين اباد ريگستان', 131212, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10740005, N'دورجان علیا', 131914, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10740013, N'معصوم آباد', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10740024, N'مزرعه یزدی', 132413, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10750001, N'علي اباد ريگستان', 131212, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10750005, N'تل محمد', 131914, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10750013, N'كوشكوئيه', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10750024, N'سیدآباد', 132413, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10760001, N'كچورستاق', 131212, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10760005, N'دره ماسون', 131914, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10760013, N'نرگور', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10760024, N'مهدی آباد', 132412, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10770001, N'كريم اباد', 131212, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10770005, N'دورجان سفلی', 131914, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10770013, N'حسين آبادنرگور', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10770024, N'علی آباد', 132413, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10780001, N'گلزارمحمد', 131213, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10780002, N'مهدي آباد', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10780005, N'بی بی سیدان', 131911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10780013, N'نوشي', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10790001, N'شهيد رجايي', 131212, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10790002, N'سروشبادران', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10790005, N'رهیز', 131914, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10790013, N'بلابادچه', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10800001, N'گلستان مهدي', 131213, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10800002, N'امين اباد', 134311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10800005, N'کهنگان', 131914, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10800013, N'نوجوك', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10810001, N'معين اباد', 131212, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10810002, N'جلادران', 134311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10820001, N'نير اباد', 131212, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10820002, N'جيلان آباد', 134311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10820005, N'نادرآباد', 131911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10820013, N'سنجدو', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10830001, N'علي اباد منصور', 131212, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10830002, N'حسن اباد', 134311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10830005, N'کاسگان علیا', 131911, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10830013, N'راحت آباد', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10840001, N'دهنو', 131212, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10840002, N'سوسارت', 134311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10840013, N'عشرت آباد', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10850001, N'سيد اباد', 131213, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10850002, N'فيروزاباد', 134311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10850005, N'ذبیح آباد', 131914, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10850013, N'كمال آباد', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10860001, N'نجف اباد ريگ', 131212, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10860002, N'كلمنجان', 134311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10860013, N'ورپشت', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10870001, N'حبيب اباد ريگ', 131212, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10870002, N'محمداباد', 134311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10870013, N'ورچم', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10880001, N'عباس اباد ريگ', 131212, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10880002, N'مزرعه گورت', 134311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10880013, N'مزرعه نو حاج قنبري', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10890001, N'باقر اباد ريگ', 131212, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10890002, N'هتم آباد', 134311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10890013, N'مزيک', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10900001, N'حاجي اباد ريگ', 131212, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10900002, N'اندوان', 134311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10900013, N'حسين آباد حاج کاظم', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10910001, N'بلهور', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10910002, N'بهاران', 134311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10910013, N'نجف آباد', 132811, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10920001, N'حسن اباد مظاهري', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10920002, N'جلمرز', 134311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10930001, N'كماسه', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10930002, N'گيان', 134311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10940001, N'حسن اباد ريگ', 131212, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10940002, N'مورنان', 134311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10950001, N'نصر اباد', 131212, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10950002, N'زمان آباد', 131304, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10960001, N'خالق اباد', 131212, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10960002, N'دينان', 134311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10970001, N'يافت اباد', 131212, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10970002, N'مولنجان', 131304, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10980001, N'پنارت', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10980002, N'آمرزيدآباد', 134311, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (10990001, N'ميرزاعلي', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (11000001, N'بيدشك', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (11010001, N'ونين', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (11010002, N'دشتي', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (11020001, N'سهامیه', 131213, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (11020002, N'قلعه شور', 139011, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (11030001, N'امیدیه', 131213, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (11040001, N'دشت آزادگان', 131213, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (11040002, N'كبجوان', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (11050001, N'تل آباد', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (11060001, N'سهرویه', 131211, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (11060002, N'كيچي كراج', 139011, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (11070001, N'خرم آباد', 131213, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (11090002, N'شهرك فجر', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (11100002, N'شهرك سرو', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (11120002, N'كوي راه حق', 139011, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (11130002, N'حيدرآبادكراج', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (11590002, N'اشكاوند', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (11600002, N'قلعه چوم', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (11610002, N'اصفهانك', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (11630002, N'چيريان', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (11640002, N'زردنجان', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (11660002, N'راشنان', 134411, 1)
GO
INSERT [LocationPool].[Municipality] ([Id], [Title], [ZoneId], [IsVillage]) VALUES (11680002, N'هفشوئيه', 134311, 1)
GO