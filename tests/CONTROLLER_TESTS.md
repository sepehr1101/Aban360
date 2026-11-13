# Consolidated controller regression tests

Run all 10 suites on Windows:

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
| `CrudChecks` | 276 simple actions, 1,875 success/error/cancellation/await scenarios; handler/save ordering, input, user, token and response envelope |
| `PermissionChecks` | Production authorization filter with fake claims; preserved permission identifiers and allowed/denied access |
| `WorkflowChecks` | ZIP contents/name/MIME and failures; job scheduling, awaiting and cancellation; SMS with fake services |
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

## Intentional contract changes

Ordinary execution never rewrites the snapshot. After reviewing an intentional API change and its client impact, capture the new contract explicitly:

```powershell
dotnet run --project tests/Aban360.MergedControllers.SmokeTests -- --capture
```

Review the `Contracts.json` diff. Do not capture merely to silence a failing test. Rebuild and run afterward so the updated fixture is copied to the output directory. Capture selection uses the earlier baseline, `Groups.json` manifests and the explicit batch-110 selection; this dependency on old source selections exists only in capture mode.
