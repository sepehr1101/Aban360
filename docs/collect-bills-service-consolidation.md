# CollectBills application service

`ICollectBillsApplicationService` / `CollectBillsApplicationService` now contain the file operations, last-step report and job workflow previously implemented by `CollectBillsFilesService`, `CollectBillsDetailWithLastStepGetHadler` and `CollectBillsDetailJobService`.

The controller and recurring `CollectBillsJob` use the unified contract. The report entry point is `GetDetailWithLastStepAsync`; the existing file and workflow method names are preserved. The external HTTP provider `Infrastructure.Providers.CollectBills.Contracts.ICollectBillsService` keeps its existing role and implementation.

## Queued-job compatibility

Hangfire persists the concrete worker type and method signature. The old `Features.Base.CollectBillsDetailJobService` and its two-method interface therefore remain as a thin forwarding adapter, with no duplicate business logic. Its six public methods retain their names, parameter types and return types. Previously saved `CreateFile`, `Upload`, `SetFileDetail` and `Confirm` jobs can still deserialize and activate. New jobs created by the unified workflow target `CollectBillsApplicationService`.

## Preserved behavior

- The six controller actions, routes, HTTP verbs, GET query binding, POST body binding, response metadata and ZIP download behavior are preserved.
- The recurring job's environment check, date offset, logging, cancellation and concurrency attribute are preserved.
- SQL connection/transaction boundaries, inserts/updates, log grouping IDs, external calls, success branches, queue order and the 30-minute confirmation delay are preserved.
- ZIP entry naming, UTF-8 BOM, text contents, date filtering, paths and report counts are preserved. The former file-service call to the job service's `CreateZip` becomes an internal method call.

## Verification

The dedicated suite compares complete method syntax, SQL/strings, controller actions and the recurring job against six frozen pre-change source files with SHA-256 hashes. Runtime checks exercise actual scoped registrations, report success/empty/failure/cancellation/pending completion, ZIP bytes, HTTP download bytes, legacy Hangfire serialization and the adapter's argument/task/error/cancellation forwarding. Dependencies are isolated; no live database or remote upload is performed.

From native PowerShell in `D:\Projects\Aban360`:

```powershell
dotnet build Aban360.Api/Aban360.Api.csproj --no-restore -m:1
dotnet run --project tests/Aban360.CollectBillsConsolidation.RegressionTests --no-restore
```

The dedicated runner uses the freshly built API's exact binary dependencies and the SDK's Roslyn parser. Build the API in the same configuration first. The merged-controller verification script runs this suite before verifying older source waves. The older operations-19 runtime suite delegates its two former file-operation cases to the new CollectBills runtime checks and retains its other 68 operations.

The existing CollectBills GET/POST contract-fixture differences discussed before this consolidation are outside this change; those historical endpoint fixtures have not been recaptured.

Validation on native Windows (2026-11-20): API and Bridge API builds succeeded. The dedicated runner builds with zero warnings/errors and passes 70 source-equivalence checks and 66 runtime checks, including activation of the actual legacy scoped adapter and round-trip deserialization of both legacy interface methods. The operations runtime suite passes 2,437 checks / 1,328 before-after scenarios / 244 SQL fault-and-await scenarios, in addition to the new CollectBills runtime checks. Compact runtime (1,107 checks / 659 scenarios), Next28 runtime (204 checks / 80 scenarios plus 27 review checks), and the preceding source suites (625 + 801 checks) pass.

An additional run of the older `CriticalServiceStructure -- --operations19` source suite stops at its historical protected-file hash for `TariffCreateHandler.cs`, which was intentionally replaced during the previously authorized compact consolidation. This guard predates this CollectBills change; its baseline has not been rewritten to hide the mismatch. The dedicated CollectBills equivalence suite compares the actual current implementation, including all workflow SQL and transactions, directly against its own frozen inputs.
