# Application service consolidation: rows 71–100

Consolidated 28 capabilities containing 39 active handlers into public service interfaces and internal implementations. Rows 97 (TrackingDetails, 9 handlers) and 100 (BillReturns, 10 handlers) were excluded. The commented-out BillInstallmentHandler and its contract remain unchanged; row 81 migrates the active ServiceLinkInstallmentHandler only. The old UpdateUsersInRole and MeterReadingDetailSendSms contracts/types are retained as forwarding adapters so previously persisted Hangfire jobs can still execute; their operation logic lives in the consolidated services.

Forty-one controllers and two existing application services now consume the new interfaces. Handler bodies, helper methods, field initializers, SQL, transaction boundaries, controller action signatures, routes, permission metadata and save sequencing are preserved. Existing Scrutor registrations discover the new matching interfaces with scoped lifetime. DTOs, validators, persistence and TrackingFlowGetHandler are unchanged.

| Row | Capability | Active handlers |
|---:|---|---:|
| 71 | UserRoleMembership | 1 |
| 72 | AssessmentByZone | 1 |
| 73 | UserPolicy | 1 |
| 74 | OnlineUsers | 1 |
| 75 | BroadcastMessages | 1 |
| 76 | WorkflowStatus | 1 |
| 77 | CustomerLookup | 1 |
| 78 | JudicalNotice | 1 |
| 79 | VillageLookup | 1 |
| 80 | ModifyCauseLookup | 1 |
| 81 | BillInstallments | 1 |
| 82 | PaymentIds | 3 |
| 83 | InvoiceTogether | 1 |
| 84 | BillLists | 2 |
| 85 | ImpactSign | 1 |
| 86 | LatestBillSms | 1 |
| 87 | MeterReadingSms | 1 |
| 88 | NonReadingBills | 1 |
| 89 | MeterReadingFileRemoval | 1 |
| 90 | MeterReadingExcludeCause | 1 |
| 91 | TankerTariff | 1 |
| 92 | CalculationRequestPricing | 1 |
| 93 | CalculationRequestWorkflow | 3 |
| 94 | RequestFinalization | 3 |
| 95 | RequestDictionaries | 2 |
| 96 | RequestCartable | 3 |
| 98 | GeneralRequestInformation | 1 |
| 99 | BillRemoval | 2 |

Validation includes a Git-pinned source comparison of all methods, fields, constructor assignments/guards and consumers; scoped DI checks for every new service; differential query scenarios; and the existing controller regression suites. Direct SQL workflows are checked by executable-token comparison. Tests use isolated dependencies, not a live SQL Server or IIS deployment.

Run source verification:

```powershell
dotnet run --project tests/Aban360.CriticalServiceStructure.RegressionTests -- --next28
```

Run application builds and regression suites:

```powershell
./scripts/test-merged-controllers.ps1 -NoRestore
```

Focused runtime checks:

```powershell
dotnet run --project tests/Aban360.MergedControllers.SmokeTests --no-build -- --next28
```

The broad run exposed two pre-existing CollectBills fixture mismatches: ControllerBatch136 expects the old action count, and MergedControllers expects a combined GET/POST get endpoint. CollectBillsController is unchanged from Git HEAD by this batch. These failures are retained and reported; the historical contract fixtures were not recaptured.

Validation results (2026-11-19): 801 Git-pinned source checks passed; 204 focused runtime checks passed (including 27 follow-up review checks), with 80 before/after scenarios and 16 successful query fixtures; all 28 services have scoped DI registration. Critical controller regression passed 1,639 checks and login auditing passed 21 checks. The other ten suites in the broad run passed; the two historical CollectBills fixture failures described above remain.

## Follow-up review

- Restored the legacy MeterReadingDetailSendSmsHandler type and contract as a forwarding adapter. Previously queued SMS jobs serialize the concrete class and the CreateJob/SendSms method names. Both methods retain AutomaticRetry with two attempts. Job deserialization and activation are checked for these concrete SMS signatures and both the interface/concrete role-membership signatures, including pending completion, failure and cancellation.
- Corrected a pre-existing missing IMoshtrakService constructor dependency in GeneralRequestInformationService. The original handler left its readonly field null and failed after loading tracking/subscriber information. A dedicated test reproduces that old failure, then verifies the corrected output, selected service flags, service group, asynchronous completion and error/cancellation propagation. Other operation bodies and transaction boundaries remain unchanged.
