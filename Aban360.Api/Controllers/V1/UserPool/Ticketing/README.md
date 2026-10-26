# Ticketing API

راهنمای فارسی پیاده‌سازی فرانت:
[Frontend guide](../../../../../docs/ticketing/FRONTEND_GUIDE.fa.md).
دستور کار آماده برای Agent:
[Frontend agent prompt](../../../../../docs/ticketing/FRONTEND_AGENT_PROMPT.fa.md).

Controllers follow the existing UserPool pattern: separate `Commands` and
`Queries`, constructor injection, `BaseController`, authenticated `CurrentUser`,
and `ApiResponseEnvelope<T>`. Commands commit through the UserPool unit of work
after the handler succeeds. File content is a binary response, not an envelope.

All endpoints require the application's existing authentication. Ownership and
responder permissions are checked by the application handlers. User IDs for
creation, replies and ratings are taken from authentication, never form fields.

## Routes

| Method | Route | Input / result |
|---|---|---|
| POST | `/v1/ticket/create` | Multipart `Subject`, optional `Body`, repeated `Attachments`; returns ticket number |
| POST or PATCH | `/v1/ticket/update` | JSON `id`, `subject`, `assignedToUserId`, `statusId`; returns `true` |
| GET | `/v1/ticket/read/{id}` | Ticket details |
| POST | `/v1/ticket/list` | JSON search/paging; returns ticket page |
| POST | `/v1/ticket/my-list` | JSON status/text/date/paging; only tickets created by the authenticated user, including for admins |
| POST | `/v1/ticket/report` | JSON search filters; returns ticket/rating counts and average |
| POST | `/v1/ticket-message/create` | Multipart `TicketId`, optional `Body`, repeated `Attachments`; returns `true` |
| POST or PATCH | `/v1/ticket-message/update` | JSON `id`, `body`; returns `true` |
| GET | `/v1/ticket-message/read/{id}` | Message details |
| POST | `/v1/ticket-message/list` | JSON `ticketId`, `pageNumber`, `pageSize`; returns message page |
| POST | `/v1/ticket-attachment/create` | Multipart `TicketMessageId`, `File`; returns `true` |
| POST or PATCH | `/v1/ticket-attachment/update` | Multipart `Id`, replacement `File`; returns `true` |
| GET | `/v1/ticket-attachment/read/{id}` | Attachment metadata |
| GET | `/v1/ticket-attachment/list/{messageId}` | Message attachment metadata list |
| GET | `/v1/ticket-attachment/content/{id}` | Inline image/video bytes, with Range support |
| GET | `/v1/ticket-attachment/content/{id}?download=true` | Same content with a download filename |
| POST | `/v1/ticket-rating/create` | JSON `ticketMessageId`, `score`, optional `comment`; returns `true` |
| POST or PATCH | `/v1/ticket-rating/update` | JSON `id`, `score`, optional `comment`; returns `true` |
| GET | `/v1/ticket-rating/read/{id}` | Rating details |
| GET | `/v1/ticket-rating/read-by-message/{messageId}` | Rating details or `data: null` if unrated |
| GET | `/v1/ticket-status/read/{id}` | Status details |
| GET | `/v1/ticket-status/list` | All statuses |

## Search and paging

Ticket list/report body example:

```json
{
  "statusId": 1,
  "searchText": "pressure",
  "fromDate": "2026-10-01T00:00:00Z",
  "toDateExclusive": "2026-11-01T00:00:00Z",
  "pageNumber": 1,
  "pageSize": 20
}
```

All filters are optional. `{}` uses the default page. Dates filter ticket
creation time in UTC; the end is exclusive. Reports include ratings on the
filtered tickets. No ratings means `averageScore: null`.

For **My Tickets**, use `/v1/ticket/my-list` with `{}` or with `statusId`,
`searchText`, `fromDate`, `toDateExclusive`, `pageNumber`, `pageSize`.
No user/assignee ID is accepted as a filter. It includes all statuses by default
and always restricts ownership to the authenticated user, even for admins.
Results are paginated; follow `totalCount` to read all pages.

Use `assignedToUserId` equal to the current responder's ID for their assigned
queue. Otherwise non-admin users receive their own tickets. Administrators can
also filter `createdByUserId` or `assignedToUserId`. Creation returns the unique
ticket number; it can be searched with `searchText` to retrieve the ticket ID.

Pages contain `items`, `totalCount`, `pageNumber`, `pageSize` inside `data`.
Page numbers start at 1, and page size must be between 1 and 100. Message pages
are chronological; ticket pages show newest tickets first.

## Files

Create a ticket or message with text, files, or both. Browser FormData example:

```javascript
const form = new FormData();
form.append("Subject", "Water pressure");
form.append("Body", "Please investigate.");
for (const file of selectedFiles) form.append("Attachments", file);
const response = await fetch(`${apiBase}/v1/ticket/create`, {
  method: "POST",
  headers: { Authorization: `Bearer ${token}` },
  body: form
});
```

Let the browser set the multipart Content-Type and boundary. Do not send file
bytes as base64 JSON. Each file is limited to 50 MiB; each message to five files.
JPEG, PNG, GIF, WebP, MP4 and WebM are accepted after application validation.
Request-size and multipart limits are set on upload actions. IIS/reverse-proxy
limits must also permit the intended upload size on the deployed host.

Content requests support `Range: bytes=0-1023`, returning 206; an unsatisfiable
range returns 416. Content is private, marked no-store, and carries nosniff.
The current handler loads bytes into memory before responding; HTTP Range
support does not provide streaming directly from SQL Server.

Every content request also requires authentication. For bearer-token clients,
include the Authorization header when fetching content. A plain video/image
`src` cannot add that header; React can fetch a Blob and use an object URL for
small files, or use an authenticated media-loading approach for ranged playback.
Do not put access tokens in query strings.

## Verification

`dotnet run --project tests/Aban360.Ticketing.SmokeTests` runs persistence,
application and HTTP checks. The HTTP checks exercise the production controller,
handler and persistence sources with test-only authentication and an in-memory
database. Live SQL Server/FILESTREAM and deployed proxy limits are not exercised.
