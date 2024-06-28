# TimeSheetApp API
Model project to demonstrate all the "good implementations"

## Global Exception Handling

All exceptions are logged with serilog rolling file with a GUID reference number stated as detail.

example of a response to a controller route call that threw an exception
```
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.6.1",
  "title": "An error occurred while processing your request.",
  "status": 500,
  "detail": "55d8bb37-09e4-4ca7-9113-3adbdc5550ac",
  "traceId": "00-a277823e306dea7066ba717d6a29cd31-a13a859be44f1613-00"
}
```

The Excpetion details, such as message and a stack trace, for this particular case can be retrieved by the GUI, in the log file.
```
2024-06-27 13:02:55 -04:00 [Error] An unhandled exception has occurred while executing the request.
System.NotImplementedException: The method or operation is not implemented.
[...]
2024-06-27 13:02:55 -04:00 [Warning] detail: 55d8bb37-09e4-4ca7-9113-3adbdc5550ac
```

The intent is that a client consuming this API could communicate the 500 response body.
and the exception details could be easily retrieved from logs for further investigation.


## Validations

This is an example of a standardised return of a call that triggered validations

```
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "Email": [
      "This is not a valid e-mail address."
    ],
    "LastName": [
      "The LastName field is required.",
      "'Last Name' must not be empty.",
      "Last Name invalid"
    ],
    "UserName": [
      "The specified condition was not met for 'User Name'."
    ],
    "FirstName": [
      "'First Name' must not be empty.",
      "First Name invalid"
    ]
  },
  "traceId": "00-c37d1539a98e93954ffa6397299ad39e-9bfc89623ffe2e40-00"
}
```

For custom error that is UserName duplicate

```
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "One or more application logic errors occurred.",
  "status": 400,
  "traceId": "00-78464752aa050e451c6e6206da1d3b29-6b115a8fafd459c3-00",
  "errors": {
    "User.UserName.Duplicate": [
      "User with UserName (DRichard) already exist"
    ]
  }
}
```
this is a work in progress with validations

## Healthcheck

There is a route `/_health`
that can be used for healthchecks (Probably needs the authorisation when JWT is implemented)
example of response
```
{
    "status": "Unhealthy",
    "totalDuration": "00:00:16.2908161",
    "entries": {
        "Database": {
            "data": {},
            "description": "A network-related or instance-specific error occurred while establishing a connection to SQL Server. The server was not found or was not accessible. Verify that the instance name is correct and that SQL Server is configured to allow remote connections. (provider: TCP Provider, error: 0 - No connection could be made because the target machine actively refused it.)",
            "duration": "00:00:16.2685633",
            "exception": "A network-related or instance-specific error occurred while establishing a connection to SQL Server. The server was not found or was not accessible. Verify that the instance name is correct and that SQL Server is configured to allow remote connections. (provider: TCP Provider, error: 0 - No connection could be made because the target machine actively refused it.)",
            "status": "Unhealthy",
            "tags": []
        },
        "TypicodeAPI": {
            "data": {},
            "duration": "00:00:00.1681199",
            "status": "Healthy",
            "tags": []
        }
    }
}
```

## Info route
There is a route `/info`
that can be used to get all the relevant information on the app and the host (Probably needs the authorisation when JWT is implemented)
example of response
```
{
  "applicationInfo": {
    "version": "1.0.0.0",
    "buildNumber": null
  },
  "hostInfo": {
    "hostName": "computer",
    "path": "C:\\GIT_REPOS\\TimeSheetApp\\src\\TimeSheetApp.Api\\bin\\Debug\\net8.0\\",
    "upSince": "2024-06-28T10:41:40.5314824-04:00",
    "upTimeMinutes": 0,
    "processorCount": 8,
    "memoryUsed": 7213944,
    "processorTimeUsedSecs": 11,
    "serverDateTime": "2024-06-28T10:41:51.6390899-04:00"
  }
}
```

## Development

#### How to build the project

- To setup a database containder `docker compose -f .\docker-compose.yml up`
- Run the Api Project
- Go here in you browser `https://localhost:5001/swagger/`


#### Vertical slicing

This project is useing a vertical slicing apporoach.
All Concerns are stored in the `Concerns/ConcernName` folders.
**!!Very important!!** Concern name must be plural, while domain object must be singular.
Ex.: Concerns/User**s** while the object inside must be `User.cs`, __to avoid Namespacing problems__

#### To Do List

- ✅ Dependency Injection
- ✅ Serilog Logger + Rotating File
- ✅ Unit tests configuration
- ✅ Database with docker
- ✅ GlobalExceptionHandler
- ✅ Usage of a 3d Party API `(Typicode https://jsonplaceholder.typicode.com/)`
- ✅ Vertical Sliced Concerns
- ✅ Health check route returning App version build number and workstation information
- ✅ Validations
- 🔲 React Frontend
- 🔲 Frontend with docker
- 🔲 Integration tests
- 🔲 Use Refit to calls other apis?
- 🔲 Authentication & Authorization
