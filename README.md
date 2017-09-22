# InvestorAPI
The REST API for the Budding Share Market Investor Project.

[![Build status](https://ci.appveyor.com/api/projects/status/65amkvdw2q1f6oej?svg=true)](https://ci.appveyor.com/project/programmingproject1/investorapi)

## Contributing

The solution is written with .NET Core 2.0 and C#. In order to contribute you must either install Visual Studio 2017 Community Edition (free, but only available for Windows only) or Visual Studio Code (free). Both can be downloaded from here: 
https://www.visualstudio.com/downloads/

In addition, you need the .NET Core 2.0 SDK: 
https://www.microsoft.com/net/download/core

## Build & Deployment

#### Notes
AppVeyor is used to build the solution and run the unit tests after every commit. However it is not used for deployment because it does not support seem to support Docker and Heroku.

#### Prerequisites
* Make sure you have Docker installed and running: https://www.docker.com/docker-windows
* Download the Heroku CLI: https://devcenter.heroku.com/articles/heroku-cli

#### Build & Deploy Docker Container
1. In Visual Studio, build the 'docker-compose' project with 'Release' configuration.
2. Open a command prompt in the directory where the *.dockerfile* is located and run the commands below.

```
heroku login
heroku container:login
heroku container:push web --app investor-api
```
