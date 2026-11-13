# Application service consolidation: full review

Reviewed on 2026-11-13, covering 113 capabilities: the first 93 plus the latest 20 groups. There are now 524 consolidated operations. DynamicReport remains explicitly excluded and unchanged.

The first 70 capabilities covered 304 moved operations: 301 method bodies matched their previous implementations after normalizing injected field names, local variable names and whitespace. Three intentional corrections await removal in RequestFlat.DeleteAsync, RequestIndividualTag.DeleteAsync and RequestWaterMeterTag.DeleteAsync. The additional 20 ticketing operations also matched after validator field renaming and three internal delegation rewrites: my-list calls GetListAsync, my-report calls GetReportAsync, and attachment content calls GetSingleAsync. Two obsolete handler dependencies were consequently removed, without introducing self-injection. Return types, parameters, async signatures, operation counts and remaining dependency unions were checked. This structural check supplements behavioral tests; it does not prove SQL or database correctness.

The first five capabilities were compared with the original handler files in Git commit 5611ab16e05df03a4fbe6022a2e53c5eaa8253a7. The remaining 65 use frozen test snapshots in OriginalHandlers15, OriginalHandlers30 and OriginalHandlers20. Original controller/gateway/hub consumers remain available in the corresponding snapshot directories. Obsolete interfaces are test-only.

## Intentional correction

The original delete handlers for RequestFlat, RequestIndividualTag and RequestWaterMeterTag called the Task-returning Remove without awaiting it. Their consolidated services now await removal before returning to the controller. Controllers keep their existing save boundary. Focused tests execute each production controller with its actual application service and isolated persistence: save waits for removal; removal failure and cancellation prevent save. Frozen old code retains the missing awaits so the comparison explicitly verifies these corrections instead of rewriting history. RequestEstate and RequestIndividualEstate still call their synchronous void-returning Remove methods without await.

## Latest batch scope

The latest 20 services contain 92 operations and update 20 consumer classes (19 main API controllers and one Bridge controller). CompanyService includes its existing GetByTypeId query. Credit covers four basic read/update/delete operations; CreditByDocumentCreateHandler and CreditWithoutDocumentCreateHandler remain separate specialized creation workflows. Validators, domain models, DTOs and persistence APIs remain in their existing layers.

Consumer selection was checked against full contract namespaces: OldCalcPool Offering in Bridge ServiceLink and CalculationPool SiphonDiameter are distinct from the services in this batch and remain untouched. ReadingBlock's misplaced Create implementation and Offering's GetAll interface in a file ending in _1 were included by declaration rather than folder/filename assumptions.

## Ticketing batch

Nineteen original handler implementations and nineteen contracts were consolidated into four files containing public application interfaces and internal implementations: Ticket (7 operations, including report/my-report), TicketMessage (4), TicketRating (4) and TicketAttachment (5). Their four controllers now inject one application service, one existing unit of work where required, and the notification service where used. Save and notification sequencing stays in controllers. Access services, validators, mapper profiles, domain entities, DTOs and persistence implementations remain unchanged.

Frozen handlers, controller snapshots and test-only contracts allow the same application and HTTP checks to run against both old and consolidated implementations. Test adapters forward old test contracts to actual new services; they are not production compatibility layers. Application service signatures and scoped registrations are explicitly checked. Existing HTTP tests were updated to select the merged controllers rather than a controller class removed in the earlier controller consolidation.

The focused ticketing suite passes 621 assertions: 38 persistence checks, 93 application checks for each implementation, 185 HTTP checks for each implementation, and 27 additional asynchronous boundary checks. Coverage includes ownership/staff/admin access, list/report scoping, initial message/file graph staging, ticket/request numbers, multipart uploads, file metadata and authorized downloads, sender-only edits, closed/reopened tickets, duplicate and invalid ratings, pending file limits, and notification failures after commit. Boundary checks verify that CreateAsync awaits command completion and propagates errors/cancellation, and file bytes are loaded only after metadata authorization completes; rejected/cancelled access never requests file content. EF InMemory and TestServer are used; SQL Server and IIS are not exercised.

## Latest 19 groups

