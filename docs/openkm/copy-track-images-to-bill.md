# Copy request images to a bill folder

Frontend integration guide in Persian: [راهنمای ساده فرانت](copy-track-images-to-bill.frontend.fa.md).

`POST /v1/open-km/copy-track-images-to-bill`

Uses the existing API authentication and response envelope. No new configuration or database migration is required. The handler is registered by the existing BlobPool application assembly scan.

```json
{
  "trackNumber": 123456,
  "billId": "1234567890123"
}
```

`trackNumber` must be a positive 32-bit integer, matching the Tracking query contract. `billId` is a nonempty string of ASCII digits; leading zeros are retained. Whitespace around the bill ID is removed.

The handler reads the latest Tracking row, checks the caller's access to its zone, and compares its nonempty `BillId` with the supplied bill ID. An unrelated or unassigned bill ID is rejected before accessing OpenKM. This endpoint does not establish or update the relationship.

The source is `{BaseDirectoryPath}r_{trackNumber}`. The destination is `{BaseDirectoryPath}{billId}` and is created when absent. A missing source is an error. Only direct child documents with an `image/*` MIME type are copied. Subfolders, deleted documents, and nonimage attachments are excluded. An empty image list completes with zero counts.

The current OpenKM connection exposes download and upload operations. The handler uploads the image bytes under the original file name, then copies the available numeric `moshtarakin_file` metadata properties, including document type. The new document has its own UUID and OpenKM system history, author and timestamps. Source documents and their folder are preserved.

The response envelope's `data` contains:

```json
{
  "trackNumber": 123456,
  "billId": "1234567890123",
  "isLinked": true,
  "isComplete": true,
  "copiedCount": 1,
  "skippedCount": 0,
  "conflictCount": 0,
  "failedCount": 0,
  "warnings": [],
  "files": [
    {
      "sourceUuid": "source-document-uuid",
      "fileName": "photo.jpg",
      "destinationUuid": "new-document-uuid",
      "status": "Copied",
      "errorCode": null,
      "message": null
    }
  ]
}
```

File statuses:

| Status | Meaning |
| --- | --- |
| `Copied` | Image uploaded and its available metadata applied. |
| `Skipped` | A file with the same name and SHA-256 content hash already exists; metadata has been applied to it. No duplicate upload. |
| `Conflict` | The same file name exists with different contents. Neither file is overwritten. |
| `Failed` | Reading, upload, or metadata application failed. `errorCode` identifies `ReadSource`, `Upload`, or `Metadata`. |

A file failure does not stop subsequent images. HTTP 200 means the batch was processed; consumers must inspect `data.isComplete` and per-file results. The envelope message explicitly indicates incomplete batches. Validation, access, missing-source and batch setup errors use the existing exception handling pipeline.

After a metadata failure, the destination UUID is returned if known. Retry the same request to apply metadata to the identical existing file. If an upload response was lost, a retry first checks destination names and content before uploading. Conflicting contents require the caller to resolve the conflict. Cancellation propagates; already completed remote writes remain, and retry uses the same duplicate checks.

Copies to the same destination are serialized within one API process. Multiple API instances and other OpenKM writers still rely on the repository's file-name uniqueness behavior; there is no distributed lock or cross-system transaction. Start and completion counts are recorded in the existing user operation log; file exceptions, including source and destination UUIDs, are recorded in application logs.

If the completion audit cannot be saved, the response still contains the file results and a warning in `data.warnings` with code `CompletionAuditFailed`. The envelope message also indicates this warning. `isComplete` continues to describe the image copy, independently of audit persistence. The audit exception is recorded in application logs. A failed starting audit still aborts the operation before destination creation or upload.

Numeric metadata values accept both JSON numbers (`7`) and numeric strings (`"7"`), as well as `null`, for both list and single-property responses. Invalid nonnumeric values are rejected; values are still serialized as JSON numbers.

## Verification

```powershell
dotnet run --project tests/Aban360.OpenKm.SmokeTests/Aban360.OpenKm.SmokeTests.csproj -p:BuildInParallel=false
dotnet build Aban360.Api/Aban360.Api.csproj --no-restore -m:1
```

The smoke checks exercise the production handler with in-memory service doubles and the production metadata reader with simulated HTTP responses. They cover completion audit failures for successful and partial copies and JSON metadata values in both response shapes. They make no calls to the live OpenKM repository.
