# Sales and meter-reading report consolidation

This batch removes 13 handlers and their production contracts, expands three existing services and creates two services in CalculationPool. Seven controllers consume the unified contracts. Existing workspace changes were preserved; snapshots were captured immediately before this batch.

| Service | Handlers absorbed | Final class lines | Constructor dependencies |
| --- | ---: | ---: | ---: |
| Article11Service | 3 create/update/delete handlers | 114 | 5 |
| InstallationAndEquipmentService | 3 create/update/delete handlers | 110 | 5 |
| TankerWaterDistanceTariffService | 3 create/update/delete handlers | 72 | 3 |
| MeterReadingChangeReportsService | 2 excluded/updated report handlers | 94 | 4 |
| TankerWaterQueriesService | 2 list/detail handlers | 61 | 2 |

Lines count the implementation class, including its declaration and braces, excluding imports and the interface. Shared constructor dependencies are injected once. Public operations and private helpers are renamed only as recorded in the frozen operation manifest. Method logic is preserved.

## Review boundaries

- Existing query entry points on the three sales services retain their signatures; their contracts gain create/update/delete operations.
- Controller routes, HTTP verbs, input/response types, binding attributes, permissions and action names are preserved.
- Validation order, user-region checks, actor IDs, amounts, registration/removal dates and persistence call order are preserved.
- Existing transaction/commit boundaries remain with their callers/persistence services. No Domain, Persistence or protected TrackingFlow code was changed.
- Tanker delivery amount calculation and its clamp for negative values are preserved. Report title fields remain separate; updated-reading colour grouping is preserved.
- No selected operation schedules a job, so no production compatibility adapters are required. Removed contracts remain only in test fixtures to compile the original controllers.

## Verification

`SalesReportsBaseline/operations.json` records the 13 handlers plus the three previous services, input hashes, contract snapshots, destinations and allowed renames. `consumers.json` records all seven controllers. Source checks compare all 29 complete methods, field initializers, the constructor union and ordered guards, public signatures and consumer logic. Runnable original classes are checked against those same frozen inputs.

Runtime comparisons execute original and merged code with isolated dependencies. They cover each of the 19 application entry points, empty inputs, validation rejection, dependency failure/cancellation, pending asynchronous completion and controller call/argument order. Additional assertions check persisted monetary values and actor IDs, default removal flags, delivery calculations and caller-controlled commit boundaries.

Run from the native Windows checkout:

```powershell
dotnet build Aban360.Api/Aban360.Api.csproj --no-restore -m:1
dotnet run --project tests/Aban360.CriticalServiceStructure.RegressionTests --no-restore -- --next28
dotnet run --project tests/Aban360.MergedControllers.SmokeTests --no-restore -- --sales-reports
```

The source suite checks the current batch before preceding source waves. Older source comparisons use this batch's frozen input when a service has been expanded. The merged-controller script also runs the new runtime suite explicitly. Historical CollectBills endpoint and operations-19 Tariff protected-file fixture differences remain outside this change; their fixtures were not recaptured.

No live database, SMS, external HTTP provider or production job is executed by these checks.

Validation on native Windows (2026-11-20): both API builds succeeded. This batch passed 218 source checks and 496 runtime checks across 265 before/after scenarios, all 19 application entry points and 23 consumer methods. The STI action was also exercised with real JSON output in a uniquely named temporary directory; exported contents matched and test files were removed. Batch141 controller regression passed 342 checks. Preceding source waves passed 625 + 801 checks; preceding runtime suites passed operations-19 (2,437 checks / 1,328 scenarios / 244 SQL fault-and-await scenarios), compact (1,107 checks / 659 scenarios), Next28 (204 checks / 80 scenarios plus 27 review checks), and CollectBills (70 source + 66 runtime checks). No new consolidation defect was found during the final review.
