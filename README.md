# InvestorAPI
The REST API for the Budding Share Market Investor Project.

[![Build status](https://ci.appveyor.com/api/projects/status/65amkvdw2q1f6oej?svg=true)](https://ci.appveyor.com/project/programmingproject1/investorapi)

## Contributing

The solution is written with .NET Core 2.0 and C#. In order to contribute you must either install Visual Studio 2017 Community Edition (free, but only available for Windows only) or Visual Studio Code (free). Both can be downloaded from here: 
https://www.visualstudio.com/downloads/

In addition, you need the .NET Core 2.0 SDK: 
https://www.microsoft.com/net/download/core

## Build & Deployment

### Notes
AppVeyor is used to build the solution and run the unit tests after every commit. However it is not used for deployment because it does not support seem to support Docker and Heroku.

### Prerequisites
Download the Heroku CLI: https://devcenter.heroku.com/articles/heroku-cli

### Build
1. Select 'Release' as the build configuration.
2. Select 'docker-compose' as the start up project.
3. Build the project (press F5)
4. Run the commands below

```
heroku login
heroku container:login
heroku container:push web --app [app-name]
heroku open --app [app-name]
```
