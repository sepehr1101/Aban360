USE [Aban360]
GO
INSERT [ClaimPool].[Estate] ( [ConstructionTypeId], EstateBoundTypeId, [PostalCode], [X], [Y], [Parcel], [Address], [MunipulityId], [UsageSellId], [UsageConsumtionId], [UnitDomesticWater], [UnitCommercialWater], [UnitOtherWater], [UnitDomesticSewage], [UnitCommercialSewage], [UnitOtherSewage], [EmptyUnit], [HouseholdNumber], [Premises], [ImprovementsOverall], [ImprovementsDomestic], [ImprovementsCommercial], [ImprovementsOther], [ContractualCapacity], [Storeys], [UserId], [PreviousId], [ValidFrom], [ValidTo], [InsertLogInfo], [RemoveLogInfo], [Hash]) VALUES ( 1,1, N'1234567890', N'32.65', N'56.22', NULL, N'اصفهان آدرس آزمایشی', 11610002, 1, 1, 1, 1, 2, 1, 2, 1, 5, 2, 350, 250, 100, 20, 300, 2, 3, N'9eaaaa8c-4c95-4678-911c-048a035d877f', NULL, CAST(N'2025-02-23T13:22:38.2070000' AS DateTime2), NULL, N' ', NULL, N'')
GO
INSERT [ClaimPool].[Flat] ( [EstateId], [PostalCode], [Storey], [Description]) VALUES ( 2, N'1234567891', 2, N'جهت تست')
GO
INSERT [ClaimPool].[Individual] ( [IndividualTypeId], [FullName], [NationalId], [FatherName], [PhoneNumbers], [MobileNumbers], [UserId], [PreviousId], [ValidFrom], [ValidTo], [InsertLogInfo], [RemoveLogInfo], [Hash]) VALUES ( 1, N'تست تستیان پور', N'1234567890', N'ابوتست', N'0314567891', N'09135742557', N'9eaaaa8c-4c95-4678-911c-048a035d877f', NULL, CAST(N'2025-02-23T13:24:15.1670000' AS DateTime2), NULL, N'', NULL, N'')
GO
INSERT [ClaimPool].[IndividualEstate] ( [IndividualId], [EstateId], [IndividualEstateRelationTypeId]) VALUES ( 1, 2, 1)
GO
INSERT [ClaimPool].[IndividualEstate] ( [IndividualId], [EstateId], [IndividualEstateRelationTypeId]) VALUES (1, 2, 2)
GO
INSERT [ClaimPool].[IndividualEstate] ( [IndividualId], [EstateId], [IndividualEstateRelationTypeId]) VALUES (1, 2, 3)
GO
INSERT [ClaimPool].[IndividualEstate] ( [IndividualId], [EstateId], [IndividualEstateRelationTypeId]) VALUES (1, 2, 1)
GO
INSERT [ClaimPool].[WaterMeter] ( [ReadingNumber], [CustomerNumber], [BillId], [EstateId], [UseStateId], [SubscriptionTypeId], [InstallationLocation], [BodySerial], [InstallationDate], [ProductDate], [GuaranteeDate], [MeterDiameterId], [MeterProducerId], [MeterTypeId], [MeterMaterialId], [MeterUseTypeId], [ParentId], [UserId], [PreviousId], [ValidFrom], [ValidTo], [InsertLogInfo], [RemoveLogInfo], [Hash]) VALUES (N'10025698', 1455669, N'100189916315', 2, 1, 1, N'درب اصلی', N'123456', N'1400/01/01', N'1399/01/01', N'1409/01/01', 1, 1, 1, 1, 1, NULL, N'9eaaaa8c-4c95-4678-911c-048a035d877f', NULL, CAST(N'2025-02-23T13:31:00.6170000' AS DateTime2), NULL, N'', NULL, N'')
GO
INSERT [ClaimPool].[IndividualTagDefinition] ( [Title], [Color]) VALUES (N'بدحساب', N'red')
GO
INSERT [ClaimPool].[IndividualTagDefinition] ( [Title], [Color]) VALUES (N'برچسب اول', NULL)
GO
INSERT [ClaimPool].[IndividualTagDefinition] ( [Title], [Color]) VALUES (N'برچسب دوم', N' ')
GO
INSERT [ClaimPool].[IndividualTag] ( [IndividualId], [IndividualTagDefinitionId], [Value], [ValidFrom], [ValidTo], [InsertLogInfo], [RemoveLogInfo], [Hash]) VALUES (1, 1, N'1', CAST(N'2025-02-23T13:24:15.1670000' AS DateTime2), NULL, N'', NULL, N'')
GO
INSERT [ClaimPool].[IndividualTag] ( [IndividualId], [IndividualTagDefinitionId], [Value], [ValidFrom], [ValidTo], [InsertLogInfo], [RemoveLogInfo], [Hash]) VALUES (1, 2, N'True', CAST(N'2025-02-23T13:24:15.1670000' AS DateTime2), NULL, N'', NULL, N'')
GO
INSERT [ClaimPool].[WaterMeterTagDefinition] ( [Title], [Color]) VALUES (N'برچسب سوم', NULL)
GO
INSERT [ClaimPool].[WaterMeterTagDefinition] ( [Title], [Color]) VALUES (N'برجسب چهارم', NULL)
GO
INSERT [ClaimPool].[Siphon] ( [InstallationLocation], [InstallationDate], [SiphonDiameterId], [SiphonTypeId], [SiphonMaterialId], [UserId], [PreviousId], [ValidFrom], [ValidTo], [InsertLogInfo], [RemoveLogInfo], [Hash]) VALUES (N'درب پشتی', N'1401/05/01', 1, 1, 1, N'9eaaaa8c-4c95-4678-911c-048a035d877f', NULL, CAST(N'2025-02-23T13:31:00.6170000' AS DateTime2), NULL, N'', NULL, N'')
GO
INSERT [ClaimPool].[Siphon] ( [InstallationLocation], [InstallationDate], [SiphonDiameterId], [SiphonTypeId], [SiphonMaterialId], [UserId], [PreviousId], [ValidFrom], [ValidTo], [InsertLogInfo], [RemoveLogInfo], [Hash]) VALUES (N'وسط حیاط', N'1402/01/01', 1, 1, 1, N'9eaaaa8c-4c95-4678-911c-048a035d877f', NULL, CAST(N'2025-02-23T13:31:00.6170000' AS DateTime2), NULL, N'', NULL, N'')
GO
INSERT [ClaimPool].[WaterMeterSiphon] ( [WaterMeterId], [SiphonId]) VALUES (1, 1)
GO
INSERT [ClaimPool].[WaterMeterSiphon] ( [WaterMeterId], [SiphonId]) VALUES (1, 2)
GO