USE [Aban360]
GO
SET IDENTITY_INSERT [MeterPool].[CounterState] ON 
GO
INSERT [MeterPool].[CounterState] (  [Title], [ClientOrder], [EnterNumberOption], [NonReadable], [NumberLessThanPre], [IsChanged], [NumberRequired], [IsBroken], [IsNull], [IsEnabled], [ImageRequired], [HeadquartersId], [HeadquartersTitle]) VALUES ( N'عادی', 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 1, N'اصفهان')
GO
INSERT [MeterPool].[CounterState] (  [Title], [ClientOrder], [EnterNumberOption], [NonReadable], [NumberLessThanPre], [IsChanged], [NumberRequired], [IsBroken], [IsNull], [IsEnabled], [ImageRequired], [HeadquartersId], [HeadquartersTitle]) VALUES (N'کنتور خراب', 1, 0, 0, 0, 0, 1, 1, 0, 1, 1, 1, N'اصفهان')
GO
INSERT [MeterPool].[CounterState] (  [Title], [ClientOrder], [EnterNumberOption], [NonReadable], [NumberLessThanPre], [IsChanged], [NumberRequired], [IsBroken], [IsNull], [IsEnabled], [ImageRequired], [HeadquartersId], [HeadquartersTitle]) VALUES (N'معکوس', 2, 0, 0, 1, 0, 1, 0, 0, 1, 0, 1, N'اصفهان')
GO
INSERT [MeterPool].[CounterState] (  [Title], [ClientOrder], [EnterNumberOption], [NonReadable], [NumberLessThanPre], [IsChanged], [NumberRequired], [IsBroken], [IsNull], [IsEnabled], [ImageRequired], [HeadquartersId], [HeadquartersTitle]) VALUES (N'بسته', 3, 0, 1, 0, 0, 0, 0, 0, 1, 0, 1, N'اصفهان')
GO
INSERT [MeterPool].[CounterState] (  [Title], [ClientOrder], [EnterNumberOption], [NonReadable], [NumberLessThanPre], [IsChanged], [NumberRequired], [IsBroken], [IsNull], [IsEnabled], [ImageRequired], [HeadquartersId], [HeadquartersTitle]) VALUES (N'دور مجدد', 4, 0, 0, 1, 0, 1, 0, 0, 1, 0, 1, N'اصفهان')
GO
INSERT [MeterPool].[CounterState] (  [Title], [ClientOrder], [EnterNumberOption], [NonReadable], [NumberLessThanPre], [IsChanged], [NumberRequired], [IsBroken], [IsNull], [IsEnabled], [ImageRequired], [HeadquartersId], [HeadquartersTitle]) VALUES (N'واحد متروکه', 5, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, N'اصفهان')
GO
INSERT [MeterPool].[CounterState] (  [Title], [ClientOrder], [EnterNumberOption], [NonReadable], [NumberLessThanPre], [IsChanged], [NumberRequired], [IsBroken], [IsNull], [IsEnabled], [ImageRequired], [HeadquartersId], [HeadquartersTitle]) VALUES (N'انشعاب قطع شده', 6, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, N'اصفهان')
GO
INSERT [MeterPool].[CounterState] (  [Title], [ClientOrder], [EnterNumberOption], [NonReadable], [NumberLessThanPre], [IsChanged], [NumberRequired], [IsBroken], [IsNull], [IsEnabled], [ImageRequired], [HeadquartersId], [HeadquartersTitle]) VALUES ( N'تعویض', 7, 0, 0, 1, 1, 1, 0, 0, 1, 1, 1, N'اصفهان')
GO
INSERT [MeterPool].[CounterState] (  [Title], [ClientOrder], [EnterNumberOption], [NonReadable], [NumberLessThanPre], [IsChanged], [NumberRequired], [IsBroken], [IsNull], [IsEnabled], [ImageRequired], [HeadquartersId], [HeadquartersTitle]) VALUES (N'عدم قرائت', 8, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, N'اصفهان')
GO
INSERT [MeterPool].[CounterState] (  [Title], [ClientOrder], [EnterNumberOption], [NonReadable], [NumberLessThanPre], [IsChanged], [NumberRequired], [IsBroken], [IsNull], [IsEnabled], [ImageRequired], [HeadquartersId], [HeadquartersTitle]) VALUES (N'مانع', 9, 0, 1, 0, 0, 0, 0, 0, 1, 0, 1, N'اصفهان')
GO
INSERT [MeterPool].[CounterState] (  [Title], [ClientOrder], [EnterNumberOption], [NonReadable], [NumberLessThanPre], [IsChanged], [NumberRequired], [IsBroken], [IsNull], [IsEnabled], [ImageRequired], [HeadquartersId], [HeadquartersTitle]) VALUES (N'بدون مصرف', 10, 0, 0, 0, 0, 1, 0, 0, 1, 1, 1, N'اصفهان')
GO
INSERT [MeterPool].[CounterState] (  [Title], [ClientOrder], [EnterNumberOption], [NonReadable], [NumberLessThanPre], [IsChanged], [NumberRequired], [IsBroken], [IsNull], [IsEnabled], [ImageRequired], [HeadquartersId], [HeadquartersTitle]) VALUES (N'کمتر از قبلی', 11, 0, 0, 1, 0, 1, 0, 0, 1, 0, 1, N'اصفهان')
GO
INSERT [MeterPool].[CounterState] (  [Title], [ClientOrder], [EnterNumberOption], [NonReadable], [NumberLessThanPre], [IsChanged], [NumberRequired], [IsBroken], [IsNull], [IsEnabled], [ImageRequired], [HeadquartersId], [HeadquartersTitle]) VALUES (N'انشعاب قطع شده', 6, 0, 0, 0, 0, 1, 0, 0, 1, 0, 1, N'اصفهان')
GO
SET IDENTITY_INSERT [MeterPool].[CounterState] OFF 
GO
SET IDENTITY_INSERT [MeterPool].[ReadingConfigDefault] ON 
GO
INSERT [MeterPool].[ReadingConfigDefault] (  [NonReadDefault], [NonReadMax], [NonReadMin], [PreNumberDisplayOption], [BillIdDisplayOption], [CustomerNumberDisplayOption], [DomesticLowConstBound], [DomesticLowPercentBound], [DomesticHighConstBound], [DomesticHighPercentBound], [ConstructionLowConstBound], [ConstructionLowPercentBound], [ConstructionHighConstBound], [ConstructionHighPercentBound], [ContractualCapacityLowConstBound], [ContractualCapacityLowPercentBound], [ContractualCapacityHighConstBound], [ContractualCapacityHighPercentBound], [NonDomesticLowPercentRateBound], [NonDomesticHighPercentRateBound], [IsEnabled], [PreDateDisplayOption], [MobileDisplayOption], [DebtDisplayOption], [IconsDisplayOption], [HeadquartersId], [HeadquartersTitle]) VALUES ( 5, 30, 2, 0, 0, 1, 5, 15, 50, 25, 17, 20, 50, 32, 2, 18, 22, 25, 20, 25, 0, 0, 0, 0, 1, 1, N'اصفهان')
GO
INSERT [MeterPool].[ReadingConfigDefault] (  [NonReadDefault], [NonReadMax], [NonReadMin], [PreNumberDisplayOption], [BillIdDisplayOption], [CustomerNumberDisplayOption], [DomesticLowConstBound], [DomesticLowPercentBound], [DomesticHighConstBound], [DomesticHighPercentBound], [ConstructionLowConstBound], [ConstructionLowPercentBound], [ConstructionHighConstBound], [ConstructionHighPercentBound], [ContractualCapacityLowConstBound], [ContractualCapacityLowPercentBound], [ContractualCapacityHighConstBound], [ContractualCapacityHighPercentBound], [NonDomesticLowPercentRateBound], [NonDomesticHighPercentRateBound], [IsEnabled], [PreDateDisplayOption], [MobileDisplayOption], [DebtDisplayOption], [IconsDisplayOption], [HeadquartersId], [HeadquartersTitle]) VALUES (5, 30, 2, 0, 0, 1, 5, 15, 50, 25, 17, 20, 50, 32, 2, 18, 22, 25, 20, 25, 1, 0, 0, 0, 1, 1, N'اصفهان')
GO
SET IDENTITY_INSERT [MeterPool].[ReadingConfigDefault] OFF 
GO
SET IDENTITY_INSERT [MeterPool].[ReadingPeriodType] ON 
GO
INSERT [MeterPool].[ReadingPeriodType] (  [Title], [Days], [ClientOrder], [IsEnabled], [HeadquartersId], [HeadquartersTitle]) VALUES (N'چهل و پنج روزه', 45, 1, 1, 1, N'اصفهان')
GO
INSERT [MeterPool].[ReadingPeriodType] (  [Title], [Days], [ClientOrder], [IsEnabled], [HeadquartersId], [HeadquartersTitle]) VALUES (N'شش ماهه', 180, 1, 0, 1, N'اصفهان')
GO
INSERT [MeterPool].[ReadingPeriodType] (  [Title], [Days], [ClientOrder], [IsEnabled], [HeadquartersId], [HeadquartersTitle]) VALUES (N'چهار ماهه', 120, 1, 0, 1, N'اصفهان')
GO
INSERT [MeterPool].[ReadingPeriodType] (  [Title], [Days], [ClientOrder], [IsEnabled], [HeadquartersId], [HeadquartersTitle]) VALUES (N'سه ماهه', 90, 1, 0, 1, N'اصفهان')
GO
INSERT [MeterPool].[ReadingPeriodType] (  [Title], [Days], [ClientOrder], [IsEnabled], [HeadquartersId], [HeadquartersTitle]) VALUES (N'دو ماهه', 60, 1, 0, 1, N'اصفهان')
GO
INSERT [MeterPool].[ReadingPeriodType] (  [Title], [Days], [ClientOrder], [IsEnabled], [HeadquartersId], [HeadquartersTitle]) VALUES (N'یک ماهه', 30, 1, 1, 1, N'اصفهان')
GO
INSERT [MeterPool].[ReadingPeriodType] (  [Title], [Days], [ClientOrder], [IsEnabled], [HeadquartersId], [HeadquartersTitle]) VALUES (N'test', 7, 21, 1, 1, N'اصفهان')
GO
INSERT [MeterPool].[ReadingPeriodType] (  [Title], [Days], [ClientOrder], [IsEnabled], [HeadquartersId], [HeadquartersTitle]) VALUES (N'test23', 23, 21, 1, 1, N'اصفهان')
GO
INSERT [MeterPool].[ReadingPeriodType] (  [Title], [Days], [ClientOrder], [IsEnabled], [HeadquartersId], [HeadquartersTitle]) VALUES (N'یک ماهه', 30, 1, 0, 1, N'اصفهان')
GO
INSERT [MeterPool].[ReadingPeriodType] (  [Title], [Days], [ClientOrder], [IsEnabled], [HeadquartersId], [HeadquartersTitle]) VALUES (N'چهل و پنج روزه', 46, 1, 1, 1, N'اصفهان')
GO
INSERT [MeterPool].[ReadingPeriodType] (  [Title], [Days], [ClientOrder], [IsEnabled], [HeadquartersId], [HeadquartersTitle]) VALUES (N'چهل و پنج روزه', 45, 1, 1, 1, N'اصفهان')
GO
INSERT [MeterPool].[ReadingPeriodType] (  [Title], [Days], [ClientOrder], [IsEnabled], [HeadquartersId], [HeadquartersTitle]) VALUES (N'test', 1212, 1, 1, 1, N'اصفهان')
GO
INSERT [MeterPool].[ReadingPeriodType] (  [Title], [Days], [ClientOrder], [IsEnabled], [HeadquartersId], [HeadquartersTitle]) VALUES (N'چهل روزه', 40, 1, 1, 1, N'اصفهان')
GO
INSERT [MeterPool].[ReadingPeriodType] (  [Title], [Days], [ClientOrder], [IsEnabled], [HeadquartersId], [HeadquartersTitle]) VALUES (N'چهل و پنج روزه', 45, 1, 0, 1, N'اصفهان')
GO
SET IDENTITY_INSERT [MeterPool].[ReadingPeriodType] OFF 
GO
SET IDENTITY_INSERT [MeterPool].[ReadingPeriod] ON 
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'فروردين', 6, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'ارديبهشت ', 6, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'خرداد', 6, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'تير', 6, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'مرداد', 6, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'شهريور', 6, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'مهر', 6, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'آبان', 6, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'آذر', 6, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'دي ', 6, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'بهمن ', 6, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'اسفند', 6, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'اول', 5, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'دوم ', 5, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'سوم ', 5, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'چهارم ', 5, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES ( N'پنجم ', 5, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'ششم ', 5, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'بهار', 4, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'تابستان ', 4, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'پاييز', 4, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'زمستان ', 4, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'اول ', 3, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'دوم ', 3, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'سوم ', 3, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'شش ماهه اول ', 2, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'شش ماهه دوم ', 2, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'اول', 1, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'دوم', 1, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'سوم', 1, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'چهارم', 1, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'پنجم', 1, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'ششم', 1, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'هفتم', 1, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'هشتم', 1, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'فروردين', 9, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'آذر', 9, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'آبان', 9, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'اسفند', 9, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'دي ', 9, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES ( N'بهمن ', 9, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'مهر', 9, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'خرداد', 9, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'ارديبهشت ', 9, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'تير', 9, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'شهريور', 9, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'مرداد', 9, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'ششم', 10, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'پنجم', 10, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'هفتم', 10, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'هشتم', 10, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'دوم', 10, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'اول', 10, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'چهارم', 10, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'سوم', 10, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'اول', 11, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'دوم', 11, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'سوم', 11, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'چهارم', 11, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'ششم', 11, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'پنجم', 11, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'هفتم', 11, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'هشتم', 11, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'test', 3, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'ششم', 14, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'پنجم', 14, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'هشتم', 14, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'هفتم', 14, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'دوم', 14, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'اول', 14, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'چهارم', 14, 1)
GO
INSERT [MeterPool].[ReadingPeriod] (  [Title], [ReadingPeriodTypeId], [ClientOrder]) VALUES (N'سوم', 14, 1)
GO
SET IDENTITY_INSERT [MeterPool].[ReadingPeriod] OFF 
GO