All 93 original handler bodies match the consolidated operations after validator field renaming and whitespace normalization. Return types, parameters, async signatures, per-service method counts and dependency unions also matched. Nineteen main API consumers were updated, with their original code frozen for method, metadata and behavior comparison. CounterState selection uses the MeterPool alias; its OldCalcPool and ReportPool dependencies remain separate. ReportPool helpers used by Individual and Guild also remain unchanged. The misspelled ReadingBounde controller name and all route/action metadata were retained. Stale imports in previously consolidated controllers were removed where deleting the last legacy handler made those namespaces cease to exist.

The batch includes TariffConstant's dictionary query, RequestTracking's water-meter query, DocumentCategory's bill-ID query and all seven ReadingBound operations. An old block comment containing unrelated interface declarations in WaterMeterTagDefinition's single-read contract is retained only in the frozen test copy; the actual implemented interface was used for the production refactor.

Additional tests compare seven ReadingBound validation cases (empty/blank blocks, unequal number lengths, start mismatch, gap, nonnumeric input and reversed ordering), including the resulting IsValid flag. Instrumented uploaded files participate in differential call traces so copy completion, errors and cancellation are checked before persistence. Existing timestamp assignments in Siphon and the draft request entities are normalized only when fresh; fixed timestamps remain exact.

Three additional pre-existing defects were reproduced during consolidation: a missing RequestTracking update map and two document icon update maps that overwrite base64. The explicitly requested follow-up correction adds RequestTrackingUpdateDto-to-RequestTracking mapping and ignores Icon in the DocumentCategoryUpdateDto and DocumentTypeUpdateDto maps. The application services retain responsibility for reading uploaded bytes and assigning base64. Other editable fields still map normally. No service body, persistence implementation or save boundary was changed by these three corrections.

OriginalMappings freezes the three pre-fix mapper profiles. MappingCorrectionChecks reproduces their old failures, then exercises the corrected production profiles and real application services: tracking ID/meter/status updates preserve ownership/navigation; icon updates preserve exact uploaded bytes as base64 while changing title/CSS/category; all three query their entity once and leave committing to their caller. The main handler comparisons intentionally use the current production mapper on both sides; historical mapper behavior is checked separately with the frozen profiles. Existing copy failure/cancellation/await and validator rejection checks remain active.

## Access tree, Zone and latest 20 groups

The latest batch replaces 106 handlers and their contracts with 20 services containing 107 operations. ServerReports includes the existing Fail method as FailAsync. Twenty API controllers, three Zone gateways and the ReportGenerator background pipeline were migrated. The 23 controllers/gateways have frozen consumer snapshots; ReportGenerator's original text is retained separately and its behavior is exercised by ReportGeneration.SmokeTests, which is now part of the standard wrapper.

All 107 operations were rechecked after implementation. Of these, 103 retain identical bodies after injected field renaming and indentation normalization. Three MainTagGroup commands preserve their body and SQL sequence while obtaining connections through an immutable factory: the public constructor still uses the original SqlConnection property, while an internal constructor permits a recording provider in tests. The remaining intentional correction reverses the erroneous SubscriptionType update mapping to map the input DTO into the queried entity. The frozen original handler still reproduces its old AutoMapper failure; focused tests verify that the corrected service changes the tracked title and preserves the caller's input DTO. Return types, parameter types/names and async signatures were compared for every operation.

The critical-controller suite continues to compare real JWT validation, binding and action behavior before/after. ApplicationServiceBaseline now translates service method calls to the original handler identities in that suite, retaining its original assertions. The focused access-tree/Zone suite adds 34 checks for nested child projections, parent titles/IDs, inactive node membership, menu/order flags, exact AuthValue values, module description formatting without tracked mutations, and all three gateways using the actual Zone service, including pending count queries and duplicate IDs.

MainTagGroup adds 258 checks using the actual production Dapper command services and a recording ADO.NET provider. These verify SQL target tables and parameters, related-group IDs, ReadUncommitted isolation, deletion ordering, pending commands before commit, failure/cancellation at each command, negative affected-row results, open/begin/commit failures, already-open connections and disposal. A failed/pending related-group lookup is checked before opening the connection. These tests do not execute SQL Server or establish SQL syntax/schema correctness on a live database.

