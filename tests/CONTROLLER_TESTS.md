# Consolidated controller regression tests

Run all 12 suites on Windows:

```powershell
./scripts/test-merged-controllers.ps1
```

The wrapper builds the main API and Bridge API once, then builds and runs the suites. Any failed suite returns a nonzero exit code. Logs and `summary.json` are written under `artifacts/test-results/<run-id>/`. The `controller-regressions.yml` workflow runs this command on Windows; a local run does not imply that GitHub CI has run.

```powershell
# List suites
./scripts/test-merged-controllers.ps1 -List
# Reuse restored packages
./scripts/test-merged-controllers.ps1 -NoRestore
# Run the new suite after building
./scripts/test.ps1 -Suite Aban360.MergedControllers.SmokeTests -NoBuild -NoRestore
```

## Coverage

| Suite or component | Coverage |
|---|---|
| `MergedControllers.SmokeTests` | 745 pinned action contracts across 155 controllers: routes, verbs, parameters, binding, nullability, public DTO shapes, responses, authorization and filters |
| New HTTP checks | 2,438 routing/authentication cases with authenticated and anonymous requests in TestServer |
| `CrudChecks` | 280 simple actions, 1,903 success/error/cancellation/await scenarios; handler/save ordering, input, user, token and response envelope |
| `PermissionChecks` | Production authorization filter with fake claims; preserved permission identifiers and allowed/denied access |
| `WorkflowChecks` | ZIP contents/name/MIME and failures; job scheduling, awaiting and cancellation; SMS with fake services |
| `LookupServiceChecks` | Bank, SupportedOperator, SupportedField, AccountType and CreditorType: production DI registrations, scoped lifetime, actual mapping profiles, controller/service/query execution, null and empty results, errors, cancellation and asynchronous completion |
| `ServiceConsolidationChecks` | 84 application services / 387 operations and 89 API/Bridge/gateway/SignalR consumers (390 consumer operations); 4,978 before/after scenarios with real mapping, DI scopes, inputs, outputs, tracked mutations, dependency order, empty/missing results, flags, validation rejection, errors, cancellation and awaiting |
| `OnlineUsers.SmokeTests` | SignalR connection registry, connection/disconnection failures, presence, heartbeat and report notifications, including the consolidated HubEvent service boundary |
| `Ticketing.SmokeTests` | 621 assertions: EF InMemory persistence, the same application and HTTP workflows against frozen handlers/controllers and the four consolidated services/controllers, service signatures and scoped registrations, pending command completion, failure/cancellation, and metadata authorization before file reads |
| `CriticalControllers.RegressionTests` | Role, App, Endpoint, Module and Zone: real JWT validation, model binding, permission and action behavior |
| `ControllerBatch*` and `ControllerConsolidation` | Comparison against pre-merge routes and controller snapshots, including action behavior and responses |

The printed number of checks counts assertions, not independent scenarios. The general `scripts/test.ps1` runner now also discovers executable projects ending in `RegressionTests`.

## Boundaries

- New HTTP checks run routing and authentication/authorization middleware and stop before MVC action execution. They do not claim model-binding or real-JWT coverage for all 745 actions. Those checks exist in the critical-controller suite.
- CRUD and workflow checks invoke production actions with fake dependencies.
- These suites do not validate SQL, a real database, external services or IIS deployment. No real SMS or background job is dispatched.
- The pinned contract protects the current API against future changes. Earlier suites retain comparisons with pre-merge behavior.

## Future architecture changes

The new suite finds actions by route and HTTP verb, without depending on controller names, folders or namespaces. Public DTO field names, types and nullability remain checked.

Older suites retain original-controller snapshots. Future interface or namespace changes may require updating imports and test doubles. Do not delete behavioral checks merely to make a refactor pass.

The batch-132 suite keeps six old lookup-handler interfaces in `LegacyContracts` solely to compile unchanged pre-merge controller snapshots. These contracts are test-only; production uses the five consolidated application services and has no dependency on the removed handlers.

`Shared/ServiceConsolidation15/Contracts` holds the next batch's old contracts for test-only snapshots. The batch-110/132/136 suites link them without retaining obsolete production handlers. `ApplicationServiceBaseline` maps the new service/method names to old contract/method identities when comparing controller calls; arguments, call order, results and errors remain compared. `OriginalHandlers15` and `OriginalConsumers15` are fixed pre-refactor application and consumer snapshots. The new services preserve the existing persistence APIs and cancellation behavior, and controllers keep their original save boundary. Original quirks in action names, route casing and response metadata remain pinned by the HTTP fixture.

