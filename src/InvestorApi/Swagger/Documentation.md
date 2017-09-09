## Introduction

This API provides the backend functionality for the Budding Share Market Investor Project.

## Usage

### Authentication
Users can use the `/token` endpoint to authenticate with user name and password and retrieve a JSON Web Token ([JWT](https://jwt.io)) in return.
The token must be provided to most other API operations via `Authorization` HTTP header using `Bearer` scheme.

```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

Please note that the token expires after 60 minutes.

### Authorization
The API can be used by investors and administrators. However, most operations require the user to be a member of one or the other role. Attempts to access APIs without the required role membership will result in a 401 response.

### Content Types
The API only supports `application/json` content type.

### Date & Time Values
Date and time values in payloads are exchanged using [ISO 8601](https://en.wikipedia.org/wiki/ISO_8601) format and all values are in UTC, for example `'2017-10-18T11:30:00Z'`. Durations must be formatted using the simple `"HH:MM:SS"` format.

### Response Status Codes
The API returns standard HTTP status codes.

| Status Code | Category | Description |
|---|---|---|
| **`200-299`** | Success| Status codes in this range indicate that the requested operation has been successfully executed or at least accepted. |
| **`300-399`** | Redirection | Clients are expected to support redirection responses. |
| **`400-499`** | Error | Status codes in this range are returned if the service was unable to process an operation as expected by the client. Clients are expected to handle these error appropriately based on specific codes. |
| **`500-599`** | Fault | Status codes in this range indicate that service experienced an unexpected internal fault. |