Existing routes, permission metadata, DTOs, entities, validators, persistence implementations and the protected TrackingFlowGetHandler were not modified by this batch. Namespace inconsistencies in old Module/Workflow files are retained in frozen snapshots; new service contracts use their canonical feature namespace. The 745-action contract fixture was not recaptured.

## Existing defects found during review

| Capability | Operation | Existing mismatch |
|---|---|---|
| WaterMeterTag | GetSingleAsync | One entity is mapped directly to ICollection<WaterMeterTagGetDto>. |
| WaterMeterTag | GetBySearchAsync | A collection is mapped directly to one WaterMeterTagGetDto. |
| IndividualTag | GetSingleAsync | One entity is mapped directly to ICollection<IndividualTagGetDto>. |
| IndividualTag | GetBySearchAsync | A collection is mapped directly to one IndividualTagGetDto. |
| RequestTracking | UpdateAsync | Corrected: added the missing RequestTrackingUpdateDto-to-RequestTracking map. |
| DocumentCategory | UpdateAsync | Corrected: Icon is ignored by DTO mapping, preserving the service's base64 conversion. |
| DocumentType | UpdateAsync | Corrected: Icon is ignored by DTO mapping, preserving the service's base64 conversion. |
| SubscriptionType | UpdateAsync | Corrected: map updateDto into the queried SubscriptionType instead of mapping the entity back into the input. |

The four tag lookup defects remain explicitly printed as KNOWN LEGACY DEFECTS and are not presented as successful functional scenarios. Their expected scalar/collection responses and handling of multiple matches require a separate API decision. The three requested mapping corrections are verified separately against frozen pre-fix profiles and current production profiles/services.

## Validation

The final combined run passed all 13 suites, including OnlineUsers, before/after ticketing and the background report generation pipeline. Results are in artifacts/test-results/20261113-233958-f6cb51e3/summary.json. API and Bridge builds completed successfully. The ticketing suite adds 621 assertions. Twenty-one focused mapping correction checks reproduce four old defects (including the SubscriptionType reverse-map error) and verify the corrected production services. The critical-controller suite passed 1,639 checks; focused tree/Zone checks passed 34 assertions; Dapper transaction checks passed 258 assertions. The final source audit is saved beside the suite summary as source-review-wave20.json. The broad suite passed 36,871 assertions, including 16,242 consolidation assertions across 104 services / 494 operations (491 differential methods and 3 SQL transaction methods) and 112 consumers / 498 operations, with 6,269 before/after scenarios; the first five services have 375 separate lookup assertions. The 745-action HTTP contract fixture was not recaptured. Production validators remain in place; behavioral tests exercise valid/rejected validator results, failure, cancellation, dependency order and pending tasks. Existing wall-clock ValidFrom assignments are compared as current timestamps; deterministic dates remain exact.

Run the checks with:

```powershell
./scripts/test-merged-controllers.ps1
```

The suites isolate persistence and external services. They do not execute production SQL, send SMS, start real background jobs or validate an IIS deployment. The protected TrackingFlowGetHandler, domain/DTO definitions and persistence implementations were not edited by this consolidation.

## Capability checklist