The same scheme extends to `ServiceConsolidation30`, linked by the batch-110/132/136/151 suites. The 30 new services add 140 original operations; snapshots and consumer contracts are kept separately from production. Production validators are retained. Behavioral comparison supplies valid and invalid validator results to verify ordering and error messages without querying real databases. Envelope server timestamps and the two existing wall-clock `ValidFrom` assignments are compared as current timestamps, while deterministic dates remain compared exactly.

The latest batch extends this scheme to `ServiceConsolidation20` and adds 20 services / 92 operations, with frozen original handlers and 20 consumers. Full namespaces distinguish CalculationPool Offering from OldCalcPool Offering, and ClaimPool SiphonDiameter from its CalculationPool namesake. Credit's four basic operations are included; its two specialized create workflows remain separate. Existing wall-clock `ValidFrom` assignments in the latest batch are normalized only when freshly generated; fixed dates are still compared exactly.

## Findings from the full 70-capability review

- The review covers 304 moved methods across all 70 capabilities (the previously reviewed 212 and all 92 in the latest batch), alongside action contracts and dependency unions. See `docs/architecture/application-service-review.md`.
- `RequestFlat.DeleteAsync`, `RequestIndividualTag.DeleteAsync` and `RequestWaterMeterTag.DeleteAsync` now await `Remove`. Frozen handlers deliberately retain their missing awaits. Tests explicitly distinguish these intentional corrections from behavior-preserving changes and exercise each current controller with its real service: save must wait, and removal errors/cancellation must prevent save.
- Four pre-existing tag lookup mapping failures are explicitly reproduced in both frozen and current code: `WaterMeterTag` and `IndividualTag`, each with `GetSingleAsync` and `GetBySearchAsync`. These cases are printed as **KNOWN LEGACY DEFECTS**, not successful reads. Correcting the scalar/list contracts requires a separate API decision; the refactor does not silently change them.

## Ticketing consolidation

The four ticketing capabilities add 20 consolidated operations, bringing the total to 74 capabilities / 324 operations. Their more complex workflows are verified in the dedicated ticketing suite rather than the simple lookup comparison harness. `Shared/ServiceConsolidationTicket/Contracts` keeps the 19 removed contracts only for frozen handlers and batch-132 controller snapshots. `Ticketing.SmokeTests/OriginalHandlers` and `OriginalControllers` retain the old implementations; test-only forwarding proxies let existing scenario assertions also exercise the actual consolidated services. The standard wrapper and Windows CI command now include this suite. No API contract fixture was recaptured.

## Following 19 groups

The following 19-group batch adds 93 operations, bringing the complete consolidation scope to 93 capabilities / 417 operations. DynamicReport was explicitly excluded. `ServiceConsolidation19` extends the frozen handler and consumer comparisons and shared contract mapping to the same five projects that use the earlier manifests. Instrumented uploaded files verify asynchronous copy completion, errors and cancellation before persistence. Seven additional ReadingBound cases verify interval errors, sorted input and the IsValid flag. Individual and Siphon preserve their original BaseException validation results. Four previous tag lookup failures remain asserted and printed separately. The subsequently requested fixes add the RequestTracking update map and preserve base64 icons in both document update maps. OriginalMappings freezes pre-fix profiles; MappingCorrectionChecks reproduces the three original defects and verifies corrected production services, exact file bytes, other editable fields, ownership/navigation preservation and existing query/save boundaries. Frozen handler comparisons use current production profiles on both sides. No API fixture was recaptured.

## Intentional contract changes

Ordinary execution never rewrites the snapshot. After reviewing an intentional API change and its client impact, capture the new contract explicitly:

```powershell
dotnet run --project tests/Aban360.MergedControllers.SmokeTests -- --capture
```

Review the `Contracts.json` diff. Do not capture merely to silence a failing test. Rebuild and run afterward so the updated fixture is copied to the output directory. Capture selection uses the earlier baseline, `Groups.json` manifests and the explicit batch-110 selection; this dependency on old source selections exists only in capture mode.