| Capability | Operations | Review |
|---|---:|---|
| [AccountType](../../Aban360.PaymentPool.Application/Features/NegotiableInstrument/Services/AccountTypeService.cs) | 2 | Body, return type, operation count and dependency union matched. |
| [Bank](../../Aban360.PaymentPool.Application/Features/Remuneration/Services/BankService.cs) | 2 | Body, return type, operation count and dependency union matched. |
| [BankFileStructure](../../Aban360.PaymentPool.Application/Features/NegotiableInstrument/Services/BankFileStructureService.cs) | 5 | Body, return type, operation count and dependency union matched. |
| [CapacityCalculationIndex](../../Aban360.ClaimPool.Application/Features/Land/Services/CapacityCalculationIndexService.cs) | 5 | Body, return type, operation count and dependency union matched. |
| [ChangeMeterReason](../../Aban360.ClaimPool.Application/Features/Metering/Services/ChangeMeterReasonService.cs) | 2 | Body, return type, operation count and dependency union matched. |
| [CompanyServiceType](../../Aban360.CalculationPool.Application/Features/Bill/Services/CompanyServiceTypeService.cs) | 5 | Body, return type, operation count and dependency union matched. |
| [CordinalDirection](../../Aban360.LocationPool.Application/Features/MainHierarchy/Services/CordinalDirectionService.cs) | 5 | Body, return type, operation count and dependency union matched. |
| [Country](../../Aban360.LocationPool.Application/Features/MainHierarchy/Services/CountryService.cs) | 5 | Body, return type, operation count and dependency union matched. |
| [CreditorType](../../Aban360.PaymentPool.Application/Features/NegotiableInstrument/Services/CreditorTypeService.cs) | 2 | Body, return type, operation count and dependency union matched. |
| [DiscountType](../../Aban360.ClaimPool.Application/Features/People/Services/DiscountTypeService.cs) | 2 | Body, return type, operation count and dependency union matched. |
| [DocumentEntity](../../Aban360.BlobPool.Application/Features/DMS/Services/DocumentEntityService.cs) | 6 | Body, return type, operation count and dependency union matched. |
| [EquipmentBroker](../../Aban360.InstallationPool.Application/Features/Definition/Services/EquipmentBrokerService.cs) | 5 | Body, return type, operation count and dependency union matched. |
| [ExecutableMimetype](../../Aban360.BlobPool.Application/Features/Taxonomy/Services/ExecutableMimetypeService.cs) | 5 | Body, return type, operation count and dependency union matched. |
| [Gateway](../../Aban360.ClaimPool.Application/Features/Request/Services/GatewayService.cs) | 5 | Body, return type, operation count and dependency union matched. |
| [Handover](../../Aban360.ClaimPool.Application/Features/Land/Services/HandoverService.cs) | 5 | Body, return type, operation count and dependency union matched. |
| [Headquarter](../../Aban360.LocationPool.Application/Features/MainHierarchy/Services/HeadquarterService.cs) | 5 | Body, return type, operation count and dependency union matched. |
| [HubEvent](../../Aban360.CommunicationPool.Application/Features/Hubs/Services/HubEventService.cs) | 2 | Body, return type, operation count and dependency union matched. |
| [IndividualEstateRelationType](../../Aban360.ClaimPool.Application/Features/People/Services/IndividualEstateRelationTypeService.cs) | 5 | Body, return type, operation count and dependency union matched. |
| [IndividualTag](../../Aban360.ClaimPool.Application/Features/People/Services/IndividualTagService.cs) | 5 | Bodies preserved; two existing lookup mapping defects reproduced. |
| [IndividualType](../../Aban360.ClaimPool.Application/Features/People/Services/IndividualTypeService.cs) | 5 | Body, return type, operation count and dependency union matched. |
| [InvoiceLineItemInsertMode](../../Aban360.CalculationPool.Application/Features/Bill/Services/InvoiceLineItemInsertModeService.cs) | 5 | Body, return type, operation count and dependency union matched. |
| [MimetypeCategory](../../Aban360.BlobPool.Application/Features/Taxonomy/Services/MimetypeCategoryService.cs) | 5 | Body, return type, operation count and dependency union matched. |
| [Municipality](../../Aban360.LocationPool.Application/Features/MainHierarchy/Services/MunicipalityService.cs) | 6 | Body, return type, operation count and dependency union matched. |
| [OfferingGroup](../../Aban360.CalculationPool.Application/Features/Bill/Services/OfferingGroupService.cs) | 5 | Body, return type, operation count and dependency union matched. |
| [OfferingUnit](../../Aban360.CalculationPool.Application/Features/Bill/Services/OfferingUnitService.cs) | 5 | Body, return type, operation count and dependency union matched. |
| [OfficialHoliday](../../Aban360.ClaimPool.Application/Features/Land/Services/OfficialHolidayService.cs) | 5 | Body, return type, operation count and dependency union matched. |
| [PaymentMethod](../../Aban360.PaymentPool.Application/Features/Remuneration/Services/PaymentMethodService.cs) | 2 | Body, return type, operation count and dependency union matched. |
| [Province](../../Aban360.LocationPool.Application/Features/MainHierarchy/Services/ProvinceService.cs) | 5 | Body, return type, operation count and dependency union matched. |
| [ReadingConfigDefault](../../Aban360.MeterPool.Application/Features/Management/Services/ReadingConfigDefaultService.cs) | 5 | Body, return type, operation count and dependency union matched. |
| [ReadingPeriod](../../Aban360.MeterPool.Application/Features/Management/Services/ReadingPeriodService.cs) | 5 | Body, return type, operation count and dependency union matched. |
| [ReadingPeriodType](../../Aban360.MeterPool.Application/Features/Management/Services/ReadingPeriodTypeService.cs) | 5 | Body, return type, operation count and dependency union matched. |
| [Region](../../Aban360.LocationPool.Application/Features/MainHierarchy/Services/RegionService.cs) | 6 | Body, return type, operation count and dependency union matched. |
| [RequestFlat](../../Aban360.ClaimPool.Application/Features/Draft/Services/RequestFlatService.cs) | 3 | Intentional missing-await correction; real controller/service pipeline tested. |
| [RequestUser](../../Aban360.ClaimPool.Application/Features/Draft/Services/RequestUserService.cs) | 2 | Body, return type, operation count and dependency union matched. |
| [RequestWaterMeterSiphon](../../Aban360.ClaimPool.Application/Features/Draft/Services/RequestWaterMeterSiphonService.cs) | 3 | Body, return type, operation count and dependency union matched. |
| [SewageEquipmentBroker](../../Aban360.InstallationPool.Application/Features/Definition/Services/SewageEquipmentBrokerService.cs) | 5 | Body, return type, operation count and dependency union matched. |
| [SiphonMaterial](../../Aban360.ClaimPool.Application/Features/WasteWater/Services/SiphonMaterialService.cs) | 5 | Body, return type, operation count and dependency union matched. |
| [SiphonType](../../Aban360.ClaimPool.Application/Features/WasteWater/Services/SiphonTypeService.cs) | 5 | Body, return type, operation count and dependency union matched. |
| [State](../../Aban360.WorkflowPool.Application/Features/Design/Services/StateService.cs) | 4 | Body, return type, operation count and dependency union matched. |
| [SupportedField](../../Aban360.CalculationPool.Application/Features/Rule/Services/SupportedFieldService.cs) | 2 | Body, return type, operation count and dependency union matched. |
| [SupportedOperator](../../Aban360.CalculationPool.Application/Features/Rule/Services/SupportedOperatorService.cs) | 2 | Body, return type, operation count and dependency union matched. |
| [TicketStatus](../../Aban360.UserPool.Application/Features/Ticketing/Services/TicketStatusService.cs) | 2 | Body, return type, operation count and dependency union matched. |
| [Uploader](../../Aban360.PaymentPool.Application/Features/NegotiableInstrument/Services/UploaderService.cs) | 5 | Body, return type, operation count and dependency union matched. |
| [UsageLevel1](../../Aban360.ClaimPool.Application/Features/Land/Services/UsageLevel1Service.cs) | 5 | Body, return type, operation count and dependency union matched. |
| [UsageLevel2](../../Aban360.ClaimPool.Application/Features/Land/Services/UsageLevel2Service.cs) | 5 | Body, return type, operation count and dependency union matched. |
| [UsageLevel3](../../Aban360.ClaimPool.Application/Features/Land/Services/UsageLevel3Service.cs) | 5 | Body, return type, operation count and dependency union matched. |
| [UsageLevel4](../../Aban360.ClaimPool.Application/Features/Land/Services/UsageLevel4Service.cs) | 5 | Body, return type, operation count and dependency union matched. |
| [WaterMeterChangeNumberHistory](../../Aban360.CalculationPool.Application/Features/Bill/Services/WaterMeterChangeNumberHistoryService.cs) | 2 | Body, return type, operation count and dependency union matched. |
| [WaterMeterInstallationMethod](../../Aban360.ClaimPool.Application/Features/Metering/Services/WaterMeterInstallationMethodService.cs) | 5 | Body, return type, operation count and dependency union matched. |
| [WaterMeterTag](../../Aban360.ClaimPool.Application/Features/WasteWater/Services/WaterMeterTagService.cs) | 5 | Bodies preserved; two existing lookup mapping defects reproduced. |
| [Credit](../../Aban360.PaymentPool.Application/Features/NegotiableInstrument/Services/CreditService.cs) | 4 | Body, return type, operation count and dependency union matched. |
| [ReadingBlock](../../Aban360.LocationPool.Application/Features/MainHierarchy/Services/ReadingBlockService.cs) | 5 | Body, return type, operation count and dependency union matched. |
| [Offering](../../Aban360.CalculationPool.Application/Features/Bill/Services/OfferingService.cs) | 5 | Body, return type, operation count and dependency union matched. |
| [CompanyService](../../Aban360.CalculationPool.Application/Features/Bill/Services/CompanyServiceService.cs) | 6 | Body, return type, operation count and dependency union matched. |
| [InvoiceType](../../Aban360.CalculationPool.Application/Features/Bill/Services/InvoiceTypeService.cs) | 5 | Body, return type, operation count and dependency union matched. |
| [InvoiceStatus](../../Aban360.CalculationPool.Application/Features/Bill/Services/InvoiceStatusService.cs) | 5 | Body, return type, operation count and dependency union matched. |
| [LineItemType](../../Aban360.CalculationPool.Application/Features/Bill/Services/LineItemTypeService.cs) | 5 | Body, return type, operation count and dependency union matched. |
| [LineItemTypeGroup](../../Aban360.CalculationPool.Application/Features/Bill/Services/LineItemTypeGroupService.cs) | 5 | Body, return type, operation count and dependency union matched. |
| [TariffCalculationMode](../../Aban360.CalculationPool.Application/Features/Rule/Services/TariffCalculationModeService.cs) | 5 | Body, return type, operation count and dependency union matched. |
| [SiphonDiameter](../../Aban360.ClaimPool.Application/Features/WasteWater/Services/SiphonDiameterService.cs) | 5 | Body, return type, operation count and dependency union matched. |
| [UserLeave](../../Aban360.ClaimPool.Application/Features/Land/Services/UserLeaveService.cs) | 5 | Body, return type, operation count and dependency union matched. |
| [IndividualEstate](../../Aban360.ClaimPool.Application/Features/People/Services/IndividualEstateService.cs) | 5 | Body, return type, operation count and dependency union matched. |
| [IndividualDiscountType](../../Aban360.ClaimPool.Application/Features/People/Services/IndividualDiscountTypeService.cs) | 5 | Body, return type, operation count and dependency union matched. |
| [RequestIndividualDiscountType](../../Aban360.ClaimPool.Application/Features/Draft/Services/RequestIndividualDiscountTypeService.cs) | 5 | Body, return type, operation count and dependency union matched. |
| [EstateWaterResource](../../Aban360.ClaimPool.Application/Features/Land/Services/EstateWaterResourceService.cs) | 5 | Body, return type, operation count and dependency union matched. |
| [RequestIndividualEstate](../../Aban360.ClaimPool.Application/Features/Draft/Services/RequestIndividualEstateService.cs) | 3 | Body, return type, operation count and dependency union matched. |
| [RequestIndividualTag](../../Aban360.ClaimPool.Application/Features/Draft/Services/RequestIndividualTagService.cs) | 3 | Intentional missing-await correction; real controller/service pipeline tested. |
| [RequestWaterMeterTag](../../Aban360.ClaimPool.Application/Features/Draft/Services/RequestWaterMeterTagService.cs) | 3 | Intentional missing-await correction; real controller/service pipeline tested. |
| [RequestEstate](../../Aban360.ClaimPool.Application/Features/Draft/Services/RequestEstateService.cs) | 3 | Body, return type, operation count and dependency union matched. |
| [BankAccount](../../Aban360.PaymentPool.Application/Features/NegotiableInstrument/Services/BankAccountService.cs) | 5 | Body, return type, operation count and dependency union matched. |
| [Ticket](../../Aban360.UserPool.Application/Features/Ticketing/Services/TicketService.cs) | 7 | Bodies/signatures preserved; my-list/my-report delegation stays inside service; before/after application and HTTP tests. |
| [TicketMessage](../../Aban360.UserPool.Application/Features/Ticketing/Services/TicketMessageService.cs) | 4 | Bodies/signatures preserved; before/after application and HTTP tests. |
| [TicketRating](../../Aban360.UserPool.Application/Features/Ticketing/Services/TicketRatingService.cs) | 4 | Bodies/signatures preserved; before/after application and HTTP tests. |
| [TicketAttachment](../../Aban360.UserPool.Application/Features/Ticketing/Services/TicketAttachmentService.cs) | 5 | Bodies/signatures preserved; content authorization delegates inside service; before/after tests plus metadata-before-content checks. |
| [CounterState](../../Aban360.MeterPool.Application/Features/Management/Services/CounterStateService.cs) | 5 | Bodies, signatures, operation count and dependency union matched; frozen consumer/handler comparison. |
| [IndividualTagDefinition](../../Aban360.ClaimPool.Application/Features/People/Services/IndividualTagDefinitionService.cs) | 5 | Bodies, signatures, operation count and dependency union matched; frozen consumer/handler comparison. |
| [WaterMeterTagDefinition](../../Aban360.ClaimPool.Application/Features/WasteWater/Services/WaterMeterTagDefinitionService.cs) | 5 | Bodies, signatures, operation count and dependency union matched; frozen consumer/handler comparison. |
| [EquipmentBrokerZone](../../Aban360.InstallationPool.Application/Features/Definition/Services/EquipmentBrokerZoneService.cs) | 5 | Bodies, signatures, operation count and dependency union matched; frozen consumer/handler comparison. |
| [SewageEquipmentBrokerZone](../../Aban360.InstallationPool.Application/Features/Definition/Services/SewageEquipmentBrokerZoneService.cs) | 5 | Bodies, signatures, operation count and dependency union matched; frozen consumer/handler comparison. |
| [CompanyServiceOffering](../../Aban360.CalculationPool.Application/Features/Bill/Services/CompanyServiceOfferingService.cs) | 5 | Bodies, signatures, operation count and dependency union matched; frozen consumer/handler comparison. |
| [TariffConstant](../../Aban360.CalculationPool.Application/Features/Rule/Services/TariffConstantService.cs) | 6 | Bodies, signatures, operation count and dependency union matched; frozen consumer/handler comparison. |
| [UserWorkday](../../Aban360.ClaimPool.Application/Features/Land/Services/UserWorkdayService.cs) | 5 | Bodies, signatures, operation count and dependency union matched; frozen consumer/handler comparison. |
| [RequestTracking](../../Aban360.ClaimPool.Application/Features/Draft/Services/RequestTrackingService.cs) | 5 | Bodies, signatures, operation count and dependency union matched; frozen consumer/handler comparison. |
| [Individual](../../Aban360.ClaimPool.Application/Features/People/Services/IndividualService.cs) | 5 | Bodies, signatures, operation count and dependency union matched; frozen consumer/handler comparison. |
| [Siphon](../../Aban360.ClaimPool.Application/Features/WasteWater/Services/SiphonService.cs) | 5 | Bodies, signatures, operation count and dependency union matched; frozen consumer/handler comparison. |
| [Guild](../../Aban360.ClaimPool.Application/Features/Land/Services/GuildService.cs) | 5 | Bodies, signatures, operation count and dependency union matched; frozen consumer/handler comparison. |
| [WaterResource](../../Aban360.ClaimPool.Application/Features/Land/Services/WaterResourceService.cs) | 5 | Bodies, signatures, operation count and dependency union matched; frozen consumer/handler comparison. |
| [RequestIndividual](../../Aban360.ClaimPool.Application/Features/Draft/Services/RequestIndividualService.cs) | 3 | Bodies, signatures, operation count and dependency union matched; frozen consumer/handler comparison. |
| [RequestSiphon](../../Aban360.ClaimPool.Application/Features/Draft/Services/RequestSiphonService.cs) | 3 | Bodies, signatures, operation count and dependency union matched; frozen consumer/handler comparison. |
| [RequestWaterMeter](../../Aban360.ClaimPool.Application/Features/Draft/Services/RequestWaterMeterService.cs) | 3 | Bodies, signatures, operation count and dependency union matched; frozen consumer/handler comparison. |
| [DocumentCategory](../../Aban360.BlobPool.Application/Features/Taxonomy/Services/DocumentCategoryService.cs) | 6 | Bodies, signatures, operation count and dependency union matched; frozen consumer/handler comparison. |
| [DocumentType](../../Aban360.BlobPool.Application/Features/Taxonomy/Services/DocumentTypeService.cs) | 5 | Bodies, signatures, operation count and dependency union matched; frozen consumer/handler comparison. |
| [ReadingBound](../../Aban360.LocationPool.Application/Features/MainHierarchy/Services/ReadingBoundService.cs) | 7 | Bodies, signatures, operation count and dependency union matched; frozen consumer/handler comparison. |
| [Profession](../../Aban360.ClaimPool.Application/Features/Land/Services/ProfessionService.cs) | 5 | Body/signature/dependency comparison; consumer and workflow regressions. |
| [EstateBoundType](../../Aban360.ClaimPool.Application/Features/Land/Services/EstateBoundTypeService.cs) | 5 | Body/signature/dependency comparison; consumer and workflow regressions. |
| [SubscriptionType](../../Aban360.ClaimPool.Application/Features/Metering/Services/SubscriptionTypeService.cs) | 5 | Corrected update mapping direction; frozen old failure and entity/input assertions. |
| [ConstructionType](../../Aban360.ClaimPool.Application/Features/Land/Services/ConstructionTypeService.cs) | 5 | Body/signature/dependency comparison; consumer and workflow regressions. |
| [MeterType](../../Aban360.ClaimPool.Application/Features/Metering/Services/MeterTypeService.cs) | 5 | Body/signature/dependency comparison; consumer and workflow regressions. |
| [MeterMaterial](../../Aban360.ClaimPool.Application/Features/Metering/Services/MeterMaterialService.cs) | 5 | Body/signature/dependency comparison; consumer and workflow regressions. |
| [MeterUseType](../../Aban360.ClaimPool.Application/Features/Metering/Services/MeterUseTypeService.cs) | 5 | Body/signature/dependency comparison; consumer and workflow regressions. |
| [UseState](../../Aban360.ClaimPool.Application/Features/Metering/Services/UseStateService.cs) | 5 | Body/signature/dependency comparison; consumer and workflow regressions. |
| [MeterProducer](../../Aban360.ClaimPool.Application/Features/Metering/Services/MeterProducerService.cs) | 5 | Body/signature/dependency comparison; consumer and workflow regressions. |
| [MeterDiameter](../../Aban360.ClaimPool.Application/Features/Metering/Services/MeterDiameterService.cs) | 5 | Body/signature/dependency comparison; consumer and workflow regressions. |
| [Flat](../../Aban360.ClaimPool.Application/Features/Land/Services/FlatService.cs) | 5 | Body/signature/dependency comparison; consumer and workflow regressions. |
| [VirtualCategory](../../Aban360.OldCalcPool.Application/Features/Db70/Services/VirtualCategoryService.cs) | 5 | Body/signature/dependency comparison; consumer and workflow regressions. |
| [ServerReports](../../Aban360.ReportPool.Application/Features/FlatReports/Services/ServerReportsService.cs) | 6 | Body/signature/dependency comparison; consumer and workflow regressions. |
| [Zone](../../Aban360.LocationPool.Application/Features/MainHierarchy/Services/ZoneService.cs) | 6 | Body/signature comparison plus focused tree/Zone and critical JWT/permission regressions. |
| [App](../../Aban360.UserPool.Application/Features/AccessTree/Services/AppService.cs) | 6 | Body/signature comparison plus focused tree/Zone and critical JWT/permission regressions. |
| [SubModule](../../Aban360.UserPool.Application/Features/AccessTree/Services/SubModuleService.cs) | 6 | Body/signature comparison plus focused tree/Zone and critical JWT/permission regressions. |
| [Endpoint](../../Aban360.UserPool.Application/Features/AccessTree/Services/EndpointService.cs) | 5 | Body/signature comparison plus focused tree/Zone and critical JWT/permission regressions. |
| [Module](../../Aban360.UserPool.Application/Features/AccessTree/Services/ModuleService.cs) | 6 | Body/signature comparison plus focused tree/Zone and critical JWT/permission regressions. |
| [Workflow](../../Aban360.WorkflowPool.Application/Features/Design/Services/WorkflowService.cs) | 7 | Body/signature/dependency comparison; consumer and workflow regressions. |
| [MainTagGroup](../../Aban360.ReportPool.Application/Features/Tagging/Services/MainTagGroupService.cs) | 5 | Connection factory preserves SQL behavior; 258 Dapper/provider checks. |